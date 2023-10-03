using System;
using NoFuture.Law;

namespace NoFuture.Law.Property.US.Terms.CoOwnership
{
    public abstract class CoOwnerBaseTerm : Term<ILegalProperty>
    {
        protected CoOwnerBaseTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        protected CoOwnerBaseTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }

        public virtual Func<ITempore, decimal> GetMarketValueRent { get; set; } = t => 0m;
    }
}
