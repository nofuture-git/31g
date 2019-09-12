using System;
using NoFuture.Tokens.DotNetMeta.TokenType;

namespace NoFuture.Tokens.DotNetMeta.TokenName
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
