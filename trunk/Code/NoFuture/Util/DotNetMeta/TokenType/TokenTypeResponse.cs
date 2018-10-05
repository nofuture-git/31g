using System;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class TokenTypeResponse
    {
        public string Msg;
        public MetadataTokenStatus St;
        public MetadataTokenType[] Types;
    }
}
