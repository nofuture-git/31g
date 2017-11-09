using System;

namespace NoFuture.Shared.Core
{
    public class Arg : Exception
    {
        public Arg(string msg) : base(msg) { }

        public Arg(string msg, Exception innerException) : base(msg, innerException) { }
    }

    public class General : Exception
    {
        public General(string msg) : base(msg) { }
        public General(string msg, Exception innerException) : base(msg, innerException) { }
    }

    public class ItsDeadJim : General
    {
        public ItsDeadJim(string msg) : base(msg) { }
        public ItsDeadJim(string msg, Exception innerException) : base(msg, innerException) { }
    }

    public class RahRowRagee : General
    {
        public RahRowRagee(string msg) : base(msg) { }
        public RahRowRagee(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
