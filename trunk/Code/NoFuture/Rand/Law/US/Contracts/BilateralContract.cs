using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    public class BilateralContract : LegalContract<Promise>
    {
        [Note("assent: expression of approval or agreement")]
        public MutualAssent MutualAssent { get; set; }

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            //test consideration
            if (!base.IsValid(promisor, promisee))
                return false;

            //test mutual assent
            if (MutualAssent == null)
            {
                Audit.Add($"{nameof(MutualAssent)} is null");
                return false;
            }

            if (!MutualAssent.IsValid(promisor, promisee))
            {
                Audit.Add($"{nameof(MutualAssent)}.{nameof(IsValid)} returned false");
                return false;
            }

            return true;
        }
    }
}