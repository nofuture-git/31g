using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Opes.Options;
using NoFuture.Rand.Domus.Pneuma;

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
        #region fields
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        private readonly HashSet<Pondus> _otherIncome = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();
        #endregion

        #region ctors

        public NorthAmericanIncome(NorthAmerican american, OpesOptions options = null) :
            base(
                american, options)
        {
            if(MyOptions.StartDate == DateTime.MinValue)
                MyOptions.StartDate = GetYearNeg(-3);
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

        public virtual Pecuniam TotalAnnualExpectedExpenses => Pondus.GetExpectedAnnualSum(CurrentExpectedExpenses);

        public virtual Pecuniam TotalAnnualExpectedIncome =>
            Pondus.GetExpectedAnnualSum(CurrentExpectedOtherIncome) + TotalAnnualExpectedNetEmploymentIncome;

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

        protected internal virtual void AddExpectedOtherIncome(Pecuniam amt, string name, DateTime startDate,
            DateTime? endDate = null)
        {
            var p = new Pondus(name)
            {
                Terminus = endDate,
                Inception = startDate
            };
            p.My.ExpectedValue = amt?.Neg;
            AddExpectedOtherIncome(p);
        }

        protected internal virtual void AddExpectedExpense(Pondus expense)
        {
            if (expense == null)
                return;
            _expenses.Add(expense);
        }

        protected internal virtual void AddExpectedExpense(Pecuniam amt, string name, DateTime startDate,
            DateTime? endDate = null)
        {
            var p = new Pondus(name)
            {
                Terminus = endDate,
                Inception = startDate
            };

            p.My.ExpectedValue = amt?.Neg;
            AddExpectedExpense(p);
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

            foreach (var dtRange in GetYearsInDates(MyOptions.StartDate))
            {
                var stDt = dtRange.Item1;
                var endDt = dtRange.Item2;

                var otIncome = GetRandomExpectedIncomeAmount(stDt, Person?.GetAgeAt(stDt));
                var payIncome = GetExpectedAnnualEmplyGrossIncome(stDt);

                var totalIncome = otIncome + payIncome;

                var otPondus = GetOtherIncomeItemsForRange(otIncome, stDt, endDt);
                var expenses = GetExpenseItemsForRange(totalIncome, stDt, endDt);

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
        protected internal virtual Pondus[] GetOtherIncomeItemsForRange(Pecuniam amt, DateTime startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            startDate = startDate == DateTime.MinValue ? GetMinDateAmongExpectations() : startDate;
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
                var p = new Pondus(incomeItem, interval)
                {
                    Inception = startDate,
                    Terminus = endDate,
                };
                p.My.ExpectedValue = CalcValue(amt, incomeRate);
                p.My.UpsertName(KindsOfNames.Group, incomeItem.GetName(KindsOfNames.Group));
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
        /// <returns></returns>
        protected internal virtual Pondus[] GetExpenseItemsForRange(Pecuniam amt, DateTime startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            var name2Op = new Dictionary<string, Func<RatesDictionaryArgs, Dictionary<string, double>>>
            {
                {ExpenseGroupNames.HOME, GetHomeExpenseNames2RandomRates},
                {ExpenseGroupNames.UTILITIES, GetUtilityExpenseNames2RandomRates},
                {ExpenseGroupNames.TRANSPORTATION, GetTransportationExpenseNames2RandomRates},
                {ExpenseGroupNames.INSURANCE, GetInsuranceExpenseNames2RandomRates},
                {ExpenseGroupNames.PERSONAL, GetPersonalExpenseNames2RandomRates},
                {ExpenseGroupNames.CHILDREN, GetChildrenExpenseNames2RandomRates},
                {ExpenseGroupNames.DEBT, GetDebtExpenseNames2RandomRates},
                {ExpenseGroupNames.HEALTH, GetHealthExpenseNames2RandomRates}
            };

            var name2Slope = new Dictionary<string, double>
            {
                //concentrate portion on rent\mortgage payment
                {ExpenseGroupNames.HOME, -0.2D},

                //concentrate portion on grociers
                {ExpenseGroupNames.PERSONAL, -0.4D},

                //flatten utilities across the board
                {ExpenseGroupNames.UTILITIES, -3.0D}
            };

            if (MyOptions.HasVehicle && !MyOptions.IsVehiclePaidOff)
            {
                //concentrate portion on car payment
                name2Slope.Add(ExpenseGroupNames.TRANSPORTATION, -0.25D);
            }

            
            startDate = startDate == DateTime.MinValue ? GetMinDateAmongExpectations() : startDate;
            var itemsout = new List<Pondus>();
            amt = amt ?? Pecuniam.Zero;
            var expenseGrps = GetExpenseItemNames().Select(x => x.GetName(KindsOfNames.Group)).Distinct().ToList();

            //move this value down more to "flatten" the diminishing rate, 
            var portions =
                Etx.DiminishingPortions(expenseGrps.Count, -1.3);
            var grp2Portion = expenseGrps.Zip(portions, (n, v) => new Tuple<string, double>(n, v));

            foreach (var g2p in grp2Portion)
            {
                var grp = g2p.Item1;
                var portion = g2p.Item2;

                var derivativeSlope = name2Slope.ContainsKey(grp) ? name2Slope[grp] : -1.0D;

                //need to convert the actual amount into a ratio
                var directAssignRates = ConvertToExplicitRates(amt, MyOptions.GivenDirectly, grp);

                //use explicit def if present - otherwise default to just diminishing with no zero-outs
                var grpRates = name2Op.ContainsKey(grp)
                    ? name2Op[grp](new RatesDictionaryArgs
                    {
                        SumOfRates = portion,
                        DerivativeSlope = derivativeSlope,
                        DirectAssignNames2Rates = directAssignRates
                    })
                    : GetNames2RandomRates(DomusOpesDivisions.Expense, grp, portion, null);

                foreach (var item in grpRates.Keys)
                {
                    var p = GetPondusForItemAndGroup(item, grp, startDate, endDate, interval);
                    p.My.ExpectedValue = CalcValue(amt, grpRates[item]);
                    itemsout.Add(p);
                }

            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Factory method to create a <see cref="Pondus"/> based on the given values
        /// </summary>
        /// <param name="item"></param>
        /// <param name="grp"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected Pondus GetPondusForItemAndGroup(string item, string grp, DateTime startDate, DateTime? endDate,
            Interval interval = Interval.Annually)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            var isCreditCard = string.Equals(grp, "Debts", OPT) && string.Equals(item, "Credit Card", OPT);

            var p = isCreditCard
                ? CreditCardAccount.GetRandomCcAcct(Person, CreditScore)
                : new Pondus(item, interval)
                {
                    Inception = startDate,
                    Terminus = endDate,
                };
            p.My.Name = item;
            p.My.UpsertName(KindsOfNames.Group, grp);
            return p;
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
        /// Produces a dictionary of Transportation <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetHomeExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.333D;
            var derivativeSlope = args?.DerivativeSlope ?? -0.2D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.HOME, sumOfRates,
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
        /// Produces a dictionary of Utilities <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetUtilityExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.1D;
            var derivativeSlope = args?.DerivativeSlope ?? -1.0D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = IsRenting
                ? GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.UTILITIES, sumOfRates, null,
                    derivativeSlope,
                    "Gas", "Water", "Sewer", "Trash")
                : GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.UTILITIES, sumOfRates, null,
                    derivativeSlope);

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Transportation <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetTransportationExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.125D;
            var derivativeSlope = args?.DerivativeSlope ?? -1.0D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.TRANSPORTATION, sumOfRates,
                null, derivativeSlope, "Parking", "Registration Fees");

            var zeroOuts = new List<string>();
            if (MyOptions.HasVehicle)
            {
                if (MyOptions.IsVehiclePaidOff)
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
        /// Produces a dictionary of Health <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetInsuranceExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.05D;
            var derivativeSlope = args?.DerivativeSlope ?? -1.0D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.INSURANCE, sumOfRates, null,
                derivativeSlope, "Pet", "Vision",
                "Dental", "Health", "Disability", "Life");

            var zeroOuts = new List<string> {IsRenting ? "Home" : "Renters"};

            if (!MyOptions.HasVehicle)
                zeroOuts.Add("Vehicle");

            dk = ZeroOutRates(dk, zeroOuts.ToArray());

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Personal <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        /// items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetPersonalExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.333D;
            var derivativeSlope = args?.DerivativeSlope ?? -1.0D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.PERSONAL, sumOfRates, null,
                derivativeSlope, "Dues", "Subscriptions",
                "Gifts", "Vice", "Clothing");

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Transportation <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetChildrenExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.125D;
            var derivativeSlope = args?.DerivativeSlope ?? -1.0D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            if (!MyOptions.HasChildren)
            {
                var d = new Dictionary<string, double>();
                foreach (var name in GetExpenseItemNames().Where(e =>
                    string.Equals(e.GetName(KindsOfNames.Group), ExpenseGroupNames.CHILDREN,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    d.Add(name.Name, 0D);
                }
                return d;
            }

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.CHILDREN, sumOfRates, null,
                derivativeSlope, "Lunch Money",
                "Extracurricular", "Camp", "Transportation", "Allowance");

            var zeroOuts = new List<string> {"Education"};
            if (MyOptions.HasChildren && MyOptions.ChildrenAges.All(x => x < NAmerUtil.AVG_AGE_CHILD_ENTER_SCHOOL))
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
        /// Produces a dictionary of Debts <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetDebtExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.078D;
            var derivativeSlope = args?.DerivativeSlope ?? -1.0D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.DEBT, sumOfRates, null,
                derivativeSlope, "Health Care",
                "Other Consumer", "Student", "Tax", "Other");

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Health <see cref="WealthBase.DomusOpesDivisions.Expense"/> 
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetHealthExpenseNames2RandomRates(
            RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? 0.03D;
            var derivativeSlope = args?.DerivativeSlope ?? -1.0D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Expense, ExpenseGroupNames.HEALTH, sumOfRates, null,
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
        protected internal virtual Pondus[] GetPublicBenefitIncomeItemsForRange(DateTime startDate,
            DateTime? endDate = null)
        {
            var itemsout = new List<Pondus>();
            startDate = startDate == DateTime.MinValue ? GetMinDateAmongExpectations() : startDate;
            var isPoor = IsBelowFedPovertyAt(startDate);
            var hudAmt = isPoor ? GetHudMonthlyAmount(startDate) : Pecuniam.Zero;
            var snapAmt = isPoor ? GetFoodStampsMonthlyAmount(startDate) : Pecuniam.Zero;

            var incomeItems = GetIncomeItemNames().Where(i => i.GetName(KindsOfNames.Group) == "Public Benefits");
            foreach (var incomeItem in incomeItems)
            {
                var p = new Pondus(incomeItem, Interval.Monthly)
                {
                    Inception = startDate,
                    Terminus = endDate
                };

                switch (incomeItem.Name)
                {
                    case "Supplemental Nutrition Assistance Program":
                        p.My.ExpectedValue = snapAmt;
                        break;
                    case "Housing Choice Voucher Program Section 8":
                        p.My.ExpectedValue = hudAmt;
                        break;
                    //TODO implement the other welfare programs
                    default:
                        p.My.ExpectedValue = Pecuniam.Zero;
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
            var sdt = MyOptions.StartDate;
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
                var emply = new NorthAmericanEmployment(Person, range.Item1, range.Item2) {Occupation = occ};
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
                var f = Pondus.GetExpectedAnnualSum(payAt);
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
                var pay = Pondus.GetExpectedAnnualSum(emp.GetPayAt(dt));
                var ded = Pondus.GetExpectedAnnualSum(emp.GetDeductionsAt(dt));
                sum += pay - ded;
            }

            return sum;
        }

        #endregion
    }

}
