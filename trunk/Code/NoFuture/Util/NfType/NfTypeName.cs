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
using NoFuture.Util.NfType.InvokeCmds;

namespace NoFuture.Util.NfType
{
    /// <summary>
    /// A type and and a tool box of functions for operating and 
    /// proposing facts concerning assembly reflected types.
    /// These names are as they appear in IL and not one of 
    /// the particular .NET languages.
    /// </summary>
    public class NfTypeName
    {
        #region Fields
        private readonly string _ctorString;
        private readonly string _publicKeyToken;
        private readonly string _namespace;
        private readonly string _className;
        private readonly AssemblyName _asmName;
        private readonly List<NfTypeName> _genericArgs = new List<NfTypeName>();
        private static NfTypeNameProcess _myProcess;
        #endregion

        #region Constants
        public const string FOUR_PT_VERSION_NUMBER = @"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+";
        public const string ASM_VERSION_REGEX = @"\,\s*Version\=" + FOUR_PT_VERSION_NUMBER;
        public const string ASM_CULTURE_REGEX = @"\,\s*Culture\=[a-z_\-]+";
        public const string ASM_PRIV_TOKEN_REGEX = @"\,\s*PublicKeyToken\=(null|[a-f0-9]*)";
        public const string ASM_PROC_ARCH_REGEX = @"\,\s*ProcessorArchitecture\=(MSIL|Arm|Amd64|IA64|None|X86)";

        public const string FULL_ASSEMBLY_NAME_REGEX = @"([a-z0-9_\.]*?)\,\s*(Version\=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)\,\s*(Culture\=[a-z_\-]*?)\,\s*(PublicKeyToken\=(null|[a-f0-9]*))(\,\s*(ProcessorArchitecture\=(MSIL|Arm|Amd64|IA64|None|X86)))?";
        public const string ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX = @"([a-z0-9_\.]*?)\,\s*" + FULL_ASSEMBLY_NAME_REGEX;
        public const string NAMESPACE_CLASS_NAME_REGEX = @"\W[a-zA-Z_][a-zA-Z0-9_\x3D\x2E\x20\x3A]+";
        public const string ID_REGEX = @"[_a-z][a-z0-9_]+?";
        public const string DEFAULT_NAME_PREFIX = "_u0000";

        public static class PropertyNamePrefix
        {
            public const string GET_PREFIX = "get_";
            public const string SET_PREFIX = "set_";
            public const string ADD_PREFIX = "add_";
            public const string REMOVE_PREFIX = "remove_";
        }

        public static string DefaultNamePrefix { get; set; } = DEFAULT_NAME_PREFIX;

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
                if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(_asmName?.FullName))
                    return null;
                var myparts = new[] {FullName, _asmName?.FullName};
                return string.Join(", ", myparts.Where(n => !string.IsNullOrWhiteSpace(n)));
            }
        }

        /// <summary>
        /// This the namespace and class name
        /// e.g. ('NoFuture.MyDatabase.Dbo.AccountExecutives')
        /// </summary>
        public virtual string FullName
            => string.Join(".", new[] { Namespace, ClassName}.Where(x => !string.IsNullOrWhiteSpace(x)));

        /// <summary>
        /// This is just the namespace
        /// e.g. ('NoFuture.MyDatabase.Dbo')
        /// </summary>
        public virtual string Namespace => $"{_namespace}";

        /// <summary>
        /// This is just the class name, no namespace, no assembly
        /// e.g. ('AccountExecutives')
        /// </summary>
        public virtual string ClassName => $"{_className}";

        /// <summary>
        /// This is just the name part of an assembly's name, no Version, Culture nor PublicKeyToken
        /// e.g. ('NoFuture.MyDatabase') 
        /// </summary>
        public virtual string AssemblySimpleName => _asmName?.Name ?? FullName;

        /// <summary>
        /// This is the typical fully qualified assembly name having the Version, Culture PublicKeyToken and 
        /// sometimes ProcessorArchitecture.
        /// e.g. ('log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL')
        /// </summary>
        public virtual string AssemblyFullName => _asmName?.ToString();

        /// <summary>
        /// This is just the version part of an assembly name (both key and value)
        /// e.g. ('Version=0.0.0.0')
        /// </summary>
        public virtual string Version => _asmName?.Version == null ? "Version=0.0.0.0" :  $"Version={_asmName.Version}";

        /// <summary>
        /// This is just the culture part of an assembly name (both key and value)
        /// e.g. ('Culture=neutral')
        /// </summary>
        public virtual string Culture => string.IsNullOrWhiteSpace(_asmName?.CultureName) ? "Culture=neutral" : $"Culture={_asmName.CultureName}";

        /// <summary>
        /// This is just the PublicKeyToken part of an assembly's name (both key and value)
        /// e.g. ('PublicKeyToken=669e0ddf0bb1aa2a')
        /// </summary>
        public virtual string PublicKeyToken => string.IsNullOrWhiteSpace(_publicKeyToken) ? "PublicKeyToken=null" : $"PublicKeyToken={_publicKeyToken}";

        /// <summary>
        /// This is simply the <see cref="AssemblySimpleName"/> with a .dll 
        /// extension added to the end.
        /// </summary>
        public virtual string AssemblyFileName => DraftCscDllName(AssemblySimpleName);

        /// <summary>
        /// This is the original string passed to the ctor.
        /// </summary>
        public string RawString => $"{_ctorString}";

        /// <summary>
        /// Gets the version number as a strongly type .NET <see cref="Version"/>
        /// </summary>
        public Version VersionValue => _asmName?.Version;

        /// <summary>
        /// Gets just the value of the <see cref="PublicKeyToken"/>
        /// </summary>
        public string PublicKeyTokenValue => _publicKeyToken ?? "null";

        public IEnumerable<NfTypeName> GenericArgs => _genericArgs;

        #endregion

        public NfTypeName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                return;

            _ctorString = name;

            if(_myProcess == null || !_myProcess.IsMyProcessRunning)
                _myProcess = new NfTypeNameProcess(null);

            var parseItem = _myProcess.GetNfTypeName(name);
            if (parseItem == null)
                return;
            _className = GetTypeNameWithoutNamespace(parseItem.FullName);
            _namespace = GetNamespaceWithoutTypeName(parseItem.FullName);
            _publicKeyToken = parseItem.PublicKeyTokenValue;
            if(!string.IsNullOrWhiteSpace(parseItem.AssemblyFullName))
                _asmName = new AssemblyName(parseItem.AssemblyFullName);

            if (parseItem.GenericItems == null || !parseItem.GenericItems.Any())
                return;

            foreach (var gi in parseItem.GenericItems)
            {
                _genericArgs.Add(new NfTypeName(gi));
            }

            //string valOut;
            //if(RegexCatalog.IsRegexMatch(name, ASM_VERSION_REGEX, out valOut))
            //    _version = valOut;

            //if (RegexCatalog.IsRegexMatch(name, ASM_CULTURE_REGEX, out valOut))
            //    _culture = valOut;

            //if (RegexCatalog.IsRegexMatch(name, ASM_PRIV_TOKEN_REGEX, out valOut))
            //    _publicKeyToken = valOut;

            //if (RegexCatalog.IsRegexMatch(name, ASM_PROC_ARCH_REGEX, out valOut))
            //    _procArch = valOut;

            //Func<string, string> trimUp = s => s.Replace(",", String.Empty).Trim();

            //var nameLessOthers = name;
            //if (!String.IsNullOrWhiteSpace(_version))
            //{
            //    nameLessOthers = nameLessOthers.Replace(_version, String.Empty);
            //    _version = trimUp(_version);
            //}

            //if (!String.IsNullOrWhiteSpace(_culture))
            //{
            //    nameLessOthers = nameLessOthers.Replace(_culture, String.Empty);
            //    _culture = trimUp(_culture);
            //}

            //if (!String.IsNullOrWhiteSpace(_publicKeyToken))
            //{
            //    nameLessOthers = nameLessOthers.Replace(_publicKeyToken, String.Empty);
            //    _publicKeyToken = trimUp(_publicKeyToken);
            //}

            //if (!String.IsNullOrWhiteSpace(_procArch))
            //{
            //    nameLessOthers = nameLessOthers.Replace(_procArch, String.Empty);
            //    _procArch = trimUp(_procArch);
            //}

            //if (nameLessOthers.Contains(","))
            //{
            //    var asmAndClassName = nameLessOthers.Split(',');
            //    _typeFullName = asmAndClassName[0].Trim();
            //    _assemblyName = asmAndClassName[1].Trim();
            //}
            //else
            //{
            //    _typeFullName = String.Empty;
            //    _assemblyName = nameLessOthers;
            //}

            //if (String.IsNullOrWhiteSpace(_typeFullName))
            //{
            //    _className = GetTypeNameWithoutNamespace(nameLessOthers);
            //    _namespace = GetNamespaceWithoutTypeName(nameLessOthers);
            //}
            //else
            //{
            //    _className = Etc.ExtractLastWholeWord(_typeFullName, null);
            //    _namespace = GetNamespaceWithoutTypeName(_typeFullName);
            //}

        }

        internal NfTypeName(NfTypeNameParseItem parseItem)
        {

            if (parseItem == null)
                return;
            _className = GetTypeNameWithoutNamespace(parseItem.FullName);
            _namespace = GetNamespaceWithoutTypeName(parseItem.FullName);
            _publicKeyToken = parseItem.PublicKeyTokenValue;
            if (!string.IsNullOrWhiteSpace(parseItem.AssemblyFullName))
                _asmName = new AssemblyName(parseItem.AssemblyFullName);

            if (parseItem.GenericItems == null || !parseItem.GenericItems.Any())
                return;

            foreach (var gi in parseItem.GenericItems)
            {
                _genericArgs.Add(new NfTypeName(gi));
            }
        }

        /// <summary>
        /// Factory method to get a <see cref="NfTypeName"/> from an assembly file on disk.
        /// </summary>
        /// <param name="filePath">Path to a .NET assembly file</param>
        /// <param name="nfTn"></param>
        /// <returns></returns>
        public static bool TryGetByAsmFile(string filePath, out NfTypeName nfTn)
        {
            nfTn = null;
            if (string.IsNullOrWhiteSpace(filePath))
                return false;
            if (!File.Exists(filePath))
                return false;
            try
            {
                var asmName = System.Reflection.AssemblyName.GetAssemblyName(filePath);
                if (asmName == null)
                    return false;
                nfTn = new NfTypeName(asmName.FullName);
            }
            catch (BadImageFormatException)
            {
                return false;
            }
            catch (FileLoadException)
            {
                return false;
            }
            return true;
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
            return !string.IsNullOrWhiteSpace(name) &&
                   Regex.IsMatch(name, ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX, RegexCatalog.MyRegexOptions);
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
            return !string.IsNullOrWhiteSpace(name) &&
                   Regex.IsMatch(name, FULL_ASSEMBLY_NAME_REGEX, RegexCatalog.MyRegexOptions);
        }

        /// <summary>
        /// Returns everything except the last entry after <see cref="Constants.DEFAULT_TYPE_SEPARATOR"/>
        /// or in the case where <see cref="Constants.DEFAULT_TYPE_SEPARATOR"/> isn't present -
        /// just returns <see cref="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNamespaceWithoutTypeName(string name)
        {
            const string ROOT_NS = "global::";

            if(string.IsNullOrWhiteSpace(name))
                return null;

            if(!name.Contains(Constants.DEFAULT_TYPE_SEPARATOR))
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

            var nameArray = name.Split(Constants.DEFAULT_TYPE_SEPARATOR);
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
                    ns.Append(Constants.DEFAULT_TYPE_SEPARATOR);
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
            if (string.IsNullOrWhiteSpace(simplePropType))
                return null;

            if (!simplePropType.Contains(Constants.DEFAULT_TYPE_SEPARATOR))
                return simplePropType;

            if (simplePropType.Contains(Constants.TYPE_METHOD_NAME_SPLIT_ON))
            {
                simplePropType = simplePropType.Substring(0,
                    simplePropType.IndexOf(Constants.TYPE_METHOD_NAME_SPLIT_ON, StringComparison.Ordinal));
            }

            return Etc.ExtractLastWholeWord(simplePropType, Constants.DEFAULT_TYPE_SEPARATOR);
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
                : GetTypeNamesFromGeneric(returnTypeToString, typeDelimiter).FirstOrDefault();
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
                return GetNfRandomName();

            var nameArray = name.ToCharArray();
            var csId = new StringBuilder();

            if (!Char.IsLetter(nameArray[0]) || Convert.ToUInt16(nameArray[0]) > 0x7F)
                csId.Append(DefaultNamePrefix);

            if (Char.IsNumber(nameArray[0]) || nameArray[0] == '_' ||
                (Char.IsLetter(nameArray[0]) && Convert.ToUInt16(nameArray[0]) <= 0x7F))
                csId.Append(nameArray[0]);

            for(var i = 1; i< nameArray.Length; i++)
            {
                char cChar = nameArray[i];
                if ((Char.IsLetter(cChar) && Convert.ToUInt16(cChar) <= 0x7F) || Char.IsNumber(cChar) ||
                    cChar == '_' || cChar == '.') //expecting ns qualified ids
                    csId.Append(cChar);
            }

            return csId.ToString();
        }

        /// <summary>
        /// Uses <see cref="someString"/> allowing for only '_', numbers and letters where the 
        /// first char must be or a letter (the prefix 'nf' is used should that not be the case).
        /// </summary>
        /// <param name="someString"></param>
        /// <param name="replaceInvalidsWithUnicodeEsc">
        /// Set this to true if you want, for example, the <see cref="someString"/> 
        /// with a value of say 'Personal Ph #' to be returned as 'Personal_u0020Ph_u0020_u0023'
        /// </param>
        /// <param name="maxLen">
        /// A max length of the output, if the input string will 
        /// clearly exceed this then some random text will be added as the last 11 chars
        /// </param>
        /// <returns></returns>
        public static string SafeDotNetIdentifier(string someString, bool replaceInvalidsWithUnicodeEsc = false, int maxLen = 80)
        {
            if (String.IsNullOrWhiteSpace(someString))
                return GetNfRandomName();

            var strChars = someString.ToCharArray();
            var strOut = new StringBuilder();
            if (!Char.IsLetter(strChars[0]) || (Char.IsLetter(strChars[0]) && Convert.ToUInt16(strChars[0]) > 0x7F))
                strOut.Append(DefaultNamePrefix);
            var iequals = 0;
            if (!replaceInvalidsWithUnicodeEsc &&
                (Char.IsNumber(strChars[0]) || strChars[0] == '_' ||
                 (Char.IsLetter(strChars[0]) && Convert.ToUInt16(strChars[0]) <= 0x7F)))
            {
                strOut.Append(strChars[0]);
                iequals = 1;
            }

            var randSuffix = "_" + GetNfRandomName();

            for (var i = iequals; i < strChars.Length; i++)
            {
                if (strOut.Length + randSuffix.Length >= maxLen - 1)
                {
                    strOut.Append(randSuffix);
                    break;

                }
                if ((Char.IsLetterOrDigit(strChars[i]) && Convert.ToUInt16(strChars[i]) <= 0x7F) ||
                    (strChars[i] == '_' && !replaceInvalidsWithUnicodeEsc))
                {
                    strOut.Append(strChars[i]);
                    continue;
                }

                if (!replaceInvalidsWithUnicodeEsc)
                    continue;

                strOut.Append($"_u{Convert.ToUInt16(strChars[i]).ToString("x4")}");
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
            const string GET_REGEX = "^"+ PropertyNamePrefix.GET_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";
            const string SET_REGEX = "^" + PropertyNamePrefix.SET_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";
            const string ADD_REGEX = "^" + PropertyNamePrefix.ADD_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";
            const string REMOVE_REGEX = "^" + PropertyNamePrefix.REMOVE_PREFIX + "([a-zA-Z_][a-zA-Z0-9_]*)";

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
                    propName2Mi.Add(derivedPropName, new List<MethodInfo> {mi});
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

        public static string GetNfRandomName()
        {
            var randchars = Path.GetRandomFileName().Replace(".", "_");
            return $"{DefaultNamePrefix}_{randchars}";
        }

        #endregion
    }
}
