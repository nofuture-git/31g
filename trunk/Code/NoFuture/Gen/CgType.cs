using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Shared.DiaSdk.LinesSwitch;
using NoFuture.Util;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta;
using NoFuture.Util.Gia;
using NoFuture.Util.NfType;

namespace NoFuture.Gen
{
    /// <summary>
    /// For code generation based on .NET <see cref="Type"/>
    /// </summary>
    [Serializable]
    public class CgType
    {
        #region fields
        internal readonly Dictionary<string, string[]> EnumValueDictionary = new Dictionary<string, string[]>();
        #endregion

        #region properties
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string FullName 
        { 
            get
            {
                return string.Format("{0}{1}", string.IsNullOrWhiteSpace(Namespace) ? string.Empty : Namespace + ".",
                    Name);
            } 
        }
        public string AssemblyQualifiedName { get; set; }
        /// <summary>
        /// <see cref="Namespace"/> for code gen purpose is not null
        /// this is a flag indicating if the <see cref="Namespace"/>
        /// was derived from the source or contrived.
        /// </summary>
        public bool IsContrivedNamespace { get; set; }
        /// <summary>
        /// Specifies that the original type's source was an Enum
        /// </summary>
        public bool IsEnum { get; set; }
        /// <summary>
        /// The original metadata token from the loaded assembly
        /// </summary>
        public int MetadataToken { get; set; }
        /// <summary>
        /// Returns the type's name as a regex pattern having the 
        /// Namespace portion encoded and optional.
        /// </summary>
        public string RegexPatternName
        {
            get
            {
                var regexPattern = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(Namespace))
                {
                    regexPattern.Append("(");
                    regexPattern.Append(Util.Core.Etc.EscapeString(Namespace, EscapeStringType.REGEX));
                    regexPattern.Append(")?");
                }
                regexPattern.Append(Name);
                return regexPattern.ToString();
            }
        }

        public List<CgMember> Properties { get; set; }
        public List<CgMember> Fields { get; set; }
        public List<CgMember> Events { get; set; }
        public List<CgMember> Methods { get; set; }

        /// <summary>
        /// Returns the <see cref="Methods"/> as a sorted set using the 
        /// <see cref="CgMemberCompare"/>
        /// </summary>
        public SortedSet<CgMember> SortedMethods
        {
            get
            {
                var sortedMethods = new SortedSet<CgMember>(new CgMemberCompare());
                foreach (var cgMem in Methods.Where(cgMem => cgMem != null))
                {
                    sortedMethods.Add(cgMem);
                }
                return sortedMethods;
            }
        }

        /// <summary>
        /// Gets just the type's name from all <see cref="Properties"/>.
        /// </summary>
        /// <remarks>
        /// For generics with multiple types only the last, most inner type name is added
        /// (e.g. Tuple`3[String, Int32, Tuple`2[String, Regex]] -> just 'Regex')
        /// </remarks>
        public List<string> AllPropertyTypes
        {
            get
            {
                return
                    Properties.Select(property => NfReflect.GetLastTypeNameFromArrayAndGeneric(property.TypeName, "<"))
                        .Where(x => !Etc.ValueTypesList.Contains(x.Trim()))
                        .ToList();
            }
        }
        #endregion

        #region ctors
        internal CgType()//only avail ctor through Etc.GetCg~
        {
            Properties = new List<CgMember>();
            Fields = new List<CgMember>();
            Events = new List<CgMember>();
            Methods = new List<CgMember>();
        }
        #endregion

        #region methods
        /// <summary>
        /// Returns the field's names of the <see cref="enumTypeName"/>
        /// being that such an enum was encountered while reflecting the type.
        /// </summary>
        /// <param name="enumTypeName"></param>
        /// <returns></returns>
        public string[] EnumsValues(string enumTypeName)
        {
            if (string.IsNullOrWhiteSpace(enumTypeName))
                return null;
            var vals = EnumValueDictionary.ContainsKey(enumTypeName) ? EnumValueDictionary[enumTypeName] : null;
            if (vals != null) return vals;

            var clearName = NfReflect.GetLastTypeNameFromArrayAndGeneric(enumTypeName);
            vals = EnumValueDictionary.ContainsKey(clearName) ? EnumValueDictionary[clearName] : null;
            
            if (vals != null) return vals;

            //try one last time for cs style generics
            clearName = NfReflect.GetLastTypeNameFromArrayAndGeneric(enumTypeName, "<");
            vals = EnumValueDictionary.ContainsKey(clearName) ? EnumValueDictionary[clearName] : null;
            return vals;
        }

        /// <summary>
        /// Test for if <see cref="someMember"/> belongs to this instance.
        /// </summary>
        /// <param name="someMember"></param>
        /// <returns></returns>
        public bool ContainsThisMember(CgMember someMember)
        {
            if (Properties.Any(p => p.Equals(someMember)))
                return true;
            if (Fields.Any(f => f.Equals(someMember)))
                return true;
            if (Events.Any(e => e.Equals(someMember)))
                return true;
            if (Methods.Any(m => m.Equals(someMember)))
                return true;

            return false;
        }

        public void AssignPdbSymbols(ModuleSymbols[] pdbData)
        {
            foreach (var property in Properties)
                property.MyCgType = this;
            foreach (var fld in Fields)
                fld.MyCgType = this;
            foreach (var m in Methods)
                m.MyCgType = this;
            var propsAndMethods = Properties.ToList();
            propsAndMethods.AddRange(Methods);
            foreach (var pm in propsAndMethods)
                pm.TryAssignPdbSymbols(pdbData);
        }

        /// <summary>
        /// Locates the <see cref="CgMember"/> in <see cref="Methods"/> who matches <see cref="tokenName"/>.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public CgMember FindCgMethodByTokenName(MetadataTokenName tokenName)
        {
            if (tokenName == null)
                return null;
            if (string.IsNullOrWhiteSpace(tokenName.Name))
                return null;
            if (!tokenName.IsMethodName())
                return null;
            var methodName = AssemblyAnalysis.ParseMethodNameFromTokenName(tokenName.Name);
            if (string.IsNullOrWhiteSpace(methodName))
                return null;
            string isPropName;
            if (NfReflect.IsClrMethodForProperty(methodName, out isPropName))
            {
                methodName = isPropName;
                var propMatches = Properties.Where(p => string.Equals(p.Name, methodName)).ToArray();
                if (propMatches.Length >= 1)
                    return propMatches.First();
            }

            var matches = Methods.Where(x => string.Equals(x.Name, methodName)).ToArray();
            if (matches.Length <= 0)
                return null;
            if (matches.Length == 1)
                return matches.First();

            //attempt to match on arg count first
            var argNames = AssemblyAnalysis.ParseArgsFromTokenName(tokenName.Name);
            var argCount = argNames == null ? 0 : argNames.Length;

            matches = Methods.Where(x => x.Args.Count == argCount).ToArray();
            if (matches.Length <= 0)
                return null;
            if (matches.Length == 1)
                return matches.First();
            if (argNames == null)
                return null;

            //attempt to match by args names
            var argNamesLikeThese = argNames.Select(x => Settings.LangStyle.TransformClrTypeSyntax(x));

            foreach (var match in matches)
            {
                if (match.Args.All(nfArg => argNamesLikeThese.Any(token => string.Equals(nfArg.ArgType, token))))
                    return match;
            }
            return null;
        }

        #endregion
    }
}