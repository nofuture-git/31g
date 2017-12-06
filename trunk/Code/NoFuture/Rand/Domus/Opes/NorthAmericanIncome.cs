using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IReditus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// Represents the income (both employment and other) along with all expenses
    /// of a NorthAmerican over some span of time.
    /// </summary>
    [Serializable]
    public class NorthAmericanIncome : WealthBase, IReditus
    {
        #region constants

        internal const string EXPENSE_GRP_HOME = "Home";
        internal const string EXPENSE_GRP_UTILITIES = "Utilities";
        internal const string EXPENSE_GRP_TRANSPORTATION = "Transportation";
        internal const string EXPENSE_GRP_INSURANCE = "Insurance Premiums";
        internal const string EXPENSE_GRP_PERSONAL = "Personal";
        internal const string EXPENSE_GRP_CHILDREN = "Children";
        internal const string EXPENSE_GRP_DEBT = "Debts";
        internal const string EXPENSE_GRP_HEALTH = "Health";
        #endregion

        #region fields
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        private readonly HashSet<Pondus> _otherIncome = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();
        private readonly DateTime _startDate;
        private readonly IncomeOptions _options;

        #endregion

        #region ctors

        public NorthAmericanIncome(NorthAmerican american, IncomeOptions options = null, DateTime? startDate = null) : base(
            american, options?.IsRenting ?? false)
        {
            _startDate = startDate ?? GetYearNeg3();
            _options = options ?? new IncomeOptions();
        }

        #endregion

        #region inner types

        /// <summary>
        /// A ctor-time options object for a <see cref="NorthAmericanIncome"/>
        /// which allows for some control over the randomness
        /// </summary>
        public class IncomeOptions
        {
            public bool HasVehicle { get; set; }
            public bool IsVehiclePaidOff { get; set; }
            public bool IsRenting { get; set; }

            public bool HasChildren => ChildrenAges != null && ChildrenAges.Any();

            public int[] ChildrenAges { get; set; }

            /// <summary>
            /// Optional, allows for direct control over the 
            /// expenses of the given names instead of randomness
            /// </summary>
            public Dictionary<IVoca, Pecuniam> DirectExpenseAssignments { get; set; }
        }

        #endregion

        #region properties
        /// <summary>
        /// Current employment - is array since can hold more than one job at a time.
        /// </summary>
        public virtual IEmployment[] CurrentEmployment
        {
            get
            {
                var e = Employment.Where(x => x.Terminus == null).ToList();
                e.Sort(Comparer);
                return e.ToArray();
            }
        }

        public virtual Pondus[] CurrentExpectedOtherIncome => GetCurrent(ExpectedOtherIncome);

        public virtual Pondus[] CurrentExpectedExpenses => GetCurrent(ExpectedExpenses);

        public virtual Pecuniam TotalAnnualExpectedExpenses => Pondus.GetAnnualSum(CurrentExpectedExpenses);

        public virtual Pecuniam TotalAnnualExpectedIncome =>
            Pondus.GetAnnualSum(CurrentExpectedOtherIncome) + TotalAnnualExpectedNetEmploymentIncome;

        public virtual Pecuniam TotalAnnualExpectedNetEmploymentIncome =>
            CurrentEmployment.Select(e => e.TotalAnnualNetPay).GetSum();

        public virtual Pecuniam TotalAnnualExpectedGrossEmploymentIncome =>
            CurrentEmployment.Select(e => e.TotalAnnualPay).GetSum();

        protected internal virtual List<IEmployment> Employment
        {
            get
            {
                var e = _employment.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        protected internal virtual List<Pondus> ExpectedOtherIncome
        {
            get
            {
                var o = _otherIncome.ToList();
                o.Sort(Comparer);
                return o.ToList();
            }
        }

        protected internal virtual List<Pondus> ExpectedExpenses
        {
            get
            {
                var e = _expenses.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        #endregion

        #region methods
        public virtual IEmployment[] GetEmploymentAt(DateTime? dt)
        {
            return dt == null
                ? new[] {Employment.LastOrDefault()}
                : Employment.Where(x => x.IsInRange(dt.Value)).ToArray();
        }

        public virtual Pondus[] GetExpectedOtherIncomeAt(DateTime? dt)
        {
            return GetAt(dt, ExpectedOtherIncome);
        }

        public virtual Pondus[] GetExpectedExpensesAt(DateTime? dt)
        {
            return GetAt(dt, ExpectedExpenses);
        }

        protected internal virtual void AddEmployment(IEmployment employment)
        {
            if (employment != null)
                _employment.Add(employment);
        }

        protected internal virtual void AddExpectedOtherIncome(Pondus otherIncome)
        {
            if (otherIncome != null)
                _otherIncome.Add(otherIncome);
        }

        protected internal virtual void AddExpectedOtherIncome(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddExpectedOtherIncome(new Pondus(name)
            {
                Value = amt?.Neg,
                Terminus = endDate,
                Inception = startDate
            });
        }

        protected internal virtual void AddExpectedExpense(Pondus expense)
        {
            if (expense == null)
                return;
            _expenses.Add(expense);
        }

        protected internal virtual void AddExpectedExpense(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            AddExpectedExpense(new Pondus(name)
            {
                Value = amt?.Neg,
                Terminus = endDate,
                Inception = startDate
            });
        }

        /// <summary>
        /// Resolves all expected income and expense items for this instance&apos;s year(s)
        /// </summary>
        protected internal void ResolveExpectedIncomeAndExpenses()
        {
            var emply = GetRandomEmployment(Person?.Personality, Person?.Education?.EduFlag ?? OccidentalEdu.None);
            foreach (var emp in emply)
            {
                AddEmployment(emp);
            }

            foreach (var dtRange in GetIncomeYearsInDates(_startDate))
            {
                var stDt = dtRange.Item1;
                var endDt = dtRange.Item2;

                var otIncome = GetRandomExpectedIncomeAmount(stDt, Person?.GetAgeAt(stDt));
                var payIncome = GetExpectedAnnualEmplyGrossIncome(stDt);

                var totalIncome = otIncome + payIncome;

                var otPondus = GetOtherIncomeItemsForRange(otIncome, stDt, endDt);
                var expenses = GetExpenseItemsForRange(totalIncome, stDt, endDt, Interval.Annually,
                    _options?.DirectExpenseAssignments);

                foreach(var otPon in otPondus)
                    AddExpectedOtherIncome(otPon);
                foreach(var expense in expenses)
                    AddExpectedExpense(expense);

            }
        }

        /// <summary>
        /// Gets the minimum date amoung all income, employment and expense items
        /// </summary>
        /// <returns></returns>
        protected internal virtual DateTime GetMinDateAmongExpectations()
        {
            var sdt = Etx.Date(-3, null, true, 60).Date;
            var minOtherIncome = ExpectedOtherIncome.FirstOrDefault()?.Inception;
            var minEmply = Employment.FirstOrDefault()?.Inception;
            var minExpense = ExpectedExpenses.FirstOrDefault()?.Inception;
            
            if (new[] { minOtherIncome, minEmply, minExpense }.All(dt => dt == null))
                return sdt;

            var mins = new[]
            {
                minOtherIncome.GetValueOrDefault(DateTime.Today),
                minEmply.GetValueOrDefault(DateTime.Today),
                minExpense.GetValueOrDefault(DateTime.Today)
            };

            return mins.Min();
        }

        /// <summary>
        /// Gets a manifold of <see cref="Pondus"/> items based on the 
        /// names from GetIncomeItemNames which are not in the Employment group 
        /// assigning random values as a portion of <see cref="amt"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetOtherIncomeItemsForRange(Pecuniam amt, DateTime? startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            startDate = startDate ?? GetMinDateAmongExpectations();
            var itemsout = new List<Pondus>();
            amt = amt ?? Pecuniam.Zero;

            //add in welfare
            itemsout.AddRange(GetPublicBenefitIncomeItemsForRange(startDate, endDate));

            var grps = new[] {"Employment", "Public Benefits"};
            var incomeItems = GetIncomeItemNames().Where(i => !grps.Contains(i.GetName(KindsOfNames.Group)));

            var incomeName2Rates = GetOtherIncomeName2RandomRates(0.999999D);
            foreach (var incomeItem in incomeItems)
            {
                var incomeRate = !incomeName2Rates.ContainsKey(incomeItem.Name)
                    ? 0D
                    : incomeName2Rates[incomeItem.Name];
                var p = new Pondus(incomeItem)
                {
                    Inception = startDate,
                    Terminus = endDate,
                    Value = CalcValue(amt, incomeRate),
                    Interval = interval
                };
                p.UpsertName(KindsOfNames.Group, incomeItem.GetName(KindsOfNames.Group));
                itemsout.Add(p);
            }

            return itemsout.ToArray();
        }

        /// <summary>
        /// Gets a manifold of <see cref="Pondus"/> items based on the 
        /// names from GetExpenseItemNames assigning random values as a portion of <see cref="amt"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <param name="explicitAmounts">
        /// Allows calling assembly direct control over the created rates.
        /// </param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetExpenseItemsForRange(Pecuniam amt, DateTime? startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually, 
            Dictionary<IVoca, Pecuniam> explicitAmounts = null)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            startDate = startDate ?? GetMinDateAmongExpectations();
            var itemsout = new List<Pondus>();
            amt = amt ?? Pecuniam.Zero;
            explicitAmounts = explicitAmounts ?? new Dictionary<IVoca, Pecuniam>();
            var expenseGrps = GetExpenseItemNames().Select(x => x.GetName(KindsOfNames.Group)).Distinct().ToList();

            var expenseGrpCount = _options.HasChildren ? expenseGrps.Count : expenseGrps.Count - 1;

            //move this value down more to "flatten" the diminishing rate, 
            var portions =
                Etx.DiminishingPortions(expenseGrpCount, -1.3);

            var name2Op = new Dictionary<string, Func<double, double, Dictionary<string, double>, Dictionary<string, double>>>
            {
                {EXPENSE_GRP_HOME, GetHomeExpenseNames2RandomRates},
                {EXPENSE_GRP_UTILITIES, GetUtilityExpenseNames2RandomRates},
                {EXPENSE_GRP_TRANSPORTATION, GetTransportationExpenseNames2RandomRates},
                {EXPENSE_GRP_INSURANCE, GetInsuranceExpenseNames2RandomRates},
                {EXPENSE_GRP_PERSONAL, GetPersonalExpenseNames2RandomRates},
                {EXPENSE_GRP_CHILDREN, GetChildrenExpenseNames2RandomRates},
                {EXPENSE_GRP_DEBT, GetDebtExpenseNames2RandomRates},
                {EXPENSE_GRP_HEALTH, GetHealthExpenseNames2RandomRates}
            };

            var name2Slope = new Dictionary<string, double>
            {
                //concentrate portion on rent\mortgage payment
                {EXPENSE_GRP_HOME, -0.2D},

                //concentrate portion on grociers
                {EXPENSE_GRP_PERSONAL, -0.4D},

                //flatten utilities across the board
                {EXPENSE_GRP_UTILITIES, -3.0D}
            };

            if (_options.HasVehicle && !_options.IsVehiclePaidOff)
            {
                //concentrate portion on car payment
                name2Slope.Add(EXPENSE_GRP_TRANSPORTATION, -0.25D);
            }

            var count = 0;
            foreach (var grp in expenseGrps)
            {
                if (grp == EXPENSE_GRP_CHILDREN && !_options.HasChildren)
                    continue;

                var portion = portions[count];

                var derivativeSlope = name2Slope.ContainsKey(grp) ? name2Slope[grp] : -1.0D;

                //integrate any direct amount assignments as a portion of total amount
                var directAssignRates = new Dictionary<string, double>();
                if (explicitAmounts.Any(x => string.Equals(x.Key.GetName(KindsOfNames.Group), grp, OPT)))
                {
                    foreach (var n in explicitAmounts.Where(x =>
                        string.Equals(x.Key.GetName(KindsOfNames.Group), grp, OPT)))
                    {
                        var directAssignRate = Math.Round(n.Value.ToDouble() / amt.ToDouble(), 8);
                        directAssignRates.Add(n.Key.GetName(KindsOfNames.Legal), directAssignRate);
                    }
                }

                //use explicit def if present - otherwise default to just diminishing with no zero-outs
                var grpRates = name2Op.ContainsKey(grp)
                    ? name2Op[grp](portion, derivativeSlope, directAssignRates)
                    : GetNames2RandomRates(DomusOpesDivisions.Expense, grp, portion, null);

                foreach (var item in grpRates.Keys)
                {
                    var p = new Pondus(item)
                    {
                        Inception = startDate,
                        Terminus = endDate,
                        Value = CalcValue(amt, grpRates[item]),
                        Interval = interval
                    };
                    p.UpsertName(KindsOfNames.Group, grp);
                    itemsout.Add(p);
                }

                count += 1;
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Produces a dictionary of other income items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates">
        /// A positive number, typically either very small or zero. Negative values are ignored are converted to positive.
        /// </param>
        /// <param name="randRateFunc">
        /// Optional, allows the calling assembly to specify how to generate random rates, the default is to pick a 
        /// random value between 0.01 and the <see cref="sumOfRates"/> 
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetOtherIncomeName2RandomRates(double sumOfRates = 0,
            Func<double> randRateFunc = null)
        {
            //get all the names of income items which are not employment nor welfare
            var excludeGrps = new[] {"Employment", "Public Benefits", "Judgments", "Subito"};
            var otherIncomeItemNames =
                GetIncomeItemNames().Where(i => !excludeGrps.Contains(i.GetName(KindsOfNames.Group)));
            var d = GetNames2RandomRates(otherIncomeItemNames, sumOfRates, randRateFunc);

            //add these back in but always at zero
            var otherGroups = new[] {"Judgments", "Subito"};
            otherIncomeItemNames = GetIncomeItemNames().Where(i => otherGroups.Contains(i.GetName(KindsOfNames.Group)));
            foreach (var otName in otherIncomeItemNames)
                d.Add(otName.Name, 0D);

            return d;
        }

        /// <summary>
        /// Produces a dictionary of Transportation expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetHomeExpenseNames2RandomRates(
            double sumOfRates = 0.333, double derivativeSlope = -0.2D,
            Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_HOME, sumOfRates,
                null, derivativeSlope, "Association Fees");

            var zeroOuts = new List<string> {"Other Lein"};
            if (!IsRenting)
            {
                zeroOuts.Add("Rent");
            }
            else
            {
                zeroOuts.Add("Mortgage");
                zeroOuts.Add("Maintenance");
                zeroOuts.Add("Property Tax");
                zeroOuts.Add("Association Fees");
            }

            dk = ZeroOutRates(dk, zeroOuts.ToArray(), derivativeSlope);

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Utilities expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates">
        /// </param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetUtilityExpenseNames2RandomRates(
            double sumOfRates = 0.1, double derivativeSlope = -1.0D,
            Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = IsRenting
                ? GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_UTILITIES, sumOfRates, null, derivativeSlope,
                    "Gas", "Water", "Sewer", "Trash")
                : GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_UTILITIES, sumOfRates, null, derivativeSlope);

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Transportation expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetTransportationExpenseNames2RandomRates(
            double sumOfRates = 0.125, double derivativeSlope = -1.0D,
            Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_TRANSPORTATION, sumOfRates,
                null, derivativeSlope, "Parking", "Registration Fees");

            var zeroOuts = new List<string>();
            if (_options.HasVehicle)
            {
                if (_options.IsVehiclePaidOff)
                    zeroOuts.Add("Loan Payments");
                zeroOuts.Add("Public Transportation");
            }
            else
            {
                zeroOuts.Add("Loan Payments");
                zeroOuts.Add("Fuel");
                zeroOuts.Add("Maintenance");
                zeroOuts.Add("Property Tax");
                zeroOuts.Add("Parking");
            }

            dk = ZeroOutRates(dk, zeroOuts.ToArray());

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Health expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetInsuranceExpenseNames2RandomRates(
            double sumOfRates = 0.05, double derivativeSlope = -1.0D,
            Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_INSURANCE, sumOfRates, null,
                derivativeSlope, "Pet", "Vision",
                "Dental", "Health", "Disability", "Life");

            var zeroOuts = new List<string> {IsRenting ? "Home" : "Renters"};

            if (!_options.HasVehicle)
                zeroOuts.Add("Vehicle");

            dk = ZeroOutRates(dk, zeroOuts.ToArray());

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Personal expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetPersonalExpenseNames2RandomRates(
            double sumOfRates = 0.333, double derivativeSlope = -1.0D,
            Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_PERSONAL, sumOfRates, null,
                derivativeSlope, "Dues", "Subscriptions",
                "Gifts", "Vice", "Clothing");

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Transportation expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetChildrenExpenseNames2RandomRates(
            double sumOfRates = 0.125, double derivativeSlope = -1.0D,
            Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            if (!_options.HasChildren)
            {
                var d = new Dictionary<string, double>();
                foreach (var name in GetExpenseItemNames().Where(e =>
                    string.Equals(e.GetName(KindsOfNames.Group), "Children", StringComparison.OrdinalIgnoreCase)))
                {
                    d.Add(name.Name, 0D);
                }
                return d;
            }

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_CHILDREN, sumOfRates, null,
                derivativeSlope, "Lunch Money",
                "Extracurricular", "Camp", "Transportation", "Allowance");

            var zeroOuts = new List<string> {"Education"};
            if (_options.HasChildren && _options.ChildrenAges.All(x => x < NAmerUtil.AVG_AGE_CHILD_ENTER_SCHOOL))
            {
                zeroOuts.Add("Transportation");
                zeroOuts.Add("School Supplies");
                zeroOuts.Add("Lunch Money");
                zeroOuts.Add("Extracurricular");
                zeroOuts.Add("Camp");
                zeroOuts.Add("Allowance");
            }

            dk = ZeroOutRates(dk, zeroOuts.ToArray());

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Debts expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetDebtExpenseNames2RandomRates(double sumOfRates = 0.078,
            double derivativeSlope = -1.0D, Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_DEBT, sumOfRates, null,
                derivativeSlope, "Health Care",
                "Other Consumer", "Student", "Tax", "Other");

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Health expense items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope"></param>
        /// <param name="directAssignNames2Rates">
        /// Allows for direct control over the generated rate.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetHealthExpenseNames2RandomRates(
            double sumOfRates = 0.03, double derivativeSlope = -1.0D,
            Dictionary<string, double> directAssignNames2Rates = null)
        {
            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, EXPENSE_GRP_HEALTH, sumOfRates, null,
                derivativeSlope, "Therapy", "Hospital",
                "Optical", "Dental", "Physician", "Supplements");

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Composes the items for Public Benefits (a.k.a. welfare) for whenever the current person
        /// has an income below the federal poverty level at time <see cref="startDate"/>
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetPublicBenefitIncomeItemsForRange(DateTime? startDate,
            DateTime? endDate = null)
        {
            var itemsout = new List<Pondus>();
            startDate = startDate ?? GetMinDateAmongExpectations();
            var isPoor = IsBelowFedPovertyAt(startDate);
            var hudAmt = isPoor ? GetHudMonthlyAmount(startDate) : Pecuniam.Zero;
            var snapAmt = isPoor ? GetFoodStampsMonthlyAmount(startDate) : Pecuniam.Zero;

            var incomeItems = GetIncomeItemNames().Where(i => i.GetName(KindsOfNames.Group) == "Public Benefits");
            foreach (var incomeItem in incomeItems)
            {
                var p = new Pondus(incomeItem)
                {
                    Inception = startDate,
                    Terminus = endDate,
                    Interval = Interval.Monthly
                };

                switch (incomeItem.Name)
                {
                    case "Supplemental Nutrition Assistance Program":
                        p.Value = snapAmt;
                        break;
                    case "Housing Choice Voucher Program Section 8":
                        p.Value = hudAmt;
                        break;
                    //TODO implement the other welfare programs
                    default:
                        p.Value = Pecuniam.Zero;
                        break;
                }

                itemsout.Add(p);
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Src https://www.hud.gov/sites/documents/DOC_11750.PDF
        /// </summary>
        /// <returns></returns>
        protected internal virtual Pecuniam GetHudMonthlyAmount(DateTime? atTime)
        {
            var thirtyPercentAdjIncome = GetExpectedAnnualEmplyNetIncome(atTime).ToDouble() / 12 * 0.3;
            var tenPercentGrossIncome =  GetExpectedAnnualEmplyGrossIncome(atTime).ToDouble() / 12 * 0.1;

            var sum = thirtyPercentAdjIncome + tenPercentGrossIncome + 25.0D;
            return sum.ToPecuniam();
        }

        /// <summary>
        /// Src https://www.fns.usda.gov/snap/how-much-could-i-receive
        /// </summary>
        /// <returns></returns>
        protected internal virtual Pecuniam GetFoodStampsMonthlyAmount(DateTime? atTime)
        {
            var thirtyPercentAdjIncome = GetExpectedAnnualEmplyNetIncome(atTime).ToDouble() / 12 * 0.3;
            return thirtyPercentAdjIncome.ToPecuniam();
        }

        /// <summary>
        /// Determines if the given annual gross income at time <see cref="dt"/> is below 
        /// the federal poverty level for that year
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="numHouseholdMembers"></param>
        /// <returns></returns>
        protected internal virtual bool IsBelowFedPovertyAt(DateTime? dt, int numHouseholdMembers = 1)
        {
            var povertyLevel = NAmerUtil.Equations.GetFederalPovertyLevel(dt);

            var payAtDt = GetEmploymentAt(dt);
            if(payAtDt == null || !payAtDt.Any())
                return false;

            numHouseholdMembers = numHouseholdMembers <= 0 ? 1 : numHouseholdMembers;

            return GetExpectedAnnualEmplyGrossIncome(dt).ToDouble() <= povertyLevel.SolveForY(numHouseholdMembers);
        }

        /// <summary>
        /// Gets a list of time ranges over for all the years of this income&apos;s start date (and then some). 
        /// Each block is assumed as a span of employment
        /// </summary>
        /// <param name="personality"></param>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetEmploymentRanges(IPersonality personality)
        {
            var emply = new List<Tuple<DateTime, DateTime?>>();
            //income always starts an even -3 years from the start of this year - employment almost never follows this
            var sdt = _startDate;
            sdt = sdt == DateTime.MinValue
                ? Etx.Date(-4, DateTime.Today, true).Date
                : sdt.AddDays(Etx.IntNumber(0, 360) * -1);

            if (personality == null)
            {
                emply.Add(new Tuple<DateTime, DateTime?>(sdt, null));
                return emply;
            }

            var lpDt = sdt;
            while (lpDt < DateTime.Today)
            {
                var randDays = Etx.IntNumber(0, 21);
                lpDt = lpDt.AddMonths(3).AddDays(randDays);
                if (personality.GetRandomActsSpontaneous())
                {
                    emply.Add(new Tuple<DateTime, DateTime?>(sdt, lpDt < DateTime.Today ? new DateTime?(lpDt) : null));
                    sdt = lpDt;
                }
            }
            if(!emply.Any())
                emply.Add(new Tuple<DateTime, DateTime?>(Etx.Date(-4, DateTime.Today, true).Date, null));
            return emply;
        }

        /// <summary>
        /// Get an ordered list of employment for the last three years at random
        /// </summary>
        /// <param name="personality"></param>
        /// <param name="eduLevel"></param>
        /// <param name="emplyRanges">
        /// Optional, allows calling assembly to set this directly, defaults to <see cref="GetEmploymentRanges"/>
        /// </param>
        /// <returns></returns>
        protected internal virtual List<IEmployment> GetRandomEmployment(IPersonality personality,
            OccidentalEdu eduLevel = OccidentalEdu.None, List<Tuple<DateTime, DateTime?>> emplyRanges = null)
        {
            var empls = new HashSet<IEmployment>();
            emplyRanges = emplyRanges ?? GetEmploymentRanges(personality);
            var occ = StandardOccupationalClassification.RandomOccupation();

            //limit result to those which match the edu level
            Predicate<SocDetailedOccupation> filter = s => true;
            if (eduLevel < (OccidentalEdu.Bachelor | OccidentalEdu.Grad))
                filter = s => !StandardOccupationalClassification.IsDegreeRequired(s);

            foreach (var range in emplyRanges)
            {
                var emply = new NorthAmericanEmployment(range.Item1, range.Item2, Person) {Occupation = occ};
                if (personality?.GetRandomActsSpontaneous() ?? false)
                {
                    occ = StandardOccupationalClassification.RandomOccupation(filter);
                }
                empls.Add(emply);
            }

            var e = empls.ToList();
            e.Sort(Comparer);
            return e;
        }

        /// <summary>
        /// Gets a money amount at random based on the age and either the 
        /// employment history or location of the <see cref="NorthAmerican"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="age">
        /// Optional, allows for some control over the randomness, it rises 
        /// quickly after age 50
        /// </param>
        /// <returns></returns>
        protected internal virtual Pecuniam GetRandomExpectedIncomeAmount(DateTime? dt, int? age = null)
        {
            dt = dt ?? DateTime.Today;

            age = age ?? Person?.GetAgeAt(dt);

            var ageAtDt = age == null || age <= 0 ? NAmerUtil.AVG_AGE_AMERICAN : age.Value;

            //get something randome near this value
            var randRate = GetRandomRateFromClassicHook(ageAtDt);

            //its income so it shouldn't be negative by definition
            if(randRate <= 0D)
                return Pecuniam.Zero;

            //get some base to calc the product 
            var someBase = Employment.Any() 
                           ? GetExpectedAnnualEmplyGrossIncome(dt) 
                           : GetRandomYearlyIncome(dt);

            var randAmt = someBase.ToDouble() * randRate;
            return randAmt.ToPecuniam();
        }

        private Pecuniam GetExpectedAnnualEmplyGrossIncome(DateTime? dt)
        {
            var payAtDt = GetEmploymentAt(dt);
            if (payAtDt == null || !payAtDt.Any())
                return Pecuniam.Zero;

            var sum = Pecuniam.Zero;
            foreach (var emp in payAtDt)
            {
                var payAt = emp.GetPayAt(dt);
                var f = Pondus.GetAnnualSum(payAt);
                sum += f;
            }

            return sum;
        }

        private Pecuniam GetExpectedAnnualEmplyNetIncome(DateTime? dt)
        {
            var payAtDt = GetEmploymentAt(dt);
            if (payAtDt == null || !payAtDt.Any())
                return Pecuniam.Zero;

            var sum = Pecuniam.Zero;
            foreach (var emp in payAtDt)
            {
                var pay = Pondus.GetAnnualSum(emp.GetPayAt(dt));
                var ded = Pondus.GetAnnualSum(emp.GetDeductionsAt(dt));
                sum += pay - ded;
            }

            return sum;
        }

        #endregion
    }

}
