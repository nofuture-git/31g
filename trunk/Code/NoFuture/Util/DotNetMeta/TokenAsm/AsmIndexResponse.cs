using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenAsm
{
    /// <summary>
    /// Bundler type for <see cref="MetadataTokenAsm"/>
    /// </summary>
    [Serializable]
    public class AsmIndexResponse
    {
        public string Msg { get; set; }
        public MetadataTokenAsm[] Asms { get; set; }
        public MetadataTokenStatus St { get; set; }

        public static AsmIndexResponse ReadFromFile(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return new AsmIndexResponse();
            var jsonContent = File.ReadAllText(fullFileName);
            return JsonConvert.DeserializeObject<AsmIndexResponse>(jsonContent);
        }

        public static void SaveToFile(string filePath, AsmIndexResponse rootTokenName)
        {
            if (rootTokenName?.Asms == null || !rootTokenName.Asms.Any())
                return;
            var json = JsonConvert.SerializeObject(rootTokenName, Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(filePath, json);
        }

        public Assembly GetAssemblyByIndex(int idx)
        {
            var owningAsmName = Asms.FirstOrDefault(x => x.IndexId == idx);
            if (owningAsmName == null)
                return null;
            var owningAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(
                        x =>
                            string.Equals(x.GetName().FullName, owningAsmName.AssemblyName,
                                StringComparison.OrdinalIgnoreCase));
            return owningAsm;
        }

        public int? GetAssemblyIndexByName(string assemblyName)
        {
            if (Asms == null || !Asms.Any())
                return null;
            foreach (var asm in Asms)
            {
                if (string.Equals(asm.AssemblyName, assemblyName))
                    return asm.IndexId;
            }

            return null;
        }

        /// <summary>
        /// Performs a lookup going from Type name to Owning Assembly Index to Assembly Name to Assembly path
        /// </summary>
        /// <param name="binFolders">
        /// The \bin folder(s) containing all the various assemblies 
        /// </param>
        /// <param name="typeName">
        /// The full name of the type whose assembly path is being sought
        /// </param>
        /// <param name="asmIndexResponse">
        /// The loaded Assembly Index response typically loaded form file
        /// </param>
        /// <param name="tokenType">
        /// The token type found elsewhere typically by loading the TokenTypeResponse from file
        /// </param>
        /// <returns>
        /// The full path to the assembly which defines the given type
        /// </returns>
        public static string GetAssemblyPathFromRoot(
            string typeName, 
            AsmIndexResponse asmIndexResponse,
            TokenType.MetadataTokenType tokenType,
            params string[] binFolders)
        {
            if (binFolders.Length <= 0 
                || asmIndexResponse == null
                || tokenType == null)
                return null;
            
            //from index to full assembly name
            var assemblyIdx = asmIndexResponse.Asms.FirstOrDefault(a => a.IndexId == tokenType.OwnAsmIdx);
            if (assemblyIdx == null)
                return null;

            var assemblyName = new AssemblyName(assemblyIdx.AssemblyName);

            foreach (var binFolder in binFolders)
            {
                if(string.IsNullOrWhiteSpace(binFolder) || !Directory.Exists(binFolder))
                    continue;
                var di = new DirectoryInfo(binFolder);
                foreach (var d in di.EnumerateFileSystemInfos())
                {
                    if (!new[] { ".dll", ".exe" }.Contains(d.Extension))
                        continue;
                    var dAsmName = AssemblyName.GetAssemblyName(d.FullName);

                    if (AssemblyName.ReferenceMatchesDefinition(dAsmName, assemblyName))
                    {
                        return d.FullName;
                    }
                }
            }

            return null;
        }
    }
}