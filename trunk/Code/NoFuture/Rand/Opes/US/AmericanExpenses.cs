using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;

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
        private readonly HashSet<NamedReceivable> _expenses = new HashSet<NamedReceivable>();

        public virtual NamedReceivable[] CurrentExpenses => GetCurrent(MyItems);

        public virtual Pecuniam TotalAnnualExpenses => CurrentExpenses.Sum().GetNeg();

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

        public virtual NamedReceivable[] GetExpensesAt(DateTime? dt)
        {
            return GetAt(dt, MyItems);
        }

        protected internal override List<NamedReceivable> MyItems
        {
            get
            {
                var e = _expenses.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        public override void AddItem(NamedReceivable expense)
        {
            if (expense == null)
                return;
            if (expense.Expectation?.Value != null)
                expense.Expectation.Value = expense.Expectation.Value.GetNeg();
            _expenses.Add(expense);
        }

        protected override Pecuniam CalcValue(Pecuniam pecuniam, double d)
        {
            return base.CalcValue(pecuniam, d).GetNeg();
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x?.Replace(",", "").Replace(" ", ""), txtCase);
            var itemData = new Dictionary<string, object>();

            foreach (var p in CurrentExpenses)
            {
                var v = p.AveragePerDueFrequency(PecuniamExtensions.GetTropicalMonth());
                if (v == Pecuniam.Zero)
                    continue;
                var expenseName = Division.ToString() + p.DueFrequency.ToInterval();
                expenseName += p.Name;
                if (itemData.ContainsKey(textFormat(expenseName)))
                    continue;
                itemData.Add(textFormat(expenseName), v.GetRounded());
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
                cloneOptions.DueFrequency = PecuniamExtensions.GetTropicalMonth();

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
            options.AddGivenDirectlyZero("Other Lein", ExpenseGroupNames.HOME);

            //we want almost all-of-it in Mortgage
            options.DerivativeSlope = -0.2D;

            if (options.IsRenting)
            {
                options.AddGivenDirectlyZero("Mortgage", ExpenseGroupNames.HOME);

                options.AddGivenDirectlyZero("Maintenance", ExpenseGroupNames.HOME);

                options.AddGivenDirectlyZero("Property Tax", ExpenseGroupNames.HOME);

                options.AddGivenDirectlyZero("Association Fees", ExpenseGroupNames.HOME);
            }
            else
            {
                options.AddGivenDirectlyZero("Rent", ExpenseGroupNames.HOME);
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
                    options.AddGivenDirectlyZero("Loan Payments", ExpenseGroupNames.TRANSPORTATION);

                options.AddGivenDirectlyZero("Public Transportation", ExpenseGroupNames.TRANSPORTATION);
            }
            else
            {
                options.AddGivenDirectlyZero("Loan Payments", ExpenseGroupNames.TRANSPORTATION);

                options.AddGivenDirectlyZero("Fuel", ExpenseGroupNames.TRANSPORTATION);

                options.AddGivenDirectlyZero("Maintenance", ExpenseGroupNames.TRANSPORTATION);

                options.AddGivenDirectlyZero("Property Tax", ExpenseGroupNames.TRANSPORTATION);

                options.AddGivenDirectlyZero("Parking", ExpenseGroupNames.TRANSPORTATION);
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

            options.AddGivenDirectlyZero(options.IsRenting ? "Home" : "Renters", ExpenseGroupNames.INSURANCE);

            if (options.NumberOfVehicles > 0)
                options.AddGivenDirectlyZero("Vehicle", ExpenseGroupNames.INSURANCE);

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
                options.AddGivenDirectlyZero("Transportation", ExpenseGroupNames.CHILDREN);
                options.AddGivenDirectlyZero("School Supplies", ExpenseGroupNames.CHILDREN);
                options.AddGivenDirectlyZero("Lunch Money", ExpenseGroupNames.CHILDREN);
                options.AddGivenDirectlyZero("Extracurricular", ExpenseGroupNames.CHILDREN);
                options.AddGivenDirectlyZero("Camp", ExpenseGroupNames.CHILDREN);
                options.AddGivenDirectlyZero("Allowance", ExpenseGroupNames.CHILDREN);
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
