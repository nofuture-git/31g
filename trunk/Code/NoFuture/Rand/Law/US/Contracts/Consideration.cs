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
    public abstract class Consideration<T> : IObjectiveLegalConcept where T : LegalDuty
    {
        private readonly List<string> _audit = new List<string>();
        public virtual List<string> Audit => _audit;

        /// <summary>
        /// What the promisor is putting out there.
        /// </summary>
        [Note("Is the manifestation of willingness to enter into a bargain")]
        public Promise Offer { get; set; }

        /// <summary>
        /// A function which resolves what the offer gets in return.
        /// </summary>
        public abstract Func<Promise, T> GetInReturnFor { get; set; }

        /// <summary>
        /// A test for if <see cref="GetInReturnFor"/> is actually what the promisor wants.
        /// </summary>
        public abstract Func<ILegalPerson, T, bool> IsSoughtByPromisor { get; set; }

        /// <summary>
        /// A test for if <see cref="Offer"/> is actually what the promisee wants.
        /// </summary>
        public abstract Func<ILegalPerson, Promise, bool> IsGivenByPromisee { get; set; }

        public bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (Offer == null)
            {
                _audit.Add($"{nameof(Audit)} is null");
                return false;
            }

            if (GetInReturnFor == null)
            {
                _audit.Add($"{nameof(GetInReturnFor)} is null");
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

            var returnPromise = GetInReturnFor(Offer);
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

            if (!IsGivenByPromisee(promisee, Offer))
            {
                _audit.Add($"the offer is not what the {promisee.Name} wants");
                return false;
            }

            return true;
        }
    }
}