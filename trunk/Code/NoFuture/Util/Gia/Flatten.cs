using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NoFuture.Shared;
using NoFuture.Util.Binary;

namespace NoFuture.Util.Gia
{
    public class Flatten
    {
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
        public static FlattenedType FlattenType(Gia.Args.FlattenTypeArgs fta)
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
            if (string.IsNullOrWhiteSpace(typeFullName))
                return printList;

            Func<PropertyInfo, string, bool> limitOnPi =
                (info, s) =>
                    string.IsNullOrWhiteSpace(s) ||
                    string.Equals(string.Format("{0}", info.PropertyType), s, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(TypeName.GetLastTypeNameFromArrayAndGeneric(info.PropertyType), s, StringComparison.OrdinalIgnoreCase) ||
                    (s == Constants.ENUM && TypeName.IsEnumType(info.PropertyType));

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
                fiValueTypes.Push(new FlattenedItem { FlName = TypeName.GetTypeNameWithoutNamespace(typeFullName), FlType = currentType });
            }

            var typeNamesList =
                currentType.GetProperties()
                    .Where(
                        x =>
                            (TypeName.IsValueTypeProperty(x) && limitOnPi(x, limitOnValueType) 
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
                    ValueType = string.Format("{0}", typeNamePair.Item1)
                });
                fiValueTypes.Pop();
            }

            //then recurse the object types
            foreach (var p in currentType.GetProperties().Where(x => !TypeName.IsValueTypeProperty(x)))
            {
                var typeIn = TypeName.GetLastTypeNameFromArrayAndGeneric(p.PropertyType);

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
                if (displayEnums && TypeName.IsEnumType(p.PropertyType, out enumVals))
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