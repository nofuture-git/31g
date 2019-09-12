using System;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenType;

namespace NoFuture.Util.DotNetMeta.TokenId
{
    /// <summary>
    /// A criteria type to send across the wire to a listening socket.
    /// </summary>
    [Serializable]
    public class TokenIdRequest : TokenTypeRequest
    {
        public string AsmName { get; set; }
        public AsmIndexResponse AsmIndices { get; set; }
    }
}