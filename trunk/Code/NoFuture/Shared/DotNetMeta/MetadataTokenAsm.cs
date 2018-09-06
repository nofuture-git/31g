using System;

namespace NoFuture.Shared
{
    /// <summary>
    /// A dictionary for assembly names.
    /// </summary>
    [Serializable]
    public class MetadataTokenAsm
    {
        public string AssemblyName;
        public int IndexId;
        public override bool Equals(object obj)
        {
            var mta = obj as MetadataTokenAsm;
            return mta != null && string.Equals(mta.AssemblyName, AssemblyName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return AssemblyName?.GetHashCode() ?? 1;
        }
    }
}