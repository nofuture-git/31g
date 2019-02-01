using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// sum of money to compensate the plaintiff.
    /// </summary>
    public abstract class MoneyDmgBase<T> : RemedyBase<T> where T : IObjectiveLegalConcept
    {
        protected MoneyDmgBase(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 347(a) ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcLossToInjured { get; set; } = o => 0m;

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 347(b) ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcLossOther { get; set; } = o => 0m;

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 347(c) ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcLossAvoided { get; set; } = o => 0m;

        /// <summary>
        /// The method to round-off a decimal value
        /// </summary>
        public Func<decimal, decimal> Rounding { get; set; } = Math.Round;

        /// <summary>
        /// Allowable round-off
        /// </summary>
        public decimal Tolerance { get; set; } = 0.01m;

        /// <summary>
        /// <![CDATA[
        /// Restatement (Second) of Contracts § 347 (a)+(b)-(c)
        /// ]]>
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        protected internal virtual decimal CalcMeasureOfDmg(ILegalPerson lp)
        {
            var lpValue = CalcLossToInjured(lp) + CalcLossOther(lp) -
                               CalcLossAvoided(lp);

            lpValue = Rounding(lpValue);

            return lpValue;
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            var offerorValue = CalcMeasureOfDmg(offeror);
            if (offerorValue >= Tolerance)
            {
                AddReasonEntry($"{offeror.Name} value {offerorValue}");
                AddReasonEntryRange(Contract.GetReasonEntries());
                return true;
            }

            var offereeValue = CalcMeasureOfDmg(offeree);
            if (offereeValue >= Tolerance)
            {
                AddReasonEntry($"{offeree.Name} value {offereeValue}");
                AddReasonEntryRange(Contract.GetReasonEntries());
                return true;
            }

            return false;
        }
    }
}
