using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Courts
{
    /// <summary>
    /// A trial court for general subject matter jurisdiction
    /// </summary>
    [Aka("district court", "superior court")]
    public class CircuitCourt : StateCourt
    {
        public CircuitCourt(string name) : base(name)
        {
        }
    }
}
