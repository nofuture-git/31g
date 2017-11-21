﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Util.Core;

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
        private Tuple<DateTime?, DateTime?> _dateRange;
        private readonly HashSet<Pondus> _pay = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _deductions = new HashSet<Pondus>();
        private SocDetailedOccupation _occupation;
        private bool _isWages;
        private bool _isTips;
        private bool _isCommission;
        #endregion

        #region ctors

        /// <summary>
        /// Creates a new instance of <see cref="NorthAmericanEmployment"/> at random.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="american"></param>
        public NorthAmericanEmployment(DateTime? startDate, DateTime? endDate, NorthAmerican american) : base(american)
        {
            _dateRange = new Tuple<DateTime?, DateTime?>(startDate, endDate);
            Occupation = StandardOccupationalClassification.RandomOccupation();
            ResolveIncomeAndDeductions();
        }

        internal NorthAmericanEmployment(DateTime? startDate, DateTime? endDate) : base(null)
        {
            _dateRange = new Tuple<DateTime?, DateTime?>(startDate, endDate);
            Occupation = StandardOccupationalClassification.RandomOccupation();
        }

        #endregion

        #region properties
        public virtual string Src { get; set; }
        public virtual string Abbrev => "Employer";
        public virtual IFirm Value { get; set; }
        public virtual bool IsOwner { get; set; }

        public virtual SocDetailedOccupation Occupation
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

        public virtual Pondus[] CurrentDeductions => GetDeductionsAt(null);
        public virtual Pondus CurrentPay => GetPayAt(null);

        public virtual DateTime? FromDate
        {
            get => _dateRange.Item1;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(value, _dateRange.Item2);
        }

        public virtual DateTime? ToDate
        {
            get => _dateRange.Item2;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(_dateRange.Item1, value);
        }

        public Pecuniam CurrentNetPay => (CurrentPay?.Value ?? Pecuniam.Zero) - Pondus.GetSum(CurrentDeductions).Abs;

        #endregion

        #region methods

        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = FromDate == null || FromDate <= dt;
            var beforeOrOnToDt = ToDate == null || ToDate.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public virtual Pondus GetPayAt(DateTime? dt)
        {
            var pay = GetAt(dt, Income);
            return pay.LastOrDefault();
        }

        public virtual Pondus[] GetDeductionsAt(DateTime? dt)
        {
            return GetAt(dt, Deductions);
        }

        /// <summary>
        /// Resolves all income and deduction items for this Employment
        /// </summary>
        protected internal virtual void ResolveIncomeAndDeductions()
        {
            if (_dateRange?.Item1 == null)
            {
                AddPondusForGivenRange(null, null);
                return;
            }
            var isCurrentEmployee = _dateRange.Item2 == null;
            var yearsOfService = GetYearsOfServiceInDates();
            for (var i = 0; i < yearsOfService.Count; i++)
            {
                var range = yearsOfService[i];
                if(i == yearsOfService.Count-1 && isCurrentEmployee)
                    range = new Tuple<DateTime, DateTime?>(range.Item1, null);
                AddPondusForGivenRange(range.Item1, range.Item2);
                    
            }
        }
        /// <summary>
        /// Adds both income and deduction items for the given date range at random
        /// </summary>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        protected internal void AddPondusForGivenRange(DateTime? startDt, DateTime? endDt)
        {
            var annualIncome = GetYearlyIncome(startDt.GetValueOrDefault(DateTime.Today));
            var incomeItems = GetPayItemsForRange(annualIncome, startDt, endDt);
            foreach (var income in incomeItems)
                AddIncome(income);
            var deductions = GetDeductionItemsForRange(annualIncome, startDt, endDt);
            foreach (var deduction in deductions)
                AddDeduction(deduction);
        }
        
        /// <summary>
        /// Breaks the employment date range into annual blocks of time.
        /// </summary>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetYearsOfServiceInDates()
        {
            var ranges = new List<Tuple<DateTime, DateTime?>>();

            var yearsOfService = GetYearsOfService();
            if (yearsOfService <= 0 || _dateRange?.Item1 == null)
                return ranges;

            var employerFiscalYearEnd = Value?.FiscalYearEndDay ?? 1;

            //assume merit increases 90 days after fiscal year end
            var annualReviewDays = employerFiscalYearEnd + 90;
            var maxEndDt = _dateRange.Item2.GetValueOrDefault(DateTime.Today);
            var prevDt = _dateRange.Item1.Value.Date;
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
        protected internal virtual int GetYearsOfService()
        {
            var hasStartDate = _dateRange?.Item1 != null;
            if (!hasStartDate)
                return 0;

            var stDt = _dateRange.Item1.Value;
            var endDt = _dateRange.Item2.GetValueOrDefault(DateTime.Today);

            var numYears = endDt.Year - stDt.Year;
            return numYears <= 0 ? 0 : numYears;
        }

        protected internal virtual List<Pondus> AllItems
        {
            get
            {
                var i = _pay.ToList();
                i.AddRange(_deductions);
                i.Sort(Comparer);
                return i;
            }
        }

        protected internal virtual List<Pondus> Deductions
        {
            get
            {
                var d = _deductions.ToList();
                d.Sort(Comparer);
                return d;
            }
        }

        protected internal virtual List<Pondus> Income
        {
            get
            {
                var p = _pay.ToList();
                p.Sort(Comparer);
                return p;
            }
        }

        /// <summary>
        /// Gets a manifold of <see cref="Pondus"/> items based on the 
        /// names from GetIncomeItemNames assigning random values as 
        /// a portion of <see cref="amt"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetPayItemsForRange(Pecuniam amt, DateTime? startDate, 
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            var itemsout = new List<Pondus>();
            amt = amt ?? Pecuniam.Zero;
            
            var salaryScaler = _isWages ? 0 : 1;
            var commissionRate = _isCommission 
                ? Etx.RandomValueInNormalDist(0.667, 0.081) 
                : 0;
            var tipsRate = _isTips 
                ? Etx.RandomValueInNormalDist(0.7889D, 0.025) 
                : 0;
            var bonusRate = Etx.TryBelowOrAt(1, Etx.Dice.Ten) 
                ? Etx.RandomValueInNormalDist(0.02, 0.001) 
                : 0D;
            var tipsAndCommission = commissionRate + tipsRate > 1D 
                ? 1D 
                : commissionRate + tipsRate;
            var wageRate = _isWages 
                ? (1D - tipsAndCommission) 
                : 0;
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
                && _dateRange?.Item2.GetValueOrDefault(DateTime.Today) < DateTime.Today 
                && Etx.TryBelowOrAt(7, Etx.Dice.OneHundred)
                    ? Etx.RandomValueInNormalDist(0.072, 0.0025)
                    : 0D;
            var incomeName2Scaler = new Dictionary<string, double>
            {

                {"Salary", salaryScaler },
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

            var incomeItems = GetIncomeItemNames();
            foreach (var incomeItem in incomeItems)
            {
                if (!incomeName2Scaler.ContainsKey(incomeItem.Name))
                    continue;
                var incomeScaler = incomeName2Scaler[incomeItem.Name];
                var p = new Pondus(incomeItem)
                {
                    FromDate = startDate,
                    ToDate = endDate,
                    Value = CalcValue(amt, incomeScaler),
                    Interval = interval
                };

                itemsout.Add(p);
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Gets a manifold of <see cref="Pondus"/> items based on the 
        /// names from GetDeductionItemNames assigning random values as 
        /// a portion of <see cref="amt"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetDeductionItemsForRange(Pecuniam amt, DateTime? startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            var itemsout = new List<Pondus>();
            amt = amt ?? Pecuniam.Zero;
            
            //https://www.cdc.gov/nchs/fastats/health-insurance.htm
            var isInsCovered = Etx.TryAboveOrAt(124, Etx.Dice.OneThousand);
            var healthCareCost = isInsCovered
                ? GetHealthInsCost(startDate)
                : 0D;

            var healthInsRate = Math.Round(healthCareCost / Convert.ToDouble(amt.Amount), 5);
            var lifeInsRate = Etx.CoinToss ? GetRandomizeRateOf(healthInsRate, 13) : 0D;
            var supplementalLifeInsRate = Etx.TryBelowOrAt(3, Etx.Dice.Ten) ? GetRandomizeRateOf(healthInsRate, 21) : 0D;
            var dependentInsRate = Etx.TryBelowOrAt(1, Etx.Dice.Four) ? GetRandomizeRateOf(healthInsRate, 21) : 0D;
            var adAndDInsRate = Etx.TryBelowOrAt(1, Etx.Dice.Four) ? GetRandomizeRateOf(healthInsRate, 34) : 0D;
            var dentalInsRate = GetRandomizeRateOf(healthInsRate, 5);
            var visionInsRate = GetRandomizeRateOf(healthInsRate, 8);
            var stDisabilityRate = Etx.TryBelowOrAt(1, Etx.Dice.Four) ? GetRandomizeRateOf(healthInsRate, 13) : 0D;
            var ltDisabilityRate = Etx.TryBelowOrAt(1, Etx.Dice.Four) ? GetRandomizeRateOf(healthInsRate, 13) : 0D;

            var fedTaxRate = NAmerUtil.Equations.FederalIncomeTaxRate.SolveForY(Convert.ToDouble(amt.Amount));
            var stateTaxRate = GetRandomizeRateOf(fedTaxRate, 5);

            //https://www.irs.gov/taxtopics/tc751
            var ficaRate = 0.062D;
            var medicareRate = 0.0145D;

            var retirementRate = Etx.CoinToss ? Etx.DiscreteRange(new[] {0D, 0.01D, 0.02D, 0.03D, 0.04D, 0.05D}) : 0D;
            var profitShareRate = Etx.TryBelowOrAt(13, Etx.Dice.OneHundred) ? GetRandomizeRateOf(0.06) : 0D;
            var pensionRate = Etx.TryBelowOrAt(9, Etx.Dice.OneHundred) ? GetRandomizeRateOf(0.03) : 0D;

            var unionDueRate = StandardOccupationalClassification.IsLaborUnion(_occupation) ? GetRandomizeRateOf(0.04) : 0D;
            var hraRate = Etx.CoinToss ? GetRandomizeRateOf(healthInsRate, 13) : 0D;
            var fsaRate = Etx.CoinToss ? GetRandomizeRateOf(healthInsRate, 13) : 0D;
            var creditUnionRate = Etx.TryBelowOrAt(3, Etx.Dice.OneHundred) ? GetRandomizeRateOf(0.01) : 0D;

            var deductionName2Scaler = new Dictionary<string, double>
            {
                {"Health", healthInsRate },
                {"Life", lifeInsRate},
                {"Supplemental Life", supplementalLifeInsRate},
                {"Dependent Life", dependentInsRate},
                {"Accidental Death & Dismemberment", adAndDInsRate },
                {"Dental", dentalInsRate },
                {"Vision", visionInsRate},
                {"Short-term Disability", stDisabilityRate},
                {"Long-term Disability", ltDisabilityRate},
                {"Federal tax", fedTaxRate},
                {"State tax", stateTaxRate},
                {"FICA", ficaRate},
                {"Medicare", medicareRate},
                {"Registered Retirement Savings Plan", retirementRate},
                {"Profit Sharing", profitShareRate},
                {"Pension", pensionRate},
                {"Union Dues", unionDueRate},
                {"Health Savings Account", hraRate},
                {"Flexible Spending Account", fsaRate},
                {"Credit Union Loan", creditUnionRate},
            };
            var deductionItems = GetDeductionItemNames();
            foreach (var deduction in deductionItems)
            {
                if (!deductionName2Scaler.ContainsKey(deduction.Name))
                    continue;
                var incomeScaler = deductionName2Scaler[deduction.Name];
                var p = new Pondus(deduction)
                {
                    FromDate = startDate,
                    ToDate = endDate,
                    Value = CalcValue(amt, incomeScaler),
                    Interval = interval
                };

                itemsout.Add(p);
            }
            return itemsout.ToArray();
         }

        protected internal virtual void AddIncome(Pondus p)
        {
            if (IsInRange(p))
            {
                p.Value = p.Value.Abs;
                _pay.Add(p);
            }
        }

        protected internal virtual void AddDeduction(Pondus d)
        {
            if (IsInRange(d))
            {
                d.Value = d.Value.Neg;
                _deductions.Add(d);
            }
        }

        protected internal bool IsInRange(Pondus item)
        {
            if (item == null)
                return false;
            var itemEndDt = item.ToDate;
            var rangeStartDt = _dateRange?.Item1;
            
            //item ended before this instance even started
            if (itemEndDt != null && rangeStartDt != null && itemEndDt.Value < rangeStartDt.Value)
                return false;

            var itemStartDt = item.FromDate;
            var rangeEndDt = _dateRange?.Item2;

            //instance ended before this item even started
            if (itemStartDt != null && rangeEndDt != null && itemStartDt.Value > rangeEndDt.Value)
                return false;
            return true;
        }

        private double GetHealthInsCost(DateTime? atDate)
        {
            var dt = atDate.GetValueOrDefault(DateTime.Now);
            var mean = NAmerUtil.Equations.HealthInsuranceCostPerPerson.SolveForY(dt.ToDouble());
            var stdDev = Math.Round(mean * 0.155, 2);

            return Etx.RandomValueInNormalDist(mean, stdDev);
        }

        private double GetRandomizeRateOf(double baseRate, double dividedby = 1)
        {
            if (baseRate == 0)
                return 0;
            var mean = baseRate / (dividedby == 0 ? 1 : dividedby);
            var stdDev = Math.Round(mean * 0.155, 5);
            return Etx.RandomValueInNormalDist(mean, stdDev);
        }

        private Pecuniam CalcValue(Pecuniam pecuniam, double d)
        {
            return Math.Round(Convert.ToDouble(pecuniam.Amount) * d, 2).ToPecuniam();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IEmployment e))
                return base.Equals(obj);

            return e.Value != null 
                   && Value != null
                   && e.Value.Equals(Value)
                   && e.FromDate == FromDate
                   && e.ToDate == ToDate;
        }

        public override int GetHashCode()
        {
            return (Value?.GetHashCode() ?? 1) +
                   _dateRange?.GetHashCode() ?? 1;
        }

        #endregion
    }
}