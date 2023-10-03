using System;

namespace NoFuture.Law.Contract.US.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 344(a) 
    /// The difference between the expected value and the actual value 
    /// ]]>
    /// </summary>
    public class Expectation<T> : MoneyDmgBase<T> where T : ILegalConcept
    {
        public Expectation(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[
        /// Restatement (Second) of Contracts § 347(a) 
        /// the loss in value incurred by failure to perform
        /// ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcLossToInjured { get; set; } = o => 0m;

        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            return CalcLossToInjured(lp);
        }
    }
}
