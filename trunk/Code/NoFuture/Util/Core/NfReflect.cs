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

        #region Static Utility Methods

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
        /// Returns everything except the last entry after <see cref="NfConfig.DefaultTypeSeparator"/>
        /// or in the case where <see cref="NfConfig.DefaultTypeSeparator"/> isn't present -
        /// just returns <see cref="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNamespaceWithoutTypeName(string name)
        {
            const string ROOT_NS = "global::";

            if (String.IsNullOrWhiteSpace(name))
                return null;

            if (!name.Contains(NfConfig.DefaultTypeSeparator))
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

            var nameArray = name.Split(NfConfig.DefaultTypeSeparator);
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
                    ns.Append(NfConfig.DefaultTypeSeparator);
            }
            return ns.ToString();
        }

        /// <summary>
        /// Gets a type's name less the namespace
        /// </summary>
        /// <param name="simplePropType"></param>
        /// <returns></returns>
        public static string GetTypeNameWithoutNamespace(string simplePropType)
        {
            if (String.IsNullOrWhiteSpace(simplePropType))
                return null;

            if (!simplePropType.Contains(NfConfig.DefaultTypeSeparator))
                return simplePropType;

            if (simplePropType.Contains(Constants.TYPE_METHOD_NAME_SPLIT_ON))
            {
                simplePropType = simplePropType.Substring(0,
                    simplePropType.IndexOf(Constants.TYPE_METHOD_NAME_SPLIT_ON, StringComparison.Ordinal));
            }

            return Etc.ExtractLastWholeWord(simplePropType, NfConfig.DefaultTypeSeparator);
        }

        /// <summary>
        /// Finds the typename without array qualifiers '[]' and without 
        /// the IL generic qualifier '`['
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static string GetLastTypeNameFromArrayAndGeneric(Type returnType)
        {
            return returnType == null ? null : GetLastTypeNameFromArrayAndGeneric(returnType.ToString());
        }

        /// <summary>
        /// Finds the first typename without array qualifiers '[]' and without 
        /// the IL generic qualifier '`['
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
        /// contain '=' nor '{'
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsIgnoreType(string typeName)
        {
            return typeName.Contains("=") || typeName.Contains("{");
        }

        /// <summary>
        /// Simply asserts the <see cref="type"/> does not 
        /// contain '=' nor '{'
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsIgnoreType(Type type)
        {
            return type != null && IsIgnoreType(type.Name);
        }

        /// <summary>
        /// Simply asserts that <see cref="returnType"/>
        /// either contains the array declarator '[]' or 
        /// contains the namespace name 'System.Collections.Generic' at the 
        /// same time as having simply the '[' somewhere.
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        /// <remarks>
        /// While it seems that one should inspect the implmentaion stack 
        /// for the type for IEnumerable - this actually tested out better.
        /// </remarks>
        public static bool IsEnumerableReturnType(Type returnType)
        {
            return returnType != null && IsEnumerableReturnType(returnType.ToString());
        }

        /// <summary>
        /// Simply asserts that <see cref="returnTypeToString"/>
        /// either contains the array declarator '[]' or 
        /// contains the namespace name 'System.Collections.Generic' at the 
        /// same time as having simply the '[' somewhere.
        /// </summary>
        /// <param name="returnTypeToString"></param>
        /// <returns></returns>
        /// <remarks>
        /// While it seems that one should inspect the implmentaion stack 
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
        /// Asserts that <see cref="type"/> doesn't contain
        /// the greater-than char nor the open curly brace char
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsClrGeneratedType(Type type)
        {
            return type != null && IsClrGeneratedType(type.ToString());
        }

        /// <summary>
        /// Asserts that <see cref="typeToString"/> doesn't contain
        /// the greater-than char nor the open curly brace char
        /// </summary>
        /// <param name="typeToString"></param>
        /// <returns></returns>
        public static bool IsClrGeneratedType(string typeToString)
        {
            return typeToString != null && (typeToString.Contains("<") || typeToString.Contains("{"));
        }

        /// <summary>
        /// Asserts that <see cref="type"/> doesn't match 
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
        /// method names for properties (e.g. MyProp {get;set;} becomes 'get_MyProp()' 
        /// and 'set_MyProp(value)').
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="matchedOnOut">
        /// When matched, gets the part of the name after the compiler gen'ed prefix
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
        /// For the given <see cref="PropertyInfo"/> the corrosponding <see cref="MethodInfo"/>
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
            var allMethods = ty.GetMethods((NfConfig.DefaultFlags));

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

        #endregion

        #region duck typing assignment
        private const string STR_FN = "System.String";
        private const string ERROR_PREFIX = "[ERROR]";

        /// <summary>
        /// Reassignable flags for selecting properties
        /// </summary>
        public static BindingFlags DefaultFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                  BindingFlags.Public;

        private static readonly List<string[]> assignPiLog = new List<string[]>();

        /// <summary>
        /// Gets info from any errors which occured in <see cref="TryAssignProperties"/>
        /// </summary>
        public static string[] GetAssignPropertiesErrors(string delimiter = "\t")
        {
            return assignPiLog.Where(x => x != null && x.Any() && x.FirstOrDefault() == ERROR_PREFIX)
                .Select(x => String.Join(delimiter, x))
                .ToArray();
        }

        /// <summary>
        /// Gets the mapping data from the last call to <see cref="TryAssignProperties"/>
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="noHeaders"></param>
        /// <returns></returns>
        public static string[] GetAssignPropertiesData(string delimiter = "\t", bool noHeaders = false)
        {
            var data = new List<string>();
            if (!noHeaders)
                data.Add(String.Join(delimiter, "FromProperty", "ToProperty", "FromToPath", "Score"));
            var dataAdd = assignPiLog.Where(x => x != null && x.Any() && x.FirstOrDefault() != ERROR_PREFIX)
                .Select(x => String.Join(delimiter, x))
                .ToList();
            data.AddRange(dataAdd);
            return data.ToArray();
        }

        /// <summary>
        /// Attempts to assign all the property&apos;s values on <see cref="fromObj"/> to 
        /// some property on <see cref="toObj"/> based on the closest matching name.
        /// </summary>
        /// <param name="fromObj"></param>
        /// <param name="toObj"></param>
        /// <returns>1.0 when it was a perfect match</returns>
        /// <remarks>
        /// Use the <see cref="GetAssignPropertiesData"/> to see what it ended up choosing.
        /// </remarks>
        public static double TryAssignProperties(object fromObj, object toObj)
        {
            if (fromObj == null || toObj == null)
                return 0;
            var rScores = new List<Tuple<int, int>>();
            var prevActions = new Dictionary<string, int>();
            try
            {
                //remove any previous records.
                assignPiLog.Clear();

                //only fromObj's value-type props
                rScores.AddRange(TryAssignValueTypeProperties(fromObj, toObj, prevActions));

                //only fromObj's ref-type props
                foreach (var fromPi in fromObj.GetType().GetProperties(DefaultFlags))
                {
                    if (fromPi == null || !fromPi.CanRead)
                        continue;

                    if (NfReflect.IsValueTypeProperty(fromPi, true))
                    {
                        continue;
                    }

                    var fromPv = fromPi.GetValue(fromObj);
                    if (fromPv == null)
                    {
                        fromPv = Activator.CreateInstance(fromPi.PropertyType);
                        fromPi.SetValue(fromObj, fromPv);
                    }
                    rScores.AddRange(TryAssignValueTypeProperties(fromPv, toObj, prevActions, fromPi.Name));
                }

            }
            catch (Exception ex)
            {
                assignPiLog.Add(new[] {ERROR_PREFIX, ex.Message, ex.StackTrace });
                return 0;
            }
            var num = rScores.Select(x => (double)x.Item2).Sum();
            var den = rScores.Select(x => (double)x.Item1).Sum();
            var ratio = den == 0.0D ? 0.0D : num / den;
            return 1D - ratio;
        }

        /// <summary>
        /// Takes the values of the value-type-only-properties on <see cref="fromObj"/> and finds some
        /// property, no matter how deep-down in the obj graph, on <see cref="toObj"/> to assign that value to.
        /// </summary>
        /// <param name="fromObj"></param>
        /// <param name="toObj"></param>
        /// <param name="prevActions"></param>
        /// <param name="contextName"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static List<Tuple<int, int>> TryAssignValueTypeProperties(object fromObj, object toObj, Dictionary<string, int> prevActions, string contextName = null)
        {
            var df = new List<Tuple<int, int>> { new Tuple<int, int>(Int32.MaxValue, 0) };
            if (fromObj == null || toObj == null)
                return df;
            var rScores = new List<Tuple<int, int>>();
            try
            {
                var fromObjTypeName = fromObj.GetType().FullName;
                var fromObjPropertyNames = fromObj.GetType().GetProperties(DefaultFlags).Select(p => p.Name).ToArray();

                //if still nothing found - just leave
                if (!fromObjPropertyNames.Any())
                    return df;

                var toPis = toObj.GetType().GetProperties(DefaultFlags);
                prevActions = prevActions ?? new Dictionary<string, int>();

                foreach (var pn in fromObjPropertyNames)
                {
                    var fromPi = fromObj.GetType().GetProperty(pn);

                    if (fromPi == null || !fromPi.CanRead || !NfReflect.IsValueTypeProperty(fromPi, true))
                        continue;
                    //this will get us those closest on name only
                    var closestMatches = GetClosestMatch(fromPi, fromObj, toPis, toObj);
                    if (closestMatches == null || !Enumerable.Any<Tuple<Action, int, string, string>>(closestMatches))
                        continue;

                    //have to decide how to break a tie
                    if (closestMatches.Count > 1)
                    {
                        closestMatches = Enumerable.Where<Tuple<Action, int, string, string>>(closestMatches, x => x.Item3.Contains(pn)).ToList();
                    }
                    foreach (var cm in closestMatches)
                    {
                        //its possiable that two different pi names are both attempting to write to the exact same target pi in toObj
                        if (prevActions.ContainsKey(cm.Item3) && cm.Item2 >= prevActions[cm.Item3])
                        {
                            //we only want the one with the shortest distance
                            continue;
                        }

                        //exec the assignment on the target
                        cm.Item1();

                        //get this distance as a ratio to the possiable max distance
                        rScores.Add(new Tuple<int, int>(cm.Item2, pn.Length));

                        //add this to the log 
                        var logPn = !String.IsNullOrWhiteSpace(contextName) ? String.Join(".", contextName, pn) : pn;
                        var logPath = !String.IsNullOrWhiteSpace(contextName) ? String.Join("`", fromObjTypeName, cm.Item4) : cm.Item4;
                        assignPiLog.Add(new[] { logPn, cm.Item3, logPath, cm.Item2.ToString() });

                        prevActions.Add(cm.Item3, cm.Item2);

                    }
                }
            }
            catch (Exception ex)
            {
                assignPiLog.Add(new[] {ERROR_PREFIX, ex.Message, ex.StackTrace });
                return df;
            }

            //return average
            return rScores;
        }

        /// <summary>
        /// Get a list of action, string distance measurement, destination object path in dot-notation, and additional info for logging
        /// for the properties on <see cref="toObj"/> which have the shortest distanct from the <see cref="fromPi"/> by name.  
        /// The return value is a list incase of a tie in which the calling assembly must decide which ones to execute.
        /// </summary>
        /// <param name="fromPi">The property whose value is to be copied over somewhere onto <see cref="toObj"/></param>
        /// <param name="fromObj">The source object of <see cref="fromPi"/> which is able to resolve it to an actual value.</param>
        /// <param name="toPis">The list of possiable canidate properties on <see cref="toObj"/>.</param>
        /// <param name="toObj">The destination object which is receiving property assignment.</param>
        /// <param name="namePrefix">Optional, used internally with <see cref="minLen"/></param>
        /// <param name="minLen">
        /// Optional, some minimum string length in which any property name which is -le it will have its name prefixed (or suffixed if its a shorter distance)
        /// with <see cref="namePrefix"/>. The most common case being something like EntityId should match to Entity.Id.
        /// </param>
        /// <returns>
        /// The Action (Item1) on the returned results will perform the actual assignment of one property to another.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static List<Tuple<Action, int, string, string>> GetClosestMatch(PropertyInfo fromPi, object fromObj, PropertyInfo[] toPis, object toObj, string namePrefix = null, int minLen = 2)
        {

            if (fromPi == null || toPis == null || !toPis.Any())
                return new List<Tuple<Action, int, string, string>>();

            //we want to map a whole assignment expression to a distance on name
            var ops2Score = new List<Tuple<Action, int, string, string>>();

            Func<PropertyInfo, string, bool, string> getMeasureableName = (gmnPi, prefix, inReverse) =>
            {
                if (gmnPi.Name.Length <= minLen)
                {
                    return inReverse ? String.Join("", gmnPi.Name, prefix) : String.Join("", prefix, gmnPi.Name);
                }
                return gmnPi.Name;
            };

            foreach (var toPi in toPis)
            {
                if (NfReflect.IsValueTypeProperty(toPi, true))
                {
                    string toFromTns;
                    Action simpleAssignment = GetSimpleAssignment(toPi, toObj, fromPi, fromObj, out toFromTns);
                    if (String.IsNullOrWhiteSpace(namePrefix))
                    {
                        //for simple value types -to- value types where dest has a very short name 
                        namePrefix = toObj.GetType().Name;
                    }
                    var fromPiName = getMeasureableName(fromPi, namePrefix, false);
                    var cpiName = getMeasureableName(toPi, namePrefix, false);
                    var fromPiReverseName = getMeasureableName(fromPi, namePrefix, true);
                    var cpiReverseName = getMeasureableName(toPi, namePrefix, true);

                    var score = (int)Etc.LevenshteinDistance(fromPiName, cpiName);

                    //with short property names, prehaps a better score is when prefix is a suffix instead
                    if (fromPiName != fromPiReverseName || cpiName != cpiReverseName)
                    {
                        var revScore = (int)Etc.LevenshteinDistance(fromPiReverseName, cpiReverseName);
                        if (revScore < score)
                            score = revScore;
                    }

                    ops2Score.Add(new Tuple<Action, int, string, string>(simpleAssignment, score, toPi.Name, toFromTns));
                }
                else
                {
                    var toInnerPi = toPi.PropertyType.GetProperties(DefaultFlags);
                    if (!toInnerPi.Any())
                        continue;

                    var toInnerObj = toPi.GetValue(toObj) ?? Activator.CreateInstance(toPi.PropertyType);
                    var innerMeasurements = GetClosestMatch(fromPi, fromObj, toInnerPi, toInnerObj, toPi.Name, minLen);

                    if (!innerMeasurements.Any())
                        continue;

                    //these will by def, be the shortest distance
                    foreach (var innerM in innerMeasurements)
                    {
                        //we will chain-link these actions
                        Action complexAction = () =>
                        {
                            innerM.Item1();
                            if (toPi.GetValue(toObj) == null)
                                toPi.SetValue(toObj, toInnerObj);
                        };
                        var actionId = String.Join(".", toPi.Name, innerM.Item3);

                        ops2Score.Add(new Tuple<Action, int, string, string>(complexAction, innerM.Item2, actionId, String.Join("`", toPi.PropertyType.FullName, innerM.Item4)));
                    }
                }
            }

            var totalMin = ops2Score.Min(x => x.Item2);
            var closest = ops2Score.Where(x => x.Item2 == totalMin).ToList();

            return closest;
        }

        /// <summary>
        /// Performs the assignment of the <see cref="toPi"/> on the instance of <see cref="toObj"/> using 
        /// the value of <see cref="fromPi"/> on the instance of <see cref="fromObj"/> - attempting to 
        /// perform the needed casting of one kind of value type to another.
        /// </summary>
        /// <param name="toPi"></param>
        /// <param name="toObj"></param>
        /// <param name="fromPi"></param>
        /// <param name="fromObj"></param>
        /// <param name="logInfo"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static Action GetSimpleAssignment(PropertyInfo toPi, object toObj, PropertyInfo fromPi, object fromObj, out string logInfo)
        {
            Action noop = () => { };
            logInfo = null;
            if (toPi == null || toObj == null || fromPi == null || fromObj == null)
                return noop;

            //only deal in value types to value types
            if (!NfReflect.IsValueTypeProperty(toPi, true) || !NfReflect.IsValueTypeProperty(fromPi, true))
                return noop;

            //enums require alot of special handling especially when wrapped in Nullable`1[
            var cpiIsEnum = NfReflect.IsPropertyEnum(toPi);
            var fromPiIsEnum = NfReflect.IsPropertyEnum(fromPi);

            //get each pi's type
            var cpiType = cpiIsEnum ? NfReflect.GetEnumType(toPi) : NfReflect.GetPropertyValueType(toPi);
            var fromPiType = fromPiIsEnum ? NfReflect.GetEnumType(fromPi) : NfReflect.GetPropertyValueType(fromPi);

            //get each pi's type's full name
            var cpiTypeFullname = cpiType.FullName;
            var fromPiTypeFullname = fromPiType.FullName;

            logInfo = String.Join("->", fromPiTypeFullname, cpiTypeFullname);

            //easy assignment for same types
            if (cpiTypeFullname == fromPiTypeFullname)
            {
                return () => toPi.SetValue(toObj, fromPi.GetValue(fromObj));
            }

            //going from Enum to a string
            if (fromPiIsEnum && cpiTypeFullname == STR_FN)
            {
                //take enum value and get its name
                return () =>
                {
                    var enumName = Enum.GetName(fromPiType, fromPi.GetValue(fromObj));
                    if (enumName != null)
                    {
                        toPi.SetValue(toObj, enumName);
                    }
                };
            }

            //going from a string to an enum
            if (cpiIsEnum && fromPiTypeFullname == STR_FN)
            {
                return () =>
                {
                    var val = fromPi.GetValue(fromObj);
                    if (val != null && !String.IsNullOrWhiteSpace(val.ToString()) &&
                        Enum.GetNames(cpiType)
                            .Any(x => String.Equals(x, val.ToString(), StringComparison.OrdinalIgnoreCase)))
                    {
                        var enumVal = Enum.Parse(cpiType, val.ToString(), true);
                        toPi.SetValue(toObj, enumVal);
                    }
                };
            }

            //going from enum to enum
            if (fromPiIsEnum && cpiIsEnum)
            {
                //will this require any cast?
                return () =>
                {
                    toPi.SetValue(toObj, fromPi.GetValue(fromObj));
                };
            }

            //going from some value-type to a string
            if (cpiTypeFullname == STR_FN)
            {
                return () => toPi.SetValue(toObj, fromPi.GetValue(fromObj).ToString());
            }

            //going from something to value-type
            switch (cpiTypeFullname)
            {
                case "System.Byte":
                    return () =>
                    {
                        byte bout;
                        var bpiv = fromPi.GetValue(fromObj);
                        var byteString = bpiv == null ? "0" : bpiv.ToString();
                        if (Byte.TryParse(byteString, out bout))
                            toPi.SetValue(toObj, bout);
                    };
                case "System.Int16":
                    return () =>
                    {
                        short vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (Int16.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Int32":
                    return () =>
                    {
                        int vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (Int32.TryParse(vStr, out vout))
                        {
                            toPi.SetValue(toObj, vout);
                        }
                    };
                case "System.DateTime":
                    return () =>
                    {
                        DateTime vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (DateTime.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Decimal":
                    return () =>
                    {
                        decimal vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (Decimal.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Single":
                    return () =>
                    {
                        float vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (Single.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Double":
                    return () =>
                    {
                        double vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (Double.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
            }

            //default out to no operation
            return noop;
        }

        #endregion 
    }
}