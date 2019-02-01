using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 349
    /// idea that contracts rely on promises 
    /// ]]>
    /// </summary>
    public class Reliance<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public Reliance(IContract<T> contract) : base(contract)
        {
        }

        protected internal override decimal CalcLoss(ILegalPerson lp)
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }
    }
}
