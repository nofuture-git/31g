using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <summary>
    /// Limits on the means and methods by which parties can prove damages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitsToDmg<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public LimitsToDmg(IContract<T> contract) : base(contract)
        {

        }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 351, Article 74 CISG ]]>
        /// American law prohibts awarding damages that were not foreseeable to both 
        /// parties at the time of contracting
        /// </summary>
        public Func<ILegalPerson, decimal> CalcUnforeseeable { get; set; } = o => 0m;

        /// <summary>
        /// cannot award damages for breach-losses when they could have been avoided by
        /// the plaintiff
        /// </summary>
        public Func<ILegalPerson, decimal> CalcAvoidable { get; set; } = o => 0m;

        /// <summary>
        /// damages cannot be beyond what reasonably certain - meaning cannot stretch in
        /// speculation
        /// </summary>
        public Func<ILegalPerson, decimal> CalcUncertianty { get; set; } = o => 0m;

        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            return CalcUnforeseeable(lp) + CalcAvoidable(lp) + CalcUncertianty(lp);
        }
    }
}
