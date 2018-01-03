using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Gov;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IEmployment" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class NorthAmericanEmployment : WealthBase, IEmployment
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
        /// Creates a new instance of <see cref="NorthAmericanEmployment"/> at random.
        /// </summary>
        /// <param name="american"></param>
        /// <param name="options"></param>
        public NorthAmericanEmployment(NorthAmerican american, OpesOptions options) : base(american, options)
        {
            if(MyOptions.Inception == DateTime.MinValue)
                MyOptions.Inception = GetYearNeg(-1);
            MyOptions.Interval = Interval.Annually;
            Occupation = StandardOccupationalClassification.RandomOccupation();
        }

        public NorthAmericanEmployment(NorthAmerican american, DateTime inception, DateTime? terminus) : base(american)
        {
            MyOptions.Inception = inception;
            MyOptions.Terminus = terminus;
            MyOptions.Interval = Interval.Annually;
            Occupation = StandardOccupationalClassification.RandomOccupation();
        }

        internal NorthAmericanEmployment(DateTime inception, DateTime? terminus) : base(null)
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
        public virtual IFirm Value { get; set; }
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

        public IDeductions Deductions { get; set; }

        public virtual Pondus[] CurrentPay => GetPayAt(null);

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

        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception == null || Inception <= dt;
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

        protected internal override void ResolveItems(OpesOptions options = null)
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
            var deductions = new NorthAmericanDeductions(this);
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

            var yearsOfService = GetYearsOfService(options);
            if (yearsOfService <= 0 || MyOptions.Inception == DateTime.MinValue)
                return ranges;

            var employerFiscalYearEnd = Value?.FiscalYearEndDay ?? 1;

            //assume merit increases 90 days after fiscal year end
            var annualReviewDays = employerFiscalYearEnd + 90;
            var maxEndDt = MyOptions.Terminus.GetValueOrDefault(DateTime.Today);
            var prevDt = MyOptions.Inception.Date;
            for (var i = 0; i <= yearsOfService; i++)
            {
                var stDt = i == 0 ? prevDt : prevDt.AddDays(1);
                var endDt = new DateTime(stDt.AddYears(1).Year, 1, 1).AddDays(annualReviewDays);
                while((endDt - stDt).Days < 360)
                    endDt = new DateTime(endDt.AddYears(1).Year, 1, 1).AddDays(annualReviewDays);

                //determine if we have past the end date
                var isPastMax = endDt > maxEndDt;

                var endDt2 = isPastMax ? maxEndDt : endDt;

                var dtRng = new Tuple<DateTime, DateTime?>(stDt, endDt2);
                
                ranges.Add(dtRng);

                if (isPastMax)
                    return ranges;

                prevDt = endDt;
            }
            return ranges;
        }

        /// <summary>
        /// Determines the number of years of employment
        /// </summary>
        /// <returns></returns>
        protected internal virtual int GetYearsOfService(OpesOptions options)
        {
            options = options ?? MyOptions;
            var hasStartDate = MyOptions.Inception != DateTime.MinValue;
            if (!hasStartDate)
                return 0;

            var stDt = MyOptions.Inception;
            var endDt = MyOptions.Terminus.GetValueOrDefault(DateTime.Today);

            var numYears = endDt.Year - stDt.Year;
            numYears = numYears <= 0 ? 0 : numYears;
            var maxAge = new[] {AmericanData.MAX_AGE_FEMALE, AmericanData.MAX_AGE_MALE}.Max();
            maxAge -= UsState.AGE_OF_ADULT;
            if(numYears > maxAge)
                throw new RahRowRagee($"the years of employment was calculated at {numYears} " +
                                      $"but the absolute max number of working years for an american is {maxAge} - " +
                                      "check your date ranges and try again");
            return numYears;
        }

        /// <summary>
        /// Produces the item names to rates for the Employment Pay (e.g. salary, tips, etc.)
        /// </summary>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetPayName2RandRates(OpesOptions options)
        {
            options = options ?? MyOptions;

            var bonusRate = Etx.TryBelowOrAt(1, Etx.Dice.Ten)
                ? Etx.RandomValueInNormalDist(0.02, 0.001)
                : 0D;
            var overtimeRate = _isWages && Etx.TryBelowOrAt(3, Etx.Dice.Ten)
                ? Etx.RandomValueInNormalDist(0.05, 0.009)
                : 0D;
            var shiftDiffRate = _isWages && Etx.TryBelowOrAt(3, Etx.Dice.Ten)
                ? Etx.RandomValueInNormalDist(0.03, 0.0076)
                : 0D;
            var selfEmplyRate = Etx.TryBelowOrAt(7, Etx.Dice.OneHundred)
                ? Etx.RandomValueInNormalDist(0.072, 0.0088)
                : 0D;
            var emplrPaidRate = Etx.TryBelowOrAt(17, Etx.Dice.OneHundred)
                ? Etx.RandomValueInNormalDist(0.012, 0.008)
                : 0D;
            var inKindRate = Etx.TryBelowOrAt(9, Etx.Dice.OneThousand)
                ? Etx.RandomValueInNormalDist(0.022, 0.0077)
                : 0D;
            var sevrcRate =
                !_isWages
                && !_isTips
                && MyOptions.Terminus.GetValueOrDefault(DateTime.Today) < DateTime.Today
                && Etx.TryBelowOrAt(7, Etx.Dice.OneHundred)
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
            if (!(obj is IEmployment e))
                return base.Equals(obj);

            var termEquals = e.Inception == Inception
                             && e.Terminus == Terminus;

            //neither have a company assigned
            if (Value == null && e.Value == null)
                return termEquals;

            //one-and-only-one has company assigned
            if (Value == null ^ e.Value == null)
                return false;

            return Value.Equals(e.Value) && termEquals;
        }

        public override int GetHashCode()
        {
            return (Value?.GetHashCode() ?? 1) +
                   MyOptions?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var t = new Tuple<string, string, DateTime?, DateTime?, Pecuniam>(Value?.ToString(), Occupation?.ToString(),
                Inception, Terminus, Pondus.GetExpectedSum(MyItems));
            return t.ToString();
        }

        #endregion
    }
}