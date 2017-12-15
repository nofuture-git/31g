using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Domus.Opes
{
    public class NorthAmericanExpenses : WealthBase, IExpense
    {
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();

        public NorthAmericanExpenses(NorthAmerican american, OpesOptions options = null) : base(american, options)
        {
            if (MyOptions.StartDate == DateTime.MinValue)
                MyOptions.StartDate = GetYearNeg(-1);
        }

        public virtual Pondus[] CurrentExpectedExpenses => GetCurrent(MyItems);

        public virtual Pecuniam TotalAnnualExpectedExpenses => Pondus.GetExpectedAnnualSum(CurrentExpectedExpenses);

        protected override DomusOpesDivisions Division => DomusOpesDivisions.Expense;

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

        protected internal override void ResolveItems(OpesOptions options)
        {
            options = options ?? MyOptions;
            var stDt = options.StartDate == DateTime.MinValue ? GetYearNeg(-1) : options.StartDate;
            var ranges = GetYearsInDates(stDt);

            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
            {
                var randIncome = Math.Round(GetRandomYearlyIncome(MyOptions.StartDate).ToDouble() * 85);
                options.SumTotal = randIncome.ToPecuniam();
            }

            foreach (var range in ranges)
            {
                var cloneOptions = options.GetClone();
                cloneOptions.StartDate = range.Item1;
                cloneOptions.EndDate = range.Item2;
                cloneOptions.Interval = Interval.Annually;

                var items = GetItemsForRange(cloneOptions);
                foreach (var item in items)
                    AddItem(item);
            }
        }

        /// <summary>
        /// Produces a dictionary of Expense-Home names to rates whose sum equals 1
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
                options.PossiableZeroOuts.Add("Association Fees");
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.HOME, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces a dictionary of Expense-Utilities names to rates whose sum equals 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetUtilityExpenseNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            if (options.IsRenting)
                options.PossiableZeroOuts.AddRange(new[] { "Gas", "Water", "Sewer", "Trash" });

            var d = GetItemNames2Portions(ExpenseGroupNames.UTILITIES, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces a dictionary of Expense-Transportation names to rates whose sum equals 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetTransportationExpenseNames2RandomRates(
            OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossiableZeroOuts.AddRange(new[] { "Parking", "Registration Fees" });

            //focus most-of-it on Loan Payments or fuel
            options.DerivativeSlope = -0.33D;

            if (options.HasVehicles)
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
        /// Produces a dictionary of Expense-Insurance names to rates whose sum equals 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetInsuranceExpenseNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossiableZeroOuts.AddRange(new[] { "Pet", "Vision",
                "Dental", "Health", "Disability", "Life" });

            options.GivenDirectly.Add(
                options.IsRenting
                    ? new Mereo("Home", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero }
                    : new Mereo("Renters", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero });

            if (options.HasVehicles)
                options.GivenDirectly.Add(new Mereo("Vehicle", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero });

            var d = GetItemNames2Portions(ExpenseGroupNames.INSURANCE, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces a dictionary of Expense-Personal names to rates whose sum equals 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetPersonalExpenseNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossiableZeroOuts.AddRange(new[] { "Dues", "Subscriptions",
                "Gifts", "Vice", "Clothing" });
            var d = GetItemNames2Portions(ExpenseGroupNames.PERSONAL, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces a dictionary of Expense-Children names to rates whose sum equals 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetChildrenExpenseNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            //when children are young we want to reflect that
            if (MyOptions.HasChildren && MyOptions.ChildrenAges.All(x => x < NAmerUtil.AVG_AGE_CHILD_ENTER_SCHOOL))
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
                options.PossiableZeroOuts.AddRange(new[]{"Lunch Money",
                    "Extracurricular", "Camp", "Transportation", "Allowance"});
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.CHILDREN, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces a dictionary of Expense-Debt names to rates whose sum equals 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetDebtExpenseNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossiableZeroOuts.AddRange(new[] { "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            var d = GetItemNames2Portions(ExpenseGroupNames.DEBT, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces a dictionary of Expense-Health names to rates whose sum is equal to 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetHealthExpenseNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.PossiableZeroOuts.AddRange(new[] { "Therapy", "Hospital",
                "Optical", "Dental", "Physician", "Supplements" });
            var d = GetItemNames2Portions(ExpenseGroupNames.HEALTH, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }
    }
}
