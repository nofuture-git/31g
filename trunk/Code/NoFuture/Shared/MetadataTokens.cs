using System;

namespace NoFuture.Shared
{
    [Serializable]
    public class MetadataTokenId
    {
        public int Id;
        public MetadataTokenId[] Items;
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
        public int AsmIndexId;
        public string Label;
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
    }

    [Serializable]
    public enum MetadataTokenStatus
    {
        Ok,
        Error
    }
}
