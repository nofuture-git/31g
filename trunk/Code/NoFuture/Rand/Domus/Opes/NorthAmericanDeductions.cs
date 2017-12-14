using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    public class NorthAmericanDeductions : WealthBase, IDeductions
    {
        private readonly HashSet<Pondus> _deductions = new HashSet<Pondus>();
        private readonly NorthAmericanEmployment _employment;

        internal NorthAmericanDeductions(NorthAmericanEmployment employment) : base(employment?.Person, employment?.MyOptions)
        {
            _employment = employment ?? throw new ArgumentNullException(nameof(employment));
        }

        #region properties

        public virtual Pondus[] CurrentDeductions => GetDeductionsAt(null);
        public Pecuniam TotalAnnualDeductions => Pondus.GetExpectedAnnualSum(CurrentDeductions).Neg;
        public virtual Pondus[] GetDeductionsAt(DateTime? dt)
        {
            return GetAt(dt, MyItems);
        }

        protected internal override List<Pondus> MyItems
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

        protected internal override void AddItem(Pondus d)
        {
            d.My.ExpectedValue = d.My.ExpectedValue.Neg;
            _deductions.Add(d);
        }

        protected override Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<OpesOptions, Dictionary<string, double>>>
            {
                {DeductionGroupNames.EMPLOYMENT, GetEmploymentDeductionName2Rates},
                {DeductionGroupNames.GOVERNMENT, GetGovernmentDeductionName2Rates},
                {DeductionGroupNames.INSURANCE, GetInsuranceDeductionName2RandRates},
                {DeductionGroupNames.JUDGMENTS, GetJudgementDeductionName2RandomRates}
            };
        }

        protected internal override void ResolveItems(OpesOptions options)
        {
            throw new NotImplementedException();
        }

        protected internal Dictionary<string, double> GetInsuranceDeductionName2RandRates(OpesOptions options)
        {
            var employeeHealthInsRate = GetEmployeeHealthInsRate(options);
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
            };
        }

        protected internal Dictionary<string, double> GetGovernmentDeductionName2Rates(OpesOptions options)
        {
            var annualIncomeAmount = options.SumTotal;
            var fedTaxRate = NAmerUtil.Equations.FederalIncomeTaxRate.SolveForY(annualIncomeAmount.ToDouble());
            var stateTaxRate = GetRandomizeRateOf(fedTaxRate, 5);

            //https://www.irs.gov/taxtopics/tc751
            var ficaRate = 0.062D;
            var medicareRate = 0.0145D;

            return new Dictionary<string, double>
            {
                {"Federal tax", fedTaxRate},
                {"State tax", stateTaxRate},
                {"FICA", ficaRate},
                {"Medicare", medicareRate},
            };
        }

        protected internal Dictionary<string, double> GetEmploymentDeductionName2Rates(OpesOptions options)
        {
            var startDate = options.StartDate;
            var annualIncomeAmount = options.SumTotal;

            //https://www.cdc.gov/nchs/fastats/health-insurance.htm
            var isInsCovered = Etx.TryAboveOrAt(124, Etx.Dice.OneThousand);

            var healthCareCost = isInsCovered
                ? GetHealthInsCost(startDate)
                : 0D;

            var totalHealthInsRate = healthCareCost / annualIncomeAmount.ToDouble();
            var employeeHealthInsRate = Math.Round(totalHealthInsRate * 0.17855D, DF_ROUND_DECIMAL_PLACES);

            var retirementRate = Etx.CoinToss
                ? Etx.DiscreteRange(new[] { 0D, 0.01D, 0.02D, 0.03D, 0.04D, 0.05D })
                : 0D;
            var profitShareRate = Etx.TryBelowOrAt(13, Etx.Dice.OneHundred)
                ? GetRandomizeRateOf(0.03)
                : 0D;
            var pensionRate = Etx.TryBelowOrAt(9, Etx.Dice.OneHundred)
                ? GetRandomizeRateOf(0.03)
                : 0D;

            var unionDueRate = StandardOccupationalClassification.IsLaborUnion(_employment.Occupation)
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
                {"Registered Retirement Savings Plan", retirementRate},
                {"Profit Sharing", profitShareRate},
                {"Pension", pensionRate},
                {"Union Dues", unionDueRate},
                {"Health Savings Account", hraRate},
                {"Flexible Spending Account", fsaRate},
                {"Credit Union Loan", creditUnionRate},
            };
        }

        protected internal Dictionary<string, double> GetJudgementDeductionName2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            var tOptions = options.GetClone();
            var d = GetItemNames2Portions(DeductionGroupNames.JUDGMENTS, tOptions);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        #endregion
    }
}
