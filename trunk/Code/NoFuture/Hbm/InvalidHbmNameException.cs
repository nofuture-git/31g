using System;

namespace NoFuture.Hbm
{
    public class InvalidHbmNameException : Exceptions.RahRowRagee
    {
        public InvalidHbmNameException(string msg) : base(msg) { }
        public InvalidHbmNameException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
