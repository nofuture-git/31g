using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Org;
using NoFuture.Rand.Sp;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Opes.US
{
    /// <inheritdoc cref="AmericanDomusOpesBase"/>
    /// <summary>
    /// </summary>
    [Serializable]
    public class AmericanDeductions : AmericanDomusOpesBase, IDeinde
    {
        private readonly HashSet<NamedTradeline> _deductions = new HashSet<NamedTradeline>();
        private readonly AmericanEmployment _employment;

        /// <summary>
        /// Only makes sense in the context of employment
        /// </summary>
        /// <param name="employment"></param>
        public AmericanDeductions(AmericanEmployment employment)
        {
            _employment = employment ?? throw new ArgumentNullException(nameof(employment));
            if (_employment.Inception == DateTime.MinValue)
                _employment.Inception = GetYearNeg(-1);
        }
        internal AmericanDeductions(){ }

        #region properties

        public override Pecuniam Total => CurrentItems.Sum().GetNeg();

        protected internal override List<NamedTradeline> MyItems
        {
            get
            {
                var d = _deductions.ToList();
                d.Sort(Comparer);
                return d;
            }
        }

        protected override DomusOpesDivisions Division => DomusOpesDivisions.Deduction;

        #endregion

        #region methods

        public override void AddItem(NamedTradeline d)
        {
            if (d == null)
                return;
            //d.Expectation.Value = d.Expectation.Value.GetNeg();
            _deductions.Add(d);
        }

        protected override Pecuniam CalcValue(Pecuniam pecuniam, double d)
        {
            return base.CalcValue(pecuniam, d).GetNeg();
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x?.Replace(",", "").Replace(" ", ""), txtCase);
            var itemData = new Dictionary<string, object>();

            foreach (var p in CurrentItems)
            {
                var v = p.Value;
                if(v == Pecuniam.Zero)
                    continue;

                var deductionName = Division.ToString() + p.DueFrequency.ToInterval();
                deductionName += p.Name;
                if(itemData.ContainsKey(textFormat(deductionName)))
                    continue;
                itemData.Add(textFormat(deductionName), v);
            }
            return itemData;
        }

        protected override Dictionary<string, Func<AmericanDomusOpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<AmericanDomusOpesOptions, Dictionary<string, double>>>
            {
                {DeductionGroupNames.EMPLOYMENT, GetEmploymentDeductionName2Rates},
                {DeductionGroupNames.GOVERNMENT, GetGovernmentDeductionName2Rates},
                {DeductionGroupNames.INSURANCE, GetInsuranceDeductionName2RandRates},
                {DeductionGroupNames.JUDGMENTS, GetJudgmentDeductionName2RandomRates}
            };
        }

        protected internal override void RandomizeAllItems(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var ranges = _employment.MyItems.Any()
                ? _employment.MyItems.Select(e => new Tuple<DateTime, DateTime?>(e.Inception, e.Terminus))
                : GetYearsInDates(options.Inception);

            foreach (var range in ranges.Distinct())
            {
                var cloneOptions = options.GetClone();
                cloneOptions.DueFrequency = Constants.TropicalYear;
               
                cloneOptions.Inception = range.Item1;
                cloneOptions.Terminus = range.Item2;

                var items = GetItemsForRange(cloneOptions);
                foreach (var item in items)
                    AddItem(item);
            }
        }

        protected internal override List<Tuple<string, double>> GetGroupNames2Portions(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();
            //deductions act differently than other items since they are calculated from income
            var grps = new List<Tuple<string, double>>
            {
                new Tuple<string, double>(DeductionGroupNames.INSURANCE, 1),
                new Tuple<string, double>(DeductionGroupNames.EMPLOYMENT, 1),
                new Tuple<string, double>(DeductionGroupNames.GOVERNMENT, 1),
            };

            var assignedJudgmentsDirectly = options.AnyGivenDirectlyOfName(DeductionGroupNames.JUDGMENTS)
                || options.AnyGivenDirectlyOfGroupName(DeductionGroupNames.JUDGMENTS);

            var optionsIncludeJudgment = options.IsPayingChildSupport || options.IsPayingSpousalSupport;

            var judgments = assignedJudgmentsDirectly || optionsIncludeJudgment ? 1 : 0;
            grps.Add(new Tuple<string, double>(DeductionGroupNames.JUDGMENTS, judgments));

            return grps;
        }

        /// <summary>
        /// Produces the item names to rates for the Insurance Deductions
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetInsuranceDeductionName2RandRates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            options.AddPossibleZeroOuts(GetAllowZeroNames(Division, DeductionGroupNames.INSURANCE));

            //if the caller has assign values themselves - then just use those and leave
            if (options.AnyGivenDirectly())
            {
                return GetItemNames2Portions(DeductionGroupNames.INSURANCE, options)
                    .ToDictionary(t => t.Item1, t => t.Item2);
            }

            var expectedHealthInsCost = GetRandomEmployeeHealthInsCost(options.Inception);
            var expectedDentalInsCost = GetRandomValueFrom(expectedHealthInsCost, 8);
            var expectedVisionInsCost = GetRandomValueFrom(expectedHealthInsCost, 13);

            options.AddGivenDirectly("Health", DeductionGroupNames.INSURANCE, expectedHealthInsCost);

            options.AddGivenDirectly("Dental", DeductionGroupNames.INSURANCE, expectedDentalInsCost);

            options.AddGivenDirectly("Vision", DeductionGroupNames.INSURANCE, expectedVisionInsCost);

            var someRandRate = GetRandomRateFromClassicHook(options.FactorOptions.GetAge());

            //we will use to force the SumTotal to exceed current GivenDirectly's sum
            var currentTotal = expectedHealthInsCost + expectedDentalInsCost + expectedVisionInsCost;
            var otherInsCost = currentTotal * someRandRate;

            //this will be used later to create Pondus so only overwrite it if is unassigned
            if (options.SumTotal == null || options.SumTotal == 0)
                options.SumTotal = otherInsCost + currentTotal;

            var d = GetItemNames2Portions(DeductionGroupNames.INSURANCE, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Government Deductions (i.e. taxes)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetGovernmentDeductionName2Rates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            //if the caller has assign values themselves - then just use those and leave
            if (options.AnyGivenDirectly())
            {
                return GetItemNames2Portions(DeductionGroupNames.GOVERNMENT, options)
                    .ToDictionary(t => t.Item1, t => t.Item2);
            }

            var pay = GetPay(options);

            var fedTaxRate = AmericanEquations.FederalIncomeTaxRate.SolveForY(pay);
            var stateTaxRate = GetRandomValueFrom(fedTaxRate, 5);

            var fedTaxAmt = pay * fedTaxRate;
            var stateTaxAmt = pay * stateTaxRate;
            var ficaTaxAmt = pay * AmericanData.FICA_DEDUCTION_TAX_RATE;
            var medicareTaxAmt = pay * AmericanData.MEDICARE_DEDUCTION_TAX_RATE;

            options.AddGivenDirectly("Federal tax", DeductionGroupNames.GOVERNMENT, fedTaxAmt);
            options.AddGivenDirectly("State tax", DeductionGroupNames.GOVERNMENT, stateTaxAmt);
            options.AddGivenDirectly("FICA", DeductionGroupNames.GOVERNMENT, ficaTaxAmt);
            options.AddGivenDirectly("Medicare", DeductionGroupNames.GOVERNMENT, medicareTaxAmt);

            //this will be used later to create Pondus so only overwrite it if is unassigned
            if (options.SumTotal == null || options.SumTotal == 0)
                options.SumTotal = (fedTaxAmt + stateTaxAmt + ficaTaxAmt + medicareTaxAmt);

            return GetItemNames2Portions(DeductionGroupNames.GOVERNMENT, options)
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Employment Deductions (e.g. 401K)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetEmploymentDeductionName2Rates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            options.AddPossibleZeroOuts(GetAllowZeroNames(Division, DeductionGroupNames.EMPLOYMENT));

            //if the caller has assign values themselves - then just use those and leave
            if (options.AnyGivenDirectly())
            {
                return GetItemNames2Portions(DeductionGroupNames.EMPLOYMENT, options)
                    .ToDictionary(t => t.Item1, t => t.Item2);
            }

            var pay = GetPay(options);

            var retirementRate = Etx.RandomPickOne(new[] {0.01D, 0.02D, 0.03D, 0.04D, 0.05D});
            var retirementAmt = pay * retirementRate;
            options.AddGivenDirectly("Registered Retirement Savings Plan", DeductionGroupNames.EMPLOYMENT,
                retirementAmt);

            var unionDuesAmt = StandardOccupationalClassification.IsLaborUnion(_employment.Occupation)
                ? pay * GetRandomValueFrom(0.04)
                : 0.0D;

            options.AddGivenDirectly("Union Dues", DeductionGroupNames.EMPLOYMENT, unionDuesAmt);

            //we need to have a SumTotal exceeding the current GivenDirectly's sum to have any of the others show up at random
            var someRandRate = GetRandomRateFromClassicHook(options.FactorOptions.GetAge());

            //we will use to force the SumTotal to exceed current GivenDirectly's sum
            var currentTotal = retirementAmt + unionDuesAmt;
            var someRandAmount = currentTotal * someRandRate;

            //this will be used later to create Pondus so only overwrite it if is unassigned
            if (options.SumTotal == null || options.SumTotal == 0)
                options.SumTotal = currentTotal + someRandAmount;

            return GetItemNames2Portions(DeductionGroupNames.EMPLOYMENT, options)
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Judgement Deductions (e.g. alimony paid-out)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetJudgmentDeductionName2RandomRates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();
            const string CHILD_SUPPORT = "Child Support";
            const string ALIMONY = "Alimony";

            var pay = GetPay(options);
            if (options.IsPayingChildSupport &&
                !options.AnyGivenDirectlyOfNameAndGroup(CHILD_SUPPORT, DeductionGroupNames.JUDGMENTS))
            {
                var adjPay = AmericanEquations.GetInflationAdjustedAmount(pay, 2015, options.Inception);
                var adjMonthlyPay = adjPay / 12;
                var childSupportEquation =
                    AmericanEquations.GetChildSupportMonthlyCostEquation(options.GetChildrenAges().Count);
                var childSupport = Math.Round(childSupportEquation.SolveForY(adjMonthlyPay), 2);

                //need to turn this back into annual amount
                childSupport = childSupport * 12;
                options.AddGivenDirectly(CHILD_SUPPORT, DeductionGroupNames.JUDGMENTS, childSupport);
            }

            if (options.IsPayingSpousalSupport &&
                !options.AnyGivenDirectlyOfNameAndGroup(ALIMONY, DeductionGroupNames.JUDGMENTS))
            {
                //this is technically computed as 0.25 * (diff in spousal income)
                var randRate = Etx.RandomDouble(0.01, 0.25);
                var spouseSupport = Math.Round(randRate * pay, 2);
                options.AddGivenDirectly(ALIMONY, DeductionGroupNames.JUDGMENTS, spouseSupport);
            }

            var d = GetItemNames2Portions(DeductionGroupNames.JUDGMENTS, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        private double GetPay(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var pPay = _employment.GetAt(options.Inception).Sum() ?? Pecuniam.Zero;
            var pay = pPay == Pecuniam.Zero ? options.GetRandomYearlyIncome(options.Inception).ToDouble() : pPay.ToDouble();
            return pay;
        }

        public override string ToString()
        {
            var t = new Tuple<string, string, DateTime?, DateTime?, Pecuniam>(_employment.EmployingCompanyName,
                _employment.Occupation?.ToString(),
                _employment.Inception, _employment.Terminus, GetCurrent(MyItems).Sum());
            return t.ToString();
        }

        private double GetRandomValueFrom(double baseRate, double dividedby = 1)
        {
            if (baseRate == 0)
                return 0;
            var mean = baseRate / (dividedby == 0 ? 1 : dividedby);
            var stdDev = Math.Round(mean * 0.155, 5);
            return Etx.RandomValueInNormalDist(mean, stdDev);
        }

        private double GetRandomEmployeeHealthInsCost(DateTime? atDate)
        {
            var totalCost = GetRandomHealthInsCost(atDate);
            return totalCost * (1 - AmericanData.PERCENT_EMPLY_INS_COST_PAID_BY_EMPLOYER);
        }

        private double GetRandomHealthInsCost(DateTime? atDate)
        {
            var dt = atDate.GetValueOrDefault(DateTime.UtcNow);
            var mean = AmericanEquations.HealthInsuranceCostPerPerson.SolveForY(dt.ToDouble());
            var stdDev = Math.Round(mean * 0.155, 2);

            return Etx.RandomValueInNormalDist(mean, stdDev);
        }
        #endregion
    }
}
