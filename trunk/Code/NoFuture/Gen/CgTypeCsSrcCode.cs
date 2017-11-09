using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoFuture.Shared.Core;

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
        public CgType CgType => _cgType;
        public string AssemblyPath { get; set; }
        public List<string> ErrorMessages { get; set; }

        #endregion

        #region ctor

        public CgTypeCsSrcCode(string assemblyPath, string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                throw new ArgumentNullException(nameof(assemblyPath));
            AssemblyPath = assemblyPath;
            if (!File.Exists(AssemblyPath))
            {
                throw new ItsDeadJim($"No such file '{AssemblyPath}'.");
            }

            var invokeDia2Dump = new InvokeDia2Dump.GetPdbData(assemblyPath);
            var pdbLines = invokeDia2Dump.SingleTypeNamed(typeFullName);
            if(pdbLines == null)
                throw new ItsDeadJim($"Dia2Dump.exe did not return anything for the type named '{typeFullName}'");
            
            _cgType = Etc.GetIsolatedCgOfType(assemblyPath, typeFullName, true);

            _cgType.AssignPdbSymbols(pdbLines.moduleSymbols);
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
