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

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IReditus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class NorthAmericanIncome : WealthBase, IReditus
    {
        #region fields
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        private readonly HashSet<Pondus> _otherIncome = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _expenses = new HashSet<Pondus>();
        private readonly DateTime _startDate;

        #endregion

        #region ctors

        public NorthAmericanIncome(NorthAmerican american, bool isRenting = false, DateTime? startDate = null) : base(
            american, isRenting)
        {
            _startDate = startDate ?? GetYearNeg3();
            //TODO - finish this
            var emply = GetRandomEmployment(american?.Personality, american?.Education?.EduFlag ?? OccidentalEdu.None);
        }

        #endregion

        #region properties
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
        public virtual Pecuniam TotalAnnualExpectedIncome => Pondus.GetAnnualSum(CurrentExpectedOtherIncome) + TotalAnnualExpectedNetEmploymentIncome;
        public virtual Pecuniam TotalAnnualExpectedNetEmploymentIncome => CurrentEmployment.Select(e => e.TotalAnnualNetPay).GetSum();
        public virtual Pecuniam TotalAnnualExpectedGrossEmploymentIncome => CurrentEmployment.Select(e => e.TotalAnnualPay).GetSum();

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

            var incomeName2Rates = GetOtherIncomeName2RandomeRates(0.999999D);
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

                itemsout.Add(p);
            }

            return itemsout.ToArray();
        }

        /// <summary>
        /// Produces a dictionary of other income items by names to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="sumOfRates">
        /// A positive number, typically either very small or zero. Negative values are ignored - place those in 
        /// Expenses.
        /// </param>
        /// <param name="randRateFunc">
        /// Optional, allows the calling assembly to specify how to generate random rates, the default is to pick a 
        /// random value between 0.01 and the <see cref="sumOfRates"/> 
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetOtherIncomeName2RandomeRates(double sumOfRates = 0,
            Func<double> randRateFunc = null)
        {
            //get all the names of income items which are not employment nor welfare
            var grps = new[] {"Employment", "Public Benefits", "Judgments", "Subito"};
            var otherIncomeItemNames = GetIncomeItemNames().Where(i => !grps.Contains(i.GetName(KindsOfNames.Group)));
            var d = GetNames2RandomRates(otherIncomeItemNames, sumOfRates, randRateFunc);

            //add these back in but always at zero
            var otherGroups = new[] {"Judgments", "Subito"};
            otherIncomeItemNames = GetIncomeItemNames().Where(i => otherGroups.Contains(i.GetName(KindsOfNames.Group)));
            foreach (var otName in otherIncomeItemNames)
                d.Add(otName.Name, 0D);

            return d;
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

            return payAtDt.Select(e => e.TotalAnnualPay).GetSum();
        }

        private Pecuniam GetExpectedAnnualEmplyNetIncome(DateTime? dt)
        {
            var payAtDt = GetEmploymentAt(dt);
            if (payAtDt == null || !payAtDt.Any())
                return Pecuniam.Zero;

            return payAtDt.Select(e => e.TotalAnnualNetPay).GetSum();
        }

        #endregion
    }

}
