using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Enforceable Promise")]
    public class LegalContract : Promise
    {
        [Note("assent: expression of approval or agreement")]
        public MutualAssent MutualAssent { get; set; }

        [Note("bargained for: if it is sought by one and given by the other")]
        public Consideration Consideration { get; set; }

        public override bool IsEnforceableInCourt => true;

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (Consideration == null || MutualAssent == null)
                return false;
            return Consideration.IsValid(promisor, promisee) && MutualAssent.IsValid(promisor, promisee);
        }
    }
}