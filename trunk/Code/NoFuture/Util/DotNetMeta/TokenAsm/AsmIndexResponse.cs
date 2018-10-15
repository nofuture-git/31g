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

        public string GetAssemblyPathFromRoot(string folderPath, int idx)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                return null;
            if (Asms == null || !Asms.Any())
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

        public static Tuple<string,int> GetAssemblyPathFromRoot(string folderPath, string typeName)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                return null;

            var di = new DirectoryInfo(folderPath);
            var name2Score = new Dictionary<string, int>();
            foreach (var d in di.EnumerateFileSystemInfos())
            {
                if (!new[] { ".dll", ".exe" }.Contains(d.Extension))
                    continue;
                var dAsmName = AssemblyName.GetAssemblyName(d.FullName);
                //length of the name less this assembly as a namespace
                var dSimpleAsmName = dAsmName.Name;
                var lenLessAsmName = typeName.Replace(dSimpleAsmName, "").Length;
                if (name2Score.ContainsKey(d.FullName))
                {
                    if(name2Score[d.FullName] < lenLessAsmName)
                        name2Score[d.FullName] = lenLessAsmName;
                }
                else
                {
                    name2Score.Add(d .FullName, lenLessAsmName);
                }
            }

            var minLen = name2Score.Select(kv => kv.Value).Min();
            var path = name2Score.FirstOrDefault(kv => kv.Value == minLen).Key;
            return new Tuple<string, int>(path, minLen);
        }

        public string GetAssemblyPathFromRoot(string[] folders, int idx)
        {
            if (folders == null || !folders.Any())
                return null;

            foreach (var f in folders)
            {
                var m = GetAssemblyPathFromRoot(f, idx);
                if (m != null)
                    return m;
            }

            return null;
        }

        public static string GetAssemblyPathFromRoot(string[] folders, string typeName)
        {
            if (folders == null || !folders.Any())
                return null;

            var name2Score = new Dictionary<string, int>();
            foreach (var f in folders)
            {
                var m = GetAssemblyPathFromRoot(f, typeName);
                if (m != null)
                {
                    if (name2Score.ContainsKey(m.Item1))
                    {
                        if (name2Score[m.Item1] < m.Item2)
                            name2Score[m.Item1] = m.Item2;
                    }
                    else
                    {
                        name2Score.Add(m.Item1, m.Item2);
                    }
                }
            }

            var minLen = name2Score.Select(kv => kv.Value).Min();
            return name2Score.FirstOrDefault(kv => kv.Value == minLen).Key;
        }
    }
}