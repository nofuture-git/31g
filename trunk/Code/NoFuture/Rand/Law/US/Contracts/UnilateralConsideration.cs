using System;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    public class UnilateralConsideration : Consideration<Performance>
    {
        public override Func<Promise, Performance> GetInReturnFor { get; set; }
        public override Func<ILegalPerson, Performance, bool> IsSoughtByPromisor { get; set; }
        public override Func<ILegalPerson, Promise, bool> IsGivenByPromisee { get; set; }
    }
}