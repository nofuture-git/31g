using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// If a party’s tender of performance fails in any way 
    /// to meet the contract requirements, the party cannot 
    /// recover under the contract
    /// ]]>
    /// </summary>
    /// <remarks>
    /// does not apply to UCC installment contracts
    /// </remarks>
    [Aka("UCC 2-601")]
    public class PerfectTender<T> : StandardsBase<T> where T : IObjectiveLegalConcept
    {
        public  PerfectTender(IContract<T> contract) : base(contract)
        {
        }

        protected internal override bool StandardsTest(IObjectiveLegalConcept a, IObjectiveLegalConcept b)
        {
            return a.Equals(b) || b.Equals(a);
        }
    }
}
