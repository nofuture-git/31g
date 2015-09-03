using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NoFuture.Shared
{
    [Serializable]
    public class MetadataTokenId
    {
        public int RswAsmIdx;
        public int Id;
        public MetadataTokenId[] Items;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var objMdti = obj as MetadataTokenId;
            if (objMdti == null)
                return false;
            return objMdti.Id == Id && objMdti.RswAsmIdx == RswAsmIdx;
        }

        public override int GetHashCode()
        {
            return RswAsmIdx.GetHashCode() + Id.GetHashCode();
        }
    }

    [Serializable]
    public class TokenIds
    {
        public string Msg;
        public MetadataTokenId[] Tokens;
        public MetadataTokenStatus St;
    }

    [Serializable]
    public class MetadataTokenName
    {
        public int Id;
        public string Name;
        public int ObyAsmIdx;
        public string Label;

        public bool IsMethodName()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.Contains(Constants.TypeMethodNameSplitOn);
        }
    }

    [Serializable]
    public class TokenNames
    {
        public string Msg;
        public MetadataTokenStatus St;
        public MetadataTokenName[] Names;
    }

    [Serializable]
    public class MetadataTokenAsm
    {
        public string AssemblyName;
        public int IndexId;
    }

    [Serializable]
    public class AsmIndicies
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
    }

    [Serializable]
    public class GetTokenIdsCriteria
    {
        private string _ranlB64;//persist this a base64 since regex can be difficult to encode\decode
        public string AsmName;

        /// <summary>
        /// A regex pattern on which to any assembly's name is a match 
        /// tokens-of-tokens (Call, Callvirt) will be resolved.
        /// </summary>
        public string ResolveAllNamedLike
        {
            get { return Encoding.UTF8.GetString(Convert.FromBase64String(_ranlB64)); }
            set
            {
                var t = value;
                if (string.IsNullOrWhiteSpace(t))
                    _ranlB64 = null;
                _ranlB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            }
        }
    }

    [Serializable]
    public enum MetadataTokenStatus
    {
        Ok,
        Error
    }
}
