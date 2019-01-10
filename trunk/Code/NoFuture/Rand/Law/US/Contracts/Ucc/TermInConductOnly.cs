using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// The default UCC contract formation where the battle-of-the-forms 
    /// cancelled each other out.
    /// </summary>
    [Aka("UCC 2-207(3)")]
    public class TermInConductOnly : Term<object>
    {
        private const string REF_TO = "TermInConductOnly";
        private static TermInConductOnly _singleton;
        private TermInConductOnly() : base("", REF_TO)
        {
        }

        public static TermInConductOnly Value => _singleton ?? (_singleton = new TermInConductOnly());
        public override int GetHashCode()
        {
            return REF_TO.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is TermInConductOnly;
        }
    }
}
