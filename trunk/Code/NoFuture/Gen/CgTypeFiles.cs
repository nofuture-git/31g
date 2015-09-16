using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Exceptions;
using NoFuture.Gen.LangRules;
using NoFuture.Tokens;
using NoFuture.Util;
using NoFuture.Util.Binary;

namespace NoFuture.Gen
{
    /// <summary>
    /// Handler between <see cref="CgType"/> and PDB data.
    /// Represents a directory of parsed PDB symbols and the utility
    /// methods to attain them.
    /// </summary>
    [Serializable]
    public class CgTypeFiles
    {
        #region fields
        internal string[] _srcFileContent;
        private TypeName _typeName;
        private CgTypeFileIndex _fileIndex;
        private readonly List<string> _pdbFilesUsed = new List<string>();
        private string _srcRefFileFullName;
        private int _originalFirstLine;
        private string[] _modifiedSrcFileContent;
        #endregion

        #region properties
        public string OriginalSource { get; set; }
        public string RootPdbFolder { get; set; }
        public string SymbolFolder { get; set; }
        public string SymbolName { get; set; }
        public string UsingStatementFile { get; set; }
        public CgTypeFileIndex FileIndex { get { return _fileIndex; } }

        public string[] OriginalSourceFileContent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OriginalSource) || !File.Exists(OriginalSource)) return null;
                return File.ReadAllLines(OriginalSource);
            }
        }

        public int OriginalSourceFirstLine
        {
            get
            {
                if (_originalFirstLine > 0)
                    return _originalFirstLine;
                int lnNumOut;
                if (Settings.LangStyle.TryFindFirstLineInClass(_typeName.FullName, OriginalSourceFileContent,
                    out lnNumOut))
                {
                    _originalFirstLine = lnNumOut;
                }
                return _originalFirstLine;
            }
        }

        #endregion

        #region ctors

        public CgTypeFiles(string outputPath, CgTypeFileIndex indexFile)
        {
            _fileIndex = indexFile;
            Init(outputPath,_fileIndex.OriginalSrcCodeFile,_fileIndex.AssemblyQualifiedTypeName);
        }

        public CgTypeFiles(string outputPath, string csSrcFile, string fullAssemblyQualTypeName)
        {
            Init(outputPath,csSrcFile,fullAssemblyQualTypeName);
        }

        //ctor's worker actual ctor's all resolving to this arg list
        private void Init(string outputPath, string csSrcFile, string fullAssemblyQualTypeName)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
                outputPath = TempDirectories.AppData;
            if (string.IsNullOrWhiteSpace(csSrcFile))
                throw new ArgumentNullException("csSrcFile");
            if (fullAssemblyQualTypeName == null)
                throw new ArgumentNullException("fullAssemblyQualTypeName");
            if (!TypeName.IsFullAssemblyQualTypeName(fullAssemblyQualTypeName))
                throw new ItsDeadJim(string.Format("The value '{0}' does not match the expected format.", fullAssemblyQualTypeName));

            OriginalSource = csSrcFile;

            _typeName = new TypeName(fullAssemblyQualTypeName);
            if(_fileIndex == null)
            {
                _fileIndex = new CgTypeFileIndex
                {
                    AssemblyQualifiedTypeName = fullAssemblyQualTypeName,
                    OriginalSrcCodeFile = csSrcFile
                };
            }

            SymbolName = _typeName.FullName;
            RootPdbFolder = outputPath;
            SymbolFolder = GetSymbolFolderPath(outputPath, fullAssemblyQualTypeName);

            if (!Directory.Exists(SymbolFolder))
                Directory.CreateDirectory(SymbolFolder);

            UsingStatementFile = Path.Combine(SymbolFolder, string.Format("{0}.{1}", SymbolName, Settings.PdbUsingStmtExtension));

            _srcRefFileFullName = Path.Combine(SymbolFolder, Settings.DefaultFileIndexName);

            ReadAllLinesOfOriginalSource();
            
        }
        #endregion

        #region interchange
        /// <summary>
        /// The operates in the reverse of its other overloads by attempting to derive a Type from 
        /// a source code file.
        /// </summary>
        /// <param name="outputPath">
        /// Optional, will default to <see cref="TempDirectories.AppData"/>
        /// </param>
        /// <param name="asmContainingTarget"></param>
        /// <param name="csSrcFile">Only implemented for C#</param>
        /// <returns></returns>
        public static string GetSymbolFolderPath(string outputPath, Assembly asmContainingTarget, string csSrcFile)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
                outputPath = TempDirectories.AppData;
            if(asmContainingTarget == null)
                throw new ArgumentNullException("asmContainingTarget");
            if(string.IsNullOrWhiteSpace(csSrcFile))
                throw new ArgumentNullException("csSrcFile");
            if(!File.Exists(csSrcFile))
                throw new ItsDeadJim(string.Format("There is not file at '{0}'",csSrcFile));
            string deriveTypeName;
            var deriveTypeNameAttempt =
                Settings.LangStyle.TryDeriveTypeNameFromFile(File.ReadAllLines(csSrcFile),
                    out deriveTypeName);
            if(!deriveTypeNameAttempt)
                throw new ItsDeadJim(string.Format("Could not derive a type name from the contents of the file at '{0}'", csSrcFile));

            var targetType = asmContainingTarget.NfGetType(deriveTypeName);
            if (targetType == null)
            {
                throw new ItsDeadJim(
                    string.Format(
                        "The type named '{0}' was derived from the code file '{1}' but this type was not present in the assembly '{2}'.",
                        deriveTypeName, csSrcFile, asmContainingTarget.GetName().Name));
            }

            return GetSymbolFolderPath(outputPath, targetType.AssemblyQualifiedName);
        }

        /// <summary>
        /// Single manner by which a Symbol folder is constructed from a Full Assembly-Qualified Type Name
        /// </summary>
        /// <param name="outputPath">
        /// Optional, will default to <see cref="TempDirectories.AppData"/>
        /// </param>
        /// <param name="fullAssemblyQualTypeName">
        /// This must be a match to the regex pattern at <see cref="NoFuture.Util.TypeName.ASSEMBLY_QUALIFIED_CLASS_NAME_REGEX"/>
        /// ignoring case.
        /// </param>
        /// <returns></returns>
        public static string GetSymbolFolderPath(string outputPath, string fullAssemblyQualTypeName)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
                outputPath = TempDirectories.AppData;
            if (!TypeName.IsFullAssemblyQualTypeName(fullAssemblyQualTypeName))
                throw new ItsDeadJim(string.Format("The value '{0}' does not match the expected format.", fullAssemblyQualTypeName));
            var asmTypeNameHash = fullAssemblyQualTypeName.Md5Hash();

            return Path.Combine(outputPath, asmTypeNameHash);
        }
        #endregion

        #region save api
        /// <summary>
        /// Having some content at <see cref="OriginalSource"/> this will find the "using" statments therein 
        /// and write them to file at <see cref="UsingStatementFile"/>
        /// </summary>
        public virtual void SaveUsingStatementsToFile()
        {
            if (!ReadAllLinesOfOriginalSource())
                return;

            File.WriteAllLines(UsingStatementFile, Settings.LangStyle.ExtractNamespaceImportStatements(_srcFileContent),
                Encoding.UTF8);
        }

        /// <summary>
        /// Will delete every file present in <see cref="SymbolFolder"/>
        /// </summary>
        public void ClearWorkingFolderOfAllContent()
        {
            foreach(var f in Directory.GetFiles(SymbolFolder))
                File.Delete(f);
        }

        /// <summary>
        /// Creates files for each <see cref="pdbTargetLines"/> and an XML directory file
        /// thereof.
        /// </summary>
        /// <param name="pdbTargetLines"></param>
        /// <remarks>
        /// Do not expect the content to look exactly
        /// like the original source being that only "useful" lines (from the prespective of 
        /// the compiler) are retained <see cref="ILangStyle.CleanupPdbLinesCodeBlock"/>
        /// </remarks>
        public virtual void SavePdbLinesToFile(List<PdbTargetLine> pdbTargetLines)
        {
            if (pdbTargetLines == null || pdbTargetLines.Count <= 0)
                return;
            if (!ReadAllLinesOfOriginalSource())
                return;

            var pertinent = pdbTargetLines.Where(x => x.StartAt > 0 && x.StartAt < x.EndAt).ToList();

            var irregulars = PdbTargetLine.GetIrregulars(pertinent);

            var regulars = pertinent.Where(x => !irregulars.Any(y => y.Equals(x))).ToList();

            var consumedRange = new List<Tuple<int, int>>();

            foreach (var pdb in regulars.OrderBy(x => x.StartAt))
            {
                string[] pdbFileContent;
                if (!TryReadLinesFromOriginalSrc(pdb, out pdbFileContent))
                    continue;

                consumedRange.Add(new Tuple<int, int>(pdb.StartAt, pdb.EndAt));

                var pdbLineLoc = pdb.GetPdbLinesFileLocation();
                File.WriteAllBytes(pdbLineLoc, Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, pdbFileContent)));
                _fileIndex.PdbFilesHash.Add(pdb.GetHashCodeAsString(), pdb);
            }

            foreach (var irregular in irregulars.OrderBy(x => x.StartAt))
            {
                string[] pdbFileContent;
                if (!TryReadLinesFromOriginalSrc(irregular, out pdbFileContent))
                    continue;

                var irregularRanges = PdbTargetLine.SpliceIrregularRanges(regulars, irregular);
                var content = new List<string>();
                foreach (var r in irregularRanges)
                {
                    if (consumedRange.Any(y => y.Equals(r)))
                        continue;
                    
                    content.AddRange(ReadLinesFromOriginalSrc(r.Item1, r.Item2));

                    consumedRange.Add(new Tuple<int, int>(r.Item1, r.Item2));
                }

                var irPdbLineLoc = irregular.GetPdbLinesFileLocation();
                File.WriteAllBytes(irPdbLineLoc, Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, content)));
                _fileIndex.PdbFilesHash.Add(irregular.GetHashCodeAsString(), irregular);
            }


            _fileIndex.WriteIndexToFile(SymbolFolder);
        }

        #endregion

        #region refactor api

        /// <summary>
        /// Blanks out each method leaving the total file line count unchanged.
        /// </summary>
        /// <param name="blankOutCgMems"></param>
        /// <param name="outputFileName"></param>
        public void BlankOutMethods(List<CgMember> blankOutCgMems, string outputFileName)
        {
            if (blankOutCgMems == null || blankOutCgMems.Count <= 0)
                return;

            var srcLines = OriginalSourceFileContent;
            if (srcLines == null || srcLines.Length <= 0)
                return;

            const string blankLine = " ";

            if (string.IsNullOrWhiteSpace(outputFileName))
                outputFileName = OriginalSource;

            var d = new SortedList<int, int>();
            foreach (var cgMem in blankOutCgMems)
            {
                var startLnIdx = cgMem.GetMyStartEnclosure(srcLines);
                if(startLnIdx != null && !d.ContainsKey(startLnIdx.Item1))
                    d.Add(startLnIdx.Item1, startLnIdx.Item2);
                var endLnIdx = cgMem.GetMyEndEnclosure(srcLines);
                if(endLnIdx != null && !d.ContainsKey(endLnIdx.Item1))
                    d.Add(endLnIdx.Item1, endLnIdx.Item2);
            }

            var lineOn = true;
            var srcLinesOut = new List<string>();
            for (var i = 0; i < srcLines.Length; i++)
            {
                if (d.ContainsKey(i))
                {
                    if (lineOn)
                    {
                        srcLinesOut.Add(srcLines[i].Substring(0, d[i]));
                    }
                    else
                    {
                        var idx = d[i];
                        if (idx < 0)
                            idx = srcLines[i].Length;
                        if(idx + 1 >= srcLines[i].Length)
                            srcLinesOut.Add(blankLine);
                        else
                            srcLinesOut.Add(srcLines[i].Substring(d[i] + 1));
                    }
                    lineOn = !lineOn;
                    continue;
                }
                srcLinesOut.Add(lineOn ? srcLines[i] : blankLine);
            }
            File.WriteAllLines(outputFileName, srcLinesOut, Encoding.UTF8);
        }

        /// <summary>
        /// Performs dual operation of, one, moving <see cref="moveMembers"/> out of <see cref="OriginalSource"/> and into the new 
        /// <see cref="outFilePath"/> (adjusting thier signatures as needed to accommodate dependencies), and, two,
        /// modifying the <see cref="OriginalSource"/> file to call the new.
        /// </summary>
        /// <param name="moveMembers"></param>
        /// <param name="newVariableName"></param>
        /// <param name="outFilePath"></param>
        /// <param name="outFileNamespaceAndTypeName"></param>
        /// <param name="includeUsingStmts">Optional filter for namespace imports statements to INCLUDE.</param>
        /// <param name="excludeUsingStmts">Optional filter for namespace imports statements to EXCLUDE.</param>
        public void MoveMethods(List<CgMember> moveMembers, string newVariableName, string outFilePath,
            Tuple<string, string> outFileNamespaceAndTypeName, string[] includeUsingStmts = null, string[] excludeUsingStmts = null)
        {
            if (moveMembers == null || moveMembers.Count < 0)
                return;
            CgType cgtype = null;
            if (moveMembers.Any(x => x.MyCgType != null))
            {
                cgtype = moveMembers.Select(x => x.MyCgType).First();
            }
            if (cgtype == null)
                return;

            //need this to be the same for the whole batch of refactored lines
            if (string.IsNullOrWhiteSpace(newVariableName))
                newVariableName = Path.GetRandomFileName().Replace(".", string.Empty);

            if (string.IsNullOrWhiteSpace(outFilePath))
                outFilePath = Path.Combine(TempDirectories.AppData, Path.GetRandomFileName());


            //need to move the existing code to the new file
            var newNs = outFileNamespaceAndTypeName == null ||
                        string.IsNullOrWhiteSpace(outFileNamespaceAndTypeName.Item1)
                ? cgtype.Namespace
                : TypeName.SafeDotNetTypeName(outFileNamespaceAndTypeName.Item1);

            var newTn = outFileNamespaceAndTypeName == null ||
                        string.IsNullOrWhiteSpace(outFileNamespaceAndTypeName.Item2)
                ? cgtype.Name
                : TypeName.SafeDotNetIdentifier(outFileNamespaceAndTypeName.Item2);

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

            if (!string.IsNullOrWhiteSpace(UsingStatementFile) && File.Exists(UsingStatementFile))
            {
                File.WriteAllLines(outFilePath,
                    FilterNamespaceImportStmts(File.ReadAllLines(UsingStatementFile), includeUsingStmts,
                        excludeUsingStmts), Encoding.UTF8);
            }
            else
            {
                File.WriteAllText(outFilePath, string.Empty, Encoding.UTF8);
            }

            File.AppendAllText(outFilePath,
                Settings.LangStyle.ToClass(newCgType, CgAccessModifier.Public, CgClassModifier.AsIs), Encoding.UTF8);

            //now must modify existing file
            var src = OriginalSourceFileContent;
            if (src == null)
                return;

            var k = 0;

            foreach (var idx in idxRefactor)
            {
                var s = idx.Key.Item1 + k;
                var l = idx.Key.Item2;

                //get everything up to start of member
                var left = src.Take(s - 1).ToList();

                //add in the new refactored code
                left.AddRange(idx.Value);

                //add in all the remainder less the removed member
                left.AddRange(src.Skip(s + l).Take(src.Length).ToList());

                //keep a running total of the number of lines removed
                var len = src.Length;
                src = left.ToArray();
                k = src.Length - len;//this will typically be a negative int
            }

            //need to add the new variable decl

            var oldFilesInstanceToNew = Settings.LangStyle.ToDecl(newCgType, newVariableName,
                    CgAccessModifier.Private);
            var g = src.Take(OriginalSourceFirstLine).ToList();
            g.Add(oldFilesInstanceToNew);
            g.AddRange(src.Skip(OriginalSourceFirstLine).Take(src.Length));
            src = g.ToArray();

            //write the new content out to the file
            File.WriteAllLines(OriginalSource, src, Encoding.UTF8);
        }
        #endregion

        #region read api
        /// <summary>
        /// Will attempt to locate the best match for <see cref="member"/> within
        /// the current <see cref="SymbolFolder"/>. Based first on file name then
        /// on the scores from <see cref="CgMember.CodeBlockUsesMyArgs"/>
        /// </summary>
        /// <param name="member"></param>
        /// <param name="pdbTargetOut"></param>
        /// <returns></returns>
        public bool TryFindPdbTargetLine(CgMember member, out PdbTargetLine pdbTargetOut)
        {
            pdbTargetOut = null;

            if (member == null)
                return false;
            if (string.IsNullOrWhiteSpace(SymbolName))
                return false;
            if (string.IsNullOrWhiteSpace(SymbolFolder))
                return false;
            if (!Directory.Exists(SymbolFolder))
                return false;
            if (string.IsNullOrWhiteSpace(member.Name))
                return false;

            //these are not implicit to the ctor and must be assigned or fetched from file
            if (_fileIndex.PdbFilesHash.Count <= 0)
            {
                _fileIndex = CgTypeFileIndex.ReadIndexFromFile(_srcRefFileFullName);
                if (_fileIndex.AssemblyQualifiedTypeName != _typeName.RawString)
                    throw new RahRowRagee(
                        string.Format(
                            "The CgTypeFileIndex at '{0}' has a differnet type name.\nName at ctor time was '{1}'.\nName in this file is '{2}'.",
                            _srcRefFileFullName, _typeName.RawString, _fileIndex.AssemblyQualifiedTypeName));
            }
            //find all the entry keys whose pdbTargetLine's memberName matches this one.
            var matchedPdbTargets = _fileIndex.PdbFilesHash.Where(x => x.Value.MemberName == member.Name).Select(x => x.Value).ToList();

            if (matchedPdbTargets.Count <= 0)
                return false;
            if (matchedPdbTargets.Count == 1)
            {
                pdbTargetOut = matchedPdbTargets.First();
                //record that this file has been claimed at least once for the life of this instance
                _pdbFilesUsed.Add(pdbTargetOut.GetPdbLinesFileLocation());
                return true;
            }

            //thread arriving here means the member is an overloaded function and it gets tricky
            var matchScoreBoard = new Dictionary<PdbTargetLine, int>();
            foreach (var fi in matchedPdbTargets)
            {
                var fiPath = fi.GetPdbLinesFileLocation();
                if (_pdbFilesUsed.Contains(fiPath))
                    continue;

                var pdbFileLines = File.ReadAllLines(fiPath);
                var matchedScore = member.CodeBlockUsesMyArgs(pdbFileLines);
                matchScoreBoard.Add(fi, matchedScore);
            }

            if (matchScoreBoard.Keys.Count <= 0)
                return false;
            var bestMatchScore = matchScoreBoard.Max(x => x.Value);
            var bestMatch = matchScoreBoard.First(x => x.Value == bestMatchScore).Key;

            pdbTargetOut = bestMatch;

            //this lets the owning CgMember know that the match is uncertian
            if (matchScoreBoard.Count(x => x.Value == bestMatchScore) > 1)
                member.TiedForBestFileMatch = true;
            if (bestMatchScore < member.Args.Count)
                member.LessThanPerfectFileMatch = true;

            _pdbFilesUsed.Add(pdbTargetOut.GetPdbLinesFileLocation());
            return true;
        }
        
        /// <summary>
        /// Gets the line number for <see cref="lineValue"/> matching line in <see cref="OriginalSourceFileContent"/> 
        /// </summary>
        /// <param name="lineValue"></param>
        /// <param name="startingAt"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public int? FindLineNumByValue(string lineValue, int startingAt, StringComparison comparison = StringComparison.Ordinal)
        {
            if (lineValue == null)
                return null;
            if (startingAt < 0)
                return null;
            var content = OriginalSourceFileContent;
            if (content == null || content.Length <= 0)
                return null;
            for (var i = startingAt; i < content.Length; i++)
            {
                var cLine = content[i].Trim();
                if (string.Equals(lineValue.Trim(), cLine, comparison))
                    return i;
            }
            return null;
        }

        /// <summary>
        /// Utility method to get the line at <see cref="lineNumber"/> from <see cref="OriginalSourceFileContent"/>
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public string GetValueByLineNum(int lineNumber)
        {
            if (lineNumber < 0)
                return null;
            var content = OriginalSourceFileContent;
            if (content == null || content.Length <= 0 || lineNumber >= content.Length)
                return null;
            return content[lineNumber];
        }

        #endregion

        #region internal helpers
        protected internal bool TryReadLinesFromOriginalSrc(PdbTargetLine pdb, out string[] pdbFileContent)
        {
            pdbFileContent = null;
            if (_typeName.FullName != pdb.OwningTypeFullName)
                return false;
            pdb.SymbolFolder = SymbolFolder;

            pdbFileContent = ReadLinesFromOriginalSrc(pdb);
            if (pdbFileContent == null || pdbFileContent.Length <= 0)
                return false;

            pdbFileContent = Settings.LangStyle.CleanupPdbLinesCodeBlock(pdbFileContent);
            return true;
        }

        internal string[] ReadLinesFromOriginalSrc(PdbTargetLine pdb)
        {
            return ReadLinesFromOriginalSrc(pdb.StartAt, pdb.EndAt);
        }

        internal string[] ReadLinesFromOriginalSrc(int startAt, int endAt)
        {
            if (endAt < startAt)
                return null;
            var endAtLen = endAt - startAt;
            return
                _srcFileContent.Skip(startAt + Settings.PdbLinesStartAtAddition)
                    .Take(endAtLen + Settings.PdbLinesEndAtAddition)
                    .ToArray();
            
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
                    if (filter.Trim().EndsWith("*") && filtered.Contains(filter.Trim().Replace("*", string.Empty)) && filteredStmts.Contains(filtered))
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

        private bool ReadAllLinesOfOriginalSource()
        {
            if (_srcFileContent != null && _srcFileContent.Length > 0)
                return true;

            if (string.IsNullOrWhiteSpace(OriginalSource) || !File.Exists(OriginalSource)) return false;
            
            _srcFileContent = File.ReadAllLines(OriginalSource);
            return true;
        }
        #endregion
    }
}
