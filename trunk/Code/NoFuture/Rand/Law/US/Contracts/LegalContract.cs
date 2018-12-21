using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Enforceable Promise")]
    public class LegalContract : Promise
    {
        private readonly List<string> _audit = new List<string>();
        public override IList<string> Audit => _audit;

        [Note("assent: expression of approval or agreement")]
        public MutualAssent MutualAssent { get; set; }

        [Note("bargained for: if it is sought by one and given by the other")]
        public Consideration Consideration { get; set; }

        public override bool IsEnforceableInCourt => true;

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (Consideration == null)
            {
                _audit.Add($"{nameof(Consideration)} is null");
                return false;
            }

            if (MutualAssent == null)
            {
                _audit.Add($"{nameof(MutualAssent)} is null");
                return false;
            }

            if (!Consideration.IsValid(promisor, promisee))
            {
                _audit.Add($"{nameof(Consideration)}.{nameof(IsValid)} returned false");
                return false;
            }

            if (!MutualAssent.IsValid(promisor, promisee))
            {
                _audit.Add($"{nameof(MutualAssent)}.{nameof(IsValid)} returned false");
                return false;
            }

            return true;
        }
    }
}