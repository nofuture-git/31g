using System;
using NoFuture.Tokens.DotNetMeta.TokenAsm;
using NoFuture.Tokens.DotNetMeta.TokenType;

namespace NoFuture.Tokens.DotNetMeta.TokenId
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