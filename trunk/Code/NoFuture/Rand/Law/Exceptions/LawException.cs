using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Exceptions
{
    public class LawException : Exception
    {
        public LawException(string msg) : base(msg) { }
        public LawException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
