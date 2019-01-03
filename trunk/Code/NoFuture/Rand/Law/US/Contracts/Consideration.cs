using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <summary>
    /// The type which from the reciprocal of an offer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Note("a consideration: a performance or return promise must be bargained for")]
    public class Consideration<T> : IObjectiveLegalConcept where T : ObjectiveLegalConcept
    {
        private readonly List<string> _audit = new List<string>();
        public virtual List<string> Audit => _audit;
        private readonly LegalContract<T> _contract;

        public Consideration(LegalContract<T> contract)
        {
            _contract = contract;
            _contract.Consideration = this;
        }

        /// <summary>
        /// A test for if Acceptance is actually what the promisor wants.
        /// </summary>
        public virtual Func<ILegalPerson, T, bool> IsSoughtByPromisor { get; set; }

        /// <summary>
        /// A test for if Offer is actually what the promisee wants.
        /// </summary>
        public virtual Func<ILegalPerson, ObjectiveLegalConcept, bool> IsGivenByPromisee { get; set; }

        public bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (_contract?.Offer == null)
            {
                _audit.Add($"{nameof(Audit)} is null");
                return false;
            }

            if (_contract?.Acceptance == null)
            {
                _audit.Add($"{nameof(_contract.Acceptance)} is null");
                return false;
            }

            if (IsSoughtByPromisor == null)
            {
                _audit.Add($"{nameof(IsSoughtByPromisor)} is null");
                return false;
            }

            if (IsGivenByPromisee == null)
            {
                _audit.Add($"{nameof(IsGivenByPromisee)} is null");
                return false;
            }

            if (!_contract.Offer.IsEnforceableInCourt)
            {
                _audit.Add($"the offer is not enforceable in court");
                _audit.AddRange(_contract.Offer.Audit);
                return false;
            }

            var returnPromise = _contract.Acceptance(_contract.Offer);
            if (returnPromise == null)
            {
                _audit.Add($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!returnPromise.IsEnforceableInCourt)
            {
                _audit.Add($"the return promise is not enforceable in court");
                _audit.AddRange(returnPromise.Audit);
                return false;
            }

            if (!IsSoughtByPromisor(promisor, returnPromise))
            {
                _audit.Add($"the return promise is not what {promisor.Name} wants");
                return false;
            }

            if (!IsGivenByPromisee(promisee, _contract.Offer))
            {
                _audit.Add($"the offer is not what the {promisee.Name} wants");
                return false;
            }

            return true;
        }
    }
}