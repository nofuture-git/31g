using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Domus.Opes.US
{
    /// <inheritdoc cref="IExpense" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class AmericanExpenses : WealthBase, IExpense
    {
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();

        public AmericanExpenses(OpesOptions options = null) : base(options)
        {
            if (MyOptions.Inception == DateTime.MinValue)
                MyOptions.Inception = GetYearNeg(-1);
        }

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
            var exp = new AmericanExpenses(options);
            exp.ResolveItems(options);
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

        protected internal override void AddItem(Pondus expense)
        {
            if (expense == null)
                return;
            if (expense.My?.ExpectedValue != null)
                expense.My.ExpectedValue = expense.My.ExpectedValue.Neg;
            _expenses.Add(expense);
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

        protected internal override void ResolveItems(OpesOptions options = null)
        {
            options = options ?? MyOptions;
            var stDt = options.Inception == DateTime.MinValue ? GetYearNeg(-1) : options.Inception;
            var ranges = GetYearsInDates(stDt);

            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
            {
                var randIncome = Math.Round(GetRandomYearlyIncome(MyOptions.Inception).ToDouble() * 85);
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
            options = (options ?? MyOptions) ?? new OpesOptions();

            //TODO - integrate ability to have multiple mortgages in options
            options.GivenDirectly.Add(new Mereo("Other Lein", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

            //we want almost all-of-it in Mortgage
            options.DerivativeSlope = -0.2D;

            if (options.IsRenting)
            {
                options.GivenDirectly.Add(
                    new Mereo("Mortgage", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Maintenance", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Property Tax", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Association Fees", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });
            }
            else
            {
                options.GivenDirectly.Add(
                    new Mereo("Rent", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });
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
            options = (options ?? MyOptions) ?? new OpesOptions();

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
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Parking", "Registration Fees" });

            //focus most-of-it on Loan Payments or fuel
            options.DerivativeSlope = -0.33D;

            if (options.NumberOfVehicles > 0)
            {
                if (options.IsVehiclePaidOff)
                    options.GivenDirectly.Add(
                        new Mereo("Loan Payments", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Public Transportation", ExpenseGroupNames.TRANSPORTATION)
                    {
                        ExpectedValue = Pecuniam.Zero
                    });
            }
            else
            {
                options.GivenDirectly.Add(
                    new Mereo("Loan Payments", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Fuel", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Maintenance", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Property Tax", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Parking", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });
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
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Pet", "Vision",
                "Dental", "Health", "Disability", "Life" });

            options.GivenDirectly.Add(
                options.IsRenting
                    ? new Mereo("Home", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero }
                    : new Mereo("Renters", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero });

            if (options.NumberOfVehicles > 0)
                options.GivenDirectly.Add(new Mereo("Vehicle", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero });

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
            options = (options ?? MyOptions) ?? new OpesOptions();

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
            options = (options ?? MyOptions) ?? new OpesOptions();

            //when children are young we want to reflect that
            if (MyOptions.GetChildrenAges().All(x => x < AmericanData.AVG_AGE_CHILD_ENTER_SCHOOL))
            {
                options.GivenDirectly.Add(
                    new Mereo("Transportation", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("School Supplies", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Lunch Money", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Extracurricular", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Camp", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                options.GivenDirectly.Add(
                    new Mereo("Allowance", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
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
            options = (options ?? MyOptions) ?? new OpesOptions();

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
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossibleZeroOuts.AddRange(new[] { "Therapy", "Hospital",
                "Optical", "Dental", "Physician", "Supplements" });
            var d = GetItemNames2Portions(ExpenseGroupNames.HEALTH, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }
    }
}
