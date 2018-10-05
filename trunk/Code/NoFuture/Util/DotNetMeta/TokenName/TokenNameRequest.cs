using System;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    [Serializable]
    public class TokenNameRequest
    {
        public MetadataTokenId[] Tokens;
    }
}
