using System;
using System.Linq;

namespace NoFuture.Law.Contract.US.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// with property it is possible that some performance may cost a lot while it adds little value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyExpectation<T> : MoneyDmgBase<T> where T : ILegalConcept
    {
        public PropertyExpectation(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// How much market value the said performance would add to the property
        /// </summary>
        public Func<ILegalPerson, decimal> CalcDiminutionInValue { get; set; } = lp => 0m;

        /// <summary>
        /// How much it would cost to perform something on the property
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
