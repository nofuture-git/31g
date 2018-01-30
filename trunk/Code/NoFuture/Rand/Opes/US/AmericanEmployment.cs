using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Org;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Opes.US
{
    /// <inheritdoc cref="ILaboris" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class AmericanEmployment : WealthBase, ILaboris
    {
        #region fields
        private readonly HashSet<Pondus> _pay = new HashSet<Pondus>();
        private SocDetailedOccupation _occupation;
        private bool _isWages;
        private bool _isTips;
        private bool _isCommission;
        private DateTime _startDate;
        private DateTime? _endDate;
        #endregion

        #region properties
        public virtual string EmployingCompanyName { get; set; }
        public virtual int FiscalYearEndDay { get; set; } = 1;
        public virtual bool IsOwner { get; set; }
        protected override DomusOpesDivisions Division => DomusOpesDivisions.Employment;
        public SocDetailedOccupation Occupation
        {
            get => _occupation;
            set
            {
                _occupation = value;
                _isWages = StandardOccupationalClassification.IsWages(_occupation);
                _isTips = StandardOccupationalClassification.IsTips(_occupation);
                _isCommission = StandardOccupationalClassification.IsCommissions(_occupation);
            }
        }

        public ITributum Deductions { get; set; }

        public virtual Pondus[] CurrentPay => GetCurrent(MyItems);

        /// <summary>
        /// Convenience method which is more straight-foward than <see cref="Inception"/>
        /// </summary>
        public virtual DateTime StartDate
        {
            get => Inception;
            set => Inception = value;
        }

        public virtual DateTime Inception
        {
            get
            {
                //don't let this out as year 1
                if (_startDate == DateTime.MinValue)
                    _startDate = GetYearNeg(-1);
                return _startDate;
            }
            set => _startDate = value;
        }

        public virtual DateTime? Terminus
        {
            get => _endDate;
            set => _endDate = value;
        }

        public Pecuniam TotalAnnualPay => Pondus.GetExpectedAnnualSum(CurrentPay).Abs;
        public Pecuniam TotalAnnualNetPay => TotalAnnualPay - Deductions?.TotalAnnualDeductions.Abs ?? Pecuniam.Zero;

        protected internal override List<Pondus> MyItems
        {
            get
            {
                var p = _pay.ToList();
                p.Sort(Comparer);
                return p;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Gets employment (with deductions) at random
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanEmployment RandomEmployment(OpesOptions options = null)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var emply = new AmericanEmployment
            {
                Inception = options.Inception,
                Terminus = options.Terminus
            };
            emply.RandomizeAllItems(options);
            return emply;
        }

        /// <summary>
        /// Gets a random number of years of tenure
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [RandomFactory]
        public static TimeSpan RandomEmploymentTenure(OpesOptions options = null)
        {
            //TODO - use[https://www.bls.gov/news.release/tenure.nr0.htm]
            return new TimeSpan(Etx.RandomInteger(745, 1855), 0, 0, 0);
        }

        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public virtual Pondus[] GetPayAt(DateTime? dt)
        {
            return GetAt(dt, MyItems);
        }

        protected override Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<OpesOptions, Dictionary<string, double>>>
            {
                {EmploymentGroupNames.PAY, GetPayName2RandRates},
            };
        }

        protected internal override void RandomizeAllItems(OpesOptions options)
        {
            //expecting the caller to have passed in a hire-date to termination-date
            options = options ?? OpesOptions.RandomOpesOptions();
            if (options.Inception == DateTime.MinValue)
                options.Inception = GetYearNeg(-1);
            if (Inception == DateTime.MinValue)
                Inception = options.Inception;

            //this is a single company employment; 
            //however, its expected one receives different pay as the years progress
            var yearsOfService = GetYearsOfServiceInDates(options);
            var isCurrentEmployee = options.Terminus == null;

            for (var i = 0; i < yearsOfService.Count; i++)
            {
                var range = yearsOfService[i];
                if (i == yearsOfService.Count - 1 && isCurrentEmployee)
                    range = new Tuple<DateTime, DateTime?>(range.Item1, null);
                var argOptions = options.GetClone();
                argOptions.Inception = range.Item1;
                argOptions.Terminus = range.Item2;

                var items = GetItemsForRange(argOptions);
                foreach(var item in items)
                    AddItem(item);

            }

            if (options.Terminus != null)
                Terminus = options.Terminus;

            var deductions = new AmericanDeductions(this);
            deductions.RandomizeAllItems(options);
            Deductions = deductions;
        }
        
        /// <summary>
        /// Breaks the employment date range into annual blocks of time attempting 
        /// to capture the discrete time-ranges where wage\salary do not change.
        /// </summary>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetYearsOfServiceInDates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var ranges = new List<Tuple<DateTime, DateTime?>>();

            var stDt = options.Inception.Date;

            stDt = stDt == DateTime.MinValue 
                ? DateTime.Today.AddDays(-1 * RandomEmploymentTenure().TotalDays) 
                : stDt;

            var tenure = options.Terminus == null
                ? DateTime.Today - options.Inception
                : options.Terminus.Value - options.Inception;

            if (tenure == TimeSpan.Zero || options.Inception == DateTime.MinValue)
                return ranges;

            //assume merit increases 90 days after fiscal year end
            var annualReviewDays = FiscalYearEndDay + 90;
            var mark = stDt;
            for (var i = 0; i < tenure.TotalDays; i++)
            {
                if (i + 1 >= tenure.TotalDays)
                {
                    ranges.Add(new Tuple<DateTime, DateTime?>(mark, options.Terminus));
                    break;
                }

                if (stDt.DayOfYear != annualReviewDays)
                {
                    stDt = stDt.AddDays(1);
                    continue;
                }
                
                ranges.Add(new Tuple<DateTime, DateTime?>(mark, stDt));
                stDt = stDt.AddDays(1);
                mark = stDt;
            }
            return ranges;
        }

        /// <summary>
        /// Produces the item names to rates for the Employment Pay (e.g. salary, tips, etc.)
        /// </summary>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetPayName2RandRates(OpesOptions options)
        {
            var bonusRate = Etx.RandomRollBelowOrAt(1, Etx.Dice.Ten)
                ? Etx.RandomValueInNormalDist(0.02, 0.001)
                : 0D;
            var overtimeRate = _isWages && Etx.RandomRollBelowOrAt(3, Etx.Dice.Ten)
                ? Etx.RandomValueInNormalDist(0.05, 0.009)
                : 0D;
            var shiftDiffRate = _isWages && Etx.RandomRollBelowOrAt(3, Etx.Dice.Ten)
                ? Etx.RandomValueInNormalDist(0.03, 0.0076)
                : 0D;
            var selfEmplyRate = Etx.RandomRollBelowOrAt(7, Etx.Dice.OneHundred)
                ? Etx.RandomValueInNormalDist(0.072, 0.0088)
                : 0D;
            var emplrPaidRate = Etx.RandomRollBelowOrAt(17, Etx.Dice.OneHundred)
                ? Etx.RandomValueInNormalDist(0.012, 0.008)
                : 0D;
            var inKindRate = Etx.RandomRollBelowOrAt(9, Etx.Dice.OneThousand)
                ? Etx.RandomValueInNormalDist(0.022, 0.0077)
                : 0D;
            var sevrcRate =
                !_isWages
                && !_isTips
                && options.Terminus.GetValueOrDefault(DateTime.Today) < DateTime.Today
                && Etx.RandomRollBelowOrAt(7, Etx.Dice.OneHundred)
                    ? Etx.RandomValueInNormalDist(0.072, 0.0025)
                    : 0D;

            var commissionRate = _isCommission && !_isTips
                ? Etx.RandomValueInNormalDist(0.667, 0.081)
                : 0;
            var tipsRate = _isTips && !_isCommission
                ? Etx.RandomValueInNormalDist(0.7889, 0.025)
                : 0;

            var sumOfRate = bonusRate + overtimeRate + shiftDiffRate + selfEmplyRate +
                            emplrPaidRate + inKindRate + sevrcRate + commissionRate +
                            tipsRate;

            var wageRate = _isWages
                ? 1D - sumOfRate
                : 0D;

            var salaryRate = !_isWages
                ? 1D - sumOfRate
                : 0D;

            //TODO - why is there here in the middle of something else?
            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
            {
                options.SumTotal = GetRandomYearlyIncome(options.Inception, options);
                options.Interval = Interval.Annually;
            }

            var incomeName2Rate = new Dictionary<string, double>
            {

                {"Salary", salaryRate },
                {"Wages", wageRate},
                {"Employer Paid Expenses", emplrPaidRate},
                {"Shift Differential", shiftDiffRate},
                {"Severance Pay", sevrcRate },
                {"Overtime", overtimeRate },
                {"Self-employment", selfEmplyRate},
                {"Tips", tipsRate},
                {"Commissions", commissionRate},
                {"Bonuses", bonusRate},
                {"In-Kind",inKindRate}
            };
            return incomeName2Rate;
        }

        public override void AddItem(Pondus p)
        {
            if (IsInRange(p))
            {
                p.Expectation.Value = p.Expectation.Value.Abs;
                _pay.Add(p);
            }
        }

        protected internal bool IsInRange(Pondus item)
        {
            if (item == null)
                return false;
            var itemEndDt = item.Terminus;
            var rangeStartDt = Inception;
            
            //item ended before this instance even started
            if (itemEndDt != null && itemEndDt.Value < rangeStartDt)
                return false;

            var itemStartDt = item.Inception;
            var rangeEndDt = Terminus;

            //instance ended before this item even started
            if (rangeEndDt != null && itemStartDt > rangeEndDt.Value)
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ILaboris e))
                return base.Equals(obj);

            var termEquals = e.Inception == Inception
                             && e.Terminus == Terminus;

            //neither have a company assigned
            if (EmployingCompanyName == null && e.EmployingCompanyName == null)
                return termEquals;

            //one-and-only-one has company assigned
            if (EmployingCompanyName == null ^ e.EmployingCompanyName == null)
                return false;


            return (EmployingCompanyName ?? "").Equals(e.EmployingCompanyName) && termEquals;
        }

        public override int GetHashCode()
        {
            return (EmployingCompanyName?.GetHashCode() ?? 1) +
                   Inception.GetHashCode() +
                   Terminus?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var t = new Tuple<string, string, DateTime?, DateTime?, Pecuniam>(EmployingCompanyName, Occupation?.ToString(),
                Inception, Terminus, Pondus.GetExpectedSum(GetCurrent(MyItems)));
            return t.ToString();
        }

        #endregion
    }
}