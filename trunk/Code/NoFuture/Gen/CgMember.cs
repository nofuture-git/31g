using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Shared;
using NoFuture.Tokens;
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
        protected internal readonly List<int> opCodeCallsAndCallvirtsMetadatTokens;

        internal PdbTargetLine[] _myPdbTargetLine;
        internal string[] _myOriginalLines;

        private string[] _myImplementation;
        private CgType _myCgType;
        private Dictionary<Tuple<int, int>, string[]> _myRefactor;
        private readonly List<CgMember> _opCodeCallAndCallvirts;
        private Tuple<int, int> _myStartEnclosure;
        private Tuple<int, int> _myEndEnclosure;
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
                if (MyPdbLines == null || MyPdbLines.Length <= 0)
                    return new Tuple<int?, int?>(null, null);
                var sa = new int?(MyPdbLines.First().StartAt);
                var ea = new int?(MyPdbLines.First().EndAt);
                return new Tuple<int?, int?>(sa, ea);
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
        /// Renders the invocation of this <see cref="CgMember"/> as a regex pattern.
        /// </summary>
        public string AsInvokeRegexPattern(params string[] varNames)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return ".";

            return Settings.LangStyle.ToInvokeRegex(this, varNames);
        }

        /// <summary>
        /// Renders the signature of this <see cref="CgMember"/> as a regex pattern
        /// </summary>
        /// <returns></returns>
        public string AsSignatureRegexPattern()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return ".";
            return Settings.LangStyle.ToSignatureRegex(this);
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
            if (_myCgType == null || _myCgType.TypeFiles == null || MyPdbLines == null || MyPdbLines.Length <= 0)
            {
                myImplementation.Add(Settings.NoImplementationDefault);
            }
            else
            {
                foreach (var pdbFile in MyPdbLines)
                {
                    myImplementation.AddRange(pdbFile.GetMyPdbTargetLines(_myCgType.TypeFiles.SymbolFolder, null));
                }

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
        /// <param name="buffer">optional buffer to have the lines in both directions extended by</param>
        /// <returns></returns>
        public string[] MyOriginalLines(int buffer = 0)
        {
            if (_myOriginalLines != null)
                return _myOriginalLines;

            if (_myCgType == null || _myCgType.TypeFiles == null || MyPdbLines == null || MyPdbLines.Length <= 0)
                return null;
            var maxEndAt = MaxEndAt;
            var minStartAt = MinStartAt;

            if (buffer > 0)
            {
                minStartAt = minStartAt - buffer;
                maxEndAt = maxEndAt + buffer;
            }

            _myOriginalLines = _myCgType.TypeFiles.ReadLinesFromOriginalSrc(minStartAt, maxEndAt);
            return _myOriginalLines;
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
            if (_myCgType == null || _myCgType.TypeFiles == null || MyPdbLines == null || MyPdbLines.Length <= 0)
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
                dido.Add(new Tuple<int, int>(ofl, 1), new[] { oldFilesInstanceToNew });
            }
            var nKey = new Tuple<int, int>(MinStartAt,
                MaxEndAt - MinStartAt - 1);

            dido.Add(nKey, new[] { Settings.LangStyle.ToStmt(this, null, newVariableName) });
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
            if (_myCgType == null || _myCgType.TypeFiles == null || MyPdbLines == null)
                return null;

            var ssrcCode = new List<string>();
            foreach (var pdb in MyPdbLines)
            {
                ssrcCode.AddRange(pdb.GetMyPdbTargetLines(_myCgType.TypeFiles.SymbolFolder, null));
            }
            if (ssrcCode.Count <= 0)
                return null;

            var dependencyArgs = new List<CgArg>();

            var flattenCode = new string(Settings.LangStyle.FlattenCodeToCharStream(ssrcCode.ToArray()).ToArray());
            bool irregular = false;
            flattenCode = Settings.LangStyle.EscStringLiterals(flattenCode, EscapeStringType.UNICODE, ref irregular);

            var instanceMembers =
                _myCgType.Fields.Where(f => Regex.IsMatch(flattenCode, f.AsInvokeRegexPattern())).ToList();

            //apply filters since arg names and types may be duplicated with instance variables.
            dependencyArgs.AddRange(
                instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, false))
                    .Where(x => Args.All(myArg => !myArg.Equals(x) && dependencyArgs.All(nArg => !nArg.Equals(x)))));

            instanceMembers =
                _myCgType.Properties.Where(p => Regex.IsMatch(flattenCode, p.AsInvokeRegexPattern())).ToList();
            dependencyArgs.AddRange(
                instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, false))
                    .Where(x => Args.All(myArg => !myArg.Equals(x) && dependencyArgs.All(nArg => !nArg.Equals(x)))));

            instanceMembers =
                _myCgType.Methods.Where(
                    m => Regex.IsMatch(flattenCode, m.AsInvokeRegexPattern())).ToList();
            dependencyArgs.AddRange(
                instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, true))
                    .Where(x => Args.All(myArg => !myArg.Equals(x) && dependencyArgs.All(nArg => !nArg.Equals(x)))));

            //arg names common to obj methods will cause confusion
            var objMethods = typeof(object).GetMethods().Select(x => x.Name).ToArray();
            foreach (var dpArg in dependencyArgs)
            {
                if (objMethods.Contains(dpArg.ArgName))
                    dpArg.ArgName = string.Format("{0}{1}", Util.TypeName.DEFAULT_NAME_PREFIX, dpArg.ArgName);
            }

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
        public int CodeBlockUsesMyArgs(string[] codeBlockLines)
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

        /// <summary>
        /// Gets the exact start of the member signature as a line index and 
        /// the char index therein.
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="isClean">
        /// optional parameter to not have the <see cref="srcFile"/>
        /// cleaned of comments, preprocs and string literals.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Does not capture the decorators (aka Attributes)
        /// </remarks>
        public Tuple<int, int> GetMyStartEnclosure(string[] srcFile, bool isClean = false)
        {
            if (_myStartEnclosure != null) return _myStartEnclosure;

            if (srcFile == null || srcFile.Length <= 0)
                return null;

            var minStartAt = MinStartAt;

            //expecting auto-properties to not be represented in pdb data.
            if (minStartAt < 0 && (HasSetter || HasGetter))
            {
                minStartAt = srcFile.Length - 1;//so we search the whole file
            }

            var sigRegexPattern = AsSignatureRegexPattern();

            //HACK - to get just the method name like its a property
            var reserveValue = IsMethod;
            IsMethod = false;
            var asNameOnlyRegexPattern = AsSignatureRegexPattern();
            IsMethod = reserveValue;

            var sigRegex = new Regex("(" + sigRegexPattern + ")",
                RegexOptions.IgnoreCase);

            var justNameRegex = new Regex("(" + asNameOnlyRegexPattern + ")");

            if (!isClean)
            {
                srcFile = Settings.LangStyle.RemoveBlockComments(srcFile);
                srcFile = Settings.LangStyle.RemoveLineComments(srcFile, Settings.LangStyle.LineComment);
                srcFile = Settings.LangStyle.RemovePreprocessorCmds(srcFile);
                var irregular = false;
                for (var m = 0; m < srcFile.Length; m++)
                {
                    srcFile[m] = Settings.LangStyle.EscStringLiterals(srcFile[m], EscapeStringType.BLANK, ref irregular);
                }
            }

            var fileSig = new Tuple<int, string>(0, string.Empty);
            for (var i = minStartAt; i >= 0; i--)
            {
                var encodedLine = srcFile[i];
                Regex posRegex;
                //try for exact match first
                if (sigRegex.IsMatch(encodedLine))
                {
                    posRegex = sigRegex;
                }
                else if (justNameRegex.IsMatch(encodedLine))
                {
                    posRegex = justNameRegex;
                }
                else
                {
                    continue;
                }
                var groups = posRegex.Matches(encodedLine)[0];

                if (!groups.Groups[0].Success)
                    continue;

                fileSig = new Tuple<int, string>(i, groups.Groups[0].Value.Trim());

                break;
            }

            if (string.IsNullOrWhiteSpace(fileSig.Item2))
                return null;

            var idx = srcFile[fileSig.Item1].IndexOf(fileSig.Item2, StringComparison.Ordinal) - 1;
            if (idx < 0)
                idx = 0;

            _myStartEnclosure = new Tuple<int, int>(fileSig.Item1, idx);
            return _myStartEnclosure;
        }

        /// <summary>
        /// Gets the exact end of the method body as a line index and
        /// char index therein.
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="isClean">
        /// optional parameter to not have the <see cref="srcFile"/>
        /// cleaned of comments, preprocs and string literals.
        /// </param>
        /// <returns></returns>
        public Tuple<int, int> GetMyEndEnclosure(string[] srcFile, bool isClean = false)
        {
            if (_myEndEnclosure != null) return _myEndEnclosure;

            if (srcFile == null || srcFile.Length <= 0)
                return null;

            var minStartAt = MinStartAt;
            var maxEndAt = MaxEndAt;

            //check if this is auto-property before cleaning all this content
            if (minStartAt < 0 && !HasSetter && !HasGetter)
                return null;

            //set source to pristine for tokens to operate on
            if (!isClean)
            {
                srcFile = Settings.LangStyle.RemoveBlockComments(srcFile);
                srcFile = Settings.LangStyle.RemoveLineComments(srcFile, Settings.LangStyle.LineComment);
                srcFile = Settings.LangStyle.RemovePreprocessorCmds(srcFile);
                var irregular = false;
                for (var m = 0; m < srcFile.Length; m++)
                {
                    srcFile[m] = Settings.LangStyle.EscStringLiterals(srcFile[m], EscapeStringType.BLANK, ref irregular);
                }
            }

            var openToken = Settings.LangStyle.GetEnclosureOpenToken(this);
            var closeToken = Settings.LangStyle.GetEnclosureCloseToken(this);

            //handle finding auto-properties
            if (minStartAt < 0 && (HasGetter || HasSetter))
            {
                var autoPropSigAt = GetMyStartEnclosure(srcFile, true);
                if (autoPropSigAt == null || autoPropSigAt.Item1 < 0 || autoPropSigAt.Item1 >= srcFile.Length)
                    return null;

                //get this line
                var autoPropLine = srcFile[autoPropSigAt.Item1];
                var idxInLine = autoPropSigAt.Item2 < 0 || autoPropSigAt.Item2 >= autoPropLine.Length
                    ? 0
                    : autoPropSigAt.Item2;

                //reduce the line to where we know the auto-prop starts
                autoPropLine = autoPropLine.Substring(idxInLine);

                //now find the very first occurance of the open-token
                idxInLine = autoPropLine.IndexOf(openToken, StringComparison.Ordinal);

                //if we can move one index past this then that is our min
                if (idxInLine <= 0 || idxInLine + 1 > autoPropLine.Length)
                    return null;
                minStartAt = idxInLine + 1;

                //if there is a close token already on this line it must be our ending
                autoPropLine = autoPropLine.Substring(minStartAt);
                if (autoPropLine.Contains(closeToken))
                {
                    idxInLine = autoPropLine.IndexOf(closeToken, StringComparison.Ordinal);
                    idxInLine -= 1;
                    idxInLine = (srcFile[autoPropSigAt.Item1].Length - autoPropLine.Length) + idxInLine;
                    _myEndEnclosure = new Tuple<int, int>(autoPropSigAt.Item1, idxInLine);

                    System.Diagnostics.Debug.WriteLine(_myEndEnclosure);
                    return _myEndEnclosure;
                }
            }

            //get the original file as an array of tokens
            XDocFrame myFilesFrame;
            if (openToken.Length == 1 && closeToken.Length == 1)
            {
                myFilesFrame = new XDocFrame(openToken.ToCharArray()[0], closeToken.ToCharArray()[0]);
            }
            else
            {
                myFilesFrame = new XDocFrame(openToken, closeToken);
            }

            //turn the original file into a single string
            var srcFileStr = string.Join(Environment.NewLine, srcFile);
            var myTokens = myFilesFrame.FindEnclosingTokens(srcFileStr);

            myTokens = myTokens.OrderByDescending(x => x.Start).ToList();

            //turn the pdb start line into an array index value
            var charCount = 0;
            for (var j = 0; j < minStartAt; j++)
            {
                charCount += srcFile[j].Length + Environment.NewLine.Length;
            }
            //we want the last token whose start is less-than-equal-to the array position of the Pdb Start line
            var myTokenFromMin = myTokens.FirstOrDefault(x => x.Start < charCount);

            //need to validate our token on both ends
            charCount = 0;
            for (var j = 0; j < maxEndAt; j++)
            {
                charCount += srcFile[j].Length + Environment.NewLine.Length;
            }
            var myTokenFromMax = myTokens.FirstOrDefault(x => x.Start < charCount) ?? myTokenFromMin;

            if (myTokenFromMin != null)
            {
                //being lopsided indicates the very first char of the method body is another open token
                if (!myTokenFromMin.Equals(myTokenFromMax))
                {
                    charCount = 0;
                    //go up one line and try again
                    for (var j = 0; j < minStartAt - 1; j++)
                    {
                        charCount += srcFile[j].Length + Environment.NewLine.Length;
                    }
                    myTokenFromMin = myTokens.FirstOrDefault(x => x.Start < charCount) ?? myTokenFromMin;
                }

                //need to reverse the Token's End back into a line number
                charCount = 0;
                for (var k = 0; k < srcFile.Length; k++)
                {
                    //if we go to the next line will be blow past the token's end
                    if (charCount + srcFile[k].Length + Environment.NewLine.Length < myTokenFromMin.End)
                    {
                        charCount += srcFile[k].Length + Environment.NewLine.Length;
                        continue;
                    }
                    //this should be the index in line k
                    var sl = myTokenFromMin.End - charCount - 1;
                    _myEndEnclosure = new Tuple<int, int>(k, sl);
                    return _myEndEnclosure;
                }
            }

            //default to look up from pdb line.
            for (var i = MaxEndAt; i > 0; i--)
            {
                if (!srcFile[i].Contains(closeToken))
                    continue;
                var idx = srcFile[i].LastIndexOf(closeToken, StringComparison.Ordinal);
                _myEndEnclosure = new Tuple<int, int>(i, idx);
                return _myEndEnclosure;

            }

            return null;
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
            if (MyPdbLines == null || MyPdbLines.Length <= 0)
                return false;
            if (_myCgType.TypeFiles.FileIndex == null)
                return false;

            PdbTargetLine pdbOut;
            if (!_myCgType.TypeFiles.FileIndex.TryFindPdbTargetLine(lnNum, out pdbOut))
                return false;

            return pdbOut.Equals(MyPdbLines.First());
        }

        internal PdbTargetLine[] MyPdbLines
        {
            get
            {
                if (_myPdbTargetLine != null)
                    return _myPdbTargetLine;
                if (_myCgType == null || _myCgType.TypeFiles == null)
                    return null;

                PdbTargetLine[] pdbTout;
                if (!_myCgType.TypeFiles.TryFindPdbTargetLine(this, out pdbTout))
                    return null;
                _myPdbTargetLine = pdbTout;
                return _myPdbTargetLine;
            }
        }

        internal int MaxEndAt
        {
            get
            {
                if (MyPdbLines == null || MyPdbLines.Length <= 0)
                {
                    return -1;
                }
                return MyPdbLines.Length == 1
                    ? MyPdbLines.First().EndAt
                    : MyPdbLines.Select(x => x.EndAt).Where(x => x > 0).Max();
            }
        }

        internal int MinStartAt
        {
            get
            {
                if (MyPdbLines == null || MyPdbLines.Length <= 0)
                {
                    return -1;
                }
                return MyPdbLines.Length == 1
                    ? MyPdbLines.First().StartAt
                    : MyPdbLines.Select(x => x.StartAt).Where(x => x > 0 && x < MaxEndAt).Min();
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
