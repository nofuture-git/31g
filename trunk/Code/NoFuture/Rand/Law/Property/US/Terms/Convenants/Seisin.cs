using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms.Convenants
{
    /// <summary>
    /// a promise that the offeree has legal possession of the property
    /// </summary>
    [Aka("warranty of possession")]
    [EtymologyNote("old french", "seisine", "completion of the ceremony of feudal investiture")]
    public class CovenantOfSeisin : Term<ILegalProperty>
    {
        public CovenantOfSeisin(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public CovenantOfSeisin(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
