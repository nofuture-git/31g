using System;
using NoFuture.Util.DotNetMeta.TokenType;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    [Serializable]
    public class TokenReassignRequest
    {
        public MetadataTokenName SubjectTokenNames;
        public MetadataTokenName ForeignTokenNames;
        public MetadataTokenType SubjectTokenTypes;
        public MetadataTokenType ForeignTokenTypes;
        public string AsmName { get; set; }
    }
}
