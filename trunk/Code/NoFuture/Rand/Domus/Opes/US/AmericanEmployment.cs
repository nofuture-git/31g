﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Domus.Opes.US
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
        #endregion

        #region ctors

        /// <summary>
        /// Creates a new instance of <see cref="AmericanEmployment"/> at random.
        /// </summary>
        /// <param name="options"></param>
        public AmericanEmployment(OpesOptions options) : base(options)
        {
            if(MyOptions.Inception == DateTime.MinValue)
                MyOptions.Inception = GetYearNeg(-1);
            MyOptions.Interval = Interval.Annually;
            Occupation = StandardOccupationalClassification.RandomOccupation();
        }

        internal AmericanEmployment(DateTime inception, DateTime? terminus) : base(null)
        {
            MyOptions.Inception = inception;
            MyOptions.Terminus = terminus;
            MyOptions.Interval = Interval.Annually;
            Occupation = StandardOccupationalClassification.RandomOccupation();
        }

        #endregion

        #region properties
        public virtual string Src { get; set; }
        public virtual string Abbrev => IncomeGroupNames.EMPLOYMENT;
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

        public virtual DateTime Inception
        {
            get => MyOptions.Inception;
            set => MyOptions.Inception = value;
        }

        public virtual DateTime? Terminus
        {
            get => MyOptions.Terminus;
            set => MyOptions.Terminus = value;
        }

        public Pecuniam TotalAnnualPay => Pondus.GetExpectedAnnualSum(CurrentPay).Abs;
        public Pecuniam TotalAnnualNetPay => TotalAnnualPay - Deductions.TotalAnnualDeductions.Abs;

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
            var emply = new AmericanEmployment(options);
            emply.ResolveItems(options);
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

        protected internal override void ResolveItems(OpesOptions options)
        {
            options = options ?? MyOptions;
            if (options.Inception == DateTime.MinValue)
                options.Inception = GetYearNeg(-1);
            var yearsOfService = GetYearsOfServiceInDates(options);
            var isCurrentEmployee = MyOptions.Terminus == null;

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
            var deductions = new AmericanDeductions(this);
            deductions.ResolveItems(options);
            Deductions = deductions;
        }
        
        /// <summary>
        /// Breaks the employment date range into annual blocks of time attempting 
        /// to capture the discrete time-ranges where wage\salary do not change.
        /// </summary>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetYearsOfServiceInDates(OpesOptions options)
        {
            options = options ?? MyOptions;
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
                && MyOptions.Terminus.GetValueOrDefault(DateTime.Today) < DateTime.Today
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
                options.SumTotal = GetRandomYearlyIncome(options.Inception);
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

        protected internal override void AddItem(Pondus p)
        {
            if (IsInRange(p))
            {
                p.My.ExpectedValue = p.My.ExpectedValue.Abs;
                _pay.Add(p);
            }
        }

        protected internal bool IsInRange(Pondus item)
        {
            if (item == null)
                return false;
            var itemEndDt = item.Terminus;
            var rangeStartDt = MyOptions.Inception;
            
            //item ended before this instance even started
            if (itemEndDt != null && itemEndDt.Value < rangeStartDt)
                return false;

            var itemStartDt = item.Inception;
            var rangeEndDt = MyOptions.Terminus;

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
                   MyOptions?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var t = new Tuple<string, string, DateTime?, DateTime?, Pecuniam>(EmployingCompanyName, Occupation?.ToString(),
                Inception, Terminus, Pondus.GetExpectedSum(MyItems));
            return t.ToString();
        }

        #endregion
    }
}