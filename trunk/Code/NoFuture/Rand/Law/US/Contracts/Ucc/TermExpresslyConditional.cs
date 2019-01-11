using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// A kind of term which cancels out the typically UCC allowment for adding in new 
    /// terms to an agreement.
    /// </summary>
    [Aka("UCC 2-207(2)(a)")]
    public class TermExpresslyConditional : Term<object>
    {
        private const string REF_TO = "TermExpresslyConditional";
        private static TermExpresslyConditional _singleton;
        private TermExpresslyConditional() : base("", REF_TO)
        {
        }

        public static TermExpresslyConditional Value => _singleton ?? (_singleton = new TermExpresslyConditional());

        public override int GetHashCode()
        {
            return REF_TO.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is TermExpresslyConditional;
        }
    }
}
