using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Terms
{
    /// <inheritdoc />
    /// <summary>
    /// A kind of term which cancels out the typically UCC allowment for adding in new 
    /// terms to an agreement.
    /// </summary>
    [Aka("UCC 2-207(2)(a)", "merger clause")]
    public class ExpresslyConditionalTerm : ContractTerm<object>
    {
        private const string REF_TO = "TermExpresslyConditional";
        private static ExpresslyConditionalTerm _singleton;
        private ExpresslyConditionalTerm() : base(REF_TO, REF_TO)
        {
        }

        public static ExpresslyConditionalTerm Value => _singleton ?? (_singleton = new ExpresslyConditionalTerm());

        public override int GetHashCode()
        {
            return REF_TO.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is ExpresslyConditionalTerm;
        }
    }
}
