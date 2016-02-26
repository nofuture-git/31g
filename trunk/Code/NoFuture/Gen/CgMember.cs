using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Shared;
using NoFuture.Shared.DiaSdk.LinesSwitch;
using NoFuture.Tokens;

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

        private string[] _myImplementation;
        private CgType _myCgType;
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
        public bool IsInterfaceImpl { get; set; }

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
        /// PDB data bound to this <see cref="CgMember"/>
        /// </summary>
        public ModuleSymbols PdbModuleSymbols { get; set; }
        #endregion

        #region public api

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
        public string[] MyCgLines()
        {
            if (SkipIt)
                return new[] { string.Empty };

            if (_myImplementation != null && _myImplementation.Length > 0)
            {
                return _myImplementation;
            }

            var myImplementation = new List<string>();
            if (PdbModuleSymbols == null || string.IsNullOrWhiteSpace(PdbModuleSymbols.file) ||
                !File.Exists(PdbModuleSymbols.file))
            {
                myImplementation.Add(string.Format("        {0}", Settings.LangStyle.ToDecl(this, true)));
                myImplementation.Add(string.Format("        {0}", Settings.LangStyle.GetEnclosureOpenToken(this)));
                myImplementation.Add(Settings.NoImplementationDefault);
                myImplementation.Add(string.Format("        {0}", Settings.LangStyle.GetEnclosureCloseToken(this)));
                return myImplementation.ToArray();

            }
            _myImplementation = MyOriginalLines();
            return _myImplementation;
        }

        /// <summary>
        /// Returns the lines, sourced from the PDB, exactly as-is from the original source (null otherwise).
        /// </summary>
        /// <returns></returns>
        public string[] MyOriginalLines()
        {
            if (PdbModuleSymbols == null || string.IsNullOrWhiteSpace(PdbModuleSymbols.file) || !File.Exists(PdbModuleSymbols.file))
                return null;

            var content = File.ReadAllLines(PdbModuleSymbols.file);
            var start = GetMyStartEnclosure(content);
            var end = GetMyEndEnclosure(content);

            var bodyOut = new List<string> {content[start.Item1].Substring(start.Item2)};

            for (var i = start.Item1+1; i < end.Item1; i++)
            {
                bodyOut.Add(content[i]);
            }
            bodyOut.Add(content[end.Item1].Substring(0,end.Item2));

            return bodyOut.ToArray();
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

            if (PdbModuleSymbols == null || PdbModuleSymbols.firstLine == null)
                return null;

            var minStartAt = PdbModuleSymbols.firstLine.lineNumber;

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

            //set source to pristine for regex to get a match
            if (!isClean)
            {
                srcFile = RefactorExtensions.CleanSrcFile(srcFile);
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
            //only do this once-per-instance
            if (_myEndEnclosure != null) return _myEndEnclosure;

            //validate we have everything needed to go foward
            if (srcFile == null || srcFile.Length <= 0)
                return null;
            if (PdbModuleSymbols == null || PdbModuleSymbols.firstLine == null || PdbModuleSymbols.lastLine == null)
                return null;

            //set source to pristine for tokens to operate on
            if (!isClean)
            {
                srcFile = RefactorExtensions.CleanSrcFile(srcFile);
            }

            //will resolve the start to lock in on the very next token pair
            var myStartEnclosure = GetMyStartEnclosure(srcFile, true);

            if (myStartEnclosure == null)
                return null;

            var openToken = Settings.LangStyle.GetEnclosureOpenToken(this);
            var closeToken = Settings.LangStyle.GetEnclosureCloseToken(this);

            //handle single line enclosures differently
            if (GetSingleLineEndEnclosure(srcFile, myStartEnclosure))
            {
                return _myEndEnclosure;
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

            List<Token> myTokens = null;
            try
            {
                //get the token frame - this will throw an exception if its out-of-balance
                myTokens = myFilesFrame.FindEnclosingTokens(srcFileStr);
            }
            catch (OutOfBalanceException)
            {
                return GetDefaultEndEnclosure(srcFile);
            }

            if (myTokens == null)
                return GetDefaultEndEnclosure(srcFile);

            myTokens = myTokens.OrderBy(x => x.Start).ToList();

            //need the enclosure's start to be comparable to the tokens array
            var charCount = myStartEnclosure.Item2;
            
            for (var j = 1; j < myStartEnclosure.Item1; j++)
            {
                if (srcFile.Length < j)
                    break;

                charCount += srcFile[j].Length + Environment.NewLine.Length;
            }

            //we want the very first token we can find which started just after our method signature
            var myToken = myTokens.FirstOrDefault(x => x.Start > charCount);

            if (myToken == null)
                return GetDefaultEndEnclosure(srcFile);

            //need to reverse the Token's End back into a line number
            charCount = 0;
            for (var k = 0; k < srcFile.Length; k++)
            {
                //if we go to the next line will be blow past the token's end?
                if (charCount + srcFile[k].Length + Environment.NewLine.Length < myToken.End)
                {
                    charCount += srcFile[k].Length + Environment.NewLine.Length;
                    continue;
                }
                //this should be the index in line k
                var sl = myToken.End - charCount;

                _myEndEnclosure = new Tuple<int, int>(k, sl);
                return _myEndEnclosure;
            }

            return GetDefaultEndEnclosure(srcFile);
        }
        #endregion

        #region internal helpers

        internal Tuple<int, int> GetDefaultEndEnclosure(string[] srcFile)
        {
            var closeToken = Settings.LangStyle.GetEnclosureCloseToken(this);
            var maxEndAt = PdbModuleSymbols.lastLine.lineNumber;

            for (var i = maxEndAt; i > 0; i--)
            {
                if (!srcFile[i].Contains(closeToken))
                    continue;
                var idx = srcFile[i].LastIndexOf(closeToken, StringComparison.Ordinal);
                _myEndEnclosure = new Tuple<int, int>(i, idx + closeToken.Length);
            }
            return _myEndEnclosure;
        }

        internal bool GetSingleLineEndEnclosure(string[] srcFile, Tuple<int, int> myStartEnclosure)
        {
            var openToken = Settings.LangStyle.GetEnclosureOpenToken(this);
            var closeToken = Settings.LangStyle.GetEnclosureCloseToken(this);
            var minStartAt = PdbModuleSymbols.firstLine.lineNumber;
            var maxEndAt = PdbModuleSymbols.lastLine.lineNumber;

            if (minStartAt > 0 && minStartAt != maxEndAt) return false;

            if (myStartEnclosure == null || myStartEnclosure.Item1 < 0 || myStartEnclosure.Item1 >= srcFile.Length)
                return false;

            //get this line
            var autoPropLine = srcFile[myStartEnclosure.Item1];
            var idxInLine = myStartEnclosure.Item2 < 0 || myStartEnclosure.Item2 >= autoPropLine.Length
                ? 0
                : myStartEnclosure.Item2;

            //reduce the line to where we know the auto-prop starts
            autoPropLine = autoPropLine.Substring(idxInLine);

            //now find the very first occurance of the open-token
            idxInLine = autoPropLine.IndexOf(openToken, StringComparison.Ordinal);

            //if we can move one index past this then that is our min
            if (idxInLine <= 0 || idxInLine + 1 > autoPropLine.Length)
                return false;
            minStartAt = idxInLine + 1;

            //if there is a close token already on this line it must be our ending
            autoPropLine = autoPropLine.Substring(minStartAt);
            if (autoPropLine.Contains(closeToken))
            {
                idxInLine = autoPropLine.IndexOf(closeToken, StringComparison.Ordinal);
                idxInLine -= 1;
                idxInLine = (srcFile[myStartEnclosure.Item1].Length - autoPropLine.Length) + idxInLine;
                _myEndEnclosure = new Tuple<int, int>(myStartEnclosure.Item1, idxInLine);

                return true;
            }
            return false;
        }

        internal CgType MyCgType { get { return _myCgType; } set { _myCgType = value; } }
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
