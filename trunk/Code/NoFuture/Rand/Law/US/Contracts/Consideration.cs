using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Note("a consideration: a performance or return promise must be bargained for")]
    public class Consideration
    {
        public Promise Offer { get; set; }

        public Func<Promise, Promise> GetReturnPromise { get; set; }

        public Func<ILegalPerson, Promise, bool> IsSoughtByPromisor { get; set; }
        public Func<ILegalPerson, Promise, bool> IsGivenByPromisee { get; set; }

        public bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (Offer == null || GetReturnPromise == null || IsSoughtByPromisor == null || IsGivenByPromisee == null)
                return false;

            var returnPromise = GetReturnPromise(Offer);
            if (returnPromise == null)
                return false;

            if (!Offer.IsEnforceableInCourt || !returnPromise.IsEnforceableInCourt)
                return false;

            return IsSoughtByPromisor(promisor, returnPromise)
                   && IsGivenByPromisee(promisee, Offer);
        }
    }
}