using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Exceptions;
using NoFuture.Shared.DiaSdk;
using NoFuture.Shared.DiaSdk.LinesSwitch;
using NoFuture.Util.Binary;

namespace NoFuture.Gen
{
    /// <summary>
    /// Utility type to get <see cref="PdbTargetLine"/> data by source code file.
    /// </summary>
    public class SortPdb
    {
        /// <summary>
        /// Given the various inputs this function produces a dictionary of many <see cref="PdbTargetLine"/>
        /// per source code file's full path name.
        /// </summary>
        /// <param name="asm">
        /// This assembly must have full type resolution (meaning all and every assembly it depends on must 
        /// be resolvable upon the runtime's call to <see cref="AppDomain.AssemblyResolve"/>)  or a  
        /// <see cref="System.TypeLoadException"/> will occur.
        /// </param>
        /// <param name="staticsOnly"></param>
        /// <param name="pdblines">
        /// This is produced in two steps, one is the autonomous invocation of NoFuture 
        /// modification to Dia2Dump.exe using the '-l' (x2D x6C) switch, the second step is deserializing the invocation's
        /// output using <see cref="AllLinesJsonDataFile.GetData"/> 
        /// </param>
        /// <returns></returns>
        public static Dictionary<string, List<PdbTargetLine>> GetSortedPdbDataByFileName(Assembly asm, bool staticsOnly, PdbAllLines pdblines)
        {
            var mems = GetPdbMembers(asm, staticsOnly);
            if(mems.targets.Count <= 0)
                throw new RahRowRagee("NoFuture.Gen.SortPdb.GetPdbMembers returned an empty list.");

            var unsortedTargetLines = GetPdbTargetLines(mems.GetDistinctTargets(), pdblines);

            if(unsortedTargetLines.Count <= 0)
                throw new RahRowRagee("NoFuture.Gen.SortPdb.GetPdbTargetLines returned an empty array.");

            return SortTargetLinesBySrcFile(unsortedTargetLines);
        }

        #region internal steps
        /// <summary>
        /// Intended to delimit a target list of member names to member types from an assembly.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="staticsOnly"></param>
        /// <returns></returns>
        internal static SortedPdbMembers GetPdbMembers(Assembly asm, bool staticsOnly)
        {
            if(asm == null)
                throw new ArgumentNullException("asm");
            var mems = new SortedPdbMembers();
            if (staticsOnly)
            {
                foreach (
                    var t in
                        asm.NfGetTypes()
                            .Where(x => !Util.TypeName.IsClrGeneratedType(x.Name)))
                {
                    var dclType = t.FullName;

                    foreach (var mem in t.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Static |
                                                     BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        mems.targets.Add(new SortedPdbMemberTarget
                        {
                            declaringType = dclType,
                            name = mem.Name
                        });
                    }
                }

                return mems;
            }

            foreach (
                var t in
                    asm.NfGetTypes()
                        .Where(x => !Util.TypeName.IsClrGeneratedType(x.Name)))
            {
                var dclType = t.FullName;

                foreach (var mem in t.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                 BindingFlags.NonPublic | BindingFlags.Public))
                {
                    mems.targets.Add(new SortedPdbMemberTarget
                    {
                        declaringType = dclType,
                        name = mem.Name
                    });
                }
            }
            return mems;
        }

        /// <summary>
        /// Performs the union op of reflected types to pdb lines
        /// </summary>
        /// <param name="distinctTargets"></param>
        /// <param name="pdblines"></param>
        /// <returns></returns>
        internal static List<PdbTargetLine> GetPdbTargetLines(List<SortedPdbMemberTarget> distinctTargets, PdbAllLines pdblines)
        {
            if(distinctTargets == null)
                throw new ArgumentNullException("distinctTargets");
            if(pdblines == null || pdblines.allLines == null || pdblines.allLines.Length <= 0)
                throw new ArgumentNullException("pdblines");

            var unsortedTargetLines = new List<PdbTargetLine>();
            foreach (var v in distinctTargets)
            {
                var dclName = v.name;
                var dclType = v.declaringType;

                foreach (
                    var p in
                        pdblines.allLines.Where(x => x.moduleName == dclType)
                            .SelectMany(x => x.moduleSymbols)
                            .Where(x => x.symbolName == dclName)
                            .ToList())
                {
                    if (!File.Exists(p.file))
                    {
                        Console.WriteLine(
                            @"Cannot extract members for {0} {1} because it references a non-existent file", dclType,
                            dclName);
                    }
                    else
                    {
                        unsortedTargetLines.Add(new PdbTargetLine()
                        {
                            SrcFile = p.file,
                            StartAt = p.firstLine.lineNumber,
                            EndAt = p.lastLine.lineNumber,
                            OwningTypeFullName = dclType,
                            MemberName = dclName,
                            IndexId = p.symIndexId
                        });
                    }
                }
            }
            return unsortedTargetLines;
        }

        /// <summary>
        /// Tags the union op results of <see cref="GetPdbTargetLines"/> with a specific file's full name.
        /// </summary>
        /// <param name="unsortedTargetLines"></param>
        /// <returns></returns>
        internal static Dictionary<string, List<PdbTargetLine>> SortTargetLinesBySrcFile(List<PdbTargetLine> unsortedTargetLines)
        {
            if(unsortedTargetLines == null)
                throw new ArgumentNullException("unsortedTargetLines");

            var uniqueSrcFiles = unsortedTargetLines.Select(x => x.SrcFile).Distinct().ToList();

            var locatedMembers = uniqueSrcFiles.ToDictionary(usf => usf, usf => new List<PdbTargetLine>());

            foreach (var fileKey in uniqueSrcFiles.Where(x => x != null))
                locatedMembers[fileKey].AddRange(unsortedTargetLines.Where(x => x.SrcFile == fileKey).ToList());

            return locatedMembers;
        }
        #endregion
    }

    #region internal types used to sort
    [Serializable]
    internal class SortedPdbMembers
    {
        public List<SortedPdbMemberTarget> targets = new List<SortedPdbMemberTarget>();

        public List<SortedPdbMemberTarget> GetDistinctTargets()
        {
            return targets.Distinct(new SortedPdbMemberComparer()).ToList();
        }
    }
    [Serializable]
    internal class SortedPdbMemberTarget
    {
        public string name;
        public string declaringType;

        public override int GetHashCode()
        {
            return (string.IsNullOrWhiteSpace(name) ? 1 : name.GetHashCode()) +
                   (string.IsNullOrWhiteSpace(declaringType) ? 1 : declaringType.GetHashCode());
        }
    }

    [Serializable]
    internal class TargetLines
    {
        public List<PdbTargetLine> unsortedMembers = new List<PdbTargetLine>();
    }

    internal class SortedPdbMemberComparer : IEqualityComparer<SortedPdbMemberTarget>
    {
        public bool Equals(SortedPdbMemberTarget x, SortedPdbMemberTarget y)
        {
            if (x == null && y == null)
                return false;
            if (x == null || y == null)
                return false;
            return string.Equals(x.declaringType, y.declaringType) && string.Equals(x.name, y.name);
        }

        public int GetHashCode(SortedPdbMemberTarget obj)
        {
            return obj.GetHashCode();
        }
    }

    #endregion
}
