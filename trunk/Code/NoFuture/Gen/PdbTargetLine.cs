using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Exceptions;

namespace NoFuture.Gen
{
    /// <summary>
    /// Represents an individual symbol's PDB data
    /// </summary>
    [Serializable]
    public class PdbTargetLine
    {
        public const string DEFAULT_MEMBER_NAME = "unknownMember";
        public const string DEFAULT_TYPE_NAME = "unknownType";

        /// <summary>
        /// Factory method to immediately transpose the Dia2Dump.exe's output into the <see cref="PdbTargetLine"/>
        /// </summary>
        /// <param name="symbolsPdb"></param>
        /// <param name="fullAssemblyQualTypeName">
        /// This is required since its needed to 
        /// assign each <see cref="PdbTargetLine.OwningTypeFullName"/> and 
        /// <see cref="PdbTargetLine.SymbolFolder"/>.
        /// </param>
        /// <returns></returns>
        public static List<PdbTargetLine> ParseFrom(Shared.DiaSdk.LinesSwitch.PdbCompiland symbolsPdb, string fullAssemblyQualTypeName)
        {
            if(symbolsPdb == null)
                throw new ArgumentNullException("symbolsPdb");
            if(string.IsNullOrWhiteSpace(fullAssemblyQualTypeName))
                throw new ArgumentNullException("fullAssemblyQualTypeName");
            if(symbolsPdb.moduleSymbols == null || symbolsPdb.moduleSymbols.Length <= 0)
                throw new ItsDeadJim(string.Format("The type '{0}' has no module symbols",fullAssemblyQualTypeName));

            var symbolFolder = CgTypeFiles.GetSymbolFolderPath(TempDirectories.AppData, fullAssemblyQualTypeName);
            var nfTypeName = new Util.TypeName(fullAssemblyQualTypeName);
            var listOut = new List<PdbTargetLine>();

            foreach (var p in symbolsPdb.moduleSymbols)
            {
                listOut.Add(new PdbTargetLine()
                {
                    EndAt = p.lastLine.lineNumber,
                    StartAt = p.firstLine.lineNumber,
                    IndexId = p.symIndexId,
                    MemberName = p.symbolName,
                    SrcFile = p.file,
                    OwningTypeFullName = nfTypeName.FullName,
                    SymbolFolder = symbolFolder
                });
            }
            return listOut;
        }

        /// <summary>
        /// The full file-path name to the source code file for which this object is to target.
        /// </summary>
        public string SrcFile { get; set; }
        /// <summary>
        /// The starting line number derived from the pdb file.
        /// </summary>
        public int StartAt { get; set; }
        /// <summary>
        /// The ending line number derived from the pdb file.
        /// </summary>
        public int EndAt { get; set; }
        /// <summary>
        /// The full type name to which the <see cref="MemberName"/> belongs.
        /// This is the type's namespace and type name NOT the full assembly qualified name.
        /// </summary>
        public string OwningTypeFullName { get; set; }
        /// <summary>
        /// The full name of a member.
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// A unique integer value only present in pdb files
        /// </summary>
        public int IndexId { get; set; }

        /// <summary>
        /// The folder which is exclusive to this particular symbol.
        /// </summary>
        public string SymbolFolder { get; set; }

        /// <summary>
        /// This will produce the results of the <see cref="GetHashCode"/> in the form 
        /// of a string whose contents are hex.
        /// </summary>
        /// <returns></returns>
        public string GetHashCodeAsString()
        {
            var myHashCode = GetHashCode().ToString(CultureInfo.InvariantCulture);
            var myHashBytes = Encoding.UTF8.GetBytes(myHashCode);
            var myHashString = new StringBuilder();
            foreach (var b in myHashBytes)
                myHashString.Append(b.ToString("x2"));
            return myHashString.ToString();
        }

        /// <summary>
        /// Override to only take into account the same properties used in the overload of <see cref="Equals"/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (string.IsNullOrWhiteSpace(OwningTypeFullName) ? 1 : OwningTypeFullName.GetHashCode()) +
                   (string.IsNullOrWhiteSpace(MemberName) ? 1 : MemberName.GetHashCode()) +
                   StartAt.GetHashCode() + EndAt.GetHashCode() + IndexId.GetHashCode();
        }

        /// <summary>
        /// Equality test for this instance to another instance of <see cref="PdbTargetLine"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(PdbTargetLine obj)
        {
            if (obj == null)
                return false;
            var p = new bool[]
            {
                obj.StartAt == StartAt,
                obj.EndAt == EndAt,
                obj.OwningTypeFullName == OwningTypeFullName,
                obj.MemberName == MemberName,
                obj.IndexId == IndexId
            };

            return p.All(x => x);
        }

        /// <summary>
        /// Returns a full path location of where this type's member's parsed
        /// pdb lines are expected.
        /// </summary>
        /// <returns></returns>
        public string GetPdbLinesFileLocation()
        {
            if (string.IsNullOrWhiteSpace(SymbolFolder))
                throw new RahRowRagee(string.Format("The SymbolFolder property has not been set for this instance (ref: {0})", GetHashCodeAsString()));

            return Path.Combine(SymbolFolder,
                string.Format("{0}.{1}", GetHashCodeAsString(), Settings.PdbLinesExtension));
        }

        /// <summary>
        /// See this methods overload for details.
        /// </summary>
        /// <param name="pdbOutputPath"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public string[] GetMyPdbTargetLines(string pdbOutputPath, List<string> messages)
        {
            var pdbTargetLineFile = GetPdbLinesFileLocation();
            if (!File.Exists(pdbTargetLineFile))
            {
                if (messages != null) messages.Add(string.Format("No PdbTargetLine file found at '{0}'", pdbTargetLineFile));
                return new[] { Settings.LangStyle.NoImplementationDefault };
            }

            var pdbFileLines = File.ReadAllLines(pdbTargetLineFile);

            var cleanPdbFileLines = Settings.LangStyle.CleanupPdbLinesCodeBlock(pdbFileLines);

            if (cleanPdbFileLines == null || cleanPdbFileLines.Length <= 0)
            {
                if (messages != null) messages.Add(string.Format("Clean Pdb Lines resulted in an empty array using the file '{0}'", pdbTargetLineFile));
                return new[] { Settings.LangStyle.NoImplementationDefault };
            }

            if (cleanPdbFileLines.All(string.IsNullOrWhiteSpace))
            {
                if (messages != null) messages.Add(string.Format("Clean Pdb Lines resulted in all empty strings using the file '{0}'", pdbTargetLineFile));
                return new[] { Settings.LangStyle.NoImplementationDefault };
            }

            return cleanPdbFileLines;
        }

        /// <summary>
        /// Its expected that each Pdb's range should be exclusive and no pdb range should be shared.
        /// </summary>
        /// <param name="pdbTargetLines"></param>
        /// <returns></returns>
        public static List<PdbTargetLine> GetIrregulars(List<PdbTargetLine> pdbTargetLines)
        {
            var irregulars = new List<PdbTargetLine>();

            var stackOfLines = new Stack<PdbTargetLine>(pdbTargetLines.OrderByDescending(x => x.StartAt).ToArray());
            var t = stackOfLines.Pop();
            while (t != null)
            {
                var shouldBeLowest = t.EndAt;//no entry should have a start at nor end at less than this.
                var allRemaining = stackOfLines.ToArray();
                if (allRemaining.Length <= 0)
                    break;
                if(allRemaining.Any(x => x.StartAt < shouldBeLowest) || allRemaining.Any(x => x.EndAt < shouldBeLowest))
                    irregulars.Add(t);

                t = stackOfLines.Pop();
            }
            return irregulars;
        }

        public static List<Tuple<int, int>> SpliceIrregularRanges(List<PdbTargetLine> regulars,
            PdbTargetLine irregular)
        {
            if (regulars == null)
                return null;
            if (irregular == null)
                return null;

            var irregularRanges = new List<Tuple<int, int>>();

            //get the irregular instance out if its present
            var myRegulars = regulars.Where(x => !x.Equals(irregular)).ToList();

            //get just the ranges
            var myRanges = myRegulars.Select(x => new Tuple<int, int>(x.StartAt, x.EndAt)).ToList();

            if (myRanges.Any(x => x.Item1 <= irregular.StartAt && x.Item2 >= irregular.EndAt))
                return null;

            if (myRanges.Any(x => x.Item1 <= irregular.StartAt && x.Item2 >= irregular.StartAt))
            {
                irregular.StartAt = myRanges.OrderBy(x => x.Item2).First(x => x.Item2 > irregular.StartAt).Item2;
            }
            if (myRanges.Any(x => x.Item1 <= irregular.EndAt && x.Item2 >= irregular.EndAt))
            {
                irregular.EndAt = myRanges.OrderByDescending(x => x.Item1).First(x => x.Item1 < irregular.EndAt).Item1 - 1;
            }

            var irj = myRanges.SelectMany(x => new[] {x.Item1 - 1, x.Item2 + 1}).ToList();
            irj.Add(irregular.StartAt);
            irj.Add(irregular.EndAt);

            var fk = myRanges.All(x => x.Item1 > irregular.StartAt) ? 0 : 1;

            irj = irj.Distinct().OrderBy(x => x).ToList();

            for (var i = fk; i < irj.Count; i++)
            {
                if (i + 1 >= irj.Count || irj[i] > irregular.EndAt)
                    break;
                if(i % 2 == 0)
                    irregularRanges.Add(new Tuple<int, int>(irj[i], irj[i+1]));
            }

            //irregular.StartAt = -1;
            //irregular.EndAt = 0;
            return irregularRanges.Where(x => x.Item1 > 0 && x.Item2 >= x.Item1).ToList();
        }
    }
}