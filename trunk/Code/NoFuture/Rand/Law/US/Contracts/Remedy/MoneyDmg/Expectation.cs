using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 344(a) 
    /// The difference between the expected value and the actual value 
    /// ]]>
    /// </summary>
    public class Expectation<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public Expectation(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 347(a) ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcLossToInjured { get; set; } = o => 0m;

        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            return CalcLossToInjured(lp);
        }
    }
}
