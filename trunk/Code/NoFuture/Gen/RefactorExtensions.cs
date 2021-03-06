﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Shared.Core;
using NoFuture.Shared.DiaSdk.LinesSwitch;
using NoFuture.Util.Core;

namespace NoFuture.Gen
{
    public static class RefactorExtensions
    {
        /// <summary>
        /// Useful for refactoring, finds a list of all instance level fields, properties 
        /// and methods which are used within this member's implementation.  
        /// </summary>
        /// <param name="cgMem"></param>
        /// <returns> </returns>
        /// <remarks>
        /// Depends on PDB data, results are not derived from assembly reflection.
        /// </remarks>
        public static List<CgArg> GetAllRefactorDependencies(this CgMember cgMem)
        {
            if (cgMem.PdbModuleSymbols == null || string.IsNullOrWhiteSpace(cgMem.PdbModuleSymbols.file) ||
                !File.Exists(cgMem.PdbModuleSymbols.file))
                return null;

            var idx = cgMem.PdbModuleSymbols.firstLine.lineNumber;
            var len = cgMem.PdbModuleSymbols.lastLine.lineNumber - idx + 1;

            var ssrcCode =
                File.ReadAllLines(cgMem.PdbModuleSymbols.file)
                    .Skip(idx)
                    .Take(len)
                    .ToArray();

            if (ssrcCode.Length <= 0)
                return null;

            var dependencyArgs = new List<CgArg>();

            var flattenCode = new string(Settings.LangStyle.FlattenCodeToCharStream(ssrcCode).ToArray());
            bool irregular = false;
            flattenCode = Settings.LangStyle.EscStringLiterals(flattenCode, EscapeStringType.UNICODE, ref irregular);

            var instanceMembers =
                cgMem.MyCgType.Fields.Where(f => Regex.IsMatch(flattenCode, f.AsInvokeRegexPattern())).ToList();

            //apply filters since arg names and types may be duplicated with instance variables.
            dependencyArgs.AddRange(
                instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, false))
                    .Where(x => cgMem.Args.All(myArg => !myArg.Equals(x) && dependencyArgs.All(nArg => !nArg.Equals(x)))));

            instanceMembers =
                cgMem.MyCgType.Properties.Where(p => Regex.IsMatch(flattenCode, p.AsInvokeRegexPattern())).ToList();
            dependencyArgs.AddRange(
                instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, false))
                    .Where(x => cgMem.Args.All(myArg => !myArg.Equals(x) && dependencyArgs.All(nArg => !nArg.Equals(x)))));

            instanceMembers =
                cgMem.MyCgType.Methods.Where(
                    m => Regex.IsMatch(flattenCode, m.AsInvokeRegexPattern())).ToList();
            dependencyArgs.AddRange(
                instanceMembers.Select(ic => Settings.LangStyle.ToParam(ic, true))
                    .Where(x => cgMem.Args.All(myArg => !myArg.Equals(x) && dependencyArgs.All(nArg => !nArg.Equals(x)))));

            //arg names common to obj methods will cause confusion
            var objMethods = typeof(object).GetMethods().Select(x => x.Name).ToArray();
            foreach (var dpArg in dependencyArgs)
            {
                if (objMethods.Contains(dpArg.ArgName))
                    dpArg.ArgName = string.Format("{0}{1}", Util.Core.NfString.DefaultNamePrefix, dpArg.ArgName);
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
        /// <param name="cgMem"></param>
        /// <param name="codeBlockLines"></param>
        /// <returns>
        /// A number being the number of matches for ArgName's found starting
        /// on the left.
        /// </returns>
        /// <remarks>
        /// This logic works if the overloaded signatures are ordered by
        /// the number of parameters having highest go first.
        /// </remarks>
        public static int CodeBlockUsesMyArgs(this CgMember cgMem, string[] codeBlockLines)
        {
            if (cgMem.Args.Count == 0)
                return 0;

            //first get a cleaned up copy of the contents so not mislead by comments
            var cleanCodeBlock = Settings.LangStyle.CleanupPdbLinesCodeBlock(codeBlockLines).ToList();

            //start by looking for all names, failing that, drop one off the right and try again till down to one
            for (var i = cgMem.Args.Count; i > 0; i--)
            {
                var argNameList = cgMem.Args.Take(i).Select(arg => arg.ArgName).ToList();
                var prop =
                    argNameList.All(arg => cleanCodeBlock.Any(ln => Regex.IsMatch(ln, string.Format(@"\W{0}\W", arg))));
                if (prop)
                    return i;
            }

            return 0;
        }

        /// <summary>
        /// Moves the given code-gen source code definition to another file 
        /// </summary>
        /// <returns>The full paths to the created source code files </returns>
        public static string[] MoveType(CgType cgType, string outputDirectory = null, CgClassModifier typeModifier = CgClassModifier.AsIs)
        {
            if(cgType == null)
                return new string[0];

            outputDirectory = outputDirectory ?? NfSettings.AppData;

            //TODO - this is inconsistent where enclosure is one but src files are many
            var srcCodeFile = cgType.GetMySrcCodeFiles()?.FirstOrDefault() ??
                              Path.Combine(NfSettings.AppData, cgType.Name + Settings.LangStyle.FileExtension);

            var fileName = cgType.Name;
            var fileExtension = Path.GetExtension(srcCodeFile);
            var outFilePath = Path.Combine(outputDirectory, fileName + fileExtension);

            //don't overwrite whatever is already there
            if (File.Exists(outFilePath))
            {
                outFilePath = Path.Combine(outputDirectory, fileName + NfString.GetNfRandomName() + fileExtension);
            }

            var newlines = new List<string>();
            var hasNs = !string.IsNullOrWhiteSpace(cgType.Namespace);
            if (hasNs)
                newlines.AddRange(Settings.LangStyle.ToNamespaceDecl(cgType).ConvertToLf().Split('\n'));

            if (File.Exists(srcCodeFile))
            {
                var start = cgType.GetMyStartEnclosure().Item1;
                var end = cgType.GetMyEndEnclosure().Item1;
                var srcLines = File.ReadAllLines(srcCodeFile);
                newlines.AddRange(srcLines.Skip(start - 1).Take(end + 1 - start));
            }
            else
            {
                var genLines = typeModifier == CgClassModifier.AsInterface
                    ? Settings.LangStyle.ToInterface(cgType)
                    : Settings.LangStyle.ToClass(cgType, CgAccessModifier.Public, typeModifier);
                newlines.AddRange(genLines.ConvertToLf().Split('\n'));
            }
            if(hasNs)
                newlines.Add(Settings.LangStyle.GetEnclosureCloseToken(null));

            //write the type def to file
            File.WriteAllLines(outFilePath, newlines, Encoding.UTF8);

            return new[] {outFilePath};
        }

        /// <summary>
        /// Replaces the implementation of each <see cref="CgMember"/>, including signature, with whitespace.
        /// The implementation is resolved on the <see cref="CgMember.PdbModuleSymbols"/>
        /// </summary>
        /// <param name="rmCgMems"></param>
        /// <param name="removeEmptyLines">Optional, set to true to not have the results as blank char-for-char</param>
        public static string[] RemoveMembersFromSrcCodeFiles(List<CgMember> rmCgMems, bool removeEmptyLines = false)
        {
            if (rmCgMems == null || rmCgMems.Count <= 0)
                return new string[0];

            var byFile =
                rmCgMems.Where(
                    x =>
                        !string.IsNullOrWhiteSpace(x?.PdbModuleSymbols?.file) && File.Exists(x.PdbModuleSymbols.file))
                    .ToList();
            if (byFile.Count <= 0)
                return new string[0];
            var modifiedFiles = new List<string>();
            foreach (var fl in byFile.Select(x => x.PdbModuleSymbols.file).Distinct())
            {
                var srcLines = File.ReadAllLines(fl);
                var cgMems =
                    byFile.Where(x => string.Equals(x.PdbModuleSymbols.file, fl, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                RemoveMembersFromSrcCodeFiles(srcLines, cgMems, fl);
                modifiedFiles.Add(fl);
                if (removeEmptyLines)
                {
                    System.Threading.Thread.Sleep(50);
                    NfPath.ToDoubleSpaced(fl);
                }
            }

            return modifiedFiles.ToArray();
        }

        /// <summary>
        /// Replaces the implementation of each <see cref="CgMember"/>, including signature, with whitespace.
        /// </summary>
        /// <param name="rmCgMems"></param>
        /// <param name="sourceCodeFilePath">
        /// The full path to the source code file on disk
        /// </param>
        /// <param name="removeEmptyLines">Optional, set to true to not have the results as blank char-for-char</param>
        public static void RemoveMembersFromSrcCodeFiles(List<CgMember> rmCgMems, string sourceCodeFilePath, bool removeEmptyLines = false)
        {
            if (rmCgMems == null || rmCgMems.Count <= 0)
                return;

            if (string.IsNullOrWhiteSpace(sourceCodeFilePath))
                return;
            if (!File.Exists(sourceCodeFilePath))
                return;

            var srcLines = File.ReadAllLines(sourceCodeFilePath);

            RemoveMembersFromSrcCodeFiles(srcLines, rmCgMems, sourceCodeFilePath);
            if (removeEmptyLines)
            {
                System.Threading.Thread.Sleep(50);
                NfPath.ToDoubleSpaced(sourceCodeFilePath);
            }
        }

        /// <summary>
        /// Performs dual operation of, one, moving <see cref="MoveMethodsArgs.MoveMembers"/> 
        /// out of <see cref="MoveMethodsArgs.SrcFile"/> and into the new 
        /// <see cref="MoveMethodsArgs.OutFilePath"/> (adjusting their signatures as needed to 
        /// accommodate dependencies), and, two, modifying the <see cref="MoveMethodsArgs.SrcFile"/> 
        /// file to call the new.
        /// </summary>
        /// <param name="cgType"></param>
        /// <param name="args"></param>
        public static void MoveMembers(this CgType cgType, MoveMethodsArgs args)
        {
            var srcFile = args.SrcFile;
            var moveMembers = args.MoveMembers;
            var newVariableName = args.NewVariableName;
            var outFilePath = args.OutFilePath;
            var outFileNamespaceAndTypeName = args.OutFileNamespaceAndType;
            var includeUsingStmts = args.IncludeUsingStmts;
            var excludeUsingStmts = args.ExcludedUsingStmts;

            if (moveMembers == null || moveMembers.Count < 0)
                return;

            if (string.IsNullOrWhiteSpace(srcFile) || !File.Exists(srcFile))
                return;

            var srcLines = File.ReadAllLines(srcFile);

            //need this to be the same for the whole batch of refactored lines
            if (string.IsNullOrWhiteSpace(newVariableName))
                newVariableName = Util.Core.NfString.GetNfRandomName();

            if (string.IsNullOrWhiteSpace(outFilePath))
                outFilePath = Path.Combine(Shared.Core.NfSettings.AppData, Util.Core.NfString.GetNfRandomName());


            //need to move the existing code to the new file
            var newNs =  NfString.SafeDotNetTypeName(outFileNamespaceAndTypeName.Item1);

            var newTn =  NfString.SafeDotNetIdentifier(outFileNamespaceAndTypeName.Item2);

            var idxRefactor = new Dictionary<Tuple<int, int>, string[]>();
            var newCgType = new CgType { Namespace = newNs, Name = newTn };
            foreach (var cgm in moveMembers)
            {
                //the signature must be changed to handle instance-level dependencies
                cgm.Args.AddRange(cgm.GetAllRefactorDependencies());
                var refactorLines = cgm.MyRefactoredLines(newVariableName, outFileNamespaceAndTypeName, false);
                foreach (var key in refactorLines.Keys)
                {
                    if (idxRefactor.Keys.Any(x => x.Equals(key)))
                        continue;
                    idxRefactor.Add(key, refactorLines[key]);
                }
                newCgType.Methods.Add(cgm);
            }

            //clear file content
            File.WriteAllText(outFilePath, string.Empty, Encoding.UTF8);
            var nsImports = Settings.LangStyle.ExtractNamespaceImportStatements(srcLines);
            if (nsImports != null)
            {
                nsImports = FilterNamespaceImportStmts(nsImports, includeUsingStmts,
                    excludeUsingStmts);
            }

            File.AppendAllLines(outFilePath,new []{ Settings.LangStyle.ToNamespaceDecl(newCgType)});

            File.AppendAllText(outFilePath,
                Settings.LangStyle.ToClass(newCgType, CgAccessModifier.Public, CgClassModifier.AsIs, nsImports), Encoding.UTF8);

            File.AppendAllLines(outFilePath, new[] { Settings.LangStyle.GetEnclosureCloseToken(null)});

            //now must modify existing file
            var k = 0;

            //get fresh copy from disk.
            srcLines = File.ReadAllLines(srcFile);

            foreach (var idx in idxRefactor)
            {
                var s = idx.Key.Item1 + k;
                var l = idx.Key.Item2;

                //get everything up to start of member
                var left = srcLines.Take(s - 1).ToList();

                //add in the new refactored code
                left.AddRange(idx.Value);

                //add in all the remainder less the removed member
                left.AddRange(srcLines.Skip(s + l).Take(srcLines.Length).ToList());

                //keep a running total of the number of lines removed
                var len = srcLines.Length;
                srcLines = left.ToArray();
                k = srcLines.Length - len;//this will typically be a negative int
            }

            //need to add the new variable decl
            int lnNumOut;
            Settings.LangStyle.TryFindFirstLineInClass(cgType.FullName, srcLines.ToArray(),
                out lnNumOut);

            var oldFilesInstanceToNew = Settings.LangStyle.ToDecl(newCgType, newVariableName,
                    CgAccessModifier.Private);

            var g = srcLines.Take(lnNumOut).ToList();
            g.Add(oldFilesInstanceToNew);
            g.AddRange(srcLines.Skip(lnNumOut).Take(srcLines.Length));
            srcLines = g.ToArray();

            //write the new content out to the file
            File.WriteAllLines(srcFile, srcLines, Encoding.UTF8);
        }

        /// <summary>
        /// Perform binding this <see cref="CgMember"/> to PDB data
        /// </summary>
        /// <param name="cgMem"></param>
        /// <param name="pdbData"></param>
        /// <returns></returns>
        public static bool TryAssignPdbSymbols(this CgMember cgMem, ModuleSymbols[] pdbData)
        {
            if (string.IsNullOrWhiteSpace(cgMem.Name))
                return false;

            //only expecting multi pdb line files for properties
            if (cgMem.HasGetter || cgMem.HasSetter)
            {
                cgMem.PdbModuleSymbols = new ModuleSymbols();
                if (cgMem.HasGetter)
                {
                    var getterMatch =
                        pdbData.Where(
                            x =>
                                x.symbolName ==
                                string.Format("{0}{1}", NfReflect.PropertyNamePrefix.GET_PREFIX, cgMem.Name))
                            .Select(x => x)
                            .FirstOrDefault();
                    if (getterMatch != null)
                    {
                        cgMem.PdbModuleSymbols = getterMatch;
                    }
                }
                if (cgMem.HasSetter)
                {
                    var setterMatch =
                        pdbData.Where(
                            x =>
                                x.symbolName ==
                                string.Format("{0}{1}", NfReflect.PropertyNamePrefix.SET_PREFIX, cgMem.Name))
                            .Select(x => x)
                            .FirstOrDefault();
                    if (setterMatch != null)
                    {
                        var propMax = setterMatch.lastLine.lineNumber > cgMem.PdbModuleSymbols.lastLine.lineNumber
                            ? setterMatch.lastLine
                            : cgMem.PdbModuleSymbols.lastLine;
                        var propMin = setterMatch.firstLine.lineNumber < cgMem.PdbModuleSymbols.firstLine.lineNumber
                            ? setterMatch.firstLine
                            : cgMem.PdbModuleSymbols.firstLine;

                        cgMem.PdbModuleSymbols = setterMatch;
                        cgMem.PdbModuleSymbols.firstLine = propMin;
                        cgMem.PdbModuleSymbols.lastLine = propMax;
                    }
                }
                return cgMem.PdbModuleSymbols != null;
            }

            //find all the entry keys whose pdbTargetLine's memberName matches this one.
            var matchedPdbTargets = pdbData.Where(x => x.symbolName == cgMem.Name).Select(x => x).ToList();

            if (matchedPdbTargets.Count <= 0)
                return false;
            if (matchedPdbTargets.Count == 1)
            {
                cgMem.PdbModuleSymbols = matchedPdbTargets.First();
                return true;
            }

            //thread arriving here means the member is an overloaded function and it gets tricky
            var matchScoreBoard = new Dictionary<ModuleSymbols, int>();
            foreach (var fi in matchedPdbTargets)
            {
                var fiPath = fi.file;

                var idx = fi.firstLine.lineNumber;
                var len = fi.lastLine.lineNumber - idx + 1;
                var pdbFileLines = File.ReadAllLines(fiPath).Skip(idx).Take(len).ToArray();
                var matchedScore = CodeBlockUsesMyArgs(cgMem,pdbFileLines);
                matchScoreBoard.Add(fi, matchedScore);
            }

            if (matchScoreBoard.Keys.Count <= 0)
                return false;
            var bestMatchScore = matchScoreBoard.Max(x => x.Value);
            cgMem.PdbModuleSymbols = matchScoreBoard.First(x => x.Value == bestMatchScore).Key;

            return true;
        }

        #region internal api
        internal static IEnumerable<string> RemoveOrphanedDecorators(IEnumerable<string> lines, IEnumerable<int> atLines)
        {
            var srcLines = lines?.ToArray();
            if (srcLines == null || srcLines.Length <= 0)
                return srcLines;

            var decoratorRegexPattern = Settings.LangStyle.GetDecoratorRegex();
            var openEncl = Settings.LangStyle.GetEnclosureOpenToken(null);
            var closeEncl = Settings.LangStyle.GetEnclosureCloseToken(null);
            if (string.IsNullOrWhiteSpace(decoratorRegexPattern))
                return srcLines;

            decoratorRegexPattern = "(" + decoratorRegexPattern + ")";
            var totalLen = srcLines.Length;
            var lineNums = atLines.OrderBy(v => v).ToArray();
            for (var i = 0; i < lineNums.Length; i++)
            {
                var startLnIdx = lineNums[i];
                var prevLnIdx = i == 0 ? -1 : lineNums[i - 1];

                //need to remove all decorators from here where abs min is previous target
                for (var sLnNum = startLnIdx; sLnNum > prevLnIdx; sLnNum--)
                {
                    var sLn = sLnNum < 0 || sLnNum >= totalLen ? string.Empty : srcLines[sLnNum];

                    //break out on any line having an open or close enclosure token
                    bool myRef = false;
                    var sLnTrim = Settings.LangStyle.EscStringLiterals(sLn.Trim(), EscapeStringType.BLANK, ref myRef);
                    if (sLnNum != startLnIdx && (sLnTrim.Contains(openEncl) || sLnTrim.Contains(closeEncl)))
                        break;
                    
                    if (RegexCatalog.IsRegexMatch(sLn, decoratorRegexPattern, out var decorator))
                    {
                        srcLines[sLnNum] = sLn.Replace(decorator, new string(' ', decorator.Length));
                    }
                }
            }

            return srcLines;
        }

        //presumes all the Members are defined within the contents of srcLines
        internal static void RemoveMembersFromSrcCodeFiles(string[] srcLines, List<CgMember> rmCgMems, string outputFileName)
        {
            if (rmCgMems == null || rmCgMems.Count <= 0)
                return;

            if (srcLines == null || srcLines.Length <= 0)
                return;
            var originalSrc = new string[srcLines.Length];
            Array.Copy(srcLines, originalSrc, srcLines.Length);

            srcLines = CleanSrcFile(srcLines);

            if (string.IsNullOrWhiteSpace(outputFileName))
                outputFileName = Path.Combine(NfSettings.AppData, Util.Core.NfString.GetNfRandomName());

            var line2ColStartEnds = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
            foreach (var cgMem in rmCgMems.Where(x => x != null))
            {
                var startLnIdx = cgMem.GetMyStartEnclosure(srcLines, true);
                var endLnIdx = cgMem.GetMyEndEnclosure(srcLines, true);

                //only add them if there  is a pair
                if (startLnIdx == null || endLnIdx == null)
                    continue;
                line2ColStartEnds.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(startLnIdx,endLnIdx));
            }

            var srcLinesOut = Util.Core.Etc.BlankOutLines(originalSrc, line2ColStartEnds).ToList();

            //expecting that antlr results include the attributes as the start-maker
            if (rmCgMems.Any(cgm => cgm.PdbModuleSymbols == null))
            {
                File.WriteAllLines(outputFileName, srcLinesOut, Encoding.UTF8);
                return;
            }

            var lineNums = new List<int>();
            foreach (var cgMem in rmCgMems)
            {
                var lnTuple = cgMem?.GetMyStartEnclosure(srcLines, true);
                if (lnTuple == null)
                    continue;
                lineNums.Add(lnTuple.Item1);
            }

            srcLinesOut = RemoveOrphanedDecorators(srcLinesOut, lineNums).ToList();

            File.WriteAllLines(outputFileName, srcLinesOut, Encoding.UTF8);
        }

        /// <summary>
        /// Gets a dictionary for use in replacing content from the original source file.
        /// </summary>
        /// <param name="cgMem"></param>
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
        internal static Dictionary<Tuple<int, int>, string[]> MyRefactoredLines(this CgMember cgMem, string newVariableName,
            Tuple<string, string> namespaceAndTypeName, bool includeVariableDecl = true)
        {
            if (string.IsNullOrWhiteSpace(cgMem.PdbModuleSymbols?.file) 
                || !File.Exists(cgMem.PdbModuleSymbols.file) 
                || cgMem.PdbModuleSymbols.firstLine == null 
                || cgMem.PdbModuleSymbols.lastLine == null)
                return null;

            if (string.IsNullOrWhiteSpace(newVariableName))
                newVariableName = Util.Core.NfString.GetNfRandomName();

            int lnNumOut;
            Settings.LangStyle.TryFindFirstLineInClass(cgMem.MyCgType.FullName, File.ReadAllLines(cgMem.PdbModuleSymbols.file),
                out lnNumOut);

            var ofl = lnNumOut;
            var dido = new Dictionary<Tuple<int, int>, string[]>();

            //allow calling assembly to specify a new namespace and type name
            var renameCgType = namespaceAndTypeName == null
                ? cgMem.MyCgType
                : new CgType()
                {
                    Namespace = namespaceAndTypeName.Item1 ?? cgMem.MyCgType.Namespace,
                    Name = namespaceAndTypeName.Item2 ?? cgMem.MyCgType.Name
                };

            //this is an instance level declaration added to the original source
            if (ofl > 1 && includeVariableDecl)
            {
                var oldFilesInstanceToNew = Settings.LangStyle.ToDecl(renameCgType, newVariableName,
                    CgAccessModifier.Private);
                dido.Add(new Tuple<int, int>(ofl, 1), new[] { oldFilesInstanceToNew });
            }

            var minAt = cgMem.PdbModuleSymbols.firstLine.lineNumber;
            var maxAt = cgMem.PdbModuleSymbols.lastLine.lineNumber;

            var nKey = new Tuple<int, int>(minAt,
                maxAt - minAt - 1);

            dido.Add(nKey, new[] { Settings.LangStyle.ToStmt(cgMem, null, newVariableName) });
            return dido;
        }

        internal static string[] FilterNamespaceImportStmts(string[] originalUsingStatements, string[] include, string[] exclude)
        {
            if (originalUsingStatements == null || originalUsingStatements.Length == 0)
                return new[] { string.Empty };

            if (include == null && exclude == null)
                return originalUsingStatements;

            var filteredStmts = new List<string>();

            if (include != null && include.Length == 1 && include[0].Trim() == "*")
                include = null;

            //add in matches
            if (include != null)
            {
                foreach (var originStmt in originalUsingStatements)
                {
                    if (filteredStmts.Contains(originStmt))
                        continue;
                    foreach (var filter in include)
                    {
                        if (filter.Trim().EndsWith("*") && originStmt.Contains(filter.Trim().Replace("*", string.Empty)))
                        {
                            filteredStmts.Add(originStmt);
                            continue;
                        }

                        if (originStmt.Replace(";", string.Empty).EndsWith(filter))
                            filteredStmts.Add(originStmt);
                    }
                }
            }
            else
            {
                filteredStmts.AddRange(originalUsingStatements);
            }
            if (exclude == null)
                return filteredStmts.ToArray();

            //remove exclusions
            foreach (var filtered in filteredStmts.ToArray())
            {
                foreach (var filter in exclude)
                {
                    if (filter.Trim().EndsWith("*") && filtered.Contains(filter.Trim().Replace("*", string.Empty)) &&
                        filteredStmts.Contains(filtered))
                    {
                        filteredStmts.Remove(filtered);
                        continue;
                    }

                    if (filtered.Replace(";", string.Empty).EndsWith(filter) && filteredStmts.Contains(filtered))
                        filteredStmts.Remove(filtered);
                }
            }

            return filteredStmts.ToArray();
        }

        internal static string[] CleanSrcFile(string[] srcFile)
        {
            srcFile = Settings.LangStyle.RemoveBlockComments(srcFile);
            srcFile = Settings.LangStyle.RemoveLineComments(srcFile, Settings.LangStyle.LineComment);
            srcFile = Settings.LangStyle.RemovePreprocessorCmds(srcFile);
            var irregular = false;
            for (var m = 0; m < srcFile.Length; m++)
            {
                srcFile[m] = Settings.LangStyle.EscStringLiterals(srcFile[m], EscapeStringType.BLANK, ref irregular);
            }
            return srcFile;
        }
        #endregion
    }

    /// <summary>
    /// Class containing all info needed to move a method from one src file to another.
    /// </summary>
    public class MoveMethodsArgs
    {
        public string SrcFile { get; set; }
        public List<CgMember> MoveMembers { get; set; }
        public string NewVariableName { get; set; }
        public string OutFilePath { get; set; }
        public Tuple<string, string> OutFileNamespaceAndType { get; set; }
        public string[] IncludeUsingStmts { get; set; }
        public string[] ExcludedUsingStmts { get; set; }
    }
}
