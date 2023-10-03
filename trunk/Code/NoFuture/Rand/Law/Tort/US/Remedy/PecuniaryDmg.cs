using System;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Tort.US.Remedy
{
    /// <summary>
    /// reproduce the amount of money lost by the plaintiff 
    /// </summary>
    public class PecuniaryDmg : CompensatoryDmg
    {
        public PecuniaryDmg(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Func<IPlaintiff, decimal> CalcLostIncome { get; set; } = lp => 0m;
        public Func<IPlaintiff, decimal> CalcIncreasedExpenditure { get; set; } = lp => 0m;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
            {
                return false;
            }

            var title = plaintiff.GetLegalPersonTypeName();

            if (CalcLostIncome == null || CalcIncreasedExpenditure == null)
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(CalcLostIncome)} or " +
                               $"{nameof(CalcIncreasedExpenditure)} is unassigned");
                return false;
            }

            var lostIncome = CalcLostIncome(plaintiff);
            AddReasonEntry($"{title} {plaintiff.Name}, {nameof(CalcLostIncome)} is {lostIncome}");

            var increasedExpense = CalcIncreasedExpenditure(plaintiff);
            AddReasonEntry($"{title} {plaintiff.Name}, {nameof(CalcIncreasedExpenditure)} is {increasedExpense}");

            return lostIncome + increasedExpense > 0m;
        }
    }
}
