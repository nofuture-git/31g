using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Opes.Options;
using NoFuture.Rand.Gov;
using NoFuture.Shared.Core;
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
        /// <param name="american"></param>
        /// <param name="options"></param>
        public NorthAmericanEmployment(NorthAmerican american, OpesOptions options) : base(american, options)
        {
            Occupation = StandardOccupationalClassification.RandomOccupation();
            ResolvePayAndDeductions();
        }

        public NorthAmericanEmployment(NorthAmerican american, DateTime startDate, DateTime? endDate) : base(american)
        {
            MyOptions.StartDate = startDate;
            MyOptions.EndDate = endDate;
            Occupation = StandardOccupationalClassification.RandomOccupation();
            ResolvePayAndDeductions();
        }


        internal NorthAmericanEmployment(DateTime startDate, DateTime? endDate) : base(null)
        {
            MyOptions.StartDate = startDate;
            MyOptions.EndDate = endDate;
            Occupation = StandardOccupationalClassification.RandomOccupation();
        }

        #endregion

        #region properties
        public virtual string Src { get; set; }
        public virtual string Abbrev => IncomeGroupNames.EMPLOYMENT;
        public virtual IFirm Value { get; set; }
        public virtual bool IsOwner { get; set; }

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

        public virtual Pondus[] CurrentDeductions => GetDeductionsAt(null);
        public virtual Pondus[] CurrentPay => GetPayAt(null);

        public virtual DateTime Inception
        {
            get => MyOptions.StartDate;
            set => MyOptions.StartDate = value;
        }

        public virtual DateTime? Terminus
        {
            get => MyOptions.EndDate;
            set => MyOptions.EndDate = value;
        }

        public Pecuniam TotalAnnualDeductions => Pondus.GetExpectedAnnualSum(CurrentDeductions).Neg;
        public Pecuniam TotalAnnualPay => Pondus.GetExpectedAnnualSum(CurrentPay).Abs;
        public Pecuniam TotalAnnualNetPay => TotalAnnualPay - TotalAnnualDeductions;

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
            return GetAt(dt, Income);
        }

        public virtual Pondus[] GetDeductionsAt(DateTime? dt)
        {
            return GetAt(dt, Deductions);
        }

        /// <summary>
        /// Resolves all salary\wage and deduction items for this Employment
        /// </summary>
        /// <param name="annualIncome"></param>
        protected internal void ResolvePayAndDeductions(Pecuniam annualIncome = null)
        {
            if (MyOptions.StartDate == DateTime.MinValue)
            {
                AddPondusForGivenRange(DateTime.Today, null, annualIncome);
                return;
            }
            var isCurrentEmployee = MyOptions.EndDate == null;
            var yearsOfService = GetYearsOfServiceInDates();
            if (yearsOfService.Count <= 0)
            {
                AddPondusForGivenRange(MyOptions.StartDate, MyOptions.EndDate, annualIncome);
                return;
            }
            for (var i = 0; i < yearsOfService.Count; i++)
            {
                var range = yearsOfService[i];
                if(i == yearsOfService.Count-1 && isCurrentEmployee)
                    range = new Tuple<DateTime, DateTime?>(range.Item1, null);
                AddPondusForGivenRange(range.Item1, range.Item2, annualIncome);
                    
            }
        }

        /// <summary>
        /// Adds both income and deduction items for the given date range at random
        /// </summary>
        /// <param name="startDt">Optional, defaults to the instances startdate if null</param>
        /// <param name="endDt">Optional, defaults to the instances enddate if null</param>
        /// <param name="annualIncome">Optional, will be generated at random if this is null or Zero</param>
        protected internal void AddPondusForGivenRange(DateTime startDt , DateTime? endDt = null, Pecuniam annualIncome = null)
        {
            startDt = startDt == DateTime.MinValue ? MyOptions.StartDate : startDt;

            annualIncome = annualIncome == null || annualIncome == Pecuniam.Zero
                ? GetRandomYearlyIncome(startDt)
                : annualIncome;
            var incomeItems = GetPayItemsForRange(annualIncome, startDt, endDt);
            foreach (var income in incomeItems)
                AddIncome(income);
            var deductions = GetDeductionItemsForRange(annualIncome, startDt, endDt);
            foreach (var deduction in deductions)
                AddDeduction(deduction);
        }
        
        /// <summary>
        /// Breaks the employment date range into annual blocks of time attempting 
        /// to capture the discrete time-ranges where wage\salary do not change.
        /// </summary>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetYearsOfServiceInDates()
        {
            var ranges = new List<Tuple<DateTime, DateTime?>>();

            var yearsOfService = GetYearsOfService();
            if (yearsOfService <= 0 || MyOptions.StartDate == DateTime.MinValue)
                return ranges;

            var employerFiscalYearEnd = Value?.FiscalYearEndDay ?? 1;

            //assume merit increases 90 days after fiscal year end
            var annualReviewDays = employerFiscalYearEnd + 90;
            var maxEndDt = MyOptions.EndDate.GetValueOrDefault(DateTime.Today);
            var prevDt = MyOptions.StartDate.Date;
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
            var hasStartDate = MyOptions.StartDate != DateTime.MinValue;
            if (!hasStartDate)
                return 0;

            var stDt = MyOptions.StartDate;
            var endDt = MyOptions.EndDate.GetValueOrDefault(DateTime.Today);

            var numYears = endDt.Year - stDt.Year;
            numYears = numYears <= 0 ? 0 : numYears;
            var maxAge = new[] {NAmerUtil.MAX_AGE_FEMALE, NAmerUtil.MAX_AGE_MALE}.Max();
            maxAge -= UsState.AGE_OF_ADULT;
            if(numYears > maxAge)
                throw new RahRowRagee($"the years of employment was calculated at {numYears} " +
                                      $"but the absolute max number of working years for an american is {maxAge} - " +
                                      "check your date ranges and try again");
            return numYears;
        }

        /// <summary>
        /// Gets a manifold of <see cref="Pondus"/> items based on the 
        /// names from GetIncomeItemNames in the Employment group assigning 
        /// random values as a portion of <see cref="amt"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetPayItemsForRange(Pecuniam amt, DateTime startDate, 
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            var itemsout = new List<Pondus>();
            startDate = startDate == DateTime.MinValue ? MyOptions.StartDate : startDate;
            amt = amt ?? Pecuniam.Zero;

            var incomeName2Rates = GetIncomeName2RandomRates();
            var incomeItems = GetIncomeItemNames();
            foreach (var incomeItem in incomeItems.Where(i => i.GetName(KindsOfNames.Group) == IncomeGroupNames.EMPLOYMENT))
            {
                var incomeRate = !incomeName2Rates.ContainsKey(incomeItem.Name) 
                    ? 0D 
                    : incomeName2Rates[incomeItem.Name];
                var p = new Pondus(incomeItem, interval)
                {
                    Inception = startDate,
                    Terminus = endDate
                };
                p.My.ExpectedValue = CalcValue(amt, incomeRate);
                itemsout.Add(p);
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Produces a dictionary of income item names to random rates where the sum of 
        /// all the rates equals 1
        /// </summary>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetIncomeName2RandomRates()
        {
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
                && MyOptions.EndDate.GetValueOrDefault(DateTime.Today) < DateTime.Today
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
        protected internal virtual Pondus[] GetDeductionItemsForRange(Pecuniam amt, DateTime startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            var itemsout = new List<Pondus>();
            amt = amt ?? Pecuniam.Zero;
            startDate = startDate == DateTime.MinValue ? MyOptions.StartDate : startDate;

            var deductionNames2Rates = GetDeductionNames2RandomRates(amt, startDate);
            var deductionItems = GetDeductionItemNames();
            foreach (var deduction in deductionItems)
            {
                var deductionRate = !deductionNames2Rates.ContainsKey(deduction.Name) 
                    ? 0D 
                    : deductionNames2Rates[deduction.Name];
                var p = new Pondus(deduction, interval)
                {
                    Inception = startDate,
                    Terminus = endDate,
                };
                p.My.ExpectedValue = CalcValue(amt, deductionRate);
                itemsout.Add(p);
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Produces a dictionary of deduction item names to random rates.
        /// </summary>
        /// <param name="annualIncomeAmount">Needed to calculate Health Insurance cost</param>
        /// <param name="startDate">Needed to calculate Health Insurance cost</param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetDeductionNames2RandomRates(Pecuniam annualIncomeAmount,
            DateTime? startDate)
        {
            //https://www.cdc.gov/nchs/fastats/health-insurance.htm
            var isInsCovered = Etx.TryAboveOrAt(124, Etx.Dice.OneThousand);
            var healthCareCost = isInsCovered
                ? GetHealthInsCost(startDate)
                : 0D;

            var totalHealthInsRate = healthCareCost / annualIncomeAmount.ToDouble();
            var employeeHealthInsRate = Math.Round(totalHealthInsRate * 0.17855D, 5);
            var dentalInsRate = GetRandomizeRateOf(employeeHealthInsRate, 8);
            var visionInsRate = GetRandomizeRateOf(employeeHealthInsRate, 13);

            var lifeInsRate = Etx.CoinToss
                ? GetRandomizeRateOf(employeeHealthInsRate, 13)
                : 0D;
            var supplementalLifeInsRate = Etx.TryBelowOrAt(3, Etx.Dice.Ten)
                ? GetRandomizeRateOf(employeeHealthInsRate, 21)
                : 0D;
            var dependentInsRate = Etx.TryBelowOrAt(1, Etx.Dice.Four)
                ? GetRandomizeRateOf(employeeHealthInsRate, 21)
                : 0D;
            var adAndDInsRate = Etx.TryBelowOrAt(1, Etx.Dice.Four)
                ? GetRandomizeRateOf(employeeHealthInsRate, 34)
                : 0D;
            var stDisabilityRate = Etx.TryBelowOrAt(1, Etx.Dice.Four)
                ? GetRandomizeRateOf(employeeHealthInsRate, 13)
                : 0D;
            var ltDisabilityRate = Etx.TryBelowOrAt(1, Etx.Dice.Four)
                ? GetRandomizeRateOf(employeeHealthInsRate, 13)
                : 0D;

            var fedTaxRate = NAmerUtil.Equations.FederalIncomeTaxRate.SolveForY(annualIncomeAmount.ToDouble());
            var stateTaxRate = GetRandomizeRateOf(fedTaxRate, 5);

            //https://www.irs.gov/taxtopics/tc751
            var ficaRate = 0.062D;
            var medicareRate = 0.0145D;

            var retirementRate = Etx.CoinToss
                ? Etx.DiscreteRange(new[] {0D, 0.01D, 0.02D, 0.03D, 0.04D, 0.05D})
                : 0D;
            var profitShareRate = Etx.TryBelowOrAt(13, Etx.Dice.OneHundred)
                ? GetRandomizeRateOf(0.03)
                : 0D;
            var pensionRate = Etx.TryBelowOrAt(9, Etx.Dice.OneHundred)
                ? GetRandomizeRateOf(0.03)
                : 0D;

            var unionDueRate = StandardOccupationalClassification.IsLaborUnion(_occupation)
                ? GetRandomizeRateOf(0.04)
                : 0D;
            var hraRate = Etx.CoinToss
                ? GetRandomizeRateOf(employeeHealthInsRate, 21)
                : 0D;
            var fsaRate = Etx.CoinToss
                ? GetRandomizeRateOf(employeeHealthInsRate, 21)
                : 0D;
            var creditUnionRate = Etx.TryBelowOrAt(3, Etx.Dice.OneHundred)
                ? GetRandomizeRateOf(0.01)
                : 0D;

            return new Dictionary<string, double>
            {
                {"Health", employeeHealthInsRate},
                {"Life", lifeInsRate},
                {"Supplemental Life", supplementalLifeInsRate},
                {"Dependent Life", dependentInsRate},
                {"Accidental Death & Dismemberment", adAndDInsRate},
                {"Dental", dentalInsRate},
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
        }

        protected internal virtual void AddIncome(Pondus p)
        {
            if (IsInRange(p))
            {
                p.My.ExpectedValue = p.My.ExpectedValue.Abs;
                _pay.Add(p);
            }
        }

        protected internal virtual void AddDeduction(Pondus d)
        {
            if (IsInRange(d))
            {
                d.My.ExpectedValue = d.My.ExpectedValue.Neg;
                _deductions.Add(d);
            }
        }

        protected internal bool IsInRange(Pondus item)
        {
            if (item == null)
                return false;
            var itemEndDt = item.Terminus;
            var rangeStartDt = MyOptions.StartDate;
            
            //item ended before this instance even started
            if (itemEndDt != null && itemEndDt.Value < rangeStartDt)
                return false;

            var itemStartDt = item.Inception;
            var rangeEndDt = MyOptions.EndDate;

            //instance ended before this item even started
            if (rangeEndDt != null && itemStartDt > rangeEndDt.Value)
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

        public override bool Equals(object obj)
        {
            if (!(obj is IEmployment e))
                return base.Equals(obj);

            return e.Value != null 
                   && Value != null
                   && e.Value.Equals(Value)
                   && e.Inception == Inception
                   && e.Terminus == Terminus;
        }

        public override int GetHashCode()
        {
            return (Value?.GetHashCode() ?? 1) +
                   MyOptions?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var t = new Tuple<string, string, DateTime?, DateTime?, Pecuniam>(Value?.ToString(), Occupation?.ToString(),
                Inception, Terminus, TotalAnnualPay);
            return t.ToString();
        }

        #endregion
    }
}