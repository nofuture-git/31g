using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Note("a consideration: a performance or return promise must be bargained for")]
    public class Consideration : IObjectiveLegalConcept
    {
        private readonly List<string> _audit = new List<string>();
        public IList<string> Audit => _audit;

        public Promise Offer { get; set; }

        public Func<Promise, Promise> GetReturnPromise { get; set; }

        public Func<ILegalPerson, Promise, bool> IsSoughtByPromisor { get; set; }
        public Func<ILegalPerson, Promise, bool> IsGivenByPromisee { get; set; }

        public bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (Offer == null)
            {
                _audit.Add($"{nameof(Audit)} is null");
                return false;
            }

            if (GetReturnPromise == null)
            {
                _audit.Add($"{nameof(GetReturnPromise)} is null");
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

            var returnPromise = GetReturnPromise(Offer);
            if (returnPromise == null)
            {
                _audit.Add($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!Offer.IsEnforceableInCourt)
            {
                _audit.Add($"the offer is not enforceable in court");
                return false;
            }

            if (!returnPromise.IsEnforceableInCourt)
            {
                _audit.Add($"the return promise is not enforceable in court");
                return false;
            }

            if (!IsSoughtByPromisor(promisor, returnPromise))
            {
                _audit.Add($"the return promise is not what {promisor.Name} wants");
                return false;
            }

            if (IsGivenByPromisee(promisee, Offer))
            {
                _audit.Add($"the offer is not what the {promisee.Name} wants");
                return false;
            }

            return true;
        }
    }
}