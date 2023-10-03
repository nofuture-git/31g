using NoFuture.Rand.Law;

namespace NoFuture.Rand.Law.US.Courts
{
    /// <summary>
    /// The base type of courts of a state with broad subject matter jurisdiction
    /// </summary>
    public class StateCourt : VocaBase, ICourt
    {
        public StateCourt(string name) : base(name)
        {

        }
    }
}
