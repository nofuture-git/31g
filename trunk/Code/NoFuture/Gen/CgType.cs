using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util;
using NoFuture.Util.Gia;
using NfTypeName = NoFuture.Util.TypeName;

namespace NoFuture.Gen
{
    /// <summary>
    /// For code generation based on .NET <see cref="Type"/>
    /// </summary>
    [Serializable]
    public class CgType
    {
        #region fields
        private CgTypeFiles _typeFile;

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
                    regexPattern.Append(Util.Etc.EscapeString(Namespace, EscapeStringType.REGEX));
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
        /// The type file is pertinent only to PDB data.
        /// </summary>
        public CgTypeFiles TypeFiles 
        { 
            get { return _typeFile; }
            set
            {
                _typeFile = value;
                //propagate this only at the time its assigned.
                foreach (var property in Properties)
                    property.MyCgType = this;
                foreach (var fld in Fields)
                    fld.MyCgType = this;
                foreach (var m in Methods)
                    m.MyCgType = this;
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
                    Properties.Select(property => TypeName.GetLastTypeNameFromArrayAndGeneric(property.TypeName, "<"))
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

        #region public api
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

            var clearName = TypeName.GetLastTypeNameFromArrayAndGeneric(enumTypeName);
            vals = EnumValueDictionary.ContainsKey(clearName) ? EnumValueDictionary[clearName] : null;
            
            if (vals != null) return vals;

            //try one last time for cs style generics
            clearName = TypeName.GetLastTypeNameFromArrayAndGeneric(enumTypeName, "<");
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

        /// <summary>
        /// Locates the <see cref="CgMember"/> in <see cref="Methods"/> who owns this line number.
        /// </summary>
        /// <param name="lnNum"></param>
        /// <returns></returns>
        /// <remarks>
        /// Depends on parsed PDB data.
        /// </remarks>
        public CgMember FindCgMethodByLineNumber(int lnNum)
        {
            return TypeFiles == null ? null : Methods.FirstOrDefault(x => x.IsOneOfMyLines(lnNum));
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
            if (TypeName.IsClrMethodForProperty(tokenName.Name, out isPropName))
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

        /// <summary>
        /// Returns the starting line number, sourced from the PDB of 
        /// the given type from the original source
        /// </summary>
        /// <returns></returns>
        public int? StartAtLineNumber()
        {
            if (TypeFiles == null)
                return null;
            return TypeFiles.OriginalSourceFirstLine;
        }

        /// <summary>
        /// Returns edge definitions specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public string ToGraphVizEdge()
        {
            var graphViz = new StringBuilder();
            var myName = NfTypeName.SafeDotNetIdentifier(FullName);
            var edges = new List<string>();
            foreach (
                var property in
                    Properties.Where(
                        x => !Etc.ValueTypesList.Contains(TypeName.GetLastTypeNameFromArrayAndGeneric(x.TypeName, "<")))
                )
            {
                var toName =
                    TypeName.SafeDotNetIdentifier(TypeName.GetLastTypeNameFromArrayAndGeneric(property.TypeName, "<"));
                var edg = new StringBuilder();
                edg.AppendFormat("{0} -> {1}", myName, toName);
                edg.Append(property.IsEnumerableType ? " [arrowhead=odiamond]" : " [arrowhead=vee]");
                edg.Append(";");
                if (!edges.Contains(edg.ToString()))
                    edges.Add(edg.ToString());
            }
            foreach (var edge in edges)
            {
                graphViz.AppendLine(edge);
            }
            return graphViz.ToString();
        }

        /// <summary>
        /// Returns node definition specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public string ToGraphVizNode()
        {
            var graphViz = new StringBuilder();
            graphViz.Append(TypeName.SafeDotNetIdentifier(FullName));
            graphViz.AppendLine(" [shape=Mrecord, label=<<table bgcolor=\"white\" border=\"0\" >");
            graphViz.AppendLine("<th>");
            graphViz.AppendLine("<td bgcolor=\"grey\" align=\"center\">");
            graphViz.Append("<font color=\"white\">");
            graphViz.AppendFormat("{0} :: {1}", Name, string.IsNullOrWhiteSpace(Namespace) ? "global" : Namespace);
            graphViz.AppendLine("</font></td></th>");

            foreach (var prop in Properties)
                graphViz.AppendLine(prop.ToGraphVizString());
            foreach (var me in SortedMethods)
                graphViz.AppendLine(me.ToGraphVizString());
            graphViz.AppendLine("</table>> ];");

            return graphViz.ToString();
        }

        /// <summary>
        /// Returns a node definition with just the type name's header.
        /// This is specific to graph-viz (ver. 2.38+)
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <param name="enumValues">Optional values to be listed as line items with no type specifiers nor .gv port ids.</param>
        /// <returns></returns>
        public static string EmptyGraphVizClassNode(string typeFullName, string[] enumValues)
        {
            var className = TypeName.GetTypeNameWithoutNamespace(typeFullName);
            var ns = TypeName.GetNamespaceWithoutTypeName(typeFullName);
            var fullName = string.Format("{0}{1}", string.IsNullOrWhiteSpace(ns) ? string.Empty : ns + ".", className);
            var graphViz = new StringBuilder();
            graphViz.Append(TypeName.SafeDotNetIdentifier(fullName));
            graphViz.AppendLine(" [shape=Mrecord, label=<<table bgcolor=\"white\" border=\"0\" >");
            graphViz.AppendLine("<th>");
            graphViz.AppendLine("<td bgcolor=\"grey\" align=\"center\">");
            graphViz.Append("<font color=\"white\">");
            graphViz.AppendFormat("{0} :: {1}", className, string.IsNullOrWhiteSpace(ns) ? "global" : ns);
            graphViz.AppendLine("</font></td></th>");
            if (enumValues != null && enumValues.Length > 0)
            {
                foreach (var enumVal in enumValues)
                {
                    graphViz.Append("<tr><td><font color=\"blue\">");
                    graphViz.Append(enumVal);
                    graphViz.AppendLine("</font></td></tr>");
                }
            }
            else
            {
                graphViz.AppendLine("<tr><td></td></tr>");
                graphViz.AppendLine("<tr><td></td></tr>");
            }
            graphViz.AppendLine("</table>> ];");

            return graphViz.ToString();
        }
        #endregion
    }
}