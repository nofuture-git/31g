using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Antlr.CSharp4;
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
                throw new FileNotFoundException("Cannot find the compiled assembly.", assemblyPath);
            }

            //this is how we line up a source code file to a reflected runtime type
            var invokeDia2Dump = new InvokeDia2Dump.GetPdbData(assemblyPath);
            var pdbLines = invokeDia2Dump.SingleTypeNamed(typeFullName);
            if(pdbLines == null)
                throw new ItsDeadJim("Dia2Dump.exe did not return anything for the type " +
                                     $"named '{typeFullName}' from '{invokeDia2Dump.PdbAssemblyFilePath}'");
            
            //but we don't want the type in our appDomain, so we shell it out as a code-gen type
            _cgType = Etc.GetIsolatedCgOfType(assemblyPath, typeFullName, true);

            _cgType.AssignPdbSymbols(pdbLines.moduleSymbols);

            var sourceCodeFiles = pdbLines.moduleSymbols.Select(ms => ms.file);

            foreach (var src in sourceCodeFiles)
            {
                var antlrParseRslts = CsharpParseTree.InvokeParse(src);
                _cgType.AssignAntlrParseItems(GetAntlrParseItems(antlrParseRslts));
            }
        }

        public CgTypeCsSrcCode(Assembly asm, string typeFullName, params string[] sourceCodeFiles)
        {
            if (asm == null)
                throw new ArgumentNullException(nameof(asm));
            AssemblyPath = asm.Location;
            if (!File.Exists(AssemblyPath))
            {
                throw new FileNotFoundException("Cannot find the compiled assembly.", AssemblyPath);
            }

            //this is how we line up a source code file to a reflected runtime type
            var invokeDia2Dump = new InvokeDia2Dump.GetPdbData(AssemblyPath);
            var pdbLines = invokeDia2Dump.SingleTypeNamed(typeFullName);
            if (pdbLines == null)
                throw new ItsDeadJim("Dia2Dump.exe did not return anything for the type " +
                                     $"named '{typeFullName}' from '{invokeDia2Dump.PdbAssemblyFilePath}'");

            _cgType = Etc.GetCgOfType(asm, typeFullName, true);

            _cgType.AssignPdbSymbols(pdbLines.moduleSymbols);

            var scf = sourceCodeFiles?.ToList() ?? new List<string>();
            scf.AddRange(pdbLines.moduleSymbols.Select(ms => ms.file));

            foreach (var src in sourceCodeFiles)
            {
                var antlrParseRslts = CsharpParseTree.InvokeParse(src);
                _cgType.AssignAntlrParseItems(GetAntlrParseItems(antlrParseRslts));
            }
        }

        public CgTypeCsSrcCode(string assemblyPath, string typeFullName, params string[] sourceCodeFiles)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
                throw new ArgumentNullException(nameof(assemblyPath));
            AssemblyPath = assemblyPath;
            if (!File.Exists(AssemblyPath))
            {
                throw new FileNotFoundException("Cannot find the compiled assembly.", assemblyPath);
            }
            _cgType = Etc.GetIsolatedCgOfType(assemblyPath, typeFullName, true);
            if (sourceCodeFiles == null || !sourceCodeFiles.Any())
                return;

            foreach (var src in sourceCodeFiles)
            {
                var antlrParseRslts = CsharpParseTree.InvokeParse(src);
                _cgType.AssignAntlrParseItems(GetAntlrParseItems(antlrParseRslts));
            }
        }

        #endregion

        #region internal helpers

        internal static int[] FilterOutLinesNotInMethods(int[] lineNumbers, List<Tuple<int, int>> affirmList)
        {
            return lineNumbers.Where(ln => affirmList.Any(x => ln >= x.Item1 && ln <= x.Item2)).ToArray();
        }

        /// <summary>
        /// Factory method to convert the same type to one defined in this assembly, 
        /// </summary>
        /// <param name="parseResults"></param>
        /// <returns></returns>
        internal List<LangRules.AntlrParseItem> GetAntlrParseItems(CsharpParseResults parseResults)
        {
            var pItems = new List<LangRules.AntlrParseItem>();
            if (parseResults == null || !parseResults.TypeMemberBodies.Any())
                return pItems;
            foreach (var csItem in parseResults.TypeMemberBodies)
            {
                var pItem = new LangRules.AntlrParseItem
                {
                    Name = csItem.Name,
                    Start = csItem.Start,
                    End = csItem.End,
                    BodyStart = csItem.BodyStart,
                    DeclTypeName = csItem.DeclTypeName,
                    Namespace = csItem.Namespace,
                    DeclBodyStart = csItem.DeclBodyStart,
                    DeclEnd = csItem.DeclEnd,
                    NamespaceBodyStart = csItem.NamespaceBodyStart,
                    NamespaceEnd = csItem.NamespaceEnd,
                    NamespaceStart = csItem.NamespaceStart,
                    DeclStart = csItem.DeclStart,
                };

                pItem.Attributes.AddRange(csItem.Attributes);
                pItem.AccessModifiers.AddRange(csItem.AccessModifiers);
                pItem.Parameters.AddRange(csItem.Parameters);
                pItems.Add(pItem);
            }

            return pItems;
        }

        #endregion
    }
}
