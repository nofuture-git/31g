using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <summary>
    /// The type which from the reciprocal of an offer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Note("a consideration: a performance or return promise must be bargained for")]
    public class Consideration<T> : ObjectiveLegalConcept where T : ObjectiveLegalConcept
    {
        private readonly LegalContract<T> _contract;
        public override bool IsEnforceableInCourt => true;

        public Consideration(LegalContract<T> contract)
        {
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

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (_contract?.Offer == null)
            {
                AddAuditEntry($"{nameof(_contract.Offer)} is null");
                return false;
            }

            if (_contract?.Acceptance == null)
            {
                AddAuditEntry($"{nameof(_contract.Acceptance)} is null");
                return false;
            }

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

            if (!_contract.Offer.IsEnforceableInCourt)
            {
                AddAuditEntry($"the offer is not enforceable in court");
                AddAuditEntryRange(_contract.Offer.GetAuditEntries());
                return false;
            }

            var returnPromise = _contract.Acceptance(_contract.Offer);
            if (returnPromise == null)
            {
                AddAuditEntry($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!returnPromise.IsEnforceableInCourt)
            {
                AddAuditEntry($"the return promise is not enforceable in court");
                AddAuditEntryRange(returnPromise.GetAuditEntries());
                return false;
            }

            if (!IsSoughtByPromisor(offeror, returnPromise))
            {
                AddAuditEntry($"the return promise is not what {offeror.Name} wants");
                return false;
            }

            if (!IsGivenByPromisee(offeree, _contract.Offer))
            {
                AddAuditEntry($"the offer is not what the {offeree.Name} wants");
                return false;
            }

            return true;
        }

    }
}