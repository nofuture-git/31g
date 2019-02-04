using System;
using System.Linq;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// with property it is possible that some performance may cost alot while it adds little value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyExpectation<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public PropertyExpectation(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 348(2)(a) ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcDiminutionInValue { get; set; } = lp => 0m;

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 348(2)(b) ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcPerformanceCost { get; set; } = lp => 0m;


        /// <summary>
        /// Get the lower calculated value of two
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            var dimVal = CalcDiminutionInValue(lp);
            var perfVal = CalcPerformanceCost(lp);

            return new[] {dimVal, perfVal}.Where(v => v > 0m).Min();
        }
    }
}
