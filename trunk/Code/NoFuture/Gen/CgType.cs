using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Shared.Core;
using NoFuture.Shared.DiaSdk.LinesSwitch;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.Gia;

namespace NoFuture.Gen
{
    /// <summary>
    /// For code generation based on .NET <see cref="Type"/>
    /// </summary>
    [Serializable]
    public class CgType : LangRules.ISrcCode
    {
        #region fields
        internal readonly Dictionary<string, string[]> EnumValueDictionary = new Dictionary<string, string[]>();
        private Tuple<int, int> _myStartEnclosure;
        private Tuple<int, int> _myEndEnclosure;
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

        public List<CgMember> Properties { get; } = new List<CgMember>();
        public List<CgMember> Fields { get; } = new List<CgMember>();
        public List<CgMember> Events { get; } = new List<CgMember>();
        public List<CgMember> Methods { get; } = new List<CgMember>();

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
        }
        #endregion

        #region methods

        public Tuple<int, int> GetMyStartEnclosure(string[] srcFile, bool isClean = false)
        {
            return _myStartEnclosure;
        }

        public Tuple<int, int> GetMyEndEnclosure(string[] srcFile, bool isClean = false)
        {
            return _myEndEnclosure;
        }

        /// <summary>
        /// To set the start-marker when it is already known.
        /// </summary>
        /// <param name="start"></param>
        public void SetMyStartEnclosure(Tuple<int, int> start)
        {
            _myStartEnclosure = start;
        }

        /// <summary>
        /// To directly set the end-marker when its already known
        /// </summary>
        /// <param name="end"></param>
        public void SetMyEndEnclosure(Tuple<int, int> end)
        {
            _myEndEnclosure = end;
        }

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
            PropagateSelfToLists();
            var propsAndMethods = Properties.ToList();
            propsAndMethods.AddRange(Methods);
            foreach (var pm in propsAndMethods)
                pm.TryAssignPdbSymbols(pdbData);
        }

        public void AssignAntlrParseItems(List<LangRules.AntlrParseItem> parseResults)
        {
            PropagateSelfToLists();
            if (parseResults == null)
                return;
            if (!parseResults.Any())
                return;
            foreach (var pi in parseResults)
            {
                if(pi == null)
                    continue;
                if (_myStartEnclosure == null && pi.DeclTypeName == Name)
                {
                    SetMyStartEnclosure(pi.DeclStart);
                }
                if (_myEndEnclosure == null && pi.DeclTypeName == Name)
                {
                    SetMyEndEnclosure(pi.DeclEnd);
                }

                //expecting that a missing name means its a constructor
                var memberName = pi.Name ?? ".ctor";
                var memberArgs = new List<string>();
                //when type is coupled with name, get just the name
                if (pi.Parameters.Any())
                {
                    foreach (var piParam in pi.Parameters)
                    {
                        if (string.IsNullOrWhiteSpace(piParam))
                            continue;
                        var piParamArgName = piParam.Contains(" ") 
                                             ? piParam.Split(' ').LastOrDefault() 
                                             : piParam;
                        memberArgs.Add(piParamArgName);
                    }
                }

                var cgMem = FindCgMember(memberName, memberArgs.ToArray());
                if (cgMem != null)
                {
                    var adjStart = new Tuple<int, int>(pi.Start.Item1 - 1, pi.Start.Item2 - 1);
                    var adjEnd = new Tuple<int, int>(pi.End.Item1 - 1, pi.End.Item2 + 1);
                    cgMem.SetMyStartEnclosure(adjStart);
                    cgMem.SetMyEndEnclosure(adjEnd);
                }
            }
        }

        /// <summary>
        /// Locates the <see cref="CgMember"/> who matches <see cref="tokenName"/>.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public CgMember FindCgMemberByTokenName(MetadataTokenName tokenName)
        {
            if (tokenName == null)
                return null;

            var byToken = FindCgMember(tokenName.Id);
            if (byToken != null)
                return byToken;

            if (string.IsNullOrWhiteSpace(tokenName.Name))
                return null;

            var methodName = AssemblyAnalysis.ParseMethodNameFromTokenName(tokenName.Name);
            var argNames = AssemblyAnalysis.ParseArgsFromTokenName(tokenName.Name);
            return FindCgMember(methodName, argNames);
        }

        /// <summary>
        /// Locates the <see cref="CgMember"/> with matching <see cref="tokenId"/>
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public CgMember FindCgMember(int tokenId)
        {
            if (tokenId <= 0)
                return null;

            var fldMatch = Fields.FirstOrDefault(f => f.MetadataToken == tokenId);
            if (fldMatch != null)
                return fldMatch;

            var propMatch = Properties.FirstOrDefault(p => p.MetadataToken == tokenId);
            if (propMatch != null)
                return propMatch;

            var mthdMatch = Methods.FirstOrDefault(x => x.MetadataToken == tokenId);
            return mthdMatch;
        }

        /// <summary>
        /// Locates the <see cref="CgMember"/> in <see cref="Methods"/> who matches the given name and args
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="argNames"></param>
        /// <returns></returns>
        public CgMember FindCgMember(string methodName, string[] argNames)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                return null;
            string isPropName;
            if (NfReflect.IsClrMethodForProperty(methodName, out isPropName))
            {
                methodName = isPropName;
            }

            var fldMatch = Fields.FirstOrDefault(f => string.Equals(f.Name, methodName));
            if (fldMatch != null)
                return fldMatch;

            var propMatch = Properties.FirstOrDefault(p => string.Equals(p.Name, methodName));
            if (propMatch != null)
                return propMatch;

            var matches = Methods.Where(x => string.Equals(x.Name, methodName)).ToArray();
            if (matches.Length <= 0)
                return null;
            if (matches.Length == 1)
                return matches.First();

            //attempt to match on arg count first
            var argCount = argNames == null ? 0 : argNames.Length;

            matches = matches.Where(x => x.Args.Count == argCount).ToArray();
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
                if (match.Args.All(nfArg => argNamesLikeThese.Any(token => string.Equals(nfArg.ArgName, token))))
                    return match;
            }
            return null;
        }

        private void PropagateSelfToLists()
        {
            foreach (var property in Properties)
                property.MyCgType = this;
            foreach (var fld in Fields)
                fld.MyCgType = this;
            foreach (var m in Methods)
                m.MyCgType = this;
            foreach (var e in Events)
                e.MyCgType = this;
        }

        #endregion
    }
}