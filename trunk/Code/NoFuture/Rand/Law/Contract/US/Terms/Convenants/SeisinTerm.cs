using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms.Convenants
{
    /// <summary>
    /// assure the grantee that the grantor is, at the time of the conveyance, lawfully seized
    /// and has the power to convey an estate of the quality and quantity which he professes to convey
    /// </summary>
    [Aka("warranty of possession", "good right to convey")]
    [EtymologyNote("old french", "seisine", "completion of the ceremony of feudal investiture")]
    public class SeisinTerm : Term<ILegalProperty>
    {
        public SeisinTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public SeisinTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
