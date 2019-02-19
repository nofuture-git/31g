using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 349
    /// spending money in expectation of the contract then the other party breach's 
    /// now the plaintiff has all this crap they only got because of this contract
    /// ]]>
    /// </summary>
    public class Reliance<T> : MoneyDmgBase<T> where T : ILegalConcept
    {
        public Reliance(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// damages based on reliance interest including expenditures made in preparation
        /// </summary>
        public Func<ILegalPerson, decimal> CalcPrepExpenditures { get; set; } = o => 0m;

        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            return CalcPrepExpenditures(lp);
        }
    }
}
