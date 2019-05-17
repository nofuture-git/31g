using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Contract.US.Ucc;

namespace NoFuture.Rand.Law.Contract.US.Breach
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// If a party’s tender of performance fails in any way 
    /// to meet the contract requirements, the party cannot 
    /// recover under the contract
    /// ]]>
    /// </summary>
    [Aka("UCC 2-601")]
    [Note("UCC 2-508 affords seller to right a cure of nonconformity")]
    public class PerfectTender<T> : StandardsBase<T> where T : ILegalConcept
    {
        public  PerfectTender(IContract<T> contract) : base(contract)
        {
        }

        protected internal override bool StandardsTest(ILegalConcept a, ILegalConcept b)
        {
            var uccContract = Contract as UccContract<Goods>;
            if (uccContract != null && uccContract.IsInstallmentContract)
            {
                AddReasonEntry("the perfect tender rule does not apply to UCC installment contracts-(see UCC 2-612)");
                return a.EquivalentTo(b) || b.EquivalentTo(a);
            }

            return a.Equals(b) || b.Equals(a);
        }
    }
}
