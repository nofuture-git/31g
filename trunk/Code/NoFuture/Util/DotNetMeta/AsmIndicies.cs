﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NoFuture.Util.DotNetMeta
{
    /// <summary>
    /// Bundler type for <see cref="MetadataTokenAsm"/>
    /// </summary>
    [Serializable]
    public class AsmIndicies : IEnumerable<MetadataTokenAsm>
    {
        public string Msg;
        public MetadataTokenAsm[] Asms;
        public MetadataTokenStatus St;

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

        public IEnumerator<MetadataTokenAsm> GetEnumerator()
        {
            return Asms.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count()
        {
            return Asms.Length;
        }
    }
}