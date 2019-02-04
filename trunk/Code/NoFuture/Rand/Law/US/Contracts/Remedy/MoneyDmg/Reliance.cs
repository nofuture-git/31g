using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 349
    /// when restitution is too little and expectation is too big,
    /// ]]>
    /// </summary>
    public class Reliance<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public Reliance(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// damages based on reliance interest including expenditures made in preparation
        /// </summary>
        public Func<ILegalPerson, decimal> CalcPrepExpenditures { get; set; } = o => 0m;

        protected internal override decimal CalcLoss(ILegalPerson lp)
        {
            return CalcPrepExpenditures(lp);
        }
    }
}
