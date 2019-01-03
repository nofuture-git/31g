using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Enforceable Promise")]
    public class LegalContract<T> : IObjectiveLegalConcept where T : ObjectiveLegalConcept
    {
        private readonly List<string> _audit = new List<string>();
        public List<string> Audit => _audit;

        [Note("bargained for: if it is sought by one and given by the other")]
        public virtual Consideration<T> Consideration { get; set; }

        public virtual bool IsEnforceableInCourt => true;

        /// <summary>
        /// What the promisor is putting out there.
        /// </summary>
        [Note("Is the manifestation of willingness to enter into a bargain")]
        public ObjectiveLegalConcept Offer { get; set; }

        /// <summary>
        /// A function which resolves what the offer gets in return.
        /// </summary>
        public Func<ObjectiveLegalConcept, T> Acceptance { get; set; }

        public virtual bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (Consideration == null)
            {
                _audit.Add($"{nameof(Consideration)} is null");
                return false;
            }

            if (!Consideration.IsValid(promisor, promisee))
            {
                _audit.Add($"{nameof(Consideration)}.{nameof(IsValid)} returned false");
                return false;
            }

            return true;
        }
    }
}