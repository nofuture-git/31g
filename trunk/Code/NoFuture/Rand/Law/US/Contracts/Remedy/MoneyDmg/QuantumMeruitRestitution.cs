using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[Restatement (Second) of Contracts § 343(2) ]]>
    /// </summary>
    /// <remarks>
    /// the amount which the services could have been purchased from 
    /// one in that position, time and circumstances.
    /// </remarks>
    public class QuantumMeruitRestitution<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public QuantumMeruitRestitution(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// the injured party is entitled to restitution for any benefit that he 
        /// has conferred on the other party  by way of part performance or reliance.
        /// </summary>
        public Func<ILegalPerson, decimal> CalcPerformanceRenderedValue { get; set; } = lp => 0m;

        protected internal override decimal CalcLoss(ILegalPerson lp)
        {
            return CalcPerformanceRenderedValue(lp);
        }
    }
}
