using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US
{
    /// <inheritdoc cref="IConsideration{T}"/>
    /// <summary>
    /// Is intended to determine what is an enforceable promise and what is a donative one.
    /// </summary>
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

        public virtual Func<ILegalPerson, T, bool> IsSoughtByOfferor { get; set; }

        public virtual Func<ILegalPerson, ILegalConcept, bool> IsGivenByOfferee { get; set; }

        public virtual Predicate<ILegalConcept> IsValueInEyesOfLaw { get; set; } = o => true;

        public virtual Predicate<ILegalConcept> IsIllusoryPromise { get; set; } = o => false;

        public virtual Predicate<ILegalConcept> IsExistingDuty { get; set; } = o => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

            if (IsSoughtByOfferor == null)
            {
                AddReasonEntry($"{nameof(IsSoughtByOfferor)} is null");
                return false;
            }

            if (IsGivenByOfferee == null)
            {
                AddReasonEntry($"{nameof(IsGivenByOfferee)} is null");
                return false;
            }

            var promise = Contract.Offer;
            var returnPromise = Contract.Acceptance(promise);
            if (returnPromise == null)
            {
                AddReasonEntry($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!IsSoughtByOfferor(offeror, returnPromise))
            {
                AddReasonEntry($"the return promise is not what {offeror.Name} wants");
                return false;
            }

            if (!IsGivenByOfferee(offeree, promise))
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

            var illusionPredicate = IsIllusoryPromise ?? (o => false);
            if (illusionPredicate(promise))
            {
                AddReasonEntry($"The promise given by {offeror.Name} is illusory - it is not a promise at all.");
                return false;
            }
            if (illusionPredicate(returnPromise))
            {
                AddReasonEntry($"The return promise given by {offeree.Name} is illusory - it is not a promise at all.");
                return false;
            }

            var existingDutyPredicate = IsExistingDuty ?? (o => false);
            if (existingDutyPredicate(promise))
            {
                AddReasonEntry($"The promise given by {offeror.Name} is an existing duty and cannot be bargained with nor for.");
                return false;
            }
            if (existingDutyPredicate(returnPromise))
            {
                AddReasonEntry($"The return promise given by {offeree.Name} is an existing duty and cannot be bargained with nor for.");
                return false;
            }


            return true;
        }

    }
}