using System;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    public class UnilateralConsideration : Consideration<Performance>
    {
        public override Func<LegalDuty, Performance> GetInReturnFor { get; set; }
        public override Func<ILegalPerson, Performance, bool> IsSoughtByPromisor { get; set; }
        public override Func<ILegalPerson, LegalDuty, bool> IsGivenByPromisee { get; set; }
    }
}