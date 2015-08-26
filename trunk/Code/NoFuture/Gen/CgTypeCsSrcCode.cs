using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoFuture.Exceptions;

namespace NoFuture.Gen
{
    /// <summary>
    /// The C# implementation of the <see cref="ICgTypeWithSrcCode"/> interface.
    /// </summary>
    [Serializable]
    public class CgTypeCsSrcCode : ICgTypeWithSrcCode
    {
        #region fields
        protected CgType _cgType;
        #endregion

        #region properties
        public CgType CgType { get { return _cgType; } }
        public string RootPdbOutDirectory { get; set; }
        public string AssemblyPath { get; set; }
        public string OriginalSrcFile { get; set; }
        public List<string> ErrorMessages { get; set; }

        #endregion

        #region ctor

        public CgTypeCsSrcCode(string assemblyPath, string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                throw new ArgumentNullException("assemblyPath");
            AssemblyPath = assemblyPath;
            if (!File.Exists(AssemblyPath))
            {
                throw new ItsDeadJim(string.Format("No such file '{0}'.", AssemblyPath));
            }

            var invokeDia2Dump = new InvokeDia2Dump.GetPdbData(assemblyPath);
            var pdbLines = invokeDia2Dump.SingleTypeNamed(typeFullName);
            if(pdbLines == null)
                throw new ItsDeadJim(string.Format("Dia2Dump.exe did not return anything for the type named '{0}'",typeFullName));
            
            RootPdbOutDirectory = TempDirectories.AppData;
            if (!Directory.Exists(RootPdbOutDirectory))
            {
                throw new ItsDeadJim(string.Format("No such Directory '{0}'.", RootPdbOutDirectory));
            }
            _cgType = Etc.GetIsolatedCgOfType(assemblyPath, typeFullName, true);

            var pdbTargetLines = PdbTargetLine.ParseFrom(pdbLines, _cgType.AssemblyQualifiedName);
            OriginalSrcFile = pdbTargetLines.First(x => !string.IsNullOrWhiteSpace(x.SrcFile)).SrcFile;
            if (!File.Exists(OriginalSrcFile))
            {
                throw new ItsDeadJim(string.Format("No such file '{0}'.", OriginalSrcFile));    
            }

            _cgType.TypeFiles = new CgTypeFiles(RootPdbOutDirectory, OriginalSrcFile, _cgType.AssemblyQualifiedName);
            var cgTypeFile = _cgType.TypeFiles;
            cgTypeFile.ClearWorkingFolderOfAllContent();
            cgTypeFile.SaveUsingStatementsToFile();
            cgTypeFile.SavePdbLinesToFile(pdbTargetLines);
        }

        #endregion

        #region internal helpers

        internal static int[] FilterOutLinesNotInMethods(int[] lineNumbers, List<Tuple<int, int>> affirmList)
        {
            return lineNumbers.Where(ln => affirmList.Any(x => ln >= x.Item1 && ln <= x.Item2)).ToArray();
        }
        #endregion
    }
}
