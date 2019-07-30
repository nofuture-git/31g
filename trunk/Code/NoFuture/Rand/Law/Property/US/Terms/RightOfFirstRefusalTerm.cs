using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms
{
    /// <summary>
    /// Given some bargain, its the right to inject yourself in place of the buyer.
    /// </summary>
    /// <remarks>
    /// Similar to an &quot;Option&quot; contract term but this one does not compel the owner to sell
    /// </remarks>
    [Aka("preemptive rights")]
    public class RightOfFirstRefusalTerm : TermCategory
    {
        protected override string CategoryName => "rights of first refusal";

        public virtual bool IsCompelOwnerToSell => false;
    }
}
