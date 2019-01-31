using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy
{
    /// <inheritdoc />
    /// <summary>
    /// damages that the parties have agreed in advance that they would pay if the contract were breached
    /// </summary>
    public class LiquidatedDmg<T> : RemedyBase<T> where T : IObjectiveLegalConcept
    {
        public LiquidatedDmg(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }
    }
}
