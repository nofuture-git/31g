using System;

namespace NoFuture.Rand.Law.Contract.US.Remedy.MoneyDmg
{
    /// <summary>
    /// Limits on the means and methods by which parties can prove damages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitsToDmg<T> : MoneyDmgBase<T> where T : ILegalConcept
    {
        public LimitsToDmg(IContract<T> contract) : base(contract)
        {

        }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 351, Article 74 CISG ]]>
        /// American law prohibits awarding damages that were not foreseeable to both 
        /// parties at the time of contracting
        /// </summary>
        public Func<ILegalPerson, decimal> CalcUnforeseeable { get; set; } = o => 0m;

        /// <summary>
        /// a plaintiff cannot hold a defendant liable for damages which need not have been incurred
        /// </summary>
        public Func<ILegalPerson, decimal> CalcAvoidable { get; set; } = o => 0m;

        /// <summary>
        /// damages cannot be beyond what reasonably certain - meaning cannot stretch in
        /// speculation
        /// </summary>
        /// <remarks>
        /// speculative, unproven, uncertain, changing, chancy, untried are indicators of unreasonable
        /// </remarks>
        public Func<ILegalPerson, decimal> CalcUncertainty { get; set; } = o => 0m;

        protected internal override decimal CalcMoneyRemedy(ILegalPerson lp)
        {
            return CalcUnforeseeable(lp) + CalcAvoidable(lp) + CalcUncertainty(lp);
        }
    }
}
