using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Reflection.Emit;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Util.Binary
{
    public static class Asm
    {
        #region constants
        public const string DEFAULT_ASM_LOG_FILE_NAME = "ResolveAssemblyEventHandler.log";
        #endregion

        #region fields
        private static readonly Dictionary<int, OpCode> _opCodes2Value = new Dictionary<int, OpCode>();
        private static string _resolveAsmLog;
        private static readonly object _resolveAsmLogLock = new object();
        #endregion

        #region asm logging
        /// <summary>
        /// A log file used to record assembly load events.
        /// </summary>
        public static string ResolveAsmLog
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(_resolveAsmLog))
                    return _resolveAsmLog;
                var logDir = String.IsNullOrWhiteSpace(NfConfig.TempDirectories.Debug) || !Directory.Exists(NfConfig.TempDirectories.Debug)
                    ? NfSettings.AppData
                    : NfConfig.TempDirectories.Debug;
                _resolveAsmLog = Path.Combine(logDir, DEFAULT_ASM_LOG_FILE_NAME);
                return _resolveAsmLog;
            }
            set { _resolveAsmLog = value; }
        }

        /// <summary>
        /// Intended for logging exception which are thrown when calling <see cref="Assembly.GetTypes"/>
        /// </summary>
        /// <param name="attemptedResolveType">
        /// Optional parameter for more detail about which type we were attempting to resolve at the time of exception.
        /// </param>
        /// <param name="ex">The exception being caught</param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        public static void AddLoaderExceptionToLog(Tuple<Type, MemberInfo> attemptedResolveType, Exception ex, string logFile = "")
        {
            if (ex == null)
                return;
            var logFilePath = string.IsNullOrWhiteSpace(logFile) ||
                              string.IsNullOrWhiteSpace(Path.GetDirectoryName(logFile)) ||
                              !Directory.Exists(Path.GetDirectoryName(logFile))
                ? ResolveAsmLog
                : logFile;

            lock (_resolveAsmLogLock)
            {
                try
                {
                    var logEntry = GetExceptionStrBldr(attemptedResolveType, ex);
                    File.AppendAllText(logFilePath,
                        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                    var fnfe = ex as FileNotFoundException;
                    var tle = ex as TypeLoadException;
                    var fle = ex as FileLoadException;
                    var bife = ex as BadImageFormatException;
                    var rftl = ex as ReflectionTypeLoadException;

                    if (rftl != null && rftl.LoaderExceptions != null && rftl.LoaderExceptions.Length > 0)
                    {
                        logEntry.AppendLine("LoaderExceptions:");
                        for (var i = 0; i < rftl.LoaderExceptions.Length; i++)
                        {
                            var loaderText = GetExceptionStrBldr(null, rftl.LoaderExceptions[i]);
                            logEntry.Append($"({i}) {loaderText}");
                        }
                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (fnfe != null)
                    {
                        logEntry.AppendFormat("FusionLog:{0}", fnfe.FusionLog);
                        logEntry.AppendLine();
                        logEntry.AppendFormat("FileName:{0}", fnfe.FileName);

                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (tle != null)
                    {
                        logEntry.AppendFormat("TypeLoaderException causing type: '{0}'", tle.TypeName);
                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (fle != null)
                    {
                        logEntry.AppendFormat("FusionLog:{0}", fle.FusionLog);
                        logEntry.AppendLine();
                        logEntry.AppendFormat("FileName:{0}", fle.FileName);

                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (bife == null) return;

                    logEntry.AppendFormat("FusionLog:{0}", bife.FusionLog);
                    logEntry.AppendLine();
                    logEntry.AppendFormat("FileName:{0}", bife.FileName);

                    File.AppendAllText(logFilePath,
                        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    //Yuron Yurown
                }
            }
        }
        #endregion

        /// <summary>
        /// The actual search functionality used by the 
        /// <see cref="FxPointers.ResolveReflectionOnlyAssembly"/> - in itself reuseable.
        /// </summary>
        /// <param name="asmFullName">This is expected as the fully qualified assembly name.</param>
        /// <param name="searchDirectory"></param>
        /// <param name="reflectionOnly">
        /// Branches search to <see cref="AppDomain.ReflectionOnlyGetAssemblies"/>
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// see
        /// [https://msdn.microsoft.com/en-us/library/k8xx4k69(v=vs.110).aspx]
        /// NOTE: attempting to send both the ReflectionOnlyAssemblyResolve and the AssemblyResolve
        /// to this same method has caused the entire process to crash.
        /// </remarks>
        public static Assembly SearchAppDomainForAssembly(string asmFullName, string searchDirectory = "",
            bool reflectionOnly = true)
        {
            Assembly foundOne = null;
            //an assembly'esque name is critical to this effort
            if (String.IsNullOrWhiteSpace(asmFullName))
                throw new ItsDeadJim(
                    $"There is no way to resolve a assembly dependency with a name of '{asmFullName}'");


            File.AppendAllText(ResolveAsmLog,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] Starting search for the assembly named '{asmFullName}' with reflection only set to '{reflectionOnly}'. \n");

            //if its already in the appdomain then just return it, why are we here in such a case?
            var itsAlreadyHere = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                .FirstOrDefault(
                    asm => AssemblyName.ReferenceMatchesDefinition(asm.GetName(), new AssemblyName(asmFullName))) ??
                                      AppDomain.CurrentDomain.GetAssemblies()
                                          .FirstOrDefault(
                                              asm => AssemblyName.ReferenceMatchesDefinition(asm.GetName(), new AssemblyName(asmFullName)));
            if (itsAlreadyHere != null)
                return itsAlreadyHere;

            //if requesting assembly has a location then start there
            if (!String.IsNullOrWhiteSpace(searchDirectory))
            {
                //expect dependecies from the GAC to be resolved using the GAC
                if (searchDirectory.Contains("\\GAC_MSIL\\") || searchDirectory.Contains("\\GAC_64\\") ||
                    searchDirectory.StartsWith(@"C:\Windows\Microsoft.NET\Framework"))
                    return reflectionOnly ? Assembly.ReflectionOnlyLoad(asmFullName) : Assembly.Load(asmFullName);

                if (SearchDirectoriesForAssembly(Path.GetDirectoryName(searchDirectory), asmFullName, out foundOne,
                    reflectionOnly))
                    return foundOne;
            }

            //search the directory explicity set
            if (NfConfig.AssemblySearchPaths.Any())
            {
                if (
                    NfConfig.AssemblySearchPaths.Distinct()
                        .Any(sd => SearchDirectoriesForAssembly(sd, asmFullName, out foundOne,
                            reflectionOnly)))
                {
                    return foundOne;
                }
            }

            //check in NoFuture's bin
            if (!String.IsNullOrWhiteSpace(NfConfig.BinDirectories.Root))
            {
                if (SearchDirectoriesForAssembly(NfConfig.BinDirectories.Root, asmFullName, out foundOne, reflectionOnly))
                    return foundOne;
            }

            //search this domain's working directory
            if (SearchDirectoriesForAssembly(AppDomain.CurrentDomain.BaseDirectory, asmFullName, out foundOne,
                reflectionOnly))
                return foundOne;

            //have next app domain handler give it a go
            return null;
        }

        #region metadata tokens
        /// <summary>
        /// Gets the length of a <see cref="OpCode.OperandType"/>
        /// as the number of bytes.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://msdn.microsoft.com/en-us/library/system.reflection.emit.operandtype(v=vs.110).aspx
        /// </remarks>
        public static int GetOpCodeOperandByteSize(OpCode op)
        {
            switch (op.OperandType)
            {
                case OperandType.InlineI8:
                case OperandType.InlineR:
                    return 8;
                case OperandType.InlineVar:
                    return 2;
                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineVar:
                case OperandType.ShortInlineI:
                    return 1;
                case OperandType.InlineNone:
                    return 0;
                default:
                    return 4;
            }
        }

        /// <summary>
        /// Gets the <see cref="OpCode"/> whose <see cref="OpCode.Value"/> equals <see cref="opCodeValue"/>
        /// </summary>
        /// <param name="opCodeValue"></param>
        /// <returns></returns>
        /// <remarks>
        /// partially generated using the following PowerShell:
        /// <![CDATA[
        ///[System.Reflection.Assembly]::LoadWithPartialName("System.Reflection.Emit")
        ///
        ///$refToOpCodes = ([System.Reflection.Emit.OpCodes])
        ///
        ///([System.Reflection.Emit.OpCodes]).GetFields() | ? {$_.FieldType.Name -eq  "OpCode"} | % { 
        ///    $opCodeName = $_.Name
        ///    $opCodeRef = $refToOpCodes::$opCodeName
        ///    $opCodeValue = $opCodeRef.Value
        ///
        ///@"
        ///                _opCodes2Value.Add($opCodeValue, OpCodes.$opCodeName);
        ///"@    
        ///}
        /// ]]>
        /// </remarks>
        public static OpCode GetOpCodeByValue(int opCodeValue)
        {
            if (_opCodes2Value.Count <= 0)
            {
                _opCodes2Value.Add(0x00, OpCodes.Nop);
                _opCodes2Value.Add(0x01, OpCodes.Break);
                _opCodes2Value.Add(0x02, OpCodes.Ldarg_0);
                _opCodes2Value.Add(0x03, OpCodes.Ldarg_1);
                _opCodes2Value.Add(0x04, OpCodes.Ldarg_2);
                _opCodes2Value.Add(0x05, OpCodes.Ldarg_3);
                _opCodes2Value.Add(0x06, OpCodes.Ldloc_0);
                _opCodes2Value.Add(0x07, OpCodes.Ldloc_1);
                _opCodes2Value.Add(0x08, OpCodes.Ldloc_2);
                _opCodes2Value.Add(0x09, OpCodes.Ldloc_3);
                _opCodes2Value.Add(0x0A, OpCodes.Stloc_0);
                _opCodes2Value.Add(0x0B, OpCodes.Stloc_1);
                _opCodes2Value.Add(0x0C, OpCodes.Stloc_2);
                _opCodes2Value.Add(0x0D, OpCodes.Stloc_3);
                _opCodes2Value.Add(0x0E, OpCodes.Ldarg_S);
                _opCodes2Value.Add(0x0F, OpCodes.Ldarga_S);
                _opCodes2Value.Add(0x10, OpCodes.Starg_S);
                _opCodes2Value.Add(0x11, OpCodes.Ldloc_S);
                _opCodes2Value.Add(0x12, OpCodes.Ldloca_S);
                _opCodes2Value.Add(0x13, OpCodes.Stloc_S);
                _opCodes2Value.Add(0x14, OpCodes.Ldnull);
                _opCodes2Value.Add(0x15, OpCodes.Ldc_I4_M1);
                _opCodes2Value.Add(0x16, OpCodes.Ldc_I4_0);
                _opCodes2Value.Add(0x17, OpCodes.Ldc_I4_1);
                _opCodes2Value.Add(0x18, OpCodes.Ldc_I4_2);
                _opCodes2Value.Add(0x19, OpCodes.Ldc_I4_3);
                _opCodes2Value.Add(0x1A, OpCodes.Ldc_I4_4);
                _opCodes2Value.Add(0x1B, OpCodes.Ldc_I4_5);
                _opCodes2Value.Add(0x1C, OpCodes.Ldc_I4_6);
                _opCodes2Value.Add(0x1D, OpCodes.Ldc_I4_7);
                _opCodes2Value.Add(0x1E, OpCodes.Ldc_I4_8);
                _opCodes2Value.Add(0x1F, OpCodes.Ldc_I4_S);
                _opCodes2Value.Add(0x20, OpCodes.Ldc_I4);
                _opCodes2Value.Add(0x21, OpCodes.Ldc_I8);
                _opCodes2Value.Add(0x22, OpCodes.Ldc_R4);
                _opCodes2Value.Add(0x23, OpCodes.Ldc_R8);
                _opCodes2Value.Add(0x25, OpCodes.Dup);
                _opCodes2Value.Add(0x26, OpCodes.Pop);
                _opCodes2Value.Add(0x27, OpCodes.Jmp);
                _opCodes2Value.Add(0x28, OpCodes.Call);
                _opCodes2Value.Add(0x29, OpCodes.Calli);
                _opCodes2Value.Add(0x2A, OpCodes.Ret);
                _opCodes2Value.Add(0x2B, OpCodes.Br_S);
                _opCodes2Value.Add(0x2C, OpCodes.Brfalse_S);
                _opCodes2Value.Add(0x2D, OpCodes.Brtrue_S);
                _opCodes2Value.Add(0x2E, OpCodes.Beq_S);
                _opCodes2Value.Add(0x2F, OpCodes.Bge_S);
                _opCodes2Value.Add(0x30, OpCodes.Bgt_S);
                _opCodes2Value.Add(0x31, OpCodes.Ble_S);
                _opCodes2Value.Add(0x32, OpCodes.Blt_S);
                _opCodes2Value.Add(0x33, OpCodes.Bne_Un_S);
                _opCodes2Value.Add(0x34, OpCodes.Bge_Un_S);
                _opCodes2Value.Add(0x35, OpCodes.Bgt_Un_S);
                _opCodes2Value.Add(0x36, OpCodes.Ble_Un_S);
                _opCodes2Value.Add(0x37, OpCodes.Blt_Un_S);
                _opCodes2Value.Add(0x38, OpCodes.Br);
                _opCodes2Value.Add(0x39, OpCodes.Brfalse);
                _opCodes2Value.Add(0x3A, OpCodes.Brtrue);
                _opCodes2Value.Add(0x3B, OpCodes.Beq);
                _opCodes2Value.Add(0x3C, OpCodes.Bge);
                _opCodes2Value.Add(0x3D, OpCodes.Bgt);
                _opCodes2Value.Add(0x3E, OpCodes.Ble);
                _opCodes2Value.Add(0x3F, OpCodes.Blt);
                _opCodes2Value.Add(0x40, OpCodes.Bne_Un);
                _opCodes2Value.Add(0x41, OpCodes.Bge_Un);
                _opCodes2Value.Add(0x42, OpCodes.Bgt_Un);
                _opCodes2Value.Add(0x43, OpCodes.Ble_Un);
                _opCodes2Value.Add(0x44, OpCodes.Blt_Un);
                _opCodes2Value.Add(0x45, OpCodes.Switch);
                _opCodes2Value.Add(0x46, OpCodes.Ldind_I1);
                _opCodes2Value.Add(0x47, OpCodes.Ldind_U1);
                _opCodes2Value.Add(0x48, OpCodes.Ldind_I2);
                _opCodes2Value.Add(0x49, OpCodes.Ldind_U2);
                _opCodes2Value.Add(0x4A, OpCodes.Ldind_I4);
                _opCodes2Value.Add(0x4B, OpCodes.Ldind_U4);
                _opCodes2Value.Add(0x4C, OpCodes.Ldind_I8);
                _opCodes2Value.Add(0x4D, OpCodes.Ldind_I);
                _opCodes2Value.Add(0x4E, OpCodes.Ldind_R4);
                _opCodes2Value.Add(0x4F, OpCodes.Ldind_R8);
                _opCodes2Value.Add(0x50, OpCodes.Ldind_Ref);
                _opCodes2Value.Add(0x51, OpCodes.Stind_Ref);
                _opCodes2Value.Add(0x52, OpCodes.Stind_I1);
                _opCodes2Value.Add(0x53, OpCodes.Stind_I2);
                _opCodes2Value.Add(0x54, OpCodes.Stind_I4);
                _opCodes2Value.Add(0x55, OpCodes.Stind_I8);
                _opCodes2Value.Add(0x56, OpCodes.Stind_R4);
                _opCodes2Value.Add(0x57, OpCodes.Stind_R8);
                _opCodes2Value.Add(0x58, OpCodes.Add);
                _opCodes2Value.Add(0x59, OpCodes.Sub);
                _opCodes2Value.Add(0x5A, OpCodes.Mul);
                _opCodes2Value.Add(0x5B, OpCodes.Div);
                _opCodes2Value.Add(0x5C, OpCodes.Div_Un);
                _opCodes2Value.Add(0x5D, OpCodes.Rem);
                _opCodes2Value.Add(0x5E, OpCodes.Rem_Un);
                _opCodes2Value.Add(0x5F, OpCodes.And);
                _opCodes2Value.Add(0x60, OpCodes.Or);
                _opCodes2Value.Add(0x61, OpCodes.Xor);
                _opCodes2Value.Add(0x62, OpCodes.Shl);
                _opCodes2Value.Add(0x63, OpCodes.Shr);
                _opCodes2Value.Add(0x64, OpCodes.Shr_Un);
                _opCodes2Value.Add(0x65, OpCodes.Neg);
                _opCodes2Value.Add(0x66, OpCodes.Not);
                _opCodes2Value.Add(0x67, OpCodes.Conv_I1);
                _opCodes2Value.Add(0x68, OpCodes.Conv_I2);
                _opCodes2Value.Add(0x69, OpCodes.Conv_I4);
                _opCodes2Value.Add(0x6A, OpCodes.Conv_I8);
                _opCodes2Value.Add(0x6B, OpCodes.Conv_R4);
                _opCodes2Value.Add(0x6C, OpCodes.Conv_R8);
                _opCodes2Value.Add(0x6D, OpCodes.Conv_U4);
                _opCodes2Value.Add(0x6E, OpCodes.Conv_U8);
                _opCodes2Value.Add(0x6F, OpCodes.Callvirt);
                _opCodes2Value.Add(0x70, OpCodes.Cpobj);
                _opCodes2Value.Add(0x71, OpCodes.Ldobj);
                _opCodes2Value.Add(0x72, OpCodes.Ldstr);
                _opCodes2Value.Add(0x73, OpCodes.Newobj);
                _opCodes2Value.Add(0x74, OpCodes.Castclass);
                _opCodes2Value.Add(0x75, OpCodes.Isinst);
                _opCodes2Value.Add(0x76, OpCodes.Conv_R_Un);
                _opCodes2Value.Add(0x79, OpCodes.Unbox);
                _opCodes2Value.Add(0x7A, OpCodes.Throw);
                _opCodes2Value.Add(0x7B, OpCodes.Ldfld);
                _opCodes2Value.Add(0x7C, OpCodes.Ldflda);
                _opCodes2Value.Add(0x7D, OpCodes.Stfld);
                _opCodes2Value.Add(0x7E, OpCodes.Ldsfld);
                _opCodes2Value.Add(0x7F, OpCodes.Ldsflda);
                _opCodes2Value.Add(0x80, OpCodes.Stsfld);
                _opCodes2Value.Add(0x81, OpCodes.Stobj);
                _opCodes2Value.Add(0x82, OpCodes.Conv_Ovf_I1_Un);
                _opCodes2Value.Add(0x83, OpCodes.Conv_Ovf_I2_Un);
                _opCodes2Value.Add(0x84, OpCodes.Conv_Ovf_I4_Un);
                _opCodes2Value.Add(0x85, OpCodes.Conv_Ovf_I8_Un);
                _opCodes2Value.Add(0x86, OpCodes.Conv_Ovf_U1_Un);
                _opCodes2Value.Add(0x87, OpCodes.Conv_Ovf_U2_Un);
                _opCodes2Value.Add(0x88, OpCodes.Conv_Ovf_U4_Un);
                _opCodes2Value.Add(0x89, OpCodes.Conv_Ovf_U8_Un);
                _opCodes2Value.Add(0x8A, OpCodes.Conv_Ovf_I_Un);
                _opCodes2Value.Add(0x8B, OpCodes.Conv_Ovf_U_Un);
                _opCodes2Value.Add(0x8C, OpCodes.Box);
                _opCodes2Value.Add(0x8D, OpCodes.Newarr);
                _opCodes2Value.Add(0x8E, OpCodes.Ldlen);
                _opCodes2Value.Add(0x8F, OpCodes.Ldelema);
                _opCodes2Value.Add(0x90, OpCodes.Ldelem_I1);
                _opCodes2Value.Add(0x91, OpCodes.Ldelem_U1);
                _opCodes2Value.Add(0x92, OpCodes.Ldelem_I2);
                _opCodes2Value.Add(0x93, OpCodes.Ldelem_U2);
                _opCodes2Value.Add(0x94, OpCodes.Ldelem_I4);
                _opCodes2Value.Add(0x95, OpCodes.Ldelem_U4);
                _opCodes2Value.Add(0x96, OpCodes.Ldelem_I8);
                _opCodes2Value.Add(0x97, OpCodes.Ldelem_I);
                _opCodes2Value.Add(0x98, OpCodes.Ldelem_R4);
                _opCodes2Value.Add(0x99, OpCodes.Ldelem_R8);
                _opCodes2Value.Add(0x9A, OpCodes.Ldelem_Ref);
                _opCodes2Value.Add(0x9B, OpCodes.Stelem_I);
                _opCodes2Value.Add(0x9C, OpCodes.Stelem_I1);
                _opCodes2Value.Add(0x9D, OpCodes.Stelem_I2);
                _opCodes2Value.Add(0x9E, OpCodes.Stelem_I4);
                _opCodes2Value.Add(0x9F, OpCodes.Stelem_I8);
                _opCodes2Value.Add(0xA0, OpCodes.Stelem_R4);
                _opCodes2Value.Add(0xA1, OpCodes.Stelem_R8);
                _opCodes2Value.Add(0xA2, OpCodes.Stelem_Ref);
                _opCodes2Value.Add(0xA3, OpCodes.Ldelem);
                _opCodes2Value.Add(0xA4, OpCodes.Stelem);
                _opCodes2Value.Add(0xA5, OpCodes.Unbox_Any);
                _opCodes2Value.Add(0xB3, OpCodes.Conv_Ovf_I1);
                _opCodes2Value.Add(0xB4, OpCodes.Conv_Ovf_U1);
                _opCodes2Value.Add(0xB5, OpCodes.Conv_Ovf_I2);
                _opCodes2Value.Add(0xB6, OpCodes.Conv_Ovf_U2);
                _opCodes2Value.Add(0xB7, OpCodes.Conv_Ovf_I4);
                _opCodes2Value.Add(0xB8, OpCodes.Conv_Ovf_U4);
                _opCodes2Value.Add(0xB9, OpCodes.Conv_Ovf_I8);
                _opCodes2Value.Add(0xBA, OpCodes.Conv_Ovf_U8);
                _opCodes2Value.Add(0xC2, OpCodes.Refanyval);
                _opCodes2Value.Add(0xC3, OpCodes.Ckfinite);
                _opCodes2Value.Add(0xC6, OpCodes.Mkrefany);
                _opCodes2Value.Add(0xD0, OpCodes.Ldtoken);
                _opCodes2Value.Add(0xD1, OpCodes.Conv_U2);
                _opCodes2Value.Add(0xD2, OpCodes.Conv_U1);
                _opCodes2Value.Add(0xD3, OpCodes.Conv_I);
                _opCodes2Value.Add(0xD4, OpCodes.Conv_Ovf_I);
                _opCodes2Value.Add(0xD5, OpCodes.Conv_Ovf_U);
                _opCodes2Value.Add(0xD6, OpCodes.Add_Ovf);
                _opCodes2Value.Add(0xD7, OpCodes.Add_Ovf_Un);
                _opCodes2Value.Add(0xD8, OpCodes.Mul_Ovf);
                _opCodes2Value.Add(0xD9, OpCodes.Mul_Ovf_Un);
                _opCodes2Value.Add(0xDA, OpCodes.Sub_Ovf);
                _opCodes2Value.Add(0xDB, OpCodes.Sub_Ovf_Un);
                _opCodes2Value.Add(0xDC, OpCodes.Endfinally);
                _opCodes2Value.Add(0xDD, OpCodes.Leave);
                _opCodes2Value.Add(0xDE, OpCodes.Leave_S);
                _opCodes2Value.Add(0xDF, OpCodes.Stind_I);
                _opCodes2Value.Add(0xE0, OpCodes.Conv_U);
                _opCodes2Value.Add(0xF8, OpCodes.Prefix7);
                _opCodes2Value.Add(0xF9, OpCodes.Prefix6);
                _opCodes2Value.Add(0xFA, OpCodes.Prefix5);
                _opCodes2Value.Add(0xFB, OpCodes.Prefix4);
                _opCodes2Value.Add(0xFC, OpCodes.Prefix3);
                _opCodes2Value.Add(0xFD, OpCodes.Prefix2);
                _opCodes2Value.Add(0xFE, OpCodes.Prefix1);
                _opCodes2Value.Add(0xFF, OpCodes.Prefixref);
                _opCodes2Value.Add(0xFE00, OpCodes.Arglist);
                _opCodes2Value.Add(0xFE01, OpCodes.Ceq);
                _opCodes2Value.Add(0xFE02, OpCodes.Cgt);
                _opCodes2Value.Add(0xFE03, OpCodes.Cgt_Un);
                _opCodes2Value.Add(0xFE04, OpCodes.Clt);
                _opCodes2Value.Add(0xFE05, OpCodes.Clt_Un);
                _opCodes2Value.Add(0xFE06, OpCodes.Ldftn);
                _opCodes2Value.Add(0xFE07, OpCodes.Ldvirtftn);
                _opCodes2Value.Add(0xFE09, OpCodes.Ldarg);
                _opCodes2Value.Add(0xFE0A, OpCodes.Ldarga);
                _opCodes2Value.Add(0xFE0B, OpCodes.Starg);
                _opCodes2Value.Add(0xFE0C, OpCodes.Ldloc);
                _opCodes2Value.Add(0xFE0D, OpCodes.Ldloca);
                _opCodes2Value.Add(0xFE0E, OpCodes.Stloc);
                _opCodes2Value.Add(0xFE0F, OpCodes.Localloc);
                _opCodes2Value.Add(0xFE11, OpCodes.Endfilter);
                _opCodes2Value.Add(0xFE12, OpCodes.Unaligned);
                _opCodes2Value.Add(0xFE13, OpCodes.Volatile);
                _opCodes2Value.Add(0xFE14, OpCodes.Tailcall);
                _opCodes2Value.Add(0xFE15, OpCodes.Initobj);
                _opCodes2Value.Add(0xFE16, OpCodes.Constrained);
                _opCodes2Value.Add(0xFE17, OpCodes.Cpblk);
                _opCodes2Value.Add(0xFE18, OpCodes.Initblk);
                _opCodes2Value.Add(0xFE1A, OpCodes.Rethrow);
                _opCodes2Value.Add(0xFE1C, OpCodes.Sizeof);
                _opCodes2Value.Add(0xFE1D, OpCodes.Refanytype);
                _opCodes2Value.Add(0xFE1E, OpCodes.Readonly);
            }
            if (_opCodes2Value.Any(x => x.Key == opCodeValue))
                return _opCodes2Value.First(x => x.Key == opCodeValue).Value;
            return OpCodes.Nop;
        }

        /// <summary>
        /// Given the <see cref="rawIlByteStream"/> the occurance of 
        /// </summary>
        /// <param name="rawIlByteStream"></param>
        /// <param name="opCodes"></param>
        /// <returns></returns>
        /// <remarks>
        /// Pass the metadata token into an assembly's manifest module's <see cref="System.Reflection.Module.ResolveMember(int)"/>
        /// to get the <see cref="MethodInfo"/>
        /// </remarks>
        public static int[] GetOpCodesArgs(byte[] rawIlByteStream, OpCode[] opCodes)
        {
            if (rawIlByteStream == null)
                return null;

            var metadataTokens = new List<int>();

            //expecting first byte to always be an OpCode, never an arg thereof
            for (var i = 0; i < rawIlByteStream.Length; i++)
            {
                var opCodei = GetOpCodeByValue(rawIlByteStream[i]);
                if (new[]
                {
                    OpCodes.Prefixref, OpCodes.Prefix1, OpCodes.Prefix2, OpCodes.Prefix3, OpCodes.Prefix4,
                    OpCodes.Prefix5, OpCodes.Prefix6, OpCodes.Prefix7
                }.Contains(opCodei))
                {
                    opCodei = GetOpCodeByValue(ByteArray.ToDWord(0x0, 0x0, rawIlByteStream[i], rawIlByteStream[i + 1]));
                    i = i + 1;
                }

                var moveBuffer = GetOpCodeOperandByteSize(opCodei);
                if (moveBuffer == 0 || moveBuffer > 4 ||//don't handle IA64 since its not resolvable as a token
                    (rawIlByteStream.Length <= i + moveBuffer) ||
                    (opCodes.All(x => opCodei != x)))
                {
                    i = i + moveBuffer;
                    continue;
                }

                var metadataToken = 0;
                switch (moveBuffer)
                {
                    case 1:
                        metadataToken = rawIlByteStream[i + 1];
                        break;
                    case 2:
                        metadataToken = ByteArray.ToWord(rawIlByteStream[i + 2], rawIlByteStream[i + 1]);
                        break;
                    case 4:
                        metadataToken = ByteArray.ToDWord(rawIlByteStream[i + 4], rawIlByteStream[i + 3],
                            rawIlByteStream[i + 2], rawIlByteStream[i + 1]);
                        break;
                }

                //get only distinct entries
                if (metadataToken != 0 && metadataTokens.All(x => x != metadataToken))
                    metadataTokens.Add(metadataToken);

                i = i + moveBuffer;
            }

            return metadataTokens.ToArray();
        }

        /// <summary>
        /// Gets a method's body as a list of IL <see cref="OpCode"/> 
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static OpCode[] GetOpCodesList(MethodBase mb)
        {
            var codesOut = new List<OpCode>();
            if (mb == null)
                return codesOut.ToArray();
            var il = GetMethodBody(mb);
            if (il == null || !il.Any())
                return codesOut.ToArray();
            for (var i = 0; i < il.Length; i++)
            {
                var opCodei = GetOpCodeByValue(il[i]);
                codesOut.Add(opCodei);
                var moveBuffer = GetOpCodeOperandByteSize(opCodei);
                if (il.Length <= i + moveBuffer)
                {
                    return codesOut.ToArray();
                }
                i = i + moveBuffer;
            }
            return codesOut.ToArray();
        }

        /// <summary>
        /// Gets the IL byte array of the method.
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static byte[] GetMethodBody(MethodBase mb)
        {
            var emptyArray = new byte[0];
            if (mb == null)
                return emptyArray;
            try
            {
                var body = mb.GetMethodBody();
                return body == null ? emptyArray : body.GetILAsByteArray();
            }
            catch
            {
                return emptyArray;
            }
        }

        /// <summary>
        /// Helper method for its overload checking 
        /// for nulls down to the <see cref="System.Reflection.MethodBody"/>
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="opCodes"></param>
        /// <returns></returns>
        public static int[] GetOpCodesArgs(MethodBase mb, OpCode[] opCodes)
        {
            var il = GetMethodBody(mb);
            return il == null || !il.Any() ? new int[0] : GetOpCodesArgs(il, opCodes);
        }

        /// <summary>
        /// Uses the <see cref="OpCodes.Callvirt"/> and <see cref="OpCodes.Call"/>
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static int[] GetCallsMetadataTokens(MethodBase mb)
        {
            return GetOpCodesArgs(mb, new[] { OpCodes.Callvirt, OpCodes.Call, OpCodes.Ldftn });
        }
        #endregion

        #region reflection api wrappers
        /// <summary>
        /// Intended when an assembly is only recognizable by its location and therefore must 
        /// be loaded as such.  The reflection only load-from assemblies are 
        /// loaded from <see cref="NfConfig.TempDirectories.Binary"/>.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Assembly NfReflectionOnlyLoadFrom(string assemblyPath, bool rethrow = true, string logFile = "")
        {
            if(String.IsNullOrWhiteSpace(assemblyPath))
                throw new ItsDeadJim($"There is no assembly at the path '{assemblyPath}'");

            //check our domain for this assembly having been loaded once before
            var asm =
                AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                    .Where(x => !x.IsDynamic)
                    .FirstOrDefault(x => x.Location == assemblyPath);
            if (asm != null)
                return asm;

            try
            {
                asm = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }
            catch (ArgumentException argEx)
            {
                AddLoaderExceptionToLog(null, argEx, logFile);
                if (rethrow)
                    throw;
            }
            catch (IOException ioEx)
            {
                AddLoaderExceptionToLog(null, ioEx, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException badEx)
            {
                AddLoaderExceptionToLog(null, badEx, logFile);
                if (rethrow)
                    throw;
            }
            
            if (asm != null &&
                NfConfig.AssemblySearchPaths.Any(
                    x => !string.Equals(x, Path.GetDirectoryName(assemblyPath), StringComparison.OrdinalIgnoreCase)))
                NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(assemblyPath));

            return asm;
        }

        /// <summary>
        /// Intended when an assembly is only recognizable by its location and therefore must 
        /// be loaded as such.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Assembly NfLoadFrom(string assemblyPath, bool rethrow = true, string logFile = "")
        {
            if (String.IsNullOrWhiteSpace(assemblyPath))
                throw new ArgumentNullException(nameof(assemblyPath));

            //check our domain for this assembly having been loaded once before
            var asm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => !x.IsDynamic)
                    .FirstOrDefault(x => x.Location == assemblyPath);
            if (asm != null)
                return asm;

            try
            {
                asm = Assembly.LoadFrom(assemblyPath);
            }
            catch (ArgumentException argEx)
            {
                AddLoaderExceptionToLog(null, argEx, logFile);
                if (rethrow)
                    throw;
            }
            catch (IOException ioEx)
            {
                AddLoaderExceptionToLog(null, ioEx, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException badEx)
            {
                AddLoaderExceptionToLog(null, badEx, logFile);
                if (rethrow)
                    throw;
            }
            
            if (asm != null &&
                NfConfig.AssemblySearchPaths.Any(
                    x => !string.Equals(x, Path.GetDirectoryName(assemblyPath), StringComparison.OrdinalIgnoreCase)))
                NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(assemblyPath));

            return asm;
        }

        /// <summary>
        /// Simply wraps the call to <see cref="Assembly.GetTypes"/> with a try-catch
        /// that adds logging.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type[] NfGetTypes(this Assembly assembly, bool rethrow = true, string logFile = "")
        {
            if (assembly == null)
                return null;
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Wrapper around static GetAssemblyName with logging and rethrow options
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="rethrow"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public static AssemblyName GetAssemblyName(string assemblyFile, bool rethrow = true, string logFile = "")
        {
            try
            {
                return AssemblyName.GetAssemblyName(assemblyFile);
            }
            catch (ArgumentException argEx)
            {
                AddLoaderExceptionToLog(null, argEx, logFile);
                if (rethrow)
                    throw;
            }
            catch (IOException ioEx)
            {
                AddLoaderExceptionToLog(null, ioEx, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException badEx)
            {
                AddLoaderExceptionToLog(null, badEx, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Simply wraps the call to <see cref="Assembly.GetExportedTypes"/> with a try-catch
        /// that adds logging.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type[] NfGetExportedTypes(this Assembly assembly, bool rethrow = true, string logFile = "")
        {
            if (assembly == null)
                return null;
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Assembly.GetType(string)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfGetType(this Assembly assembly, string typeFullName, bool rethrow = true, string logFile = "")
        {
            if (assembly == null || String.IsNullOrWhiteSpace(typeFullName))
                return null;
            try
            {
                return assembly.GetType(typeFullName);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if(rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethod(name);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String, BindingFlags)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, BindingFlags bindingAttr, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethod(name, bindingAttr);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;            
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String, Type[])"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, Type[] types, bool rethrow = true,
            string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethod(name, types);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;             
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String,BindingFlags,Binder,Type[],ParameterModifier[])"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="binder"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, BindingFlags bindingAttr, Binder binder,
            Type[] types, ParameterModifier[] modifiers, bool rethrow = true,
            string logFile = "")
        {
            if (t == null || types == null)
                return null;
            try
            {
                return t.GetMethod(name, bindingAttr, binder, types, modifiers);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethods(BindingFlags)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo[] NfGetMethods(this Type t, BindingFlags bindingAttr, bool rethrow = true,
            string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethods(bindingAttr);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;             
        }

        /// <summary>
        /// Wraps the call to <see cref="PropertyInfo.PropertyType"/> adding logging 
        /// of typical loader exceptions - the exception is still thrown.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfPropertyType(this PropertyInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;
            try
            {
                return pi.PropertyType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="FieldInfo.FieldType"/> adding logging 
        /// of typical loader exceptions - the exception is still thrown.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfFieldType(this FieldInfo fi, bool rethrow = true, string logFile = "")
        {
            if (fi == null)
                return null;

            try
            {
                return fi.FieldType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="MethodInfo.ReturnType"/> adding logging 
        /// of typical loader exceptions - the exception is still thrown.
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfReturnType(this MethodInfo mi, bool rethrow = true, string logFile = "")
        {
            if (mi == null)
                return null;
            try
            {
                return mi.ReturnType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="ParameterInfo.ParameterType"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfParameterType(this ParameterInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;

            try
            {
                return pi.ParameterType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="EventInfo.EventHandlerType"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="ei"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfEventHandlerType(this EventInfo ei, bool rethrow = true, string logFile = "")
        {
            if (ei == null)
                return null;

            try
            {
                return ei.EventHandlerType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }


        /// <summary>
        /// Wraps the call to <see cref="Type.BaseType"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfBaseType(this Type t, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;

            try
            {
                return t.BaseType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="PropertyInfo.GetGetMethod()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetGetMethod(this PropertyInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;

            try
            {
                return pi.GetGetMethod();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="PropertyInfo.GetSetMethod()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetSetMethod(this PropertyInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;

            try
            {
                return pi.GetSetMethod();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetFields()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rethrow"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public static FieldInfo[] NfGetFields(this Type t, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;

            try
            {
                return t.GetFields();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;

        }

        /// <summary>
        /// Wraps the call to <see cref="MethodInfo.GetParameters()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="rethrow"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public static ParameterInfo[] NfGetParameters(this MethodInfo mi, bool rethrow = true, string logFile = "")
        {
            if (mi == null)
                return new ParameterInfo[]{};

            try
            {
                return mi.GetParameters();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fle);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), bife, logFile);
                if (rethrow)
                    throw;
            }
            catch (NullReferenceException nre)
            {
                //this seems to have something to do with a call to System.Signature.GetSignature
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), nre, logFile);
                if (rethrow)
                    throw;
            }

            return new ParameterInfo[] { };
        }

        /// <summary>
        /// Wraps a call to <see cref="Type.GetGenericArguments()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rethrow"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public static Type[] NfGetGenericArguments(this Type t, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return new Type[]{};

            try
            {
                return t.GetGenericArguments();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return new Type[]{};
        }

        /// <summary>
        /// Wraps call to <see cref="Type.GetMembers(BindingFlags)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="rethrow"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public static MemberInfo[] NfGetMembers(this Type t, BindingFlags bindingAttr, bool rethrow = true,
            string logFile = "")
        {
            if(t == null)
                return new MemberInfo[]{};
            try
            {
                return t.GetMembers(bindingAttr);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }
            return new MemberInfo[] { };
        }

        public static MethodInfo NfGetBaseDefinition(this MethodInfo mi, bool rethrow = true,
            string logFile = "")
        {
            if (mi == null)
                return null;
            try
            {
                return mi.GetBaseDefinition();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fle);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), bife, logFile);
                if (rethrow)
                    throw;
            }
            return null;
        }

        #endregion

        #region internal api

        /// <summary>
        /// Search from <see cref="directoryLocation"/> recursively for all files 
        /// ending in *.dll extension and checks that <see cref="rqstAsmFullName"/>
        /// equals the full assembly name of the given dll file.
        /// </summary>
        /// <param name="directoryLocation"></param>
        /// <param name="rqstAsmFullName"></param>
        /// <param name="foundOne"></param>
        /// <param name="reflectionOnly"></param>
        /// <returns></returns>
        internal static bool SearchDirectoriesForAssembly(string directoryLocation, string rqstAsmFullName,
            out Assembly foundOne, bool reflectionOnly = true)
        {
            foundOne = null;
            if (String.IsNullOrWhiteSpace(directoryLocation) || !Directory.Exists(directoryLocation))
                return false;
            //this helps - a little
            var logDir = ResolveAsmLog;

            File.AppendAllText(logDir,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] Searching for '{rqstAsmFullName}' " +
                $"in '{directoryLocation}' \n");

            var di = new DirectoryInfo(directoryLocation);

            //search all the files down from the requesting assembly's location
            foreach (var d in di.EnumerateFileSystemInfos("*.dll", SearchOption.AllDirectories))
            {
                var asmName = AssemblyName.GetAssemblyName(d.FullName);
                if (asmName == null)
                    continue;

                if (!AssemblyName.ReferenceMatchesDefinition(asmName, new AssemblyName(rqstAsmFullName)))
                    continue;
                foundOne = reflectionOnly
                    ? Assembly.ReflectionOnlyLoad(File.ReadAllBytes(d.FullName))
                    : Assembly.Load(File.ReadAllBytes(d.FullName));
                return true;
            }

            return false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static StringBuilder GetExceptionStrBldr(Tuple<Type, MemberInfo> attemptedResolveType, Exception ex)
        {
            var missingTypeMsg = new StringBuilder();
            if (ex == null)
                return missingTypeMsg;

            missingTypeMsg.AppendFormat("Exception Name: '{0}'", ex.GetType().Name);
            missingTypeMsg.AppendLine();
            missingTypeMsg.Append(ex.Message);
            missingTypeMsg.AppendLine();
            if (!String.IsNullOrWhiteSpace(ex.StackTrace))
            {
                missingTypeMsg.Append(ex.StackTrace);
                missingTypeMsg.AppendLine();
            }

            if (attemptedResolveType == null)
                return missingTypeMsg;

            missingTypeMsg.Append("The following type could not be resolved.");
            missingTypeMsg.AppendLine();
            if (attemptedResolveType.Item1 != null)
            {
                missingTypeMsg.AppendFormat("Type's full name: '{0}'", attemptedResolveType.Item1.FullName);
                missingTypeMsg.AppendLine();
            }
            if (attemptedResolveType.Item2 != null)
            {
                missingTypeMsg.AppendFormat("Type's particular member's name: '{0}'", attemptedResolveType.Item2.Name);
                missingTypeMsg.AppendLine();
            }

            return missingTypeMsg;
        }
 

        #endregion
    }
}
