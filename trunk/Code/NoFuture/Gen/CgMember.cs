using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NfTypeName = NoFuture.Util.TypeName;

namespace NoFuture.Gen
{
    /// <summary>
    /// For code generation based on .NET <see cref="MemberInfo"/>
    /// </summary>
    [Serializable]
    public class CgMember
    {
        #region fields
        private PdbTargetLine _myPdbTargetLine;
        private string[] _myImplementation;
        private CgType _myCgType;
        private Dictionary<Tuple<int, int>, string[]> _myRefactor;
        private readonly List<CgMember> _opCodeCallAndCallvirts;
        protected internal readonly List<int> opCodeCallsAndCallvirtsMetadatTokens;
        #endregion

        #region ctors
        public CgMember()
        {
            Args = new List<CgArg>();
            _opCodeCallAndCallvirts = new List<CgMember>();
            opCodeCallsAndCallvirtsMetadatTokens = new List<int>();
        }
        #endregion

        #region public properties
        public string TypeName { get; set; }
        public string Name { get; set; }
        public int MetadataToken { get; set; }
        public CgAccessModifier AccessModifier { get; set; }
        public List<CgArg> Args { get; set; }
        public bool HasGetter { get; set; }
        public bool HasSetter { get; set; }
        public bool IsStatic { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsEnumerableType { get; set; }
        public bool IsCtor { get; set; }
        public bool IsEnum { get; set; }
        public bool IsMethod { get; set; }

        /// <summary>
        /// For use of <see cref="CgMember"/> types owned by another <see cref="CgMember"/>
        /// and not a <see cref="CgType"/>
        /// </summary>
        /// <remarks>
        /// <see cref="OpCodeCallAndCallvirts"/>
        /// </remarks>
        public string DeclaringTypeAsmName { get; set; }

        /// <summary>
        /// A list of CgMembers invoked within the body of this <see cref="CgMember"/>
        /// </summary>
        /// <remarks>
        /// This represents all the calls being made within this method's body 
        /// out to other type's members.
        /// </remarks>
        public List<CgMember> OpCodeCallAndCallvirts
        {
            get { return _opCodeCallAndCallvirts; }
        }

        /// <summary>
        /// Used by calling assembly for whatever reason they want.
        /// </summary>
        public bool SkipIt { get; set; }

        /// <summary>
        /// The start and end line numbers for this member in the original 
        /// source code file.
        /// </summary>
        public Tuple<int?, int?> PdbStartEndLineNumbers
        {
            get
            {
                if(MyPdbTargetLine == null)
                    return new Tuple<int?, int?>(null, null);
                var sa = new int?(MyPdbTargetLine.StartAt);
                var ea = new int?(MyPdbTargetLine.EndAt);
                return new Tuple<int?, int?>(sa,ea);
            }
        }
        #endregion

        #region public api
        /// <summary>
        /// Returns string specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public string ToGraphVizString()
        {
            var graphViz = new StringBuilder();
            graphViz.Append("<tr><td align=\"left\">");
            graphViz.Append(Name);
            graphViz.Append(" ");
            var typeColor = Etc.ValueTypesList.Contains(NfTypeName.GetLastTypeNameFromArrayAndGeneric(TypeName.Trim(), "<")) ? "blue" : "grey";
            if (HasGetter || HasSetter)
            {
                graphViz.Append("(");
                graphViz.Append(string.Join(", ", Args.Select(x => x.ToGraphVizString())));
                graphViz.Append(")");
            }
            graphViz.Append(": <font color=\"");
            graphViz.Append(typeColor);
            graphViz.Append("\">");
            graphViz.Append(NfTypeName.GetLastTypeNameFromArrayAndGeneric(TypeName, "<"));
            if (IsEnumerableType)
                graphViz.Append("[*]");
            graphViz.Append("</font></td></tr>");
            return graphViz.ToString();
        }

        /// <summary>
        /// Returns a regex pattern which will match on <see cref="Name"/> and, if applicable, number of parameters.
        /// </summary>
        public string AsInvokeRegexPattern(params string[] varNames)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return ".";

            return Settings.LangStyle.ToInvokeRegex(this, varNames);
        }

        /// <summary>
        /// Gets this member's code-generated lines.  When PDB Lines are available then
        /// the body of the method contains them otherwise the body will default to 
        /// <see cref="Settings.NoImplementationDefault"/>
        /// </summary>
        public string[] MyCgLines(bool justBody)
        {
            if (SkipIt)
                return new[] { string.Empty };

            if (_myImplementation != null && _myImplementation.Length > 0)
            {
                return _myImplementation;
            }

            var myImplementation = new List<string>();
            if (!justBody)
            {
                myImplementation.Add(string.Format("        {0}", Settings.LangStyle.ToDecl(this, true)));
                myImplementation.Add(string.Format("        {0}", Settings.LangStyle.GetEnclosureOpenToken(this)));
                
            }
            if (_myCgType == null || _myCgType.TypeFiles == null || MyPdbTargetLine == null)
            {
                myImplementation.Add(Settings.NoImplementationDefault);
            }
            else
            {
                myImplementation.AddRange(MyPdbTargetLine.GetMyPdbTargetLines(_myCgType.TypeFiles.SymbolFolder, null));
            }
            if (!justBody)
            {
                myImplementation.Add(string.Format("        {0}", Settings.LangStyle.GetEnclosureCloseToken(this)));
            }
            _myImplementation = myImplementation.ToArray();
            return _myImplementation;
        }

        /// <summary>
        /// Returns the lines, sourced from the PDB, exactly as-is from the original source (null otherwise).
        /// </summary>
        /// <returns></returns>
        public string[] MyOriginalLines()
        {
            if (_myCgType == null || _myCgType.TypeFiles == null || MyPdbTargetLine == null)
                return null;
            return _myCgType.TypeFiles.ReadLinesFromOriginalSrc(MyPdbTargetLine);
        }

        /// <summary>
        /// Gets a dictionary for use in replacing content from the original source file.
        /// </summary>
        /// <param name="newVariableName">Optional, specify the name of the injected instance-level variable. 
        /// Default to a random string.</param>
        /// <param name="namespaceAndTypeName">Optional for use in specifying a new namespace and type name.
        /// Defaults to the parent <see cref="CgType"/> values.</param>
        /// <param name="includeVariableDecl">
        /// Specify as true to have the resulting dictionary include where to inject a new 
        /// variable declaration.  Typically the very first line after the classes' opening token.
        /// </param>
        /// <returns>
        /// The dictionary's keys are in the form of index, length, same as 
        /// in the common <see cref="System.String.Substring(int, int)"/> and represent what is
        /// to be removed from the original source code file while its counterpart values are
        /// what is placed in its stead.
        /// </returns>
        public Dictionary<Tuple<int, int>, string[]> MyRefactoredLines(string newVariableName,
            Tuple<string, string> namespaceAndTypeName, bool includeVariableDecl = true)
        {
            if (_myRefactor != null)
                return _myRefactor;
            if (_myCgType == null || _myCgType.TypeFiles == null || MyPdbTargetLine == null)
                return null;
            if (string.IsNullOrWhiteSpace(newVariableName))
                newVariableName = Path.GetRandomFileName().Replace(".", string.Empty);
            var ofl = _myCgType.TypeFiles.OriginalSourceFirstLine;
            var dido = new Dictionary<Tuple<int, int>, string[]>();

            //allow calling assembly to specify a new namespace and type name
            var renameCgType = namespaceAndTypeName == null
                ? _myCgType
                : new CgType()
                {
                    Namespace = namespaceAndTypeName.Item1 ?? _myCgType.Namespace,
                    Name = namespaceAndTypeName.Item2 ?? _myCgType.Name
                };

            //this is an instance level declaration added to the original source
            if (ofl > 1 && includeVariableDecl)
            {
                var oldFilesInstanceToNew = Settings.LangStyle.ToDecl(renameCgType, newVariableName,
                    CgAccessModifier.Private);
                dido.Add(new Tuple<int, int>(ofl, 1), new[] {oldFilesInstanceToNew});
            }
            var nKey = new Tuple<int, int>(_myPdbTargetLine.StartAt,
                _myPdbTargetLine.EndAt - _myPdbTargetLine.StartAt - 1);

            dido.Add(nKey, new[] {Settings.LangStyle.ToStmt(this, null, newVariableName)});
            _myRefactor = dido;
            return _myRefactor;
        }

        /// <summary>
        /// Useful for refactoring, finds a list of all instance level fields, properties 
        /// and methods which are used within this member's implementation.  
        /// </summary>
        /// <returns> </returns>
        /// <remarks>
        /// Depends on PDB data, results are not derived from assembly reflection.
        /// </remarks>
        public List<CgArg> GetAllRefactorDependencies()
        {
            var pdbTlines = MyPdbTargetLine;
            if (pdbTlines == null)
                return null;
            if (_myCgType == null || _myCgType.TypeFiles == null)
                return null;

            var srcCode = MyPdbTargetLine.GetMyPdbTargetLines(_myCgType.TypeFiles.SymbolFolder, null);
            if (srcCode == null || srcCode.Length <= 0)
                return null;

            var dependencyArgs = new List<CgArg>();

            var flattenCode = new string(Settings.LangStyle.FlattenCodeToCharStream(srcCode).ToArray());
            flattenCode = Settings.LangStyle.EncodeAllStringLiterals(flattenCode);

            var instanceMembers =
                _myCgType.Fields.Where(f => Regex.IsMatch(flattenCode, f.AsInvokeRegexPattern())).ToList();
            dependencyArgs.AddRange(instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic,false)));

            instanceMembers =
                _myCgType.Properties.Where(p => Regex.IsMatch(flattenCode, p.AsInvokeRegexPattern())).ToList();
            dependencyArgs.AddRange(instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, false)));

            instanceMembers =
                _myCgType.Methods.Where(m => Regex.IsMatch(flattenCode, m.AsInvokeRegexPattern())).ToList();
            dependencyArgs.AddRange(instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, true)));

            return dependencyArgs;
        }

        /// <summary>
        /// Attempts to determine if the ArgNames present herein appear, as 
        /// whole words, somewhere in the <see cref="codeBlockLines"/>.  
        /// Typically developers will draft signatures where the most important
        /// parameter is furtherest left.  This plays on that by first looking
        /// for all the ArgNames then drops one off the right, tries again and 
        /// so forth.  
        /// </summary>
        /// <param name="codeBlockLines"></param>
        /// <returns>
        /// A number being the number of matches for ArgName's found starting
        /// on the left.
        /// </returns>
        /// <remarks>
        /// This logic works if the overloaded signatures are ordered by
        /// the number of parameters having highest go first.
        /// </remarks>
        public int CodeBlockUseMyLocals(string[] codeBlockLines)
        {
            if (Args.Count == 0)
                return 0;

            //first get a cleaned up copy of the contents so not mislead by comments
            var cleanCodeBlock = Settings.LangStyle.CleanupPdbLinesCodeBlock(codeBlockLines).ToList();

            //start by looking for all names, failing that, drop one off the right and try again till down to one
            for (var i = Args.Count; i > 0; i--)
            {
                var argNameList = Args.Take(i).Select(arg => arg.ArgName).ToList();
                var prop =
                    argNameList.All(arg => cleanCodeBlock.Any(ln => Regex.IsMatch(ln, string.Format(@"\W{0}\W", arg))));
                if (prop)
                    return i;
            }

            return 0;
        }
        #endregion

        #region internal helpers

        /// <summary>
        /// Specific to the use of <see cref="CgTypeFiles.TryFindPdbTargetLine"/>
        /// </summary>
        internal bool TiedForBestFileMatch { get; set; }
        /// <summary>
        /// Specific to the use of <see cref="CgTypeFiles.TryFindPdbTargetLine"/>
        /// </summary>
        internal bool LessThanPerfectFileMatch { get; set; }

        internal CgType MyCgType { get { return _myCgType; } set { _myCgType = value; } }

        internal bool IsOneOfMyLines(int lnNum)
        {
            if (lnNum < 0)
                return false;
            if (MyPdbTargetLine == null)
                return false;
            if (_myCgType.TypeFiles.FileIndex == null)
                return false;

            PdbTargetLine pdbOut;
            if (!_myCgType.TypeFiles.FileIndex.TryFindPdbTargetLine(lnNum, out pdbOut))
                return false;

            return pdbOut.Equals(MyPdbTargetLine);
        }

        internal PdbTargetLine MyPdbTargetLine
        {
            get
            {
                if (_myPdbTargetLine != null)
                    return _myPdbTargetLine;
                if (_myCgType == null || _myCgType.TypeFiles == null)
                    return null;

                PdbTargetLine pdbTout;
                if (!_myCgType.TypeFiles.TryFindPdbTargetLine(this, out pdbTout))
                    return null;
                _myPdbTargetLine = pdbTout;
                return _myPdbTargetLine;
            }
        }
        #endregion

        public virtual bool Equals(CgMember obj)
        {
            var eq = obj.TypeName == TypeName && obj.Name == Name && obj.HasGetter == HasGetter && obj.HasSetter == HasSetter &&
                     obj.IsStatic == IsStatic && obj.IsGeneric == IsGeneric;
            if (!eq)
                return false;
            if (obj.Args.Count != Args.Count)
                return false;

            var compareArgs = obj.Args.ToList();
            var myArgs = Args.ToList();

            return !myArgs.Any(myArg => compareArgs.All(x => !x.Equals(myArg)));
        }

    }

    /// <summary>
    /// Should sort those with the most number of <see cref="CgArg"/> 
    /// upon this number of <see cref="CgArg"/> count being equal
    /// then start comparing the length of the <see cref="CgArg.ArgName"/>'s length
    /// starting from the left and continue right until one is longer than the other 
    /// and exhausting all return <see cref="CgMemberCompare.X_GREATER_THAN_Y"/>
    /// Having no args then return whichever has the longest ToString 
    /// </summary>
    public class CgMemberCompare : Comparer<CgMember>
    {
        public int X_LESS_THAN_Y = -1;
        public int X_EQUALS_Y = 0;
        public int X_GREATER_THAN_Y = 1;

        public override int Compare(CgMember x, CgMember y)
        {
            if (x == null && y == null)
                return X_EQUALS_Y;
            if (y != null && x == null)
                return X_GREATER_THAN_Y;
            if (y == null)
                return X_LESS_THAN_Y;
            if (x.Args.Count < y.Args.Count)
                return X_GREATER_THAN_Y;
            if (x.Args.Count > y.Args.Count)
                return X_LESS_THAN_Y;

            var yToCsDecl = Settings.LangStyle.ToDecl(y);
            var xToCsDecl = Settings.LangStyle.ToDecl(x);
            if (x.Args.Count == 0)
                return yToCsDecl.Length > xToCsDecl.Length ? X_GREATER_THAN_Y : X_LESS_THAN_Y;
            for (var i = x.Args.Count - 1; i >= 0; i--)
            {
                var xCgArg = x.Args[i];
                var yCgArg = y.Args[i];
                if (xCgArg == null || yCgArg == null)
                    continue;
                if (xCgArg.ArgName.Length == yCgArg.ArgName.Length)
                    continue;
                if (xCgArg.ArgName.Length > yCgArg.ArgName.Length)
                    return X_LESS_THAN_Y;
                return X_GREATER_THAN_Y;
            }
            return yToCsDecl.Length > xToCsDecl.Length ? X_GREATER_THAN_Y : X_LESS_THAN_Y;
        }
    }
}
