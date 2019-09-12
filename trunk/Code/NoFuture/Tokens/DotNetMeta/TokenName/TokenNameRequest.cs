using System;
using NoFuture.Tokens.DotNetMeta.TokenId;

namespace NoFuture.Tokens.DotNetMeta.TokenName
{
    [Serializable]
    public class TokenNameRequest
    {
        public MetadataTokenId[] Tokens { get; set; }
        public bool MapFullCallStack { get; set; }
    }
}
