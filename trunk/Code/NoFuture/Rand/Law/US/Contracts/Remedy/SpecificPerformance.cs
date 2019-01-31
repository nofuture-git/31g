using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy
{
    /// <inheritdoc />
    /// <summary>
    /// an order from a court, very much like an injunction, compelling a party to do what it was supposed to do
    /// </summary>
    public class SpecificPerformance<T> : RemedyBase<T> where T : IObjectiveLegalConcept
    {
        public SpecificPerformance(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }
    }
}
