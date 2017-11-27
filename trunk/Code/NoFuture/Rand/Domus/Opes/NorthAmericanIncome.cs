using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Gov;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IReditus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class NorthAmericanIncome : WealthBase, IReditus
    {
        #region fields
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        private readonly HashSet<Pondus> _otherIncome = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();

        #endregion

        #region ctors

        public NorthAmericanIncome(NorthAmerican american, bool isRenting = false): base(american, isRenting)
        {
        }
        #endregion

        #region properties
        public virtual IEmployment[] CurrentEmployment
        {
            get
            {
                var e = Employment.Where(x => x.ToDate == null).ToList();
                e.Sort(Comparer);
                return e.ToArray();
            }
        }

        public virtual Pondus[] CurrentOtherIncome => GetCurrent(OtherIncome);
        public virtual Pondus[] CurrentExpenses => GetCurrent(Expenses);
        public virtual Pecuniam TotalAnnualExpenses => Pondus.GetAnnualSum(CurrentExpenses);
        public virtual Pecuniam TotalAnnualIncome => Pondus.GetAnnualSum(CurrentOtherIncome) + TotalAnnualNetEmploymentIncome;
        public virtual Pecuniam TotalAnnualNetEmploymentIncome => CurrentEmployment.Select(e => e.TotalAnnualNetPay).GetSum();
        public virtual Pecuniam TotalAnnualGrossEmploymentIncome => CurrentEmployment.Select(e => e.TotalAnnualPay).GetSum();

        protected internal virtual List<IEmployment> Employment
        {
            get
            {
                var e = _employment.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        protected internal virtual List<Pondus> OtherIncome
        {
            get
            {
                var o = _otherIncome.ToList();
                o.Sort(Comparer);
                return o.ToList();
            }
        }

        protected internal virtual List<Pondus> Expenses
        {
            get
            {
                var e = _expenses.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        #endregion

        #region methods
        public virtual IEmployment[] GetEmploymentAt(DateTime? dt)
        {
            return dt == null
                ? new[] {Employment.LastOrDefault()}
                : Employment.Where(x => x.IsInRange(dt.Value)).ToArray();
        }

        public virtual Pondus[] GetOtherIncomeAt(DateTime? dt)
        {
            return GetAt(dt, OtherIncome);
        }

        public virtual Pondus[] GetExpensesAt(DateTime? dt)
        {
            return GetAt(dt, Expenses);
        }

        protected internal virtual void AddEmployment(IEmployment employment)
        {
            if (employment != null)
                _employment.Add(employment);
        }

        protected internal virtual void AddOtherIncome(Pondus otherIncome)
        {
            if (otherIncome != null)
                _otherIncome.Add(otherIncome);
        }

        protected internal virtual void AddOtherIncome(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddOtherIncome(new Pondus(name)
            {
                Value = amt?.Neg,
                ToDate = endDate,
                FromDate = startDate
            });
        }

        protected internal virtual void AddExpense(Pondus expense)
        {
            if (expense == null)
                return;
            _expenses.Add(expense);
        }

        protected internal virtual void AddExpense(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddExpense(new Pondus(name)
            {
                Value = amt?.Neg,
                ToDate = endDate,
                FromDate = startDate
            });
        }

        /// <summary>
        /// Gets the minimum date amoung all income, employment and expense items
        /// </summary>
        /// <returns></returns>
        protected internal virtual DateTime GetMinDate()
        {
            var sdt = Etx.Date(-3, null, true, 60).Date;
            var minOtherIncome = OtherIncome.FirstOrDefault()?.FromDate;
            var minEmply = Employment.FirstOrDefault()?.FromDate;
            var minExpense = Expenses.FirstOrDefault()?.FromDate;

            
            if (new[] { minOtherIncome, minEmply, minExpense }.All(dt => dt == null))
                return sdt;

            var mins = new[]
            {
                minOtherIncome.GetValueOrDefault(DateTime.Today),
                minEmply.GetValueOrDefault(DateTime.Today),
                minExpense.GetValueOrDefault(DateTime.Today)
            };

            return mins.Min();
        }


        protected internal virtual Pondus[] GetOtherIncomeItemsForRange(Pecuniam amt, DateTime? startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            startDate = startDate ?? GetMinDate();

            throw new NotImplementedException();
        }

        protected internal virtual Pondus[] GetPublicBenefitIncomeItemsForRange(Pecuniam amt, DateTime? startDate,
            DateTime? endDate = null)
        {
            var itemsout = new List<Pondus>();
            startDate = startDate ?? GetMinDate();
            var isPoor = IsBelowFedPovertyAt(startDate);
            var hudAmt = isPoor ? GetHudMonthlyAmount(startDate) : Pecuniam.Zero;
            var snapAmt = isPoor ? GetFoodStampsMonthlyAmount(startDate) : Pecuniam.Zero;

            var incomeItems = GetIncomeItemNames();
            foreach (var incomeItem in incomeItems)
            {
                var p = new Pondus(incomeItem)
                {
                    FromDate = startDate,
                    ToDate = endDate,
                    Interval = Interval.Monthly
                };

                switch (incomeItem.Name)
                {
                    case "Supplemental Nutrition Assistance Program":
                        p.Value = snapAmt;
                        break;
                    case "Housing Choice Voucher Program Section 8":
                        p.Value = hudAmt;
                        break;
                    //TODO implement the other welfare programs
                    default:
                        p.Value = Pecuniam.Zero;
                        break;
                }

                itemsout.Add(p);
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Src https://www.hud.gov/sites/documents/DOC_11750.PDF
        /// </summary>
        /// <returns></returns>
        protected internal virtual Pecuniam GetHudMonthlyAmount(DateTime? atTime)
        {
            var thirtyPercentAdjIncome = GetAnnualNetEmploymentIncomeAt(atTime).ToDouble() / 12 * 0.3;
            var tenPercentGrossIncome =  GetAnnualGrossEmploymentIncomeAt(atTime).ToDouble() / 12 * 0.1;

            var sum = thirtyPercentAdjIncome + tenPercentGrossIncome + 25.0D;
            return sum.ToPecuniam();
        }

        /// <summary>
        /// Src https://www.fns.usda.gov/snap/how-much-could-i-receive
        /// </summary>
        /// <returns></returns>
        protected internal virtual Pecuniam GetFoodStampsMonthlyAmount(DateTime? atTime)
        {
            var thirtyPercentAdjIncome = GetAnnualNetEmploymentIncomeAt(atTime).ToDouble() / 12 * 0.3;
            return thirtyPercentAdjIncome.ToPecuniam();
        }

        /// <summary>
        /// Determines of the given annual gross income at time <see cref="dt"/> is below 
        /// the federal poverty level for that year
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="numHouseholdMembers"></param>
        /// <returns></returns>
        protected internal virtual bool IsBelowFedPovertyAt(DateTime? dt, int numHouseholdMembers = 1)
        {
            var povertyLevel = NAmerUtil.Equations.GetFederalPovertyLevel(dt);

            var payAtDt = GetEmploymentAt(dt);
            if(payAtDt == null || !payAtDt.Any())
                return false;

            numHouseholdMembers = numHouseholdMembers <= 0 ? 1 : numHouseholdMembers;

            return GetAnnualGrossEmploymentIncomeAt(dt).ToDouble() <= povertyLevel.SolveForY(numHouseholdMembers);
        }

        /// <summary>
        /// Gets a list of time ranges over the last three years where each block is assumed as a 
        /// span of employment
        /// </summary>
        /// <param name="personality"></param>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetEmploymentRanges(IPersonality personality)
        {
            var emply = new List<Tuple<DateTime, DateTime?>>();
            var sdt = Etx.Date(-3, null, true, 60).Date;
            if (personality == null)
            {
                emply.Add(new Tuple<DateTime, DateTime?>(sdt, null));
                return emply;
            }

            var lpDt = sdt;
            while (lpDt < DateTime.Today)
            {

                var randDays = Etx.IntNumber(0, 21);
                lpDt = lpDt.AddMonths(3).AddDays(randDays);
                if (personality.GetRandomActsSpontaneous())
                {
                    emply.Add(new Tuple<DateTime, DateTime?>(sdt, lpDt < DateTime.Today ? new DateTime?(lpDt) : null));
                    sdt = lpDt;
                }
            }
            if(!emply.Any())
                emply.Add(new Tuple<DateTime, DateTime?>(Etx.Date(-3, null, true, 60).Date, null));
            return emply;
        }

        /// <summary>
        /// Get an ordered list of employment for the last three years at random
        /// </summary>
        /// <param name="personality"></param>
        /// <param name="eduLevel"></param>
        /// <returns></returns>
        protected internal virtual List<IEmployment> ResolveEmployment(IPersonality personality,
            OccidentalEdu eduLevel = OccidentalEdu.None)
        {
            var empls = new HashSet<IEmployment>();
            var emplyRanges = GetEmploymentRanges(personality);
            var occ = StandardOccupationalClassification.RandomOccupation();

            //limit result to those which match the edu level
            Predicate<SocDetailedOccupation> filter = s => true;
            if (eduLevel < (OccidentalEdu.Bachelor | OccidentalEdu.Grad))
                filter = s => !StandardOccupationalClassification.IsDegreeRequired(s);

            foreach (var range in emplyRanges)
            {
                var emply = new NorthAmericanEmployment(range.Item1, range.Item2, Person) {Occupation = occ};
                if (personality?.GetRandomActsSpontaneous() ?? false)
                {
                    occ = StandardOccupationalClassification.RandomOccupation(filter);
                }
                empls.Add(emply);
            }

            var e = empls.ToList();
            e.Sort(Comparer);
            return e;
        }

        private Pecuniam GetAnnualGrossEmploymentIncomeAt(DateTime? dt)
        {
            var payAtDt = GetEmploymentAt(dt);
            if (payAtDt == null || !payAtDt.Any())
                return Pecuniam.Zero;

            return payAtDt.Select(e => e.TotalAnnualPay).GetSum();
        }

        private Pecuniam GetAnnualNetEmploymentIncomeAt(DateTime? dt)
        {
            var payAtDt = GetEmploymentAt(dt);
            if (payAtDt == null || !payAtDt.Any())
                return Pecuniam.Zero;

            return payAtDt.Select(e => e.TotalAnnualNetPay).GetSum();
        }

        #endregion
    }

}
