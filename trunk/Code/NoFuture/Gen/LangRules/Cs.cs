using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Antlr.DotNetIlTypeName;
using NoFuture.Shared.Core;
using NoFuture.Tokens;
using NoFuture.Util.Core;

namespace NoFuture.Gen.LangRules
{
    public class Cs : ILangStyle
    {
        private const string BLOCK_MARKER = "//NoFuture.Gen.LangRules.Cs.BLOCK_MARKER";

        public const string LINE_COMMENT_CHAR_SEQ = "//";
        public const char C_OPEN_CURLY = '{';
        public const char C_CLOSE_CURLY = '}';
        public const string CS_GET = "get;";
        public const string CS_SET = "set;";
        public const string CONST = "const";
        public const string STMT_TERM = ";";

        public string LineComment => LINE_COMMENT_CHAR_SEQ;
        public string DeclareConstantKeyword => CONST;
        public Dictionary<string, string> ValueTypeToLangAlias => NfReflect.ValueType2Cs;

        public string GetEnclosureOpenToken(CgMember cgMem)
        {
            return C_OPEN_CURLY.ToString();
        }

        public string GetEnclosureCloseToken(CgMember cgMem)
        {
            return C_CLOSE_CURLY.ToString();
        }

        public string GetStatementTerminator()
        {
            return STMT_TERM;
        }

        public string GetDecoratorRegex()
        {
            return @"\x5B[^\x5D]*\x5D";
        }

        public string ToDecl(CgMember cgMem, bool includesAccessModifier = false)
        {
            var cgArgs = cgMem.Args.Select(cgArg => String.Format("{0} {1}", cgArg.ArgType, cgArg.ArgName)).ToList();

            var cgSig = string.Join(",", cgArgs);
            var tn = cgMem.IsCtor ? string.Empty : cgMem.TypeName;
            var n = cgMem.IsCtor
                ? NfReflect.GetTypeNameWithoutNamespace(cgMem.TypeName)
                : string.Format(" {0}", cgMem.Name);

            cgSig = cgSig.Length > 0 || cgMem.IsCtor || cgMem.IsMethod
                ? string.Format("{0}{1}({2})", tn, n, cgSig)
                : string.Format("{0}{1}", tn, n);

            return includesAccessModifier
                ? string.Format("{0} {1}", TransposeCgAccessModToString(cgMem.AccessModifier), cgSig)
                : cgSig;
        }

        public string ToDecl(CgType cgType, string variableName, CgAccessModifier accessMod)
        {
            return string.Format("{2} {0} {1} = new {0}();", cgType.FullName, variableName,
                TransposeCgAccessModToString(accessMod));
        }

        public string ToStmt(CgMember cgMem, string cgNamespace, string cgInvokeOnName)
        {
            var paramNames = string.Format("({0});",
                string.Join(",",
                    cgMem.Args.Select(
                        x => x.ArgType.StartsWith("ref ") ? string.Format("ref {0}", x.ArgName) : x.ArgName)));

            if (cgMem.IsCtor)
                return string.Format("new {0}{1}", cgMem.TypeName, paramNames);

            var stmtBldr = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(cgNamespace))
                stmtBldr.AppendFormat("{0}.", cgNamespace);
            if (!string.IsNullOrWhiteSpace(cgInvokeOnName))
                stmtBldr.AppendFormat("{0}.", cgInvokeOnName);

            stmtBldr.Append(cgMem.Name);
            stmtBldr.Append(paramNames);

            return cgMem.TypeName == "void" ? stmtBldr.ToString() : string.Format("return {0}", stmtBldr);
        }

        public string ToClass(CgType cgType, CgAccessModifier cgClsAccess, CgClassModifier typeModifier, string[] nsImports)
        {
            var hasNs = !string.IsNullOrWhiteSpace(cgType.Namespace);
            var splitFileContent = new StringBuilder();
            if (nsImports != null && nsImports.Length > 0)
            {
                splitFileContent.AppendLine(string.Join(Environment.NewLine, nsImports));
            }

            if (hasNs)
            {
                splitFileContent.AppendFormat("namespace {0}{1}", cgType.Namespace, Environment.NewLine);
                splitFileContent.AppendLine(C_OPEN_CURLY.ToString());

            }
            var otherModifer = typeModifier == CgClassModifier.AsIs
                ? string.Empty
                : string.Format(" {0}", Settings.LangStyle.TransposeCgClassModToString(typeModifier));
            splitFileContent.AppendLine("    [Serializable]");
            splitFileContent.AppendLine(string.Format("    {0}{1} class {2}",
                Settings.LangStyle.TransposeCgAccessModToString(cgClsAccess), otherModifer, cgType.Name));
            splitFileContent.AppendLine("    " + C_OPEN_CURLY);
            foreach (var cg in cgType.Fields)
                splitFileContent.AppendLine(string.Join(Environment.NewLine, cg.GetMyCgLines()));
            foreach (var cg in cgType.Properties)
                splitFileContent.AppendLine(string.Join(Environment.NewLine, cg.GetMyCgLines()));
            foreach (var cg in cgType.Methods)
                splitFileContent.AppendLine(string.Join(Environment.NewLine, cg.GetMyCgLines()));
            splitFileContent.AppendLine("    " + C_CLOSE_CURLY);

            if (hasNs)
                splitFileContent.AppendLine(C_CLOSE_CURLY.ToString());

            return splitFileContent.ToString();
        }

        public CgArg ToParam(CgMember cgMem, bool asFunctionPtr)
        {
            if (!asFunctionPtr)
            {
                var isReadOnlyProp = cgMem.HasGetter && !cgMem.HasSetter;

                var addByRef = (cgMem.TypeName == "string" || cgMem.TypeName == "System.String" ||
                                IsPrimitiveTypeName(cgMem.TypeName)) && !isReadOnlyProp;

                return new CgArg
                {
                    ArgName = cgMem.Name,
                    ArgType = (addByRef ? string.Format("ref {0}", cgMem.TypeName) : cgMem.TypeName)
                };

            }

            if (string.IsNullOrWhiteSpace(cgMem.TypeName) || cgMem.TypeName == "void" || cgMem.TypeName == "System.Void")//as an Action
            {
                if (cgMem.Args.Count <= 0)
                    return new CgArg { ArgName = cgMem.Name, ArgType = "Action" };

                var actBldr = new StringBuilder();
                actBldr.Append("Action<");
                actBldr.Append(string.Join(", ", cgMem.Args.Select(x => x.ArgType)));
                actBldr.Append(">");
                return new CgArg { ArgName = cgMem.Name, ArgType = actBldr.ToString() };
            }

            var funcBldr = new StringBuilder();
            funcBldr.Append("Func<");
            if (cgMem.Args.Count > 0)
            {
                funcBldr.Append(string.Join(", ", cgMem.Args.Select(x => x.ArgType)));
                funcBldr.Append(", ");
            }
            funcBldr.AppendFormat("{0}>", cgMem.TypeName);
            return new CgArg { ArgName = cgMem.Name, ArgType = funcBldr.ToString() };
        }

        public string ToInvokeRegex(CgMember cgMem, params string[] varNames)
        {
            if (string.IsNullOrWhiteSpace(cgMem.Name))
                return ".";

            var regexPattern = new StringBuilder();

            if (cgMem.MyCgType != null && (cgMem.IsEnum || cgMem.IsStatic || cgMem.MyCgType.IsEnum))
            {
                regexPattern.Append(cgMem.MyCgType.RegexPatternName);
            }
            else
            {
                if (varNames.Length > 0)
                {
                    regexPattern.Append("(");
                    regexPattern.Append(string.Join(@"\x2e|",
                        varNames.Select(x => x.EscapeString())));
                    regexPattern.Append(@"\x2e");
                    regexPattern.Append(")");
                }
                else
                {
                    regexPattern.Append(@"((this\x2e)|(?<!\.))");
                }
                
            }

            regexPattern.AppendFormat(@"\b{0}\b", cgMem.Name);

            if (cgMem.IsGeneric)
            {
                //needs to handle crazy shit like 'global::System.Tuple<int, Func<Mynamespace.MyType, global::System.Int64>>'
                regexPattern.Append(@"\x3c.*?\x3e");
            }

            if (cgMem.MyCgType != null && cgMem.MyCgType.IsEnum && cgMem.MyCgType.EnumValueDictionary.Values.Count > 0)
            {
                regexPattern.Append("(");
                regexPattern.Append(string.Join("|", cgMem.MyCgType.EnumValueDictionary.Values.SelectMany(x => x)));
                regexPattern.Append(")");
            }
            else if (cgMem.Args.Count > 0)
                regexPattern.AppendFormat(@"\s*?\({0}\)", string.Join(@"\,", cgMem.Args.Select(x => x.AsInvokeRegexPattern)));
            else if (cgMem.IsMethod)
                regexPattern.Append(@"\(\s*?\)");

            return regexPattern.ToString();
        }

        public string ToSignatureRegex(CgMember cgMem)
        {
            var regexPattern = new StringBuilder();

            regexPattern.Append(@"(\b" + TransposeCgAccessModToString(cgMem.AccessModifier) + @"\b)");

            if (cgMem.AccessModifier == CgAccessModifier.Assembly)
                regexPattern.Append("?");

            regexPattern.Append(@"([^\x2c\x3b\x7d\x7b]+?)");

            regexPattern.AppendFormat(@"\b{0}\b", cgMem.Name);
            if (cgMem.IsGeneric)
            {
                //needs to handle crazy shit like 'global::System.Tuple<int, Func<Mynamespace.MyType, global::System.Int64>>'
                regexPattern.Append(@"\x3c.*?\x3e");
            }

            if (!cgMem.IsMethod)
                return regexPattern.ToString();

            if (cgMem.Args.Count <= 0)
            {
                regexPattern.Append(@"\(\s*?\)");
                return regexPattern.ToString();
            }

            var simpleArgTypes =
                cgMem.Args.Select(
                    x =>
                        @"\s*\b" +
                        NfReflect.GetTypeNameWithoutNamespace(x.ArgType).EscapeString() +
                        @"\b\s*([^\,]+?)").ToList();
            regexPattern.AppendFormat(@"\s*\({0}\)", string.Join(@"\,", simpleArgTypes));

            return regexPattern.ToString();
        }

        public string NoImplementationDefault
        {
            get { return "//could not parse method body \nthrow new NotImplementedException();"; }
        }

        public string[] RemovePreprocessorCmds(string[] fileMembers)
        {
            if (fileMembers == null)
                return null;

            for (var j = 0; j < fileMembers.Length; j++)
            {
                if (fileMembers[j].Contains("#") && (Regex.IsMatch(fileMembers[j], @"^\s*\#.*\n") || Regex.IsMatch(fileMembers[j], @"^\s*\#.*$")))
                {
                    fileMembers[j] = new string(' ', fileMembers[j].Length);
                }
            }

            return fileMembers;
        }

        public string[] RemoveBlockComments(string[] fileMembers, Tuple<char, char> openningBlockChars, Tuple<char, char> closingBlockChars)
        {
            const char BLANK_CHAR = ' ';
            var inBlock = false;
            var newLines = new List<string>();
            foreach (var ln in fileMembers)
            {
                var newLine = new StringBuilder();
                var lnChars = ln.ToCharArray();
                for (var i = 0; i < lnChars.Length; i++)
                {
                    var prevChar = i - 1 < 0 ? null : new char?(lnChars[i - 1]);
                    var currentChar = i >= lnChars.Length ? null : new char?(lnChars[i]);
                    var nextChar = i + 1 >= lnChars.Length ? null : new char?(lnChars[i + 1]);

                    var atBlockStart = (currentChar != null && openningBlockChars.Item1 == currentChar.Value &&
                                        nextChar != null &&
                                        openningBlockChars.Item2 == nextChar.Value && !inBlock);
                    var atBlockEnd = (prevChar != null && closingBlockChars.Item1 == prevChar.Value &&
                                      currentChar != null &&
                                      closingBlockChars.Item2 == currentChar.Value && inBlock);
                    if (atBlockStart)
                        inBlock = true;
                    if (atBlockEnd)
                        inBlock = false;
                    if (currentChar != null && (inBlock || atBlockEnd))
                    {
                        newLine.Append(BLANK_CHAR);
                    }
                    else
                    {
                        newLine.Append(currentChar);    
                    }
                    

                }

                newLines.Add(newLine.Length > 0 ? newLine.ToString() : string.Empty);

            }
            return newLines.ToArray();
        }

        public string[] RemoveBlockComments(string[] fileMembers)
        {
            return RemoveBlockComments(fileMembers, new Tuple<char, char>('/', '*'), new Tuple<char, char>('*', '/'));
        }

        public string[] RemoveLineComments(string[] fileMembers, string lineCommentSequence)
        {
            if (String.IsNullOrEmpty(lineCommentSequence))
                lineCommentSequence = LineComment;
            if (fileMembers == null)
                return null;
            const char BLANK_CHAR = ' ';

            //leverage the logic concering string literals as the logical base
            var irregular = false;
            var operationalFileMembers =
                fileMembers.Select(x => EscStringLiterals(x, EscapeStringType.BLANK, ref irregular)).ToArray();
            for (var j = 0; j < fileMembers.Length; j++)
            {
                if (!operationalFileMembers[j].Contains(lineCommentSequence))
                    continue;
                var newLine = new StringBuilder();
                var operationFileLine = operationalFileMembers[j].ToCharArray();
                var actualFileLine = fileMembers[j].ToCharArray();
                var inCommentSection = false;
                for (var i = 0; i < operationFileLine.Length; i++)
                {
                    if (inCommentSection)
                    {
                        newLine.Append(BLANK_CHAR);
                        continue;
                    }
                    if (operationFileLine[i] != lineCommentSequence[0])
                    {
                        //return the the values from the original, not the its logical base
                        newLine.Append(actualFileLine[i]);
                        continue;
                    }
                    if (i + lineCommentSequence.Length >= operationFileLine.Length)
                        break;

                    //at this point we know that the char at 'i' matches and we have more chars ahead...
                    if (new String(operationFileLine.Skip(i).Take(lineCommentSequence.Length).ToArray()) == lineCommentSequence)
                    {
                        inCommentSection = true;
                        newLine.Append(BLANK_CHAR);
                    }
                }
                fileMembers[j] = newLine.ToString();

            }

            return fileMembers;
        }

        public string EscStringLiterals(string lineIn, EscapeStringType replacement, ref bool endedIrregular)
        {
            var encodedList = new StringBuilder();
            var inDoubleQuotes = endedIrregular;
            var buffer = lineIn.ToCharArray();
            for (var j = 0; j < buffer.Length; j++)
            {
                var isDblQuote = buffer[j] == '"';
                var isEsc = j - 1 > 0 && buffer[j - 1] == (char)0x5C;
                isEsc = isEsc && j - 2 > 0 && buffer[j - 2] != (char)0x5C;
                
                if (isDblQuote && !isEsc)
                {
                    encodedList.Append('"');
                    inDoubleQuotes = !inDoubleQuotes;
                    continue;
                }

                if (inDoubleQuotes)
                    encodedList.Append(buffer[j].ToString(CultureInfo.InvariantCulture).EscapeString(replacement));
                else
                    encodedList.Append(buffer[j]);

            }
            endedIrregular = inDoubleQuotes;
            return encodedList.ToString();
        }

        public Dictionary<int, StringBuilder> ExtractAllStringLiterals(string lineIn)
        {
            var stringLiterals = new Dictionary<int, StringBuilder>();
            if (string.IsNullOrWhiteSpace(lineIn))
                return stringLiterals;

            var inDoubleQuotes = false;
            var buffer = lineIn.ToCharArray();
            var currentIdx = -1;
            var isHereString = false;
            for (var j = 0; j < buffer.Length; j++)
            {
                var isDblQuote = buffer[j] == '"';
                var isEsc = j - 1 > 0 && buffer[j - 1] == (char) 0x5C;
                if (!isHereString && j - 1 > 0 && buffer[j - 1] == '@')
                    isHereString = true;
                isEsc =  !isHereString && isEsc && j - 2 > 0 && buffer[j - 2] != (char)0x5C;

                if (isDblQuote && !isEsc)
                {
                    inDoubleQuotes = !inDoubleQuotes;
                    if (inDoubleQuotes)
                    {
                        currentIdx = isHereString ? j - 1 : j;
                        stringLiterals.Add(currentIdx, new StringBuilder());
                    }

                    if (!inDoubleQuotes && isHereString)
                        isHereString = false;

                    continue;
                }

                if (inDoubleQuotes)
                {
                    stringLiterals[currentIdx].Append(buffer[j]);
                }
            }
            return stringLiterals;
        }

        public string[] ExtractNamespaceImportStatements(string[] codeFileLines)
        {
            if (codeFileLines == null || codeFileLines.Length == 0)
                return null;

            //get lines as a continuous stream
            var buffer = FlattenCodeToCharStream(codeFileLines);

            var mark = 0;

            var markFirstCurly = 0;

            for (var i = 0; i < buffer.Count; i++)
            {
                if (mark > 0)
                    break;

                //count foward till the first '{'
                if (buffer[i] != C_OPEN_CURLY)
                {
                    markFirstCurly = i;
                    continue;
                }

                //now go backward to the first semi-colon
                for (var j = i; j >= 0; j--)
                {
                    if (buffer[j] != ';') continue;
                    mark = j;
                    break;
                }
            }
            if(markFirstCurly == 0)
                return new[] {string.Empty};

            var linesNeedSemicolon = new List<string>();
            var newlines = mark == 0 ? String.Empty : new string(buffer.Take(mark).ToArray());
            linesNeedSemicolon.AddRange(newlines.Split(';'));

            //using statements may appear within a namespace decl when they appear above all other decls
            if (markFirstCurly > 0)
            {
                var nextCurly = 0;
                for (var k = markFirstCurly+2; k < buffer.Count; k++)
                {
                    if (buffer[k] != '{') continue;
                    nextCurly = k;
                    break;
                }

                //we have to look for keywords now
                if (nextCurly > 0 && nextCurly > markFirstCurly)
                {
                    var nsLvlUsings = new string(buffer.Skip(markFirstCurly+2).Take(nextCurly-markFirstCurly-2).ToArray());
                    var csLines = nsLvlUsings.Split(';').Select(x => x.Trim()).ToList();
                    if (csLines.Count > 0)
                    {
                        foreach (var ln in csLines)
                        {
                            if (Regex.IsMatch(ln, @"using\W+[a-zA-Z0-9_\x3D\x2E\x20\x3A]*"))
                                linesNeedSemicolon.Add(ln);
                        }
                    }
                }
            }
            if (linesNeedSemicolon.Count <= 0)
                return new[] {string.Empty};
            return linesNeedSemicolon.Where(x => !string.IsNullOrWhiteSpace(x)).Select(l => $"{l};").ToArray();
        }

        public List<char> FlattenCodeToCharStream(string[] fileContent)
        {
            if (fileContent == null || fileContent.Length == 0)
                return null;

            fileContent = RemovePreprocessorCmds(fileContent);
            fileContent = RemoveBlockComments(fileContent);
            fileContent = RemoveLineComments(fileContent, LINE_COMMENT_CHAR_SEQ);

            var buffer = new List<char>();
            foreach (var ln in fileContent)
            {
                //need to perserve keywords, but control chars may be trimmed.
                var distillLn = ln.DistillCrLf().ToCharArray();
                var enclosedInQuotes = false;
                for (var j = 0; j < distillLn.Length; j++)
                {
                    if (Char.IsControl(distillLn[j]))
                        continue;
                    if (distillLn[j] == '"' && j - 1 > 0 && distillLn[j - 1] != '\\')
                        enclosedInQuotes = !enclosedInQuotes;
                    if (enclosedInQuotes)
                    {
                        buffer.Add(distillLn[j]);
                        continue;
                    }
                    var charToLeftIsCurly = buffer.Count > 0 &&
                                            (buffer[buffer.Count - 1] == C_OPEN_CURLY || buffer[buffer.Count - 1] == C_CLOSE_CURLY);

                    //only add the space when the next char is a letter or number and last char was not a curly
                    if (Char.IsWhiteSpace(distillLn[j]) &&
                        ((j + 1 < distillLn.Length && !Char.IsLetterOrDigit(distillLn[j + 1])) || charToLeftIsCurly ||
                         j == 0))
                        continue;
                    buffer.Add(distillLn[j]);
                }
            }
            return buffer;
        }

        public bool TryDeriveTypeNameFromFile(string[] codeFileLines, out string typeName)
        {
            const string NS = @"([\s\;]|^)namespace\s([a-zA-Z_][a-zA-Z0-9_\.]+)";
            const string CLASS = @"([\s\;\{]|^)class\s([a-zA-Z_][a-zA-Z0-9_\.]+)";
            var nsRegex = new Regex(NS);
            var clsRegex = new Regex(CLASS);
            typeName = null;
            if (codeFileLines == null || codeFileLines.Length <= 0)
                return false;
            var buffer = new string(FlattenCodeToCharStream(codeFileLines).ToArray());

            if (clsRegex.IsMatch(buffer))
            {
                var clsGrps = clsRegex.Matches(buffer);
                if (clsGrps.Count <= 0)
                    return false;
                var clsGrpGroups = clsGrps[0].Groups;
                if (clsGrpGroups.Count <= 2)
                    return false;
                typeName = clsGrpGroups[2].Value;
            }

            if (!nsRegex.IsMatch(buffer)) return !string.IsNullOrWhiteSpace(typeName);

            var nsGrps = nsRegex.Matches(buffer);
            if (nsGrps.Count <= 0)
                return !string.IsNullOrWhiteSpace(typeName);
            var nsGrpGroups = nsGrps[0].Groups;
            if (nsGrpGroups.Count <= 2)
                return !string.IsNullOrWhiteSpace(typeName);

            var nsPart = $"{nsGrpGroups[2].Value}.";
            typeName = $"{nsPart}{typeName}";
            return !string.IsNullOrWhiteSpace(typeName);
        }

        public string[] CleanupPdbLinesCodeBlock(string[] codeBlockLines)
        {
            if (codeBlockLines == null || codeBlockLines.Length == 0)
                return new[] { String.Empty };
            codeBlockLines = RemovePreprocessorCmds(codeBlockLines);
            codeBlockLines = RemoveLineComments(codeBlockLines, LINE_COMMENT_CHAR_SEQ);
            codeBlockLines = RemoveBlockComments(codeBlockLines);
            codeBlockLines = codeBlockLines.Where(ln => !String.IsNullOrWhiteSpace(ln)).ToArray();

            if (codeBlockLines.Length == 0)
                return new[] { String.Empty };

            var curlyCount = EnclosuresCount(codeBlockLines);

            if (curlyCount == 0)
                return
                    CleanupPdbLinesCodeBlockAddTryKeyword(
                        codeBlockLines.Where(ln => !String.IsNullOrWhiteSpace(ln)).ToArray());

            //get last line which must contain non-whitespace
            var lastLine = codeBlockLines[(codeBlockLines.Length - 1)];

            //this is the case when extra closing curly's are appended to the end
            if (curlyCount < 0)
            {
                for (var j = curlyCount; j < 0; j++)
                {
                    var kd = lastLine.LastIndexOf(C_CLOSE_CURLY);

                    //this implies the the block is indeed out of balance but the fix isn't so simple
                    if (kd < 0)
                    {
                        //mark it an leave
                        codeBlockLines[(codeBlockLines.Length - 1)] = lastLine + BlockMarker;
                        return codeBlockLines.Where(ln => !String.IsNullOrWhiteSpace(ln)).ToArray();
                    }

                    codeBlockLines[(codeBlockLines.Length - 1)] = lastLine.Substring(0, kd);

                    codeBlockLines = codeBlockLines.Where(ln => !String.IsNullOrWhiteSpace(ln)).ToArray();

                }

                return CleanupPdbLinesCodeBlockAddTryKeyword(codeBlockLines);
            }

            //this is the case when there are extra openning curly's at the beginning
            var newLastLine = new StringBuilder();
            newLastLine.Append(lastLine);
            for (var m = 0; m < curlyCount; m++)
            {
                newLastLine.Append(C_CLOSE_CURLY);
            }
            codeBlockLines[(codeBlockLines.Length - 1)] = newLastLine.ToString();

            return CleanupPdbLinesCodeBlockAddTryKeyword(codeBlockLines.Where(ln => !String.IsNullOrWhiteSpace(ln)).ToArray());
        }

        //HACK - PDB will drop a line having only the keyword 'try' when its the very first line in the body...
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static string[] CleanupPdbLinesCodeBlockAddTryKeyword(string[] codeBlockLines)
        {
            var cbl = codeBlockLines.Where(ln => !String.IsNullOrWhiteSpace(ln)).ToArray();
            if (cbl.Length <= 0)
                return codeBlockLines;
            if (cbl[0].Trim().ToCharArray()[0] == C_OPEN_CURLY)
            {
                var dd = new StringBuilder();
                var dk = cbl[0].IndexOf(C_OPEN_CURLY);
                dd.Append(cbl[0].Substring(0, dk));
                dd.Append("try");
                dd.Append(cbl[0].Substring(dk));
                cbl[0] = dd.ToString();
            }

            return cbl;
        }

        public string TransformClrTypeSyntax(string typeToString)
        {
            if (string.IsNullOrWhiteSpace(typeToString))
                return "object";

            var tnpi = TypeNameParseTree.ParseIl(typeToString);
            var bldr = new StringBuilder();
            ConvertToCs(tnpi, bldr);
            return bldr.ToString().Replace("+",".");

        }

        public string TransformClrTypeSyntax(Type type)
        {
            if (type == null)
                return "object";

            return TransformClrTypeSyntax(type.ToString());
        }

        public bool IsPrimitiveTypeName(string someTypeName)
        {
            if (String.IsNullOrWhiteSpace(someTypeName))
                return false;
            someTypeName = someTypeName.Trim();

            var dotNetNames = ValueTypeToLangAlias.Keys.Where(x => x != "System.String" && x != "System.Void").ToList();
            var csNames = ValueTypeToLangAlias.Values.Where(x => x != "string" && x != "void").ToList();
            return dotNetNames.Any(x => String.Equals(x, someTypeName)) ||
                   csNames.Any(x => String.Equals(x, someTypeName));
        }

        public string TransposeCgAccessModToString(CgAccessModifier am)
        {
            switch (am)
            {
                case CgAccessModifier.Assembly:
                    return "internal";
                case CgAccessModifier.Family:
                    return "protected";
                case CgAccessModifier.FamilyAssembly:
                    return "protected internal";
                case CgAccessModifier.Private:
                    return "private";
                default:
                    return "public";
            }
        }

        public string TransposeCgClassModToString(CgClassModifier cm)
        {
            switch (cm)
            {
                case CgClassModifier.AsAbstract:
                    return "abstract";
                case CgClassModifier.AsPartial:
                    return "partial";
                case CgClassModifier.AsStatic:
                    return "static";
                default:
                    return String.Empty;
            }
        }

        public string GenUseIsNotDefaultValueTest(string someTypeName, string variableName)
        {
            if (String.IsNullOrWhiteSpace(variableName))
                return string.Empty;
            if (String.IsNullOrWhiteSpace(someTypeName))
                return $"{variableName} != null";

            if (someTypeName == "System.String" || someTypeName == "string")
                return $"!string.IsNullOrWhiteSpace({variableName})";

            if (IsPrimitiveTypeName(someTypeName))
            {
                if (ValueTypeToLangAlias.ContainsKey(someTypeName))
                    someTypeName = ValueTypeToLangAlias[someTypeName];
                switch (someTypeName)
                {
                    case "byte":
                    case "short":
                    case "int":
                    case "long":
                    case "double":
                    case "decimal":
                        return $"{variableName} != 0";
                    case "char":
                        return $"(byte){variableName} != 0";
                    case "bool":
                        return $"!{variableName}";
                }
            }
            return String.Format("{0} != null", variableName);
        }

        public bool TryFindFirstLineInClass(string typename, string[] srcFileLines, out int firstLine)
        {
            firstLine = 1;
            if (srcFileLines == null)
                return false;
            var srcFile = srcFileLines.ToArray();
            if (String.IsNullOrWhiteSpace(typename))
                return false;
            if (srcFile.Length <= 0)
                return false;

            //these preserve all lines
            srcFile = RemoveLineComments(srcFile, LINE_COMMENT_CHAR_SEQ);
            srcFile = RemoveBlockComments(srcFile);

            var joinedContent = String.Join("\n", srcFile);

            var myXDocFrame = new XDocFrame(C_OPEN_CURLY, C_CLOSE_CURLY);

            var mytokens = myXDocFrame.FindEnclosingTokens(joinedContent);
            if (mytokens == null || mytokens.Count < 1)
                return false;
            var targetToken = NfReflect.GetTypeNameWithoutNamespace(typename) == typename ? mytokens[0] : mytokens[1];

            var joinedContentAsChars = joinedContent.ToCharArray();
            //need to count the number of newlines up to the token's start
            for (var i = 0; i <= targetToken.Start; i++)
            {
                if (joinedContentAsChars[i] != '\n')
                    continue;
                firstLine += 1;
            }

            return firstLine > 1;
        }

        public bool TryFindLastLineInClass(string typename, string[] srcFileLines, out int lastLine)
        {
            lastLine = int.MaxValue;
            if (srcFileLines == null)
                return false;
            var srcFile = srcFileLines.ToArray();
            if (String.IsNullOrWhiteSpace(typename))
                return false;
            if (srcFile.Length <= 0)
                return false;

            lastLine = srcFile.Length;

            //these preserve all lines
            srcFile = RemoveLineComments(srcFile, LINE_COMMENT_CHAR_SEQ);
            srcFile = RemoveBlockComments(srcFile);

            var joinedContent = string.Join("\n", srcFile);
            var myXDocFrame = new XDocFrame(C_OPEN_CURLY, C_CLOSE_CURLY);

            var mytokens = myXDocFrame.FindEnclosingTokens(joinedContent);
            if (mytokens == null || mytokens.Count < 1)
                return false;
            var targetToken = NfReflect.GetTypeNameWithoutNamespace(typename) == typename ? mytokens[0] : mytokens[1];

            var joinedContentAsChars = joinedContent.ToCharArray();

            //need to count the number of newlines from the bottom up to token's end
            for (var i = joinedContent.Length - 1; i >= targetToken.End; i--)
            {
                if (joinedContentAsChars[i] != '\n')
                    continue;
                lastLine -= 1;
            }
            //add one more
            if (lastLine < srcFile.Length)
                lastLine -= 1;

            return lastLine < srcFile.Length;
        }

        public string BlockMarker => BLOCK_MARKER;

        public bool IsOddNumberEnclosures(string[] codeBlockLines)
        {
            //start at the first line, count +1 for '{' and -1 for '}'
            var buffer = FlattenCodeToCharStream(codeBlockLines);
            return IsOddNumberCurlyBraces(buffer);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal bool IsOddNumberCurlyBraces(List<char> buffer)
        {
            return CurlyBraceCount(buffer) != 0;
        }

        /// <summary>
        /// Returns a integer whose value represents the  balance of curly braces (0x7B and 0x7D)
        /// within the <see cref="codeBlockLines"/> where '0' means its balanced a negative value
        /// indicates there are too many closing braces while a positive indicates the opposite.
        /// </summary>
        /// <param name="codeBlockLines"></param>
        /// <returns>The balance of curly braces.</returns>
        /// <remarks>
        /// The balance is calculated on a flattened stream of chars.
        /// Appearances behind block comments, line comments, preprocessors and string literals
        /// are not included.
        /// </remarks>
        public int EnclosuresCount(string[] codeBlockLines)
        {
            var buffer = FlattenCodeToCharStream(codeBlockLines);
            return CurlyBraceCount(buffer);

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static int CurlyBraceCount(List<char> buffer)
        {
            var tokenCount = 0;
            var inDoubleQuotes = false;
            var inSingleQuotes = false;
            for (var j = 0; j < buffer.Count; j++)
            {
                //flag entry and exit into string literals
                if (buffer[j] == '"' && j - 1 > 0 && buffer[j - 1] != '\\')
                    inDoubleQuotes = !inDoubleQuotes;
                if (buffer[j] == '\'' && !inDoubleQuotes && j - 1 > 0 && buffer[j - 1] != '\\')
                    inSingleQuotes = !inSingleQuotes;
                if (buffer[j] == C_OPEN_CURLY && (!inDoubleQuotes || !inSingleQuotes))
                    tokenCount += 1;
                if (buffer[j] == C_CLOSE_CURLY && (!inDoubleQuotes || !inSingleQuotes))
                    tokenCount -= 1;
            }
            return tokenCount;
        }

        internal void ConvertToCs(NfTypeNameParseItem tnpi, StringBuilder bldr)
        {
            bldr = bldr ?? new StringBuilder();
            if (tnpi == null)
                return;

            var fname = tnpi.FullName;
            if (ValueTypeToLangAlias.ContainsKey(fname))
                fname = ValueTypeToLangAlias[fname];

            bldr.Append(fname);

            var hasGenCounter = tnpi.GenericCounter != null && tnpi.GenericCounter > 0;
            var hasGenericItems = tnpi.GenericItems != null && tnpi.GenericItems.Any();

            if (!hasGenCounter && !hasGenericItems)
                return;

            if (hasGenCounter && !hasGenericItems)
            {
                bldr.Append($"<{new string(',', tnpi.GenericCounter.Value - 1)}>");
                return;
            }

            var genericItemsCs = new List<string>();
            foreach (var typeNameParseItem in tnpi.GenericItems)
            {
                var iBldr = new StringBuilder();
                ConvertToCs(typeNameParseItem, iBldr);
                genericItemsCs.Add(iBldr.ToString());
            }
            if (!genericItemsCs.Any())
                return;

            bldr.Append("<");
            bldr.Append(string.Join(",", genericItemsCs));
            bldr.Append(">");
        }
    }
}
