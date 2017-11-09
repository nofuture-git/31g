using System;
using NoFuture.Shared.Core;

namespace NoFuture.Hbm
{
    public class InvalidHbmNameException : RahRowRagee
    {
        public InvalidHbmNameException(string msg) : base(msg) { }
        public InvalidHbmNameException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
