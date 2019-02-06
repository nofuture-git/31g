using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// sum of money to compensate the plaintiff.
    /// </summary>
    public abstract class MoneyDmgBase<T> : RemedyBase<T> where T : IObjectiveLegalConcept
    {
        private LimitsToDmg<T> _limits;
        protected MoneyDmgBase(IContract<T> contract) : base(contract)
        {
            
        }

        public LimitsToDmg<T> Limits
        {
            get
            {
                if(_limits == null)
                    _limits = new LimitsToDmg<T>(Contract);

                _limits.Rounding = Rounding;
                _limits.Tolerance = Tolerance;
                return _limits;
            }
        }

        /// <summary>
        /// Where each child type will draw a composition of relevant functions
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        protected internal abstract decimal CalcMoneyRemedy(ILegalPerson lp);

        /// <summary>
        /// <![CDATA[
        /// Restatement (Second) of Contracts § 347(b) 
        /// any other loss, including incidental or consequential 
        /// ]]>
        /// </summary>
        public Func<ILegalPerson, decimal> CalcLossOther { get; set; } = o => 0m;

        /// <summary>
        /// <![CDATA[
        /// Restatement (Second) of Contracts § 347(c) 
        /// any cost or other loss that was avoided by the breach
        /// ]]>
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
        /// Restatement (Second) of Contracts § 347 (a)+(b)-(c)-(limits)
        /// ]]>
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        private decimal GetSumByPerson(ILegalPerson lp)
        {
            var lpValue = CalcMoneyRemedy(lp) 
                          + CalcLossOther(lp) 
                          - CalcLossAvoided(lp) 
                          - Limits.CalcMoneyRemedy(lp)
                ;

            lpValue = Rounding(lpValue);

            return lpValue;
        }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var offerorValue = GetSumByPerson(offeror);
            if (offerorValue >= Tolerance)
            {
                AddReasonEntry($"{offeror.Name} value {offerorValue}");
                AddReasonEntryRange(Contract.GetReasonEntries());
                return true;
            }

            var offereeValue = GetSumByPerson(offeree);
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
