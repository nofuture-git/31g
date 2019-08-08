using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Property.US.Terms.CoOwnership
{
    /// <summary>
    /// When a cotenant is either forcibly ejected or is barred from property
    /// </summary>
    /// <remarks>
    /// an ousted cotenant will be owed market-value rent for length-of-term of ouster
    /// </remarks>
    public class OusterTerm : CoOwnerBaseTerm, ITempore
    {
        public OusterTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public OusterTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }

        public virtual DateTime Inception { get; set; }
        public virtual DateTime? Terminus { get; set; }
        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }
    }
}
