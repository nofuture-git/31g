using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US
{
    /// <inheritdoc cref="IConsideration{T}"/>
    /// <summary>
    /// Is intended to determine what is an enforceable promise and what is a donative one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Note("is performance or return promise bargained for")]
    public class Consideration<T> : LegalConcept, IConsideration<T> where T : ILegalConcept
    {
        public override bool IsEnforceableInCourt => true;

        public Consideration(ComLawContract<T> contract)
        {
            if (contract == null)
                return;
            Contract = contract;
            Contract.Consideration = this;
        }

        protected internal ComLawContract<T> Contract { get; protected set; }

        /// <summary>
        /// A test for if Acceptance is actually what the promisor wants in return.
        /// </summary>
        public virtual Func<ILegalPerson, T, bool> IsSoughtByPromisor { get; set; }

        /// <summary>
        /// A test for if Offer is actually what the promisee wants in return.
        /// </summary>
        public virtual Func<ILegalPerson, ILegalConcept, bool> IsGivenByPromisee { get; set; }

        /// <summary>
        /// What is bargained for must have value
        /// </summary>
        public virtual Predicate<ILegalConcept> IsValueInEyesOfLaw { get; set; } = o => true;

        /// <summary>
        /// What is bargained for must not be a choice - it must be a duty
        /// </summary>
        [Note("is not just a choice")]
        public virtual Predicate<ILegalConcept> IsIllusionaryPromise { get; set; } = o => false;

        /// <summary>
        /// What is bargained for must not be an existing duty.  When it 
        /// is an existing duty there is no consideration and whatever the other side
        /// is bringing is a gift.
        /// </summary>
        /// <remarks>
        /// <![CDATA[ e.g. cannot bargain that we promise not to steal from each other]]>
        /// </remarks>
        [Note("is not already obligated to be done")]
        public virtual Predicate<ILegalConcept> IsExistingDuty { get; set; } = o => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = Contract.GetOfferor(persons);
            var offeree = Contract.GetOfferee(persons);

            if (IsSoughtByPromisor == null)
            {
                AddReasonEntry($"{nameof(IsSoughtByPromisor)} is null");
                return false;
            }

            if (IsGivenByPromisee == null)
            {
                AddReasonEntry($"{nameof(IsGivenByPromisee)} is null");
                return false;
            }

            var promise = Contract.Offer;
            var returnPromise = Contract.Acceptance(promise);
            if (returnPromise == null)
            {
                AddReasonEntry($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!IsSoughtByPromisor(offeror, returnPromise))
            {
                AddReasonEntry($"the return promise is not what {offeror.Name} wants");
                return false;
            }

            if (!IsGivenByPromisee(offeree, promise))
            {
                AddReasonEntry($"the offer is not what the {offeree.Name} wants");
                return false;
            }

            var valPredicate = IsValueInEyesOfLaw ?? (o => true);

            if (!valPredicate(promise))
            {
                AddReasonEntry($"The promise given by {offeror.Name} has no value in the eyes-of-the-law.");
                return false;
            }

            if (!valPredicate(returnPromise))
            {
                AddReasonEntry($"The return promise given by {offeree.Name} has no value in the eyes-of-the-law.");
                return false;
            }

            var illusionPredicate = IsIllusionaryPromise ?? (o => false);
            if (illusionPredicate(promise))
            {
                AddReasonEntry($"The promise given by {offeror.Name} is illusionary - it is not a promise at all.");
                return false;
            }
            if (illusionPredicate(returnPromise))
            {
                AddReasonEntry($"The return promise given by {offeree.Name} is illusionary - it is not a promise at all.");
                return false;
            }

            var existingDutyPredicate = IsExistingDuty ?? (o => false);
            if (existingDutyPredicate(promise))
            {
                AddReasonEntry($"The promise given by {offeror.Name} is an existing duty and cannot be bargined with nor for.");
                return false;
            }
            if (existingDutyPredicate(returnPromise))
            {
                AddReasonEntry($"The return promise given by {offeree.Name} is an existing duty and cannot be bargined with nor for.");
                return false;
            }


            return true;
        }

    }
}