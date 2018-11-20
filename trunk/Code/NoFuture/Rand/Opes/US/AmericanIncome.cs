using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Org;
using NoFuture.Rand.Pneuma;
using NoFuture.Rand.Sp;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Opes.US
{
    /// <inheritdoc cref="IReditus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// Represents the income (both employment and other) of a NorthAmerican over some span of time.
    /// </summary>
    [Serializable]
    public class AmericanIncome : WealthBase, IReditus
    {
        #region fields
        private readonly HashSet<ILaboris> _employment = new HashSet<ILaboris>();
        private readonly HashSet<NamedReceivable> _otherIncome = new HashSet<NamedReceivable>();
        #endregion

        #region properties
        /// <summary>
        /// Current employment - is array since can hold more than one job at a time.
        /// </summary>
        public virtual ILaboris[] CurrentEmployment
        {
            get
            {
                var e = Employment.Where(x => x.Terminus == null).ToList();
                e.Sort(Comparer);
                return e.ToArray();
            }
        }

        public virtual NamedReceivable[] CurrentOtherIncome => GetCurrent(MyItems);

        public override Pecuniam Total => CurrentItems.Sum() + TotalAnnualNetEmploymentIncome;

        public virtual Pecuniam TotalAnnualNetEmploymentIncome =>
            CurrentEmployment.Select(e => e.TotalAnnualNetPay).GetSum();

        public virtual Pecuniam TotalAnnualGrossEmploymentIncome =>
            CurrentEmployment.Select(e => e.Total).GetSum();

        protected internal virtual List<ILaboris> Employment
        {
            get
            {
                var e = _employment.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        protected internal override List<NamedReceivable> MyItems
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

        /// <summary>
        /// Get income at random
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanIncome RandomIncome(OpesOptions options = null)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var income = new AmericanIncome();
            income.RandomizeAllItems(options);
            return income;
        }

        public virtual ILaboris[] GetEmploymentAt(DateTime? dt)
        {
            return dt == null
                ? new[] {Employment.LastOrDefault()}
                : Employment.Where(x => x.IsInRange(dt.Value)).ToArray();
        }

        public virtual void AddEmployment(ILaboris employment)
        {
            if (employment != null)
                _employment.Add(employment);
        }

        public override void AddItem(NamedReceivable otherIncome)
        {
            if (otherIncome != null)
                _otherIncome.Add(otherIncome);
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x?.Replace(",", "").Replace(" ", ""), txtCase);
            var itemData = new Dictionary<string, object>();

            var jobs = CurrentEmployment;
            foreach (var job in jobs)
            {
                AddOrReplace(itemData, job.ToData(txtCase));
            }

            var coi = CurrentItems;
            foreach (var p in coi)
            {
                var v = p.Value;
                if (v == Pecuniam.Zero)
                    continue;

                var expenseName = Division.ToString() + p.DueFrequency.ToInterval();
                expenseName += p.Name;
                if(itemData.ContainsKey(textFormat(expenseName)))
                    continue;
                itemData.Add(textFormat(expenseName), v.ToString());
            }

            return itemData;
        }

        protected override Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<OpesOptions, Dictionary<string, double>>>
            {
                {IncomeGroupNames.JUDGMENTS, GetJudgmentIncomeNames2RandomRates},
                {IncomeGroupNames.SUBITO, GetSubitoIncomeNames2RandomRates},
                {IncomeGroupNames.REAL_PROPERTY,GetRealPropertyIncomeNames2RandomRates},
                {IncomeGroupNames.SECURITIES, GetSecuritiesIncomeNames2RandomRates},
                {IncomeGroupNames.INSTITUTIONAL, GetInstitutionalIncomeNames2RandomRates},
            };
        }

        protected internal override void RandomizeAllItems(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            var personality = options.Personality;
            var eduFlag = options.FactorOptions.EducationLevel;

            var emply = GetRandomEmployment(options, personality, eduFlag);
            foreach (var emp in emply)
            {
                AddEmployment(emp);
            }

            var minDate = options.Inception == DateTime.MinValue ? GetYearNeg(-1) : options.Inception;
            var ranges = GetYearsInDates(minDate);

            foreach (var range in ranges)
            {
                var cloneOptions = options.GetClone();
                cloneOptions.DueFrequency = Constants.TropicalYear;
                cloneOptions.Inception = range.Item1;
                cloneOptions.Terminus = range.Item2;
                if(cloneOptions.SumTotal == null || cloneOptions.SumTotal == Pecuniam.Zero)
                    cloneOptions.SumTotal = GetRandomExpectedIncomeAmount(cloneOptions);

                //there aren't ever random but calculated off gross and net income(s)
                if(!cloneOptions.AnyGivenDirectlyOfName(IncomeGroupNames.PUBLIC_BENEFITS))
                    cloneOptions.AddGivenDirectlyZero(IncomeGroupNames.PUBLIC_BENEFITS, null);

                //make the caller assign these directly
                if(!cloneOptions.AnyGivenDirectlyOfName(IncomeGroupNames.JUDGMENTS))
                    cloneOptions.AddGivenDirectlyZero(IncomeGroupNames.JUDGMENTS, null);

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

        /// <summary>
        /// Produces the item names to rates for the Judgement related forms of Income (e.g. alimony received)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetJudgmentIncomeNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var d = GetItemNames2Portions(IncomeGroupNames.JUDGMENTS, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Sudden, one-time forms of Income (e.g. lottery winnings)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetSubitoIncomeNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            options.PossibleZeroOuts.AddRange(new[] { "Lottery Winnings", "Gambling Winnings", "Gifts" });
            var d = GetItemNames2Portions(IncomeGroupNames.SUBITO, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Income produced from Real Property (e.g. rental property)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetRealPropertyIncomeNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var d = GetItemNames2Portions(IncomeGroupNames.REAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Income produced from Securities (e.g. dividends, capital-gains, etc.)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetSecuritiesIncomeNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            options.PossibleZeroOuts.AddRange(new []{ "Derivatives" });
            var d = GetItemNames2Portions(IncomeGroupNames.SECURITIES, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Income produced from Institutional assets (e.g. interest, stipends, etc.)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetInstitutionalIncomeNames2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            options.PossibleZeroOuts.AddRange(new[]
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
        protected internal virtual NamedReceivable[] GetPublicBenefitIncomeItemsForRange(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var startDate = options.Inception;
            var endDate = options.Terminus;
            var itemsout = new List<NamedReceivable>();
            startDate = startDate == DateTime.MinValue ? GetYearNeg(-1) : startDate;
            var isPoor = IsBelowFedPovertyAt(options);

            var grossIncome = GetAnnualEmplyGrossIncome(startDate);
            var netIncome = GetAnnualEmplyNetIncome(startDate);
            var hudAmt = isPoor ? GetHudMonthlyAmount(grossIncome, netIncome) : Pecuniam.Zero;
            var snapAmt = isPoor ? GetFoodStampsMonthlyAmount(netIncome) : Pecuniam.Zero;

            var incomeItems = GetIncomeItemNames().Where(i => i.GetName(KindsOfNames.Group) == IncomeGroupNames.PUBLIC_BENEFITS);
            foreach (var incomeItem in incomeItems)
            {
                NamedReceivable p = null;

                switch (incomeItem.Name)
                {
                    case "Supplemental Nutrition Assistance Program":
                        p = NamedReceivable.RandomNamedReceivalbleWithHistoryToSum(incomeItem.Name,
                            IncomeGroupNames.PUBLIC_BENEFITS, snapAmt,
                            PecuniamExtensions.GetTropicalMonth(), startDate, endDate);
                        break;
                    case "Housing Choice Voucher Program Section 8":
                        p = NamedReceivable.RandomNamedReceivalbleWithHistoryToSum(incomeItem.Name,
                            IncomeGroupNames.PUBLIC_BENEFITS, hudAmt,
                            PecuniamExtensions.GetTropicalMonth(), startDate, endDate);
                        break;
                    //TODO implement the other welfare programs
                }
                if(p != null)
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
            return sum.ToPecuniam().GetAbs();
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
            return thirtyPercentAdjIncome.ToPecuniam().GetAbs();
        }

        /// <summary>
        /// Determines if the annual gross income is at or below the federal poverty level.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual bool IsBelowFedPovertyAt(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var dt = options.Inception;
            var numHouseholdMembers =
                1 + (options.FactorOptions.MaritialStatus == MaritialStatus.Married ? 1 : 0) + options.ChildrenDobs?.Count ?? 0;
            numHouseholdMembers = numHouseholdMembers <= 0 ? 1 : numHouseholdMembers;
            var povertyLevel = AmericanEquations.GetFederalPovertyLevel(dt);

            var payAtDt = GetEmploymentAt(dt);
            if(payAtDt == null || !payAtDt.Any())
                return false;

            numHouseholdMembers = numHouseholdMembers <= 0 ? 1 : numHouseholdMembers;

            return GetAnnualEmplyGrossIncome(dt).ToDouble() <= povertyLevel.SolveForY(numHouseholdMembers);
        }

        /// <summary>
        /// Gets a list of time ranges over for all the years of this income&apos;s start date (and then some). 
        /// Each block is assumed as a span of employment at one single employer.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="personality"></param>
        /// <returns></returns>
        protected internal virtual List<Tuple<DateTime, DateTime?>> GetEmploymentRanges(OpesOptions options,
            IPersonality personality = null)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var emply = new List<Tuple<DateTime, DateTime?>>();

            //make it appear as if the start date is randomly before options start date
            var sdt = options.Inception;
            sdt = sdt == DateTime.MinValue
                ? Etx.RandomDate(-3, DateTime.Today, true).Date
                : sdt.AddDays(Etx.RandomInteger(0, 360) * -1);

            //TODO - tie this together with personality
            var tenure = AmericanEmployment.RandomEmploymentTenure(options);
            var lpDt = sdt;
            while (lpDt < DateTime.Today)
            {
                var randDays = Etx.RandomInteger(0, 21);
                lpDt = lpDt.AddMonths(3).AddDays(randDays);

                if (lpDt <= sdt.Add(tenure))
                    continue;
                emply.Add(new Tuple<DateTime, DateTime?>(sdt, lpDt < DateTime.Today ? new DateTime?(lpDt) : null));
                sdt = lpDt;
                tenure = AmericanEmployment.RandomEmploymentTenure(options);
            }
            if (!emply.Any())
                emply.Add(new Tuple<DateTime, DateTime?>(Etx.RandomDate(-4, DateTime.Today, true).Date, null));
            return emply;
        }

        /// <summary>
        /// Get an ordered list of employment for the last three years at random
        /// </summary>
        /// <param name="options"></param>
        /// <param name="personality"></param>
        /// <param name="eduLevel"></param>
        /// <param name="emplyRanges">
        /// Optional, allows calling assembly to set this directly, defaults to <see cref="GetEmploymentRanges"/>
        /// </param>
        /// <returns></returns>
        protected internal virtual List<ILaboris> GetRandomEmployment(OpesOptions options, IPersonality personality = null,
            OccidentalEdu eduLevel = OccidentalEdu.None, List<Tuple<DateTime, DateTime?>> emplyRanges = null)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            var empls = new HashSet<ILaboris>();
            emplyRanges = emplyRanges ?? GetEmploymentRanges(options, personality);

            //limit result to those which match the edu level
            Predicate<SocDetailedOccupation> filter = null;
            if (eduLevel < (OccidentalEdu.Bachelor | OccidentalEdu.Grad))
                filter = s => !StandardOccupationalClassification.IsDegreeRequired(s);

            var occ = StandardOccupationalClassification.RandomOccupation(filter);

            foreach (var range in emplyRanges)
            {
                var emply = new AmericanEmployment() {Occupation = occ};
                var cloneOptions = options.GetClone();
                cloneOptions.Inception = range.Item1;
                cloneOptions.Terminus = range.Item2;
                emply.RandomizeAllItems(options);
                empls.Add(emply);
            }

            var e = empls.ToList();
            e.Sort(Comparer);
            return e;
        }

        /// <summary>
        /// Gets a money amount at random based on the age and either the 
        /// employment history or location of the opes options
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Pecuniam GetRandomExpectedIncomeAmount(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            var dt = options.Inception == DateTime.MinValue ? DateTime.Today : options.Inception;

            var age = options.FactorOptions.GetAge();

            var ageAtDt = age <= 0 ? AmericanData.AVG_AGE_AMERICAN : age;

            //get something randome near this value
            var randRate = GetRandomRateFromClassicHook(ageAtDt);

            //its income so it shouldn't be negative by definition
            if(randRate <= 0D)
                return Pecuniam.Zero;

            //get some base to calc the product 
            var someBase = Employment.Any() 
                           ? GetAnnualEmplyGrossIncome(dt) 
                           : GetRandomYearlyIncome(dt, options);

            var randAmt = someBase.ToDouble() * randRate;
            return randAmt.ToPecuniam();
        }

        private Pecuniam GetAnnualEmplyGrossIncome(DateTime? dt)
        {
            var payAtDt = GetEmploymentAt(dt);
            if (payAtDt == null || !payAtDt.Any())
                return Pecuniam.Zero;

            var sum = Pecuniam.Zero;
            foreach (var emp in payAtDt)
            {
                var payAt = emp.GetAt(dt);
                var f = payAt.Sum();
                sum += f;
            }

            return sum;
        }

        private Pecuniam GetAnnualEmplyNetIncome(DateTime? dt)
        {
            var payAtDt = GetEmploymentAt(dt);
            if (payAtDt == null || !payAtDt.Any())
                return Pecuniam.Zero;

            var sum = Pecuniam.Zero;
            foreach (var emp in payAtDt)
            {
                var pay = emp.GetAt(dt).Sum();
                var ded = emp.Deductions?.GetAt(dt).Sum() ?? Pecuniam.Zero;
                sum += pay - ded.GetAbs();
            }

            return sum;
        }

        #endregion
    }
}
