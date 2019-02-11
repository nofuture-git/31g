using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    /// <summary>
    /// <![CDATA[
    /// mere technical, inadvertent or unimportant omissions or defects 
    /// would NOT amount to a breach
    /// ]]>
    /// </summary>
    [Aka("close-enough")]
    public class SubstantialPerformance<T> : PerfectTender<T> where T : ILegalConcept
    {
        public SubstantialPerformance(IContract<T> contract) : base(contract)
        {
        }

        protected internal override bool StandardsTest(ILegalConcept a, ILegalConcept b)
        {
            return a.EquivalentTo(b) || b.EquivalentTo(a);
        }
    }
}
