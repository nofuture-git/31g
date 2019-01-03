using System;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    public class BilateralConsideration : Consideration<Promise>
    {
        public override Func<ObjectiveLegalConcept, Promise> GetInReturnFor { get; set; }
        public override Func<ILegalPerson, Promise, bool> IsSoughtByPromisor { get; set; }
        public override Func<ILegalPerson, ObjectiveLegalConcept, bool> IsGivenByPromisee { get; set; }
    }
}