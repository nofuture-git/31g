using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

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
                {DeductionGroupNames.JUDGMENTS, GetJudgmentDeductionName2RandomRates}
            };
        }

        protected internal override void ResolveItems(OpesOptions options = null)
        {
            options = options ?? MyOptions;

            var ranges = _employment.MyItems.Any()
                ? _employment.MyItems.Select(e => new Tuple<DateTime, DateTime?>(e.Inception, e.Terminus))
                : GetYearsInDates(options.StartDate);

            foreach (var range in ranges.Distinct())
            {
                var cloneOptions = options.GetClone();
                cloneOptions.Interval = Interval.Annually;
               
                cloneOptions.StartDate = range.Item1;
                cloneOptions.EndDate = range.Item2;

                var items = GetItemsForRange(cloneOptions);
                foreach (var item in items)
                    AddItem(item);
            }
        }

        public override List<Tuple<string, double>> GetGroupNames2Portions(OpesOptions options)
        {
            options = options ?? MyOptions;
            //deductions act differently than other items since they are calculated from income
            var grps = new List<Tuple<string, double>>
            {
                new Tuple<string, double>(DeductionGroupNames.INSURANCE, 1),
                new Tuple<string, double>(DeductionGroupNames.EMPLOYMENT, 1),
                new Tuple<string, double>(DeductionGroupNames.GOVERNMENT, 1),
            };

            var assignedJudgmentsDirectly = options.GivenDirectly.Any(x =>
                x.AnyOfNameAs(DeductionGroupNames.JUDGMENTS) ||
                x.AnyOfKindAndValue(KindsOfNames.Group, DeductionGroupNames.JUDGMENTS));

            var optionsIncludeJudgment = options.IsPayingChildSupport || options.IsPayingSpousalSupport;

            var judgments = assignedJudgmentsDirectly || optionsIncludeJudgment ? 1 : 0;
            grps.Add(new Tuple<string, double>(DeductionGroupNames.JUDGMENTS, judgments));

            return grps;
        }

        protected internal Dictionary<string, double> GetInsuranceDeductionName2RandRates(OpesOptions options)
        {
            options = options ?? MyOptions;

            options.PossibleZeroOuts.AddRange(new[]
            {
                "Life", "Supplemental Life", "Dependent Life", "Accidental Death & Dismemberment",
                "Short-term Disability", "Long-term Disability"
            });

            //if the caller has assign values themselves - then just use those and leave
            if (options.GivenDirectly.Any())
            {
                return GetItemNames2Portions(DeductionGroupNames.INSURANCE, options)
                    .ToDictionary(t => t.Item1, t => t.Item2);
            }

            var expectedHealthInsCost = GetRandomEmployeeHealthInsCost(options.StartDate);
            var expectedDentalInsCost = GetRandomValueFrom(expectedHealthInsCost, 8);
            var expectedVisionInsCost = GetRandomValueFrom(expectedHealthInsCost, 13);

            options.GivenDirectly.Add(
                new Mereo("Health", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = expectedHealthInsCost.ToPecuniam()
                });

            options.GivenDirectly.Add(
                new Mereo("Dental", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = expectedDentalInsCost.ToPecuniam()
                });

            options.GivenDirectly.Add(
                new Mereo("Vision", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = expectedVisionInsCost.ToPecuniam()
                });
            var someRandRate = GetRandomRateFromClassicHook(Person?.GetAgeAt(options.StartDate));

            //we will use to force the SumTotal to exceed current GivenDirectly's sum
            var currentTotal = expectedHealthInsCost + expectedDentalInsCost + expectedVisionInsCost;
            var otherInsCost = currentTotal * someRandRate;

            //this will be used later to create Pondus so only overwrite it if is unassigned
            if(options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
                options.SumTotal = (otherInsCost + currentTotal).ToPecuniam();

            var d = GetItemNames2Portions(DeductionGroupNames.INSURANCE, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetGovernmentDeductionName2Rates(OpesOptions options)
        {
            options = options ?? MyOptions;

            //if the caller has assign values themselves - then just use those and leave
            if (options.GivenDirectly.Any())
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

            options.GivenDirectly.Add(
                new Mereo("Federal tax", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = fedTaxAmt.ToPecuniam()
                });
            options.GivenDirectly.Add(
                new Mereo("State tax", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = stateTaxAmt.ToPecuniam()
                });
            options.GivenDirectly.Add(
                new Mereo("FICA", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = ficaTaxAmt.ToPecuniam()
                });
            options.GivenDirectly.Add(
                new Mereo("Medicare", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = medicareTaxAmt.ToPecuniam()
                });

            //this will be used later to create Pondus so only overwrite it if is unassigned
            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
                options.SumTotal = (fedTaxAmt + stateTaxAmt + ficaTaxAmt + medicareTaxAmt).ToPecuniam();

            return GetItemNames2Portions(DeductionGroupNames.GOVERNMENT, options)
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetEmploymentDeductionName2Rates(OpesOptions options)
        {
            options = options ?? MyOptions;

            options.PossibleZeroOuts.AddRange(new[]
            {
                "Profit Sharing", "Pension", "Health Savings Account", "Credit Union Loan", "Flexible Spending Account"
            });

            //if the caller has assign values themselves - then just use those and leave
            if (options.GivenDirectly.Any())
            {
                return GetItemNames2Portions(DeductionGroupNames.EMPLOYMENT, options)
                    .ToDictionary(t => t.Item1, t => t.Item2);
            }

            var pay = GetPay(options);

            var retirementRate = Etx.DiscreteRange(new[] {0.01D, 0.02D, 0.03D, 0.04D, 0.05D});
            var retirementAmt = pay * retirementRate;
            options.GivenDirectly.Add(
                new Mereo("Registered Retirement Savings Plan", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = retirementAmt.ToPecuniam()
                });
            var unionDuesAmt = StandardOccupationalClassification.IsLaborUnion(_employment.Occupation)
                ? pay * GetRandomValueFrom(0.04)
                : 0.0D;

            options.GivenDirectly.Add(
                new Mereo("Union Dues", DeductionGroupNames.INSURANCE)
                {
                    ExpectedValue = unionDuesAmt.ToPecuniam()
                });

            //we need to have a SumTotal exceeding the current GivenDirectly's sum to have any of the others show up at random
            var someRandRate = GetRandomRateFromClassicHook(Person?.GetAgeAt(options.StartDate));

            //we will use to force the SumTotal to exceed current GivenDirectly's sum
            var currentTotal = retirementAmt + unionDuesAmt;
            var someRandAmount = currentTotal * someRandRate;

            //this will be used later to create Pondus so only overwrite it if is unassigned
            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
                options.SumTotal = (currentTotal + someRandAmount).ToPecuniam();

            return GetItemNames2Portions(DeductionGroupNames.EMPLOYMENT, options)
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetJudgmentDeductionName2RandomRates(OpesOptions options)
        {
            options = options ?? MyOptions;
            const string CHILD_SUPPORT = "Child Support";
            const string ALIMONY = "Alimony";

            var pay = GetPay(options);
            if (options.IsPayingChildSupport &&
                !options.AnyGivenDirectlyOfNameAndGroup(CHILD_SUPPORT, DeductionGroupNames.JUDGMENTS))
            {
                var adjPay = AmericanEquations.GetInflationAdjustedAmount(pay, 2015, options.StartDate);
                var adjMonthlyPay = adjPay / 12;
                var childSupportEquation =
                    AmericanEquations.GetChildSupportMonthlyCostEquation(options.ChildrenAges.Count);
                var childSupport = Math.Round(childSupportEquation.SolveForY(adjMonthlyPay), 2);

                //need to turn this back into annual amount
                childSupport = childSupport * 12;
                options.GivenDirectly.Add(
                    new Mereo(CHILD_SUPPORT, DeductionGroupNames.JUDGMENTS)
                    {
                        ExpectedValue = childSupport.ToPecuniam()
                    });
            }

            if (options.IsPayingSpousalSupport &&
                !options.AnyGivenDirectlyOfNameAndGroup(ALIMONY, DeductionGroupNames.JUDGMENTS))
            {
                //this is technically computed as 0.25 * (diff in spousal income)
                var randRate = Etx.RationalNumber(0.01, 0.25);
                var spouseSupport = Math.Round(randRate * pay, 2);
                options.GivenDirectly.Add(
                    new Mereo(ALIMONY, DeductionGroupNames.JUDGMENTS) {ExpectedValue = spouseSupport.ToPecuniam()});
            }

            var d = GetItemNames2Portions(DeductionGroupNames.JUDGMENTS, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        private double GetPay(OpesOptions options)
        {
            options = options ?? MyOptions;

            var pPay = Pondus.GetExpectedAnnualSum(_employment.GetPayAt(options.StartDate)) ?? Pecuniam.Zero;
            var pay = pPay == Pecuniam.Zero ? GetRandomYearlyIncome(options.StartDate).ToDouble() : pPay.ToDouble();
            return pay;
        }
        #endregion
    }
}
