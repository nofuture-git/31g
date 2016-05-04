using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NoFuture.Shared;
using NoFuture.Util.Binary;
using NoFuture.Util.Gia.Args;

namespace NoFuture.Util.Gia
{
    public class Flatten
    {
        /// <summary>
        /// Flattens a type and breaks each word on Pascel or camel-case
        /// then gets a count of that word.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <param name="flattenMaxDepth"></param>
        /// <returns></returns>
        /// <remarks>
        /// This is useful for discovering the nomenclature specific to an assembly.
        /// </remarks>
        public static Dictionary<string, int> GetAllPropertyWholeWordsByCount(Assembly assembly, string typeFullName, int flattenMaxDepth)
        {
            var flattenTypeArg = new FlattenTypeArgs
            {
                Assembly = assembly,
                Depth = flattenMaxDepth,
                Separator = FlattenLineArgs.DEFAULT_SEPARATOR,
                TypeFullName = typeFullName
            };
            var flattenedType = (Flatten.FlattenType(flattenTypeArg)).PrintLines();
            if (flattenedType == null)
                return null;
            var allWords = new List<string>();

            foreach (var v in flattenedType)
            {
                if (!v.Contains(" "))
                    continue;
                var parts = v.Split((char)0x20);
                if (parts.Length < 2)
                    continue;
                var line = parts[1];
                if (String.IsNullOrWhiteSpace(line))
                    continue;

                var newline = Etc.TransformCamelCaseToSeparator(line, flattenTypeArg.Separator.ToCharArray()[0]);
                allWords.AddRange(newline.Split(flattenTypeArg.Separator.ToCharArray()[0]));
            }

            var allWholeWords = allWords.Distinct().ToList();
            var wordsByCount = new Dictionary<string, int>();

            foreach (var word in allWholeWords)
            {
                var wordCount = allWords.Count(x => x == word);
                if (wordCount <= 0)
                    continue;
                if (wordsByCount.ContainsKey(word))
                    wordsByCount[word] += wordCount;
                else
                    wordsByCount.Add(word, wordCount);
            }

            return wordsByCount;
        }

        /// <summary>
        /// Same as its counterpart <see cref="GetAllPropertyWholeWordsByCount"/> except doing so on every property on 
        /// every type within the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="flattenMaxDepth"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetAssemblyPropertyWholeWordsByCount(Assembly assembly,
            int flattenMaxDepth)
        {
            var startDictionary = new Dictionary<string, int>();
            foreach (var t in assembly.NfGetTypes())
            {
                var tPropWords = GetAllPropertyWholeWordsByCount(assembly, t.FullName, flattenMaxDepth);
                if (tPropWords == null)
                    continue;
                foreach (var k in tPropWords.Keys)
                {
                    if (startDictionary.ContainsKey(k))
                        startDictionary[k] += tPropWords[k];
                    else
                    {
                        startDictionary.Add(k, tPropWords[k]);
                    }
                }
            }
            return startDictionary;
        }

        /// <summary>
        /// Dumps an entire assembly into a list of <see cref="FlattenedLine"/>
        /// </summary>
        /// <param name="fla"></param>
        /// <returns></returns>
        public static List<FlattenedLine> FlattenAssemblyToLines(FlattenLineArgs fla)
        {
            if (fla == null)
                throw new ArgumentNullException("fla");
            if (fla.Assembly == null)
                throw new ArgumentException("The Assembly reference must be passed in " +
                                            "with the FlattenLineArgs");

            var allTypeNames = fla.Assembly.NfGetTypes().Select(x => x.FullName).ToList();
            var allLines = new List<FlattenedLine>();
            foreach (var t in allTypeNames)
            {
                var flattenArgs = new FlattenTypeArgs
                {
                    Assembly = fla.Assembly,
                    TypeFullName = t,
                    Depth = fla.Depth,
                    Separator = fla.Separator,
                    UseTypeNames = fla.UseTypeNames,
                    LimitOnThisType = fla.LimitOnThisType
                };
                var flattenedType = Flatten.FlattenType(flattenArgs);
                foreach (var line in flattenedType.Lines.Where(x => !String.IsNullOrWhiteSpace(x.ValueType)))
                {
                    //if there is a limit on some type and this line is that type in any form then continue
                    if (!String.IsNullOrWhiteSpace(fla.LimitOnThisType) &&
                        !String.Equals(fla.LimitOnThisType, line.ValueType, StringComparison.OrdinalIgnoreCase) &&
                        !String.Equals(fla.LimitOnThisType,
                            NfTypeName.GetLastTypeNameFromArrayAndGeneric(line.ValueType),
                            StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (allLines.Any(x => x.Equals(line)))
                        continue;

                    allLines.Add(line);
                }
            }
            return allLines;
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
            var results = FlattenType(assembly, typeFulleName, ref startCount, maxDepth, fta.LimitOnThisType, fta.DisplayEnums, null, null);
            return new FlattenedType
            {
                Lines = results,
                UseTypeNames = fta.UseTypeNames,
                RootType = assembly.GetType(typeFulleName),
                Separator = fta.Separator
            };
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static List<FlattenedLine> FlattenType(Assembly assembly, string typeFullName,
            ref int currentDepth, int maxDepth, string limitOnValueType, bool displayEnums, Stack<FlattenedItem> fiValueTypes, Stack typeStack)
        {
            var printList = new List<FlattenedLine>();
            if (String.IsNullOrWhiteSpace(typeFullName))
                return printList;

            Func<PropertyInfo, string, bool> limitOnPi =
                (info, s) =>
                    String.IsNullOrWhiteSpace(s) ||
                    String.Equals(String.Format("{0}", info.PropertyType), s, StringComparison.OrdinalIgnoreCase) ||
                    String.Equals(NfTypeName.GetLastTypeNameFromArrayAndGeneric(info.PropertyType), s, StringComparison.OrdinalIgnoreCase) ||
                    (s == Constants.ENUM && NfTypeName.IsEnumType(info.PropertyType));

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
                fiValueTypes.Push(new FlattenedItem { FlName = NfTypeName.GetTypeNameWithoutNamespace(typeFullName), FlType = currentType });
            }

            var typeNamesList =
                currentType.GetProperties(Constants.DefaultFlags)
                    .Where(
                        x =>
                            (NfTypeName.IsValueTypeProperty(x) && limitOnPi(x, limitOnValueType) 
                             || (limitOnValueType == Constants.ENUM && limitOnPi(x, limitOnValueType))) //more limbo branching for enums
                            ) 
                    .Select(p => new Tuple<Type, string>(p.PropertyType, p.Name))
                    .ToList();


            foreach (var typeNamePair in typeNamesList)
            {
                var pVtype = typeNamePair.Item1;
                var pVname = typeNamePair.Item2;

                fiValueTypes.Push(new FlattenedItem { FlName = pVname, FlType = pVtype });
                var fiItems = fiValueTypes.ToList();
                fiItems.Reverse();
                printList.Add(new FlattenedLine(fiItems.Distinct(new FlattenedItemComparer()).ToList())
                {
                    ValueType = String.Format("{0}", typeNamePair.Item1)
                });
                fiValueTypes.Pop();
            }

            //then recurse the object types
            foreach (var p in currentType.GetProperties(Constants.DefaultFlags).Where(x => !NfTypeName.IsValueTypeProperty(x)))
            {
                var typeIn = NfTypeName.GetLastTypeNameFromArrayAndGeneric(p.PropertyType);

                if (typeIn == null || typeStack.Contains(typeIn)) continue;

                var fi = new FlattenedItem { FlName = p.Name, FlType = p.PropertyType };
                if (fiValueTypes.ToList().Any(x => x.FlType == p.PropertyType))
                    continue;

                fiValueTypes.Push(fi);
                typeStack.Push(typeIn);
                currentDepth += 1;

                //time to go
                if (currentDepth >= maxDepth)
                    return printList;

                //enum types being handled as limbo between value type and ref type
                string[] enumVals;
                if (displayEnums && NfTypeName.IsEnumType(p.PropertyType, out enumVals))
                {
                    foreach (var ev in enumVals)
                    {
                        fiValueTypes.Push(new FlattenedItem{FlName = ev, FlType = typeof(Enum)});
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
    }

}