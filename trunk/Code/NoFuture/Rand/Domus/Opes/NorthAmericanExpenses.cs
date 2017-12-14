using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    public class NorthAmericanExpenses : WealthBase, IExpense
    {
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();

        public NorthAmericanExpenses(NorthAmerican american) : base(american)
        {
        }

        public NorthAmericanExpenses(NorthAmerican american, OpesOptions options) : base(american, options)
        {
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Produces a dictionary of Expense-Home names to rates whose sum equals 1
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetHomeExpenseNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            var tOptions = options.GetClone();

            //TODO - integrate ability to have multiple mortgages in options
            tOptions.GivenDirectly.Add(new Mereo("Other Lein", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

            if (tOptions.IsRenting)
            {
                tOptions.GivenDirectly.Add(
                    new Mereo("Mortgage", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Maintenance", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Property Tax", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Association Fees", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });
            }
            else
            {
                tOptions.GivenDirectly.Add(
                    new Mereo("Rent", ExpenseGroupNames.HOME) { ExpectedValue = Pecuniam.Zero });
                tOptions.PossiableZeroOuts.Add("Association Fees");
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.HOME, tOptions);
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
            var tOptions = options.GetClone();
            if (tOptions.IsRenting)
                tOptions.PossiableZeroOuts.AddRange(new[] { "Gas", "Water", "Sewer", "Trash" });

            var d = GetItemNames2Portions(ExpenseGroupNames.UTILITIES, tOptions);
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
            var tOptions = options.GetClone();
            tOptions.PossiableZeroOuts.AddRange(new[] { "Parking", "Registration Fees" });

            if (tOptions.HasVehicles)
            {
                if (tOptions.IsVehiclePaidOff)
                    tOptions.GivenDirectly.Add(
                        new Mereo("Loan Payments", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Public Transportation", ExpenseGroupNames.TRANSPORTATION)
                    {
                        ExpectedValue = Pecuniam.Zero
                    });
            }
            else
            {
                tOptions.GivenDirectly.Add(
                    new Mereo("Loan Payments", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Fuel", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Maintenance", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Property Tax", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });

                tOptions.GivenDirectly.Add(
                    new Mereo("Parking", ExpenseGroupNames.TRANSPORTATION) { ExpectedValue = Pecuniam.Zero });
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.TRANSPORTATION, tOptions);
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
            var tOptions = options.GetClone();
            tOptions.PossiableZeroOuts.AddRange(new[] { "Pet", "Vision",
                "Dental", "Health", "Disability", "Life" });

            tOptions.GivenDirectly.Add(
                tOptions.IsRenting
                    ? new Mereo("Home", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero }
                    : new Mereo("Renters", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero });

            if (tOptions.HasVehicles)
                tOptions.GivenDirectly.Add(new Mereo("Vehicle", ExpenseGroupNames.INSURANCE) { ExpectedValue = Pecuniam.Zero });

            var d = GetItemNames2Portions(ExpenseGroupNames.INSURANCE, tOptions);
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
            var tOptions = options.GetClone();
            tOptions.PossiableZeroOuts.AddRange(new[] { "Dues", "Subscriptions",
                "Gifts", "Vice", "Clothing" });
            var d = GetItemNames2Portions(ExpenseGroupNames.PERSONAL, tOptions);
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
            var tOptions = options.GetClone();

            //when children are young we want to reflect that
            if (MyOptions.HasChildren && MyOptions.ChildrenAges.All(x => x < NAmerUtil.AVG_AGE_CHILD_ENTER_SCHOOL))
            {
                tOptions.GivenDirectly.Add(
                    new Mereo("Transportation", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                tOptions.GivenDirectly.Add(
                    new Mereo("School Supplies", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                tOptions.GivenDirectly.Add(
                    new Mereo("Lunch Money", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                tOptions.GivenDirectly.Add(
                    new Mereo("Extracurricular", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                tOptions.GivenDirectly.Add(
                    new Mereo("Camp", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
                tOptions.GivenDirectly.Add(
                    new Mereo("Allowance", ExpenseGroupNames.CHILDREN) { ExpectedValue = Pecuniam.Zero });
            }
            else
            {
                tOptions.PossiableZeroOuts.AddRange(new[]{"Lunch Money",
                    "Extracurricular", "Camp", "Transportation", "Allowance"});
            }

            var d = GetItemNames2Portions(ExpenseGroupNames.CHILDREN, tOptions);
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
            var tOptions = options.GetClone();
            tOptions.PossiableZeroOuts.AddRange(new[] { "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            var d = GetItemNames2Portions(ExpenseGroupNames.DEBT, tOptions);
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
            var tOptions = options.GetClone();
            tOptions.PossiableZeroOuts.AddRange(new[] { "Therapy", "Hospital",
                "Optical", "Dental", "Physician", "Supplements" });
            var d = GetItemNames2Portions(ExpenseGroupNames.HEALTH, tOptions);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }
    }
}
