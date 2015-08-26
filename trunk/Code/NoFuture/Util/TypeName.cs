using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Shared;
using NoFuture.Util.Binary;

namespace NoFuture.Util
{
    /// <summary>
    /// A type and and a tool box of functions for operating and 
    /// proposing facts concerning assembly reflected types.
    /// These names are as they appear in IL and not one of 
    /// the particular .NET languages.
    /// </summary>
    public class TypeName
    {
        #region Fields
        private const RegexOptions OPTIONS = RegexOptions.IgnoreCase;
        private readonly Regex _assemblyQualifiedRegex;
        private readonly Regex _fullNameRegex;
        private readonly Regex _idRegex;
        private readonly Regex _assemblyNameRegex;
        private readonly Regex _procArchRegex;

        private readonly string _ctorString;
        private readonly string _version;
        private readonly string _assemblyName;
        private readonly string _typeFullName;
        private readonly string _culture;
        private readonly string _publicKeyToken;
        private readonly string _namespace;
        private readonly string _className;
        private readonly string _procArch;

        #endregion

        #region Constants

        public const string FULL_ASSEMBLY_NAME_REGEX = @"([a-z0-9_\.]*?)\,\s*(Version\=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)\,\s*(Culture\=[a-z_\-]*?)\,\s*(PublicKeyToken\=(null|[a-f0-9]*))(\,\s*(ProcessorArchitecture\=(MSIL|Arm|Amd64|IA64|None|X86)))?";
        public const string ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX = @"([a-z0-9_\.]*?)\,\s*" + FULL_ASSEMBLY_NAME_REGEX;
        public const string NAMESPACE_CLASS_NAME_REGEX = @"([a-z0-9_\.]*?)";
        public const string ID_REGEX = @"[_a-z][a-z0-9_]+?";
        public const char DEFAULT_TYPE_SEPARATOR = '.';
        public const string ASM_VERSION_REGEX = @"\,\s*Version\=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+";
        public const string ASM_CULTURE_REGEX = @"\,\s*Culture\=[a-z_\-]+";
        public const string ASM_PRIV_TOKEN_REGEX = @"\,\s*PublicKeyToken\=(null|[a-f0-9]*)";
        public const string ASM_PROC_ARCH_REGEX = @"\,\s*ProcessorArchitecture\=(MSIL|Arm|Amd64|IA64|None|X86)";
        public const string DEFAULT_NAME_PREFIX = "_u0000";
        #endregion

        #region ReadOnly Properties

        /// <summary>
        /// This is a fully qualified type name having the namespace,class name, assembly name
        /// e.g. ('NoFuture.MyDatabase.Dbo.AccountExecutives, NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null')
        /// </summary>
        public virtual string AssemblyQualifiedName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_typeFullName))
                    return String.Empty;
                var myparts = new[] {_typeFullName, _assemblyName, _version, _culture, _publicKeyToken,_procArch};
                return String.Join(", ", myparts.Where(n => !String.IsNullOrWhiteSpace(n)));
            }
        }

        /// <summary>
        /// This the namespace and class name
        /// e.g. ('NoFuture.MyDatabase.Dbo.AccountExecutives')
        /// </summary>
        public virtual string FullName
        {
            get { return String.Format("{0}", _typeFullName); }
        }

        /// <summary>
        /// This is just the namespace
        /// e.g. ('NoFuture.MyDatabase.Dbo')
        /// </summary>
        public virtual string Namespace
        {
            get { return String.Format("{0}", _namespace); }
        }

        /// <summary>
        /// This is just the class name, no namespace, no assembly
        /// e.g. ('AccountExecutives')
        /// </summary>
        public virtual string ClassName
        {
            get { return String.Format("{0}", _className); }
        }

        /// <summary>
        /// This is just the name part of an assembly's name, no Version, Culture nor PublicKeyToken
        /// e.g. ('NoFuture.MyDatabase') 
        /// </summary>
        public virtual string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
        }

        /// <summary>
        /// This is the typical fully qualified assembly name having the Version, Culture PublicKeyToken and 
        /// sometimes ProcessorArchitecture.
        /// e.g. ('log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL')
        /// </summary>
        public virtual string AssemblyFullName
        {
            get
            {
                var myparts = new[] { _assemblyName, _version, _culture, _publicKeyToken, _procArch };
                return String.Join(", ", myparts.Where(n => !String.IsNullOrWhiteSpace(n)));
            }
        }

        /// <summary>
        /// This is just the version part of an assembly name (both key and value)
        /// e.g. ('Version=0.0.0.0')
        /// </summary>
        public virtual string Version
        {
            get { return string.IsNullOrWhiteSpace(_version) ? "Version=0.0.0.0" : _version; }
        }

        /// <summary>
        /// This is just the culture part of an assembly name (both key and value)
        /// e.g. ('Culture=neutral')
        /// </summary>
        public virtual string Culture
        {
            get { return string.IsNullOrWhiteSpace(_culture) ? "Culture=neutral" : _culture; }
        }

        /// <summary>
        /// This is just the PublicKeyToken part of an assembly's name (both key and value)
        /// e.g. ('PublicKeyToken=669e0ddf0bb1aa2a')
        /// </summary>
        public virtual string PublicKeyToken
        {
            get { return string.IsNullOrWhiteSpace(_publicKeyToken) ? "PublicKeyToken=null" : _publicKeyToken; }
        }

        /// <summary>
        /// This is simply the <see cref="AssemblyName"/> with a .dll 
        /// extension added to the end.
        /// </summary>
        public virtual string AssemblyFileName
        {
            get { return DraftCscDllName(_assemblyName); }
        }

        /// <summary>
        /// This is the original string passed to the ctor.
        /// </summary>
        public string RawString { get { return String.Format("{0}", _ctorString); } }

        #endregion

        public TypeName(string name)
        {
            if(String.IsNullOrWhiteSpace(name))
                return;

            _ctorString = name;
            _assemblyQualifiedRegex = new Regex(ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX, OPTIONS);
            _assemblyNameRegex = new Regex(FULL_ASSEMBLY_NAME_REGEX, OPTIONS);
            _fullNameRegex = new Regex(NAMESPACE_CLASS_NAME_REGEX, OPTIONS);
            _idRegex = new Regex(ID_REGEX, OPTIONS);
            _procArchRegex = new Regex(ASM_PROC_ARCH_REGEX, OPTIONS);

            var myRegex = new Regex(ASM_VERSION_REGEX, OPTIONS);
            if (myRegex.IsMatch(name))
            {
                var groups = myRegex.Matches(name)[0];
                if (groups.Groups[0].Success)
                    _version = groups.Groups[0].Value;
            }
            myRegex = new Regex(ASM_CULTURE_REGEX, OPTIONS);
            if (myRegex.IsMatch(name))
            {
                var groups = myRegex.Matches(name)[0];
                if (groups.Groups[0].Success)
                    _culture = groups.Groups[0].Value;
            }
            myRegex = new Regex(ASM_PRIV_TOKEN_REGEX, OPTIONS);
            if (myRegex.IsMatch(name))
            {
                var groups = myRegex.Matches(name)[0];
                if (groups.Groups[0].Success)
                    _publicKeyToken = groups.Groups[0].Value;
            }
            myRegex = new Regex(ASM_PROC_ARCH_REGEX, OPTIONS);
            if (myRegex.IsMatch(name))
            {
                var groups = myRegex.Matches(name)[0];
                if (groups.Groups[0].Success)
                    _procArch = groups.Groups[0].Value;
            }
            Func<string, string> trimUp = s => s.Replace(",", String.Empty).Trim();
            
            var nameLessOthers = name;
            if (!String.IsNullOrWhiteSpace(_version))
            {
                nameLessOthers = nameLessOthers.Replace(_version, String.Empty);
                _version = trimUp(_version);
            }

            if (!String.IsNullOrWhiteSpace(_culture))
            {
                nameLessOthers = nameLessOthers.Replace(_culture, String.Empty);
                _culture = trimUp(_culture);
            }

            if (!String.IsNullOrWhiteSpace(_publicKeyToken))
            {
                nameLessOthers = nameLessOthers.Replace(_publicKeyToken, String.Empty);
                _publicKeyToken = trimUp(_publicKeyToken);
            }

            if (!String.IsNullOrWhiteSpace(_procArch))
            {
                nameLessOthers = nameLessOthers.Replace(_procArch, String.Empty);
                _procArch = trimUp(_procArch);
            }

            if (nameLessOthers.Contains(","))
            {
                var asmAndClassName = nameLessOthers.Split(',');
                _typeFullName = asmAndClassName[0].Trim();
                _assemblyName = asmAndClassName[1].Trim();
            }
            else
            {
                _typeFullName = String.Empty;
                _assemblyName = nameLessOthers;
            }

            if (String.IsNullOrWhiteSpace(_typeFullName))
            {
                _className = GetTypeNameWithoutNamespace(_ctorString);
                _namespace = GetNamespaceWithoutTypeName(_ctorString);
            }
            else
            {
                _className = Etc.ExtractLastWholeWord(_typeFullName, null);
                _namespace = GetNamespaceWithoutTypeName(_typeFullName);
            }

        }

        #region Static Utility Methods

        /// <summary>
        /// Simply appends <see cref="outputNamespace"/> to basic assembly 
        /// qualifications (as would appear on a dll compiled directly by csc.exe)
        /// </summary>
        /// <param name="outputNamespace"></param>
        /// <returns></returns>
        public static string DraftCscExeAsmName(string outputNamespace)
        {
            return String.Format("{0}, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", outputNamespace);
        }

        /// <summary>
        /// Simply appens .dll to <see cref="outputNamespace"/>
        /// </summary>
        /// <param name="outputNamespace"></param>
        /// <returns></returns>
        public static string DraftCscDllName(string outputNamespace)
        {
            return String.Format("{0}.dll", outputNamespace);
        }

        /// <summary>
        /// Test the <see cref="name"/> matches <see cref="ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// A fully qualified type name is the namespace, type name and the full assembly name - that is 
        /// what this is testing for.
        /// </remarks>
        public static bool IsFullAssemblyQualTypeName(string name)
        {
            return !String.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX, OPTIONS);
        }

        /// <summary>
        /// Test the <see cref="name"/> matches <see cref="FULL_ASSEMBLY_NAME_REGEX"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// This is testing for an assembly name as it would appear in your current AppDomain.
        /// </remarks>
        public static bool IsAssemblyFullName(string name)
        {
            return !String.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, FULL_ASSEMBLY_NAME_REGEX, OPTIONS);
        }

        /// <summary>
        /// Returns everything except the last entry after <see cref="DEFAULT_TYPE_SEPARATOR"/>
        /// or in the case where <see cref="DEFAULT_TYPE_SEPARATOR"/> isn't present -
        /// just returns <see cref="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNamespaceWithoutTypeName(string name)
        {
            if(String.IsNullOrWhiteSpace(name))
                return null;

            if(!name.Contains(DEFAULT_TYPE_SEPARATOR))
                return null;

            var nameArray = name.Split(DEFAULT_TYPE_SEPARATOR);
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
                    ns.Append(DEFAULT_TYPE_SEPARATOR);
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

            if (!simplePropType.Contains(DEFAULT_TYPE_SEPARATOR))
                return simplePropType;

            return Etc.ExtractLastWholeWord(simplePropType, DEFAULT_TYPE_SEPARATOR);
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
            if (r.Contains("[]"))
                return r.Replace("[]", "");
            return GetTypeNamesFromGeneric(returnTypeToString, typeDelimiter).FirstOrDefault();
        }

        //this only returns the list of the last inner-most generic (e.g. Tuple`3[List`1[System.String], System.String, Tuple`2[System.Int32, System.String]] -> System.Int32, System.String)
        internal static string[] GetTypeNamesFromGeneric(string returnTypeToString, string typeDelimiter = "[")
        {
            if (String.IsNullOrWhiteSpace(returnTypeToString))
                return null;
            if (string.IsNullOrWhiteSpace(typeDelimiter))
                typeDelimiter = "[";

            var td = typeDelimiter.ToCharArray()[0];
            var cd = ((byte)td) + 2;
            var dt = new String (new []{ (char)cd });

            var r = returnTypeToString;

            if ((r.Contains("Collections.Generic") || r.Contains("System.Nullable") || r.Contains("System.Tuple")) &&
                r.Contains(typeDelimiter))
                //generic types could themselves be generics
                return GetTypeNamesFromGeneric(r.Substring(r.IndexOf(td) + 1), typeDelimiter);

            return r.Replace(dt, string.Empty).Replace(typeDelimiter, string.Empty).Split(',').Select(x => x.Trim()).ToArray();
            
        }

        /// <summary>
        /// Uses <see cref="name"/> allowing for only '.', '_', numbers and letters where the 
        /// first char must be '_' or a letter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string SafeDotNetTypeName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return string.Format("{0}_{1}", DEFAULT_NAME_PREFIX, Path.GetRandomFileName().Replace(".", "_"));

            var nameArray = name.ToCharArray();
            var csId = new StringBuilder();

            if (!Char.IsLetter(nameArray[0]))
                csId.Append(DEFAULT_NAME_PREFIX);

            if (Char.IsNumber(nameArray[0]) || nameArray[0] == '_' || Char.IsLetter(nameArray[0]))
                csId.Append(nameArray[0]);

            for(var i = 1; i< nameArray.Length; i++)
            {
                char cChar = nameArray[i];
                if (Char.IsLetter(cChar) || Char.IsNumber(cChar) || cChar == '_' || cChar == '.')//expecting ns qualified ids
                    csId.Append(cChar);
            }

            return csId.ToString();
        }

        /// <summary>
        /// Uses <see cref="someString"/> allowing for only '_', numbers and letters where the 
        /// first char must be or a letter (the prefix 'nf' is used should that not be the case).
        /// </summary>
        /// <param name="someString"></param>
        /// <param name="replaceInvalidsWithHexEsc">
        /// Set this to true if you want, for example, the <see cref="someString"/> 
        /// with a value of say 'Personal Ph #' to be returned as 'Personalx20Phx20x23'
        /// </param>
        /// <returns></returns>
        public static string SafeDotNetIdentifier(string someString, bool replaceInvalidsWithHexEsc = false)
        {
            if (String.IsNullOrWhiteSpace(someString))
                return string.Format("{0}_{1}", DEFAULT_NAME_PREFIX, Path.GetRandomFileName().Replace(".","_"));
            var strChars = someString.ToCharArray();
            var strOut = new StringBuilder();
            if (!Char.IsLetter(strChars[0]))
                strOut.Append(DEFAULT_NAME_PREFIX);
            var iequals = 0;
            if (!replaceInvalidsWithHexEsc && (Char.IsNumber(strChars[0]) || strChars[0] == '_' || Char.IsLetter(strChars[0])))
            {
                strOut.Append(strChars[0]);
                iequals = 1;
            }

            for (var i = iequals; i < strChars.Length; i++)
            {
                if (Char.IsLetterOrDigit(strChars[i]) || (strChars[i] == '_' && !replaceInvalidsWithHexEsc))
                {
                    strOut.Append(strChars[i]);
                    continue;
                }
                    
                if (!replaceInvalidsWithHexEsc)
                    continue;

                strOut.Append(string.Format("_u{0}", Convert.ToUInt16(strChars[i]).ToString("x4")));
            }

            return strOut.ToString();
        }

        /// <summary>
        /// Removes the <see cref="Path.GetInvalidPathChars"/> from <see cref="outFile"/>
        /// </summary>
        /// <param name="outFile"></param>
        /// <returns></returns>
        public static string RemoveInvalidPathChars(string outFile)
        {
            if (String.IsNullOrWhiteSpace(outFile))
                return outFile;

            return Path.GetInvalidPathChars().Aggregate(outFile,
                (current, invalidChar) =>
                    current.Replace(invalidChar.ToString(CultureInfo.InvariantCulture), String.Empty));

        }

        /// <summary>
        /// Test if <see cref="pi"/> type extends either <see cref="System.String"/>
        /// or <see cref="System.ValueType"/>
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static bool IsValueTypeProperty(PropertyInfo pi)
        {
            if (pi == null)
                return false;

            return TestIsValueType(pi.PropertyType.FullName) ||
                   (pi.PropertyType.BaseType != null && TestIsValueType(pi.PropertyType.BaseType.FullName));
        }

        /// <summary>
        /// Test if <see cref="fi"/> type extends either <see cref="System.String"/>
        /// or <see cref="System.ValueType"/>
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static bool IsValueTypeField(FieldInfo fi)
        {
            if (fi == null)
                return false;
            if (fi.IsLiteral)
                return false;

            return TestIsValueType(fi.FieldType.FullName) ||
                   (fi.FieldType.BaseType != null && TestIsValueType(fi.FieldType.BaseType.FullName));

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool TestIsValueType(string typeFullName)
        {
            return typeFullName == "System.String" || typeFullName == "System.ValueType";
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
            const string GET_REGEX = "^get_([a-zA-Z_][a-zA-Z0-9_]*)";
            const string SET_REGEX = "^set_([a-zA-Z_][a-zA-Z0-9_]*)";
            const string ADD_REGEX = "^add_([a-zA-Z_][a-zA-Z0-9_]*)";
            const string REMOVE_REGEX = "^remove_([a-zA-Z_][a-zA-Z0-9_]*)";

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
        public static MethodInfo GetMethodsForProperty(PropertyInfo pi, Type ty)
        {
            if (pi == null || ty == null)
                return null;

            //get a MethodInfo by probable property name
            var allMethods = ty.GetMethods((BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic |
                                            BindingFlags.Public));

            var propName2Mi = new Dictionary<string, MethodInfo>();

            foreach (var mi in allMethods)
            {
                string derivedPropName;
                if (!IsClrMethodForProperty(mi.Name, out derivedPropName)) continue;

                if (propName2Mi.ContainsKey(derivedPropName))
                {
                    propName2Mi[derivedPropName] = mi;
                }
                else
                {
                    propName2Mi.Add(derivedPropName, mi);
                }
            }

            if (propName2Mi.Count <= 0 || !propName2Mi.ContainsKey(pi.Name))
                return null;

            return propName2Mi[pi.Name];
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
            if ((type.FullName == Constants.ENUM) || type.NfBaseType() != null && type.NfBaseType().FullName == Constants.ENUM)
            {
                values = type.NfGetFields().Where(x => x.Name != "value__").Select(x => x.Name).ToArray();
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
    }
}
