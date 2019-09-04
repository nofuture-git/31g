using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Shared.Core;

namespace NoFuture.Util.Core
{
    public static class NfReflect
    {
        private static Dictionary<string, string> _valueType2Cs;

        /// <summary>
        /// Lexicon of .NET value types to the C# equivalent (e.g. System.Int32 = int)
        /// </summary>
        /// <remarks>
        /// Includes System.String
        /// </remarks>
        public static Dictionary<string, string> ValueType2Cs =>
            _valueType2Cs ?? (_valueType2Cs = new Dictionary<string, string>
            {
                {"System.Byte", "byte"},
                {"System.Int16", "short"},
                {"System.Int32", "int"},
                {"System.Int64", "long"},
                {"System.Double", "double"},
                {"System.Boolean", "bool"},
                {"System.Char", "char"},
                {"System.Decimal", "decimal"},
                {"System.Single", "float"},
                {"System.String", "string"},
                {"System.Void", "void"}
            });

        #region Constants

        public const string FOUR_PT_VERSION_NUMBER = @"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+";
        public const string ASM_VERSION_REGEX = @"\,\s*Version\=" + FOUR_PT_VERSION_NUMBER;
        public const string ASM_CULTURE_REGEX = @"\,\s*Culture\=[a-z_\-]+";
        public const string ASM_PRIV_TOKEN_REGEX = @"\,\s*PublicKeyToken\=(null|[a-f0-9]*)";
        public const string ASM_PROC_ARCH_REGEX = @"\,\s*ProcessorArchitecture\=(MSIL|Arm|Amd64|IA64|None|X86)";

        public const string FULL_ASSEMBLY_NAME_REGEX = @"([a-z0-9_\.]*?)\,\s*(Version\=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)\,\s*(Culture\=[a-z_\-]*?)\,\s*(PublicKeyToken\=(null|[a-f0-9]*))?(\,\s*(ProcessorArchitecture\=(MSIL|Arm|Amd64|IA64|None|X86)))?";
        public const string ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX = @"([a-z0-9_\.]*?)\,\s*" + FULL_ASSEMBLY_NAME_REGEX;
        public const string NAMESPACE_CLASS_NAME_REGEX = @"\W[a-zA-Z_][a-zA-Z0-9_\x3D\x2E\x20\x3A]+";
        public const string ID_REGEX = @"[_a-z][a-z0-9_]+?";

        public static class PropertyNamePrefix
        {
            public const string GET_PREFIX = "get_";
            public const string SET_PREFIX = "set_";
            public const string ADD_PREFIX = "add_";
            public const string REMOVE_PREFIX = "remove_";
        }

        #endregion

        /// <summary>
        /// Simply appends <see cref="outputNamespace"/> to basic assembly 
        /// qualifications (as would appear on a dll compiled directly by csc.exe)
        /// </summary>
        /// <param name="outputNamespace"></param>
        /// <returns></returns>
        public static string DraftCscExeAsmName(string outputNamespace)
        {
            return $"{outputNamespace}, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Simply appens .dll to <see cref="outputNamespace"/>
        /// </summary>
        /// <param name="outputNamespace"></param>
        /// <returns></returns>
        public static string DraftCscDllName(string outputNamespace)
        {
            outputNamespace = outputNamespace ?? Path.GetRandomFileName();
            return $"{outputNamespace}.dll";
        }

        /// <summary>
        /// Test the <see cref="name"/> matches <see cref="NfReflect.ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// A fully qualified type name is the namespace, type name and the full assembly name - that is 
        /// what this is testing for.
        /// </remarks>
        public static bool IsFullAssemblyQualTypeName(string name)
        {
            return !String.IsNullOrWhiteSpace(name) &&
                   Regex.IsMatch(name, NfReflect.ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX, RegexCatalog.MyRegexOptions);
        }

        /// <summary>
        /// Test the <see cref="name"/> matches <see cref="NfReflect.FULL_ASSEMBLY_NAME_REGEX"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// This is testing for an assembly name as it would appear in your current AppDomain.
        /// </remarks>
        public static bool IsAssemblyFullName(string name)
        {
            return !String.IsNullOrWhiteSpace(name) &&
                   Regex.IsMatch(name, NfReflect.FULL_ASSEMBLY_NAME_REGEX, RegexCatalog.MyRegexOptions);
        }

        /// <summary>
        /// Returns everything except the last entry after <see cref="NfSettings.DefaultTypeSeparator"/>
        /// or in the case where <see cref="NfSettings.DefaultTypeSeparator"/> isn&apos;t present -
        /// just returns <see cref="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNamespaceWithoutTypeName(string name)
        {
            const string ROOT_NS = "global::";

            if (String.IsNullOrWhiteSpace(name))
                return null;

            if (!name.Contains(NfSettings.DefaultTypeSeparator))
                return null;

            if (name.StartsWith(ROOT_NS))
            {
                name = name.Substring(ROOT_NS.Length);
            }

            string genericCount;
            if (RegexCatalog.IsRegexMatch(name, @"\x60([1-9])\x5B", out genericCount))
            {
                name = name.Substring(0, name.IndexOf("`", StringComparison.Ordinal));
            }

            if (name.Contains(Constants.TYPE_METHOD_NAME_SPLIT_ON))
            {
                name = name.Substring(0, name.IndexOf(Constants.TYPE_METHOD_NAME_SPLIT_ON, StringComparison.Ordinal));
            }

            var nameArray = name.Split(NfSettings.DefaultTypeSeparator);
            var nsLength = nameArray.Length - 1;
            if (nsLength < 0)
                return name;

            var ns = new StringBuilder();
            for (var i = 0; i < nsLength; i++)
            {
                var s = nameArray[i];
                if (String.IsNullOrWhiteSpace(s))
                    continue;
                ns.Append(s);
                //as long as its not the last entry
                if (i < nsLength - 1)
                    ns.Append(NfSettings.DefaultTypeSeparator);
            }
            return ns.ToString();
        }

        /// <summary>
        /// Gets a type&apos;s name less the namespace
        /// </summary>
        /// <param name="simplePropType"></param>
        /// <returns></returns>
        public static string GetTypeNameWithoutNamespace(string simplePropType)
        {
            if (String.IsNullOrWhiteSpace(simplePropType))
                return null;

            if (!simplePropType.Contains(NfSettings.DefaultTypeSeparator))
                return simplePropType;

            if (simplePropType.Contains(Constants.TYPE_METHOD_NAME_SPLIT_ON))
            {
                simplePropType = simplePropType.Substring(0,
                    simplePropType.IndexOf(Constants.TYPE_METHOD_NAME_SPLIT_ON, StringComparison.Ordinal));
            }

            return NfString.ExtractLastWholeWord(simplePropType, NfSettings.DefaultTypeSeparator);
        }

        /// <summary>
        /// Finds the typename without array qualifiers &quot;[]&quot; and without 
        /// the IL generic qualifier &quot;`[&quot;
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static string GetLastTypeNameFromArrayAndGeneric(Type returnType)
        {
            return returnType == null ? null : GetLastTypeNameFromArrayAndGeneric(returnType.ToString());
        }

        /// <summary>
        /// Finds the first typename without array qualifiers &quot;[]&quot; and without 
        /// the IL generic qualifier &quot;`[&quot;
        /// </summary>
        /// <param name="returnTypeToString"></param>
        /// <param name="typeDelimiter">Optional, defaults to IL style but may be assigned to the C# 0x3C</param>
        /// <returns></returns>
        public static string GetLastTypeNameFromArrayAndGeneric(string returnTypeToString, string typeDelimiter = "[")
        {
            if (String.IsNullOrWhiteSpace(returnTypeToString))
                return null;
            var r = returnTypeToString;
            return r.Contains("[]")
                ? r.Replace("[]", "")
                : Enumerable.FirstOrDefault<string>(GetTypeNamesFromGeneric(returnTypeToString, typeDelimiter));
        }

        //this only returns the list of the last inner-most generic (e.g. Tuple`3[List`1[System.String], System.String, Tuple`2[System.Int32, System.String]] -> System.Int32, System.String)

        internal static string[] GetTypeNamesFromGeneric(string returnTypeToString, string typeDelimiter = "[")
        {
            if (String.IsNullOrWhiteSpace(returnTypeToString))
                return null;
            if (String.IsNullOrWhiteSpace(typeDelimiter))
                typeDelimiter = "[";

            var td = typeDelimiter.ToCharArray()[0];
            var cd = ((byte)td) + 2;
            var dt = new String(new[] { (char)cd });

            var r = returnTypeToString;

            if ((r.Contains("Collections.Generic") || r.Contains("System.Nullable") || r.Contains("System.Tuple")) &&
                r.Contains(typeDelimiter))
                //generic types could themselves be generics
                return GetTypeNamesFromGeneric(r.Substring(r.IndexOf(td) + 1), typeDelimiter);

            return r.Replace(dt, String.Empty).Replace(typeDelimiter, String.Empty).Split(',').Select(x => x.Trim()).ToArray();

        }


        /// <summary>
        /// Test if <see cref="pi"/> type extends either <see cref="System.String"/>
        /// or <see cref="System.ValueType"/>
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="includingEnums"></param>
        /// <returns></returns>
        public static bool IsValueTypeProperty(PropertyInfo pi, bool includingEnums = false)
        {
            if (pi == null)
                return false;

            var isPiDirectVt = TestIsValueType(pi.PropertyType.FullName, includingEnums);
            var isPiBaseVt = pi.PropertyType.BaseType != null && TestIsValueType(pi.PropertyType.BaseType.FullName, includingEnums);

            return (isPiDirectVt || isPiBaseVt);
        }

        /// <summary>
        /// Test if <see cref="fi"/> type extends either <see cref="System.String"/>
        /// or <see cref="System.ValueType"/>
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="includingEnums"></param>
        /// <returns></returns>
        public static bool IsValueTypeField(FieldInfo fi, bool includingEnums = false)
        {
            if (fi == null)
                return false;
            if (fi.IsLiteral)
                return false;

            return TestIsValueType(fi.FieldType.FullName, includingEnums) ||
                   (fi.FieldType.BaseType != null && TestIsValueType(fi.FieldType.BaseType.FullName, includingEnums));

        }

        /// <summary>
        /// Determines if the given <see cref="pi"/> has a property type which is, in some capacity, 
        /// considered an Enum, being either a simple Enum or a nullable one.
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static bool IsPropertyEnum(PropertyInfo pi)
        {
            if (pi == null)
                return false;

            if (pi.PropertyType.FullName == Constants.ENUM)
                return true;
            if (pi.PropertyType.BaseType != null && pi.PropertyType.BaseType.FullName == Constants.ENUM)
                return true;
            if (pi.PropertyType.GenericTypeArguments.Any(x => x.BaseType != null && x.BaseType.FullName == Constants.ENUM))
                return true;

            return false;
        }

        /// <summary>
        /// When <see cref="pi"/> has a property type which is an Enum being either a typical one or a nullable 
        /// this will return the underlying type which actually extends System.Enum
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static Type GetEnumType(PropertyInfo pi)
        {
            if (pi == null)
                return null;
            if (!IsValueTypeProperty(pi))
                return pi.PropertyType;
            if (!IsPropertyEnum(pi))
                return pi.PropertyType;

            var enumType =
                pi.PropertyType.GenericTypeArguments.FirstOrDefault(
                    x => x.BaseType != null && x.BaseType.FullName == Constants.ENUM) ?? pi.PropertyType;
            return enumType;
        }

        /// <summary>
        /// Similar to the invoking PropertyType on <see cref="pi"/> 
        /// except for nullable value types so instead of getting
        /// &quot;System.Nullable`1[[System.Int32 ... &quot; it 
        /// returns just &quot;System.Int32&quot;
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static Type GetPropertyValueType(PropertyInfo pi)
        {
            if (pi == null)
                return null;
            if (!IsValueTypeProperty(pi))
                return pi.PropertyType;

            var piType =
                pi.PropertyType.GenericTypeArguments.FirstOrDefault(
                    x => ValueType2Cs.Keys.Any(v => x.FullName == v)) ??
                pi.PropertyType;
            return piType;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool TestIsValueType(string typeFullName, bool includingEnums = false)
        {
            if (ValueType2Cs.Keys.Contains(typeFullName))
                return true;
            var v = typeFullName == "System.String" || typeFullName == "System.ValueType";
            if (includingEnums)
                v = v || typeFullName == Constants.ENUM;
            return v;
        }

        /// <summary>
        /// Simply asserts the <see cref="typeName"/> does not 
        /// contain &quot;=&quot; nor &quot;{&quot;
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsIgnoreType(string typeName)
        {
            return typeName.Contains("=") || typeName.Contains("{");
        }

        /// <summary>
        /// Simply asserts the <see cref="type"/> does not 
        /// contain &quot;=&quot; nor &quot;{&quot;
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsIgnoreType(Type type)
        {
            return type != null && IsIgnoreType(type.Name);
        }

        /// <summary>
        /// Simply asserts that <see cref="returnType"/>
        /// either contains the array declarator &quot;[]&quot; or 
        /// contains the namespace name &quot;System.Collections.Generic&quot; at the 
        /// same time as having simply the &quot;[&quot; somewhere.
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        /// <remarks>
        /// While it seems that one should inspect the implementation stack 
        /// for the type for IEnumerable - this actually tested out better.
        /// </remarks>
        public static bool IsEnumerableReturnType(Type returnType)
        {
            return returnType != null && IsEnumerableReturnType(returnType.ToString());
        }

        /// <summary>
        /// Simply asserts that <see cref="returnTypeToString"/>
        /// either contains the array declarator &quot;[]&quot; or 
        /// contains the namespace name &quot;System.Collections.Generic&quot; at the 
        /// same time as having simply the &quot;[&quot; somewhere.
        /// </summary>
        /// <param name="returnTypeToString"></param>
        /// <returns></returns>
        /// <remarks>
        /// While it seems that one should inspect the implementation stack 
        /// for the type for IEnumerable - this actually tested out better.
        /// </remarks>
        public static bool IsEnumerableReturnType(string returnTypeToString)
        {
            if (String.IsNullOrWhiteSpace(returnTypeToString))
                return false;

            var r = returnTypeToString;
            if (r.Contains("[]"))
                return true;
            return r.Contains("Collections.Generic") && r.Contains("[");
        }

        /// <summary>
        /// Asserts that <see cref="type"/> doesn&apos;t contain
        /// the greater-than char nor the open curly brace char
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsClrGeneratedType(Type type)
        {
            return type != null && IsClrGeneratedType(type.ToString());
        }

        /// <summary>
        /// Asserts that <see cref="typeToString"/> doesn&apos;t contain
        /// the greater-than char nor the open curly brace char
        /// </summary>
        /// <param name="typeToString"></param>
        /// <returns></returns>
        public static bool IsClrGeneratedType(string typeToString)
        {
            return typeToString != null && (typeToString.Contains("<") || typeToString.Contains("{"));
        }

        /// <summary>
        /// Asserts that <see cref="type"/> doesn&apos;t match 
        /// a handful of patterns common to compile-time generated
        /// method names (e.g. MyProp {get;set;} becomes 'get_MyProp()' 
        /// and 'set_MyProp(value)')
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsClrMethodForProperty(Type type)
        {
            string nop;
            return type != null && IsClrMethodForProperty(type.Name, out nop);
        }

        /// <summary>
        /// Asserts if <see cref="memberName"/> is a match 
        /// a handful of patterns common to compile-time generated
        /// method names for properties (e.g. MyProp {get;set;} becomes &quot;get_MyProp()&quot; 
        /// and &quot;set_MyProp(value)&quot;).
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="matchedOnOut">
        /// When matched, gets the part of the name after the compiler gen&apos;ed prefix
        /// </param>
        /// <returns></returns>
        public static bool IsClrMethodForProperty(string memberName, out string matchedOnOut)
        {
            matchedOnOut = null;
            if (String.IsNullOrWhiteSpace(memberName))
                return false;
            const string GET_REGEX = "^" + NfReflect.PropertyNamePrefix.GET_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";
            const string SET_REGEX = "^" + NfReflect.PropertyNamePrefix.SET_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";
            const string ADD_REGEX = "^" + NfReflect.PropertyNamePrefix.ADD_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";
            const string REMOVE_REGEX = "^" + NfReflect.PropertyNamePrefix.REMOVE_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";

            var isMatch = Regex.IsMatch(memberName, GET_REGEX) ||
                          Regex.IsMatch(memberName, SET_REGEX) ||
                          Regex.IsMatch(memberName, ADD_REGEX) ||
                          Regex.IsMatch(memberName, REMOVE_REGEX);
            if (!isMatch)
                return false;

            var getPropRegex = new Regex(GET_REGEX);
            var setPropRegex = new Regex(SET_REGEX);
            var addPropRegex = new Regex(ADD_REGEX);
            var remPropRegex = new Regex(REMOVE_REGEX);

            if (getPropRegex.IsMatch(memberName))
            {
                var grp = getPropRegex.Matches(memberName)[0];
                if (grp.Groups.Count > 1 && grp.Groups[1].Success)
                    matchedOnOut = grp.Groups[1].Value;
                return true;
            }
            if (setPropRegex.IsMatch(memberName))
            {
                var grp = setPropRegex.Matches(memberName)[0];
                if (grp.Groups.Count > 1 && grp.Groups[1].Success)
                    matchedOnOut = grp.Groups[1].Value;
                return true;
            }
            if (addPropRegex.IsMatch(memberName))
            {
                var grp = addPropRegex.Matches(memberName)[0];
                if (grp.Groups.Count > 1 && grp.Groups[1].Success)
                    matchedOnOut = grp.Groups[1].Value;
                return true;
            }
            if (remPropRegex.IsMatch(memberName))
            {
                var grp = remPropRegex.Matches(memberName)[0];
                if (grp.Groups.Count > 1 && grp.Groups[1].Success)
                    matchedOnOut = grp.Groups[1].Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// For the given <see cref="PropertyInfo"/> the corresponding <see cref="MethodInfo"/>
        /// is returned being matched on name.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodsForProperty(PropertyInfo pi, Type ty)
        {
            if (pi == null || ty == null)
                return null;

            //get a MethodInfo by probable property name
            var allMethods = ty.GetMethods((NfSettings.DefaultFlags));

            var propName2Mi = new Dictionary<string, List<MethodInfo>>();

            foreach (var mi in allMethods)
            {
                string derivedPropName;
                if (!IsClrMethodForProperty(mi.Name, out derivedPropName)) continue;

                if (propName2Mi.ContainsKey(derivedPropName))
                {
                    propName2Mi[derivedPropName].Add(mi);
                }
                else
                {
                    propName2Mi.Add(derivedPropName, new List<MethodInfo> { mi });
                }
            }

            if (propName2Mi.Count <= 0 || !propName2Mi.ContainsKey(pi.Name))
                return null;

            return propName2Mi[pi.Name].Where(x => x != null).ToArray();
        }

        public static bool IsEnumType(Type type)
        {
            string[] discardedValues;
            return IsEnumType(type, out discardedValues);
        }

        public static bool IsEnumType(Type type, out string[] values)
        {
            values = null;
            if (type == null)
                return false;
            if ((type.FullName == Constants.ENUM) || type.BaseType != null && type.BaseType.FullName == Constants.ENUM)
            {
                values = type.GetFields().Where(x => x.Name != "value__").Select(x => x.Name).ToArray();
                return true;
            }
            if (type.IsGenericType && type.GetGenericArguments().Length == 1)
            {
                var onlyGeneric = type.GetGenericArguments().First();
                return IsEnumType(onlyGeneric, out values);
            }
            return false;
        }
    }
}