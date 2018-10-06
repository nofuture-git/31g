using System;
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

        public string GetAssemblyPathFromRoot(string folderPath, int idx)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                return null;

            var owningAsmName = Asms.FirstOrDefault(x => x.IndexId == idx);
            if (string.IsNullOrWhiteSpace(owningAsmName?.AssemblyName))
                return null;

            var di = new DirectoryInfo(folderPath);
            foreach (var d in di.EnumerateFileSystemInfos())
            {
                if (!new[] {".dll", ".exe"}.Contains(d.Extension))
                    continue;
                var dAsmName = AssemblyName.GetAssemblyName(d.FullName);
                var eAsmName = new AssemblyName(owningAsmName.AssemblyName);
                if (AssemblyName.ReferenceMatchesDefinition(dAsmName, eAsmName))
                    return d.FullName;
            }

            return null;
        }

        public int Count()
        {
            return Asms.Length;
        }
    }
}