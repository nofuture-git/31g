using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Opes.US
{
    /// <inheritdoc cref="IExpense" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class AmericanExpenses : WealthBase, IExpense
    {
        private const double PERCENT_EXPENSE_OF_INCOME = 0.85;
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();

        public virtual Pondus[] CurrentExpectedExpenses => GetCurrent(MyItems);

        public virtual Pecuniam TotalAnnualExpectedExpenses => Pondus.GetExpectedAnnualSum(CurrentExpectedExpenses);

        protected override DomusOpesDivisions Division => DomusOpesDivisions.Expense;

        /// <summary>
        /// Gets expenses at random
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanExpenses RandomExpenses(OpesOptions options = null)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var exp = new AmericanExpenses();
            exp.RandomizeAllItems(options);
            return exp;
        }

        public virtual Pondus[] GetExpectedExpensesAt(DateTime? dt)
        {
            return GetAt(dt, MyItems);
        }

        protected internal override List<Pondus> MyItems
        {
            get
            {
                var e = _expenses.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        public override void AddItem(Pondus expense)
        {
            if (expense == null)
                return;
            if (expense.Expectation?.Value != null)
                expense.Expectation.Value = expense.Expectation.Value.GetNeg();
            _expenses.Add(expense);
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x?.Replace(",", "").Replace(" ", ""), txtCase);
            var itemData = new Dictionary<string, object>();

            foreach (var p in CurrentExpectedExpenses)
            {
                if (p.Expectation == null || p.Expectation.Value == Pecuniam.Zero)
                    continue;

                var expenseName = Division.ToString() + Interval.Monthly;
                expenseName += p.Name;
                itemData.Add(textFormat(expenseName), p.Expectation.GetValueInTimespanDenominator(30));
            }

            return itemData;
        }

        protected override Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<OpesOptions, Dictionary<string, double>>>
            {
                {ExpenseGroupNames.HOME, GetHomeExpenseNames2RandomRates},
                {ExpenseGroupNames.UTILITIES, GetUtilityExpenseNames2RandomRates},
                {ExpenseGroupNames.TRANSPORTATION, GetTransportationExpenseNames2RandomRates},
                {ExpenseGroupNames.INSURANCE, GetInsuranceExpenseNames2RandomRates},
                {ExpenseGroupNames.PERSONAL, GetPersonalExpenseNames2RandomRates},
                {ExpenseGroupNames.CHILDREN, GetChildrenExpenseNames2RandomRates},
                {ExpenseGroupNames.DEBT, GetDebtExpenseNames2RandomRates},
                {ExpenseGroupNames.HEALTH, GetHealthExpenseNames2RandomRates},
            };
        }

        protected internal override void RandomizeAllItems(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var stDt = options.Inception == DateTime.MinValue ? GetYearNeg(-1) : options.Inception;
            var ranges = GetYearsInDates(stDt);

            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
            {
                var randIncome = Math.Round(GetRandomYearlyIncome(options.Inception, options).ToDouble() *
                                            PERCENT_EXPENSE_OF_INCOME);
                options.SumTotal = randIncome.ToPecuniam();
            }

            foreach (var range in ranges)
            {
                var cloneOptions = options.GetClone();
                cloneOptions.Inception = range.Item1;
                cloneOptions.Terminus = range.Item2;
                cloneOptions.Interval = Interval.Annually;

                var items = GetItemsForRange(cloneOptions);
                foreach (var item in items)
                    AddItem(item);
            }
        }

        /// <summary>
        /// Produces the item names to rates for the Home Expenses (e.g. rent)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetHomeExpenseNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            //TODO - integrate ability to have multiple mortgages in options
            options.GivenDirectly.Add(new Mereo("Other Lein", ExpenseGroupNames.HOME) { Value = Pecuniam.Zero });

            //we want almost all-of-it in Mortgage
            options.DerivativeSlope = -0.2D;

            if (options.IsRenting)
            {
                options.GivenDirectly.Add(
                    new Mereo("Mortgage", ExpenseGroupNames.HOME) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Maintenance", ExpenseGroupNames.HOME) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Property Tax", ExpenseGroupNames.HOME) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Association Fees", ExpenseGroupNames.HOME) { Value = Pecuniam.Zero });
            }
            else
            {
                options.GivenDirectly.Add(
                    new Mereo("Rent", ExpenseGroupNames.HOME) { Value = Pecuniam.Zero });
                options.PossibleZeroOuts.Add("Association Fees");
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.HOME, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Utility Expenses (e.g. gas bill)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetUtilityExpenseNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            if (options.IsRenting)
                options.PossibleZeroOuts.AddRange(new[] { "Gas", "Water", "Sewer", "Trash" });

            var d = GetItemNames2Portions(ExpenseGroupNames.UTILITIES, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Transportation Expenses (e.g. car payment, gas, etc.)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetTransportationExpenseNames2RandomRates(
            OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Parking", "Registration Fees" });

            //focus most-of-it on Loan Payments or fuel
            options.DerivativeSlope = -0.33D;

            if (options.NumberOfVehicles > 0)
            {
                if (options.IsVehiclePaidOff)
                    options.GivenDirectly.Add(
                        new Mereo("Loan Payments", ExpenseGroupNames.TRANSPORTATION) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Public Transportation", ExpenseGroupNames.TRANSPORTATION)
                    {
                        Value = Pecuniam.Zero
                    });
            }
            else
            {
                options.GivenDirectly.Add(
                    new Mereo("Loan Payments", ExpenseGroupNames.TRANSPORTATION) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Fuel", ExpenseGroupNames.TRANSPORTATION) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Maintenance", ExpenseGroupNames.TRANSPORTATION) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Property Tax", ExpenseGroupNames.TRANSPORTATION) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Parking", ExpenseGroupNames.TRANSPORTATION) { Value = Pecuniam.Zero });
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.TRANSPORTATION, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Insurance Expenses (being paid for directly, not as a deduction)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetInsuranceExpenseNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Pet", "Vision",
                "Dental", "Health", "Disability", "Life" });

            options.GivenDirectly.Add(
                options.IsRenting
                    ? new Mereo("Home", ExpenseGroupNames.INSURANCE) { Value = Pecuniam.Zero }
                    : new Mereo("Renters", ExpenseGroupNames.INSURANCE) { Value = Pecuniam.Zero });

            if (options.NumberOfVehicles > 0)
                options.GivenDirectly.Add(new Mereo("Vehicle", ExpenseGroupNames.INSURANCE) { Value = Pecuniam.Zero });

            var d = GetItemNames2Portions(ExpenseGroupNames.INSURANCE, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Personal Expenses (e.g. groceries, subscriptions, etc.)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetPersonalExpenseNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Dues", "Subscriptions",
                "Gifts", "Vice", "Clothing" });
            var d = GetItemNames2Portions(ExpenseGroupNames.PERSONAL, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Child Related Expenses (e.g. day-care), 
        /// which are NOT included in groceries (e.g. diapers, food, clothes, etc.)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetChildrenExpenseNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            //when children are young we want to reflect that
            if (options.GetChildrenAges().All(x => x < AmericanData.AVG_AGE_CHILD_ENTER_SCHOOL))
            {
                options.GivenDirectly.Add(
                    new Mereo("Transportation", ExpenseGroupNames.CHILDREN) { Value = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("School Supplies", ExpenseGroupNames.CHILDREN) { Value = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Lunch Money", ExpenseGroupNames.CHILDREN) { Value = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Extracurricular", ExpenseGroupNames.CHILDREN) { Value = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Camp", ExpenseGroupNames.CHILDREN) { Value = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Allowance", ExpenseGroupNames.CHILDREN) { Value = Pecuniam.Zero });
            }
            else
            {
                options.PossibleZeroOuts.AddRange(new[]{"Lunch Money",
                    "Extracurricular", "Camp", "Transportation", "Allowance"});
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.CHILDREN, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Personal Debt Expenses (e.g. credit card payments)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetDebtExpenseNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            var d = GetItemNames2Portions(ExpenseGroupNames.DEBT, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Health Expenses (i.e. being paid out-of-pocket)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetHealthExpenseNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Therapy", "Hospital",
                "Optical", "Dental", "Physician", "Supplements" });
            var d = GetItemNames2Portions(ExpenseGroupNames.HEALTH, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }
    }
}
