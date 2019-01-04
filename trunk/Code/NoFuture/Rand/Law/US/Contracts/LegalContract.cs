using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Enforceable Promise")]
    public class LegalContract<T> : ObjectiveLegalConcept where T : ObjectiveLegalConcept
    {

        [Note("bargained for: if it is sought by one and given by the other")]
        public virtual Consideration<T> Consideration { get; set; }

        [Note("this is what distinguishes a common (donative) promise from a legal one")]
        public override bool IsEnforceableInCourt => true;

        /// <summary>
        /// What the promisor is putting out there.
        /// </summary>
        [Note("Is the manifestation of willingness to enter into a bargain")]
        public virtual ObjectiveLegalConcept Offer { get; set; }

        /// <summary>
        /// A function which resolves what the offer gets in return.
        /// </summary>
        public virtual Func<ObjectiveLegalConcept, T> Acceptance { get; set; }

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (Consideration == null)
            {
                AddAuditEntry($"{nameof(Consideration)} is null");
                return false;
            }

            if (!Consideration.IsValid(promisor, promisee))
            {
                AddAuditEntry($"{nameof(Consideration)}.{nameof(IsValid)} returned false");
                return false;
            }

            return true;
        }
    }
}