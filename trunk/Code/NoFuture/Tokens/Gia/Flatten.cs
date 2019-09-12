using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Binary;
using NoFuture.Util.Core;
using NoFuture.Util.Gia.Args;
using NoFuture.Util.Gia.InvokeCmds;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia
{
    public class Flatten : InvokeConsoleBase
    {
        #region constants
        public const string GET_FLAT_ASM_PORT_CMD_SWITCH = "nfGetFlattenAssemblyPort";
        public static int DefaultPort = NfConfig.NfDefaultPorts.FlattenAssembly;
        #endregion

        #region fields
        private readonly InvokeGetFlattenAssembly _invokeGetFlattenAssemblyCmd;
        #endregion

        #region ctors

        /// <summary>
        /// Creates a new wrapper around the remote process. 
        /// </summary>
        /// <param name="ports"></param>
        /// <remarks>
        /// See the detailed notes on its sister type <see cref="AssemblyAnalysis"/> ctor.
        /// While not exact in terms of what its doing - it is exact in terms of how it does it.
        /// </remarks>
        public Flatten(params int[] ports)
        {
            if (string.IsNullOrWhiteSpace(NfConfig.CustomTools.InvokeFlatten) || !File.Exists(NfConfig.CustomTools.InvokeFlatten))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Util.Gia.InvokeFlatten.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.InvokeFlatten.");
            var args = string.Empty;
            var getFlatAsmPort = DefaultPort;
            if (ports != null && ports.Length > 0)
            {
                getFlatAsmPort = ports[0];
                args = ConsoleCmd.ConstructCmdLineArgs(GET_FLAT_ASM_PORT_CMD_SWITCH,
                    getFlatAsmPort.ToString(CultureInfo.InvariantCulture));
            }

            MyProcess = StartRemoteProcess(NfConfig.CustomTools.InvokeFlatten,args);

            _invokeGetFlattenAssemblyCmd = new InvokeGetFlattenAssembly()
            {
                ProcessId = MyProcess.Id,
                SocketPort = getFlatAsmPort
            };
        }
        #endregion

        #region instance methods
        /// <summary>
        /// Invokes the <see cref="GetFlattenedAssembly"/> on the remote process using the 
        /// assembly located at <see cref="assemblyPath"/>
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[ 
        /// $flAsm = New-Object NoFuture.Util.Gia.Flatten
        /// $flAsm.GetFlattenAssembly($AssemblyPath)
        /// ]]>
        /// </example>
        public FlattenAssembly GetFlattenAssembly(string assemblyPath)
        {
            return _invokeGetFlattenAssemblyCmd.Receive(assemblyPath);
        }

        #endregion

        #region static methods

        /// <summary>
        /// Dumps an entire assembly into a list of <see cref="FlattenedLine"/>
        /// </summary>
        /// <param name="fla"></param>
        /// <param name="writeProgress">Optional handler to write progress for the calling assembly.</param>
        /// <returns></returns>
        public static FlattenAssembly GetFlattenedAssembly(FlattenLineArgs fla, Action<ProgressMessage> writeProgress = null)
        {
            if (fla == null)
                throw new ArgumentNullException(nameof(fla));
            if (fla.Assembly == null)
                throw new ArgumentException("The Assembly reference must be passed in " +
                                            "with the FlattenLineArgs");

            writeProgress?.Invoke(new ProgressMessage
            {
                Activity = "Getting all types from assembly",
                ProcName = Process.GetCurrentProcess().ProcessName,
                ProgressCounter = 1,
                Status = "OK"
            });

            var allTypeNames = fla.Assembly.NfGetTypes().Select(x => x.FullName).ToList();
            var allLines = new List<FlattenedLine>();
            var counter = 0;
            var total = allTypeNames.Count;

            foreach (var t in allTypeNames)
            {
                writeProgress?.Invoke(new ProgressMessage
                {
                    Activity = t,
                    ProcName = Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(counter, total),
                    Status = "Working Type Names"
                });

                var flattenArgs = new FlattenTypeArgs
                {
                    Assembly = fla.Assembly,
                    TypeFullName = t,
                    Depth = fla.Depth,
                    Separator = fla.Separator,
                    UseTypeNames = fla.UseTypeNames,
                    LimitOnThisType = fla.LimitOnThisType
                };
                var flattenedType = FlattenType(flattenArgs);
                foreach (var line in flattenedType.Lines.Where(x => !string.IsNullOrWhiteSpace(x.ValueType)))
                {
                    //if there is a limit on some type and this line is that type in any form then continue
                    if (!string.IsNullOrWhiteSpace(fla.LimitOnThisType) &&
                        !string.Equals(fla.LimitOnThisType, line.ValueType, StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(fla.LimitOnThisType,
                            NfReflect.GetLastTypeNameFromArrayAndGeneric(line.ValueType),
                            StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (allLines.Any(x => x.Equals(line)))
                        continue;

                    allLines.Add(line);
                }
                counter += 1;
            }
            return new FlattenAssembly {AllLines = allLines, AssemblyName = fla.Assembly.GetName().FullName};
        }

        /// <summary>
        /// Takes the given type and recurses that type's properties 
        /// down to the <see cref="Gia.Args.FlattenTypeArgs.Depth"/> until a terminating property
        /// is a value type (both <see cref="System.ValueType"/> 
        /// and <see cref="System.String"/>)
        /// </summary>
        /// <param name="fta"></param>
        /// <returns>
        /// A list of strings where the item after the last <see cref="Gia.Args.FlattenTypeArgs.Separator"/> is the 
        /// terminating, value type, property and each item between the other <see cref="Gia.Args.FlattenTypeArgs.Separator"/>
        /// is either the property name or the type name.  The entries may be thought of 
        /// like dot-notation (e.g. MyObject.MyProperty.ItsProperty.ThenItsProperty). 
        /// The item before the first space (0x20) is the particular value type.
        /// </returns>
        public static FlattenedType FlattenType(FlattenTypeArgs fta)
        {
            var assembly = fta.Assembly;
            var typeFulleName = fta.TypeFullName;
            var maxDepth = fta.Depth;

            var startCount = 0;
            if (maxDepth <= 0)
                maxDepth = 16;
            var results = FlattenType(assembly, typeFulleName, ref startCount, maxDepth, fta.LimitOnThisType,
                fta.DisplayEnums, null, null);
            return new FlattenedType
            {
                Lines = results,
                UseTypeNames = fta.UseTypeNames,
                RootType = assembly.GetType(typeFulleName),
                Separator = fta.Separator
            };
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        public static List<FlattenedLine> FlattenType(Assembly assembly, string typeFullName,
            ref int currentDepth, int maxDepth, string limitOnValueType, bool displayEnums, Stack<FlattenedItem> fiValueTypes, Stack typeStack)
        {
            var printList = new List<FlattenedLine>();
            if (string.IsNullOrWhiteSpace(typeFullName))
                return printList;

            Func<PropertyInfo, string, bool> limitOnPi =
                (info, s) =>
                    string.IsNullOrWhiteSpace(s) ||
                    string.Equals($"{info.PropertyType}", s, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(NfReflect.GetLastTypeNameFromArrayAndGeneric(info.PropertyType), s,
                        StringComparison.OrdinalIgnoreCase) ||
                    (s == Constants.ENUM && NfReflect.IsEnumType(info.PropertyType));

            var currentType = assembly.NfGetType(typeFullName);
            if (currentType == null)
                return printList;

            //top frame of recursive calls will perform this
            if (fiValueTypes == null)
            {
                if (maxDepth <= 0)
                    maxDepth = 16;
                typeStack = new Stack();
                typeStack.Push(typeFullName);
                fiValueTypes = new Stack<FlattenedItem>();
                fiValueTypes.Push(new FlattenedItem(currentType)
                {
                    FlName = NfReflect.GetTypeNameWithoutNamespace(typeFullName)
                });
            }

            var typeNamesList =
                currentType.GetProperties(NfSettings.DefaultFlags)
                    .Where(
                        x =>
                            (NfReflect.IsValueTypeProperty(x) && limitOnPi(x, limitOnValueType)
                             || (limitOnValueType == Constants.ENUM && limitOnPi(x, limitOnValueType)))
                    //more limbo branching for enums
                    )
                    .Select(p => new Tuple<Type, string>(p.PropertyType, p.Name))
                    .ToList();


            foreach (var typeNamePair in typeNamesList)
            {
                var pVtype = typeNamePair.Item1;
                var pVname = typeNamePair.Item2;

                fiValueTypes.Push(new FlattenedItem(pVtype) { FlName = pVname });
                var fiItems = fiValueTypes.ToList();
                fiItems.Reverse();
                printList.Add(new FlattenedLine(fiItems.Distinct(new FlattenedItemComparer()).ToList())
                {
                    ValueType = $"{typeNamePair.Item1}"
                });
                fiValueTypes.Pop();
            }

            //then recurse the object types
            foreach (
                var p in
                    currentType.GetProperties(NfSettings.DefaultFlags)
                        .Where(x => !NfReflect.IsValueTypeProperty(x)))
            {
                currentDepth += 1;

                //time to go
                if (currentDepth >= maxDepth)
                    return printList;

                var typeIn = NfReflect.GetLastTypeNameFromArrayAndGeneric(p.PropertyType);

                if (typeIn == null || typeStack.Contains(typeIn))
                    continue;

                var fi = new FlattenedItem(p.PropertyType) {FlName = p.Name};
                if (fiValueTypes.ToList().Any(x => x.FlType == p.PropertyType))
                    continue;

                fiValueTypes.Push(fi);
                typeStack.Push(typeIn);

                //enum types being handled as limbo between value type and ref type
                string[] enumVals;
                if (displayEnums && NfReflect.IsEnumType(p.PropertyType, out enumVals))
                {
                    foreach (var ev in enumVals)
                    {
                        fiValueTypes.Push(new FlattenedItem(typeof(Enum)) {FlName = ev});
                        var fiItems = fiValueTypes.ToList();
                        fiItems.Reverse();
                        printList.Add(new FlattenedLine(fiItems.Distinct(new FlattenedItemComparer()).ToList())
                        {
                            ValueType = String.Empty
                        });
                        fiValueTypes.Pop();
                    }
                }
                else
                {
                    printList.AddRange(FlattenType(assembly, fi.TypeFullName, ref currentDepth, maxDepth,
                        limitOnValueType, displayEnums, fiValueTypes, typeStack));
                }

                fiValueTypes.Pop();
                typeStack.Pop();
                currentDepth -= 1;
            }
            return printList;
        }
        #endregion
    }

}