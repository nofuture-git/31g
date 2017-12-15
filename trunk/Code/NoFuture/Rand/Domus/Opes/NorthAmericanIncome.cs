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
using NoFuture.Rand.Domus.Pneuma;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IReditus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// Represents the income (both employment and other) of a NorthAmerican over some span of time.
    /// </summary>
    [Serializable]
    public class NorthAmericanIncome : WealthBase, IReditus
    {
        #region fields
        private readonly HashSet<IEmployment> _employment = new HashSet<IEmployment>();
        private readonly HashSet<Pondus> _otherIncome = new HashSet<Pondus>();
        #endregion

        #region ctors

        public NorthAmericanIncome(NorthAmerican american, OpesOptions options = null) :
            base(
                american, options)
        {
            if(MyOptions.StartDate == DateTime.MinValue)
                MyOptions.StartDate = GetYearNeg(-1);
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

        public virtual Pondus[] CurrentExpectedOtherIncome => GetCurrent(MyItems);

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

        protected internal override List<Pondus> MyItems
        {
            get
            {
                var o = _otherIncome.ToList();
                o.Sort(Comparer);
                return o.ToList();
            }
        }

        protected override DomusOpesDivisions Division => DomusOpesDivisions.Income;

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
            return GetAt(dt, MyItems);
        }

        protected internal virtual void AddEmployment(IEmployment employment)
        {
            if (employment != null)
                _employment.Add(employment);
        }

        protected internal override void AddItem(Pondus otherIncome)
        {
            if (otherIncome != null)
                _otherIncome.Add(otherIncome);
        }

        protected override Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<OpesOptions, Dictionary<string, double>>>
            {
                {IncomeGroupNames.JUDGMENTS, GetJudgementIncomeNames2RandomRates},
                {IncomeGroupNames.SUBITO, GetSubitoIncomeNames2RandomRates},
                {IncomeGroupNames.REAL_PROPERTY,GetRealPropertyIncomeNames2RandomRates},
                {IncomeGroupNames.SECURITIES, GetSecuritiesIncomeNames2RandomRates},
                {IncomeGroupNames.INSTITUTIONAL, GetInstitutionalIncomeNames2RandomRates},
            };
        }

        protected internal override void ResolveItems(OpesOptions options)
        {
            options = options ?? MyOptions;

            var emply = GetRandomEmployment(Person?.Personality, Person?.Education?.EduFlag ?? OccidentalEdu.None);
            foreach (var emp in emply)
            {
                AddEmployment(emp);
            }

            var minDate = options.StartDate == DateTime.MinValue ? GetYearNeg(-1) : options.StartDate;
            var ranges = GetYearsInDates(minDate);

            foreach (var range in ranges)
            {
                var cloneOptions = options.GetClone();
                cloneOptions.Interval = Interval.Annually;
                cloneOptions.StartDate = range.Item1;
                cloneOptions.EndDate = range.Item2;
                if(cloneOptions.SumTotal == null || cloneOptions.SumTotal == Pecuniam.Zero)
                    cloneOptions.SumTotal = GetRandomExpectedIncomeAmount(range.Item1, Person?.GetAgeAt(range.Item1));

                //there aren't ever random but calculated off gross and net income(s)
                cloneOptions.GivenDirectly.Add(new Mereo(IncomeGroupNames.PUBLIC_BENEFITS){ExpectedValue = Pecuniam.Zero});

                var items = GetItemsForRange(cloneOptions);
                foreach (var item in items)
                {
                    AddItem(item);
                }

                var welfareItems = GetPublicBenefitIncomeItemsForRange(cloneOptions);
                if (welfareItems.Any())
                {
                    foreach (var welfareItem in welfareItems)
                    {
                        AddItem(welfareItem);
                    }
                }
            }
        }

        protected internal Dictionary<string, double> GetJudgementIncomeNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            var d = GetItemNames2Portions(IncomeGroupNames.JUDGMENTS, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetSubitoIncomeNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            options.PossiableZeroOuts.AddRange(new[] { "Lottery Winnings", "Gambling Winnings", "Gifts" });
            var d = GetItemNames2Portions(IncomeGroupNames.SUBITO, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetRealPropertyIncomeNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            var d = GetItemNames2Portions(IncomeGroupNames.REAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetSecuritiesIncomeNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            options.PossiableZeroOuts.AddRange(new []{ "Derivatives" });
            var d = GetItemNames2Portions(IncomeGroupNames.SECURITIES, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetInstitutionalIncomeNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            options.PossiableZeroOuts.AddRange(new[]
            {
                "Royalties", "Stipends", "Fellowships", "Partnerships",
                "Trusts", "Money Market", "Profit Sharing", "Annuity",
                "Certificate of Deposit"
            });

            var d = GetItemNames2Portions(IncomeGroupNames.INSTITUTIONAL, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Composes the items for Public Benefits (a.k.a. welfare) whenever 
        /// income is below the federal poverty level
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetPublicBenefitIncomeItemsForRange(OpesOptions options)
        {
            options = options ?? MyOptions;
            var startDate = options.StartDate;
            var endDate = options.EndDate;
            var itemsout = new List<Pondus>();
            startDate = startDate == DateTime.MinValue ? GetYearNeg(-1) : startDate;
            var isPoor = IsBelowFedPovertyAt(options);

            var grossIncome = GetExpectedAnnualEmplyGrossIncome(startDate);
            var netIncome = GetExpectedAnnualEmplyNetIncome(startDate);
            var hudAmt = isPoor ? GetHudMonthlyAmount(grossIncome, netIncome) : Pecuniam.Zero;
            var snapAmt = isPoor ? GetFoodStampsMonthlyAmount(netIncome) : Pecuniam.Zero;

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
        protected internal virtual Pecuniam GetHudMonthlyAmount(Pecuniam grossAnnualIncome, Pecuniam netAnnualIncome)
        {
            if(grossAnnualIncome == null || netAnnualIncome == null)
                return Pecuniam.Zero;
            var thirtyPercentAdjIncome = grossAnnualIncome.ToDouble() / 12 * 0.3;
            var tenPercentGrossIncome = netAnnualIncome.ToDouble() / 12 * 0.1;

            var sum = thirtyPercentAdjIncome + tenPercentGrossIncome + 25.0D;
            return sum.ToPecuniam().Abs;
        }

        /// <summary>
        /// Src https://www.fns.usda.gov/snap/how-much-could-i-receive
        /// </summary>
        /// <returns></returns>
        protected internal virtual Pecuniam GetFoodStampsMonthlyAmount(Pecuniam netAnnualIncome)
        {
            if(netAnnualIncome == null)
                return Pecuniam.Zero;
            var thirtyPercentAdjIncome = netAnnualIncome.ToDouble() / 12 * 0.3;
            return thirtyPercentAdjIncome.ToPecuniam().Abs;
        }

        /// <summary>
        /// Determines if the annual gross income is at or below the federal poverty level.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual bool IsBelowFedPovertyAt(OpesOptions options)
        {
            options = options ?? MyOptions;
            var dt = options.StartDate;
            var numHouseholdMembers = options.TotalNumberOfHouseholdMembers;
            numHouseholdMembers = numHouseholdMembers <= 0 ? 1 : numHouseholdMembers;
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
                ? Etx.Date(-3, DateTime.Today, true).Date
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
                emply.ResolveItems(null);
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
                var ded = Pondus.GetExpectedAnnualSum(emp.Deductions?.GetDeductionsAt(dt)) ?? Pecuniam.Zero;
                sum += pay - ded.Abs;
            }

            return sum;
        }

        #endregion
    }

}
