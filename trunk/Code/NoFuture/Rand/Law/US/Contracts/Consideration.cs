using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <summary>
    /// Is intended to determine what is an enforcable promise and what is a donative one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Note("is performance or return promise bargained for")]
    [Aka("quid pro quo", "this for that")]
    public class Consideration<T> : ObjectiveLegalConcept where T : ObjectiveLegalConcept
    {
        private readonly LegalContract<T> _contract;
        public override bool IsEnforceableInCourt => true;

        public Consideration(LegalContract<T> contract)
        {
            if (contract == null)
                return;
            _contract = contract;
            _contract.Consideration = this;
        }

        /// <summary>
        /// A test for if Acceptance is actually what the promisor wants in return.
        /// </summary>
        public virtual Func<ILegalPerson, T, bool> IsSoughtByPromisor { get; set; }

        /// <summary>
        /// A test for if Offer is actually what the promisee wants in return.
        /// </summary>
        public virtual Func<ILegalPerson, ObjectiveLegalConcept, bool> IsGivenByPromisee { get; set; }

        /// <summary>
        /// What is bargained for must have value
        /// </summary>
        public virtual Predicate<T> IsValueInEyesOfLaw { get; set; } = o => true;

        /// <summary>
        /// What is bargined for must not be a choice - it must be a duty
        /// </summary>
        [Note("is not just a choice")]
        public virtual Predicate<T> IsIllusionaryPromise { get; set; } = o => false;

        /// <summary>
        /// What is bargined for must not be an existing duty.  When it 
        /// is an existing duty there is no consideration and whatever the other side
        /// is bringing is a gift.
        /// </summary>
        /// <remarks>
        /// <![CDATA[ e.g. cannot bargain that we promise not to steal from each other]]>
        /// </remarks>
        [Note("is not already obligated to be done")]
        public virtual Predicate<T> IsExistingDuty { get; set; } = o => false;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (IsSoughtByPromisor == null)
            {
                AddAuditEntry($"{nameof(IsSoughtByPromisor)} is null");
                return false;
            }

            if (IsGivenByPromisee == null)
            {
                AddAuditEntry($"{nameof(IsGivenByPromisee)} is null");
                return false;
            }

            var promise = _contract.Offer;
            var returnPromise = _contract.Acceptance(promise);
            if (returnPromise == null)
            {
                AddAuditEntry($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!IsSoughtByPromisor(offeror, returnPromise))
            {
                AddAuditEntry($"the return promise is not what {offeror.Name} wants");
                return false;
            }

            if (!IsGivenByPromisee(offeree, promise))
            {
                AddAuditEntry($"the offer is not what the {offeree.Name} wants");
                return false;
            }

            var valPredicate = IsValueInEyesOfLaw ?? (o => true);

            if (!valPredicate(promise as T))
            {
                AddAuditEntry($"The promise given by {offeror.Name} has no value in the eyes-of-the-law.");
                return false;
            }

            if (!valPredicate(returnPromise))
            {
                AddAuditEntry($"The return promise given by {offeree.Name} has no value in the eyes-of-the-law.");
                return false;
            }

            var illusionPredicate = IsIllusionaryPromise ?? (o => false);
            if (illusionPredicate(promise as T))
            {
                AddAuditEntry($"The promise given by {offeror.Name} is illusionary - it is not a promise at all.");
                return false;
            }
            if (illusionPredicate(returnPromise))
            {
                AddAuditEntry($"The return promise given by {offeree.Name} is illusionary - it is not a promise at all.");
                return false;
            }

            var existingDutyPredicate = IsExistingDuty ?? (o => false);
            if (existingDutyPredicate(promise as T))
            {
                AddAuditEntry($"The promise given by {offeror.Name} is an existing duty and cannot be bargined with nor for.");
                return false;
            }
            if (existingDutyPredicate(returnPromise))
            {
                AddAuditEntry($"The return promise given by {offeree.Name} is an existing duty and cannot be bargined with nor for.");
                return false;
            }


            return true;
        }

    }
}