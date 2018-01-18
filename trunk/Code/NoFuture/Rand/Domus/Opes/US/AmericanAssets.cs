using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US.Fed;
using NoFuture.Rand.Gov.US.Nhtsa;

namespace NoFuture.Rand.Domus.Opes.US
{
    /// <inheritdoc cref="IRebus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// Represents the assets of a North American over some span of time
    /// </summary>
    [Serializable]
    public class AmericanAssets : WealthBase, IRebus
    {
        #region fields

        internal const string REAL_PROPERTY_HOME_OWNERSHIP = "Home Ownership";
        internal const string PERSONAL_PROPERTY_MOTOR_VEHICLES = "Motor Vehicles";
        internal const string INSTITUTIONAL_CHECKING = "Checking";
        internal const string INSTITUTIONAL_SAVINGS = "Savings";

        private readonly HashSet<Pondus> _assets = new HashSet<Pondus>();

        private readonly double _randCheckingAcctAmt;
        private readonly double _randSavingsAcctAmt;
        private readonly double _randHomeEquity;
        private readonly double _randCarEquity;

        private readonly double _randHomeDebt;
        private readonly double _randCarDebt;

        private readonly double _totalEquity;

        private readonly double _checkingAccountRate;
        private readonly double _savingsAccountRate;
        private readonly double _homeEquityRate;
        private readonly double _carEquityRate;
        private readonly double _homeDebtRate;
        private readonly double _carDebtRate;

        #endregion

        #region ctors

        public AmericanAssets(OpesOptions options, double stdDev = DF_STD_DEV_PERCENT) : base(options)
        {
            if (MyOptions.Inception == DateTime.MinValue)
                MyOptions.Inception = GetYearNeg(-1);

            _randCheckingAcctAmt = AmericanFactors.GetRandomFactorValue(FactorTables.CheckingAccount,
                Factors.CheckingAcctFactor, stdDev);
            _randSavingsAcctAmt = AmericanFactors.GetRandomFactorValue(FactorTables.SavingsAccount,
                Factors.SavingsAcctFactor, stdDev);

            _randHomeEquity = AmericanFactors.GetRandomFactorValue(FactorTables.HomeEquity,
                Factors.HomeEquityFactor,
                DF_STD_DEV_PERCENT);

            _randCarEquity = AmericanFactors.GetRandomFactorValue(FactorTables.VehicleEquity,
                Factors.VehicleEquityFactor, stdDev);

            var randCcDebt = AmericanFactors.GetRandomFactorValue(FactorTables.CreditCardDebt,
                Factors.CreditCardDebtFactor,
                DF_STD_DEV_PERCENT);

            _randHomeDebt = AmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, Factors.HomeDebtFactor,
                DF_STD_DEV_PERCENT);

            _randCarDebt = AmericanFactors.GetRandomFactorValue(FactorTables.VehicleDebt,
                Factors.VehicleDebtFactor, stdDev);

            _totalEquity = _randCheckingAcctAmt + _randSavingsAcctAmt + _randHomeEquity + _randCarEquity;

            var randNetWorth = AmericanFactors.GetRandomFactorValue(FactorTables.NetWorth,
                Factors.NetWorthFactor, stdDev);

            var allOtherAssetEquity = _totalEquity - randNetWorth;

            _totalEquity += allOtherAssetEquity;
            if (_totalEquity == 0.0D)
                _totalEquity = 1.0D;
            _homeEquityRate = Math.Round(_randHomeEquity / _totalEquity, DF_ROUND_DECIMAL_PLACES);
            _carEquityRate = Math.Round(_randCarEquity / _totalEquity, DF_ROUND_DECIMAL_PLACES);
            _checkingAccountRate = Math.Round(_randCheckingAcctAmt / _totalEquity, DF_ROUND_DECIMAL_PLACES);
            _savingsAccountRate = Math.Round(_randSavingsAcctAmt / _totalEquity, DF_ROUND_DECIMAL_PLACES);

            var totalDebt = _randHomeDebt + _randCarDebt + randCcDebt;
            _homeDebtRate = Math.Round(_randHomeDebt / totalDebt, DF_ROUND_DECIMAL_PLACES);
            _carDebtRate = Math.Round(_randCarDebt / totalDebt, DF_ROUND_DECIMAL_PLACES);
            
        }

        #endregion

        #region properties

        public Pondus[] CurrentAssets => GetCurrent(MyItems);

        public Pecuniam TotalCurrentExpectedValue => Pondus.GetExpectedSum(CurrentAssets);

        public IMereo HousePayment { get; private set; }

        public IMereo CarPayment { get; private set; }

        protected internal override List<Pondus> MyItems
        {
            get
            {
                var e = _assets.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        protected override DomusOpesDivisions Division => DomusOpesDivisions.Assets;

        #endregion

        #region methods

        /// <summary>
        /// Gets assets at random
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanAssets RandomAssets(OpesOptions options = null)
        {
            var assets = new AmericanAssets(options);
            assets.ResolveItems(options);
            return assets;
        }

        public Pondus[] GetAssetsAt(DateTime? dt)
        {
            return GetAt(dt, MyItems);
        }

        protected internal override void AddItem(Pondus item)
        {
            if (item == null)
                return;

            _assets.Add(item);
        }

        protected override Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<OpesOptions, Dictionary<string, double>>>
            {
                {AssetGroupNames.REAL_PROPERTY, GetRealPropertyName2RandomRates},
                {AssetGroupNames.PERSONAL_PROPERTY, GetPersonalPropertyAssetNames2Rates},
                {AssetGroupNames.INSTITUTIONAL, GetInstitutionalAssetName2Rates},
                {AssetGroupNames.SECURITIES, GetSecuritiesAssetNames2RandomRates}
            };
        }

        protected internal override void ResolveItems(OpesOptions options = null)
        {
            options = options ?? MyOptions;
            var stDt = options.Inception == DateTime.MinValue ? GetYearNeg(-1) : options.Inception;
            var ranges = GetYearsInDates(stDt);

            foreach (var range in ranges)
            {
                var cloneOptions = options.GetClone();
                cloneOptions.Inception = range.Item1;
                cloneOptions.Terminus = range.Item2;
                cloneOptions.Interval = Interval.Annually;

                var items = GetItemsForRange(cloneOptions);
                foreach(var item in items)
                    AddItem(item);
            }
        }

        public override List<Tuple<string, double>> GetGroupNames2Portions(OpesOptions options)
        {
            options = options ?? MyOptions;

            var amt = options.SumTotal == null || options.SumTotal == Pecuniam.Zero
                ? _totalEquity.ToPecuniam()
                : options.SumTotal;

            var givenDirectly = new List<IMereo>();
            var assignedRealEstateDirectly =
                options.AnyGivenDirectlyOfNameAndGroup(REAL_PROPERTY_HOME_OWNERSHIP, AssetGroupNames.REAL_PROPERTY);

            var assignedVehicleDirectly = options.AnyGivenDirectlyOfNameAndGroup(PERSONAL_PROPERTY_MOTOR_VEHICLES,
                AssetGroupNames.PERSONAL_PROPERTY);

            var assignedCheckingDirectly =
                options.AnyGivenDirectlyOfNameAndGroup(INSTITUTIONAL_CHECKING, AssetGroupNames.INSTITUTIONAL);

            var assignedSavingDirectly =
                options.AnyGivenDirectlyOfNameAndGroup(INSTITUTIONAL_SAVINGS, AssetGroupNames.INSTITUTIONAL);

            if (!options.IsRenting && !assignedRealEstateDirectly)
            {
                givenDirectly.Add(new Mereo(REAL_PROPERTY_HOME_OWNERSHIP, AssetGroupNames.REAL_PROPERTY)
                {
                    ExpectedValue = (_homeEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }

            if (MyOptions.HasVehicles && !assignedVehicleDirectly)
            {
                givenDirectly.Add(new Mereo(PERSONAL_PROPERTY_MOTOR_VEHICLES, AssetGroupNames.PERSONAL_PROPERTY)
                {
                    ExpectedValue = (_carEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }
            if (!assignedCheckingDirectly)
            {
                givenDirectly.Add(new Mereo(INSTITUTIONAL_CHECKING, AssetGroupNames.INSTITUTIONAL)
                {
                    ExpectedValue = (_checkingAccountRate * amt.ToDouble()).ToPecuniam()
                });
            }
            if (!assignedSavingDirectly)
            {
                givenDirectly.Add(new Mereo(INSTITUTIONAL_SAVINGS, AssetGroupNames.INSTITUTIONAL)
                {
                    ExpectedValue = (_savingsAccountRate * amt.ToDouble()).ToPecuniam()
                });
            }

            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
            {
                options.DerivativeSlope = -0.2D;
                options.GivenDirectly.AddRange(givenDirectly);
                MyOptions = options;
            }
            return base.GetGroupNames2Portions(options);
        }

        protected internal override Pondus GetPondusForItemAndGroup(string item, string grp, OpesOptions options)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            const float FED_RATE = RiskFreeInterestRate.DF_VALUE;

            options = (options ?? MyOptions) ?? new OpesOptions();

            var startDate = options.Inception;
            var amt = options.SumTotal;

            var isCheckingAccount = string.Equals(grp, AssetGroupNames.INSTITUTIONAL, OPT) &&
                                    string.Equals(item, INSTITUTIONAL_CHECKING, OPT);
            var isSavingsAccount = string.Equals(grp, AssetGroupNames.INSTITUTIONAL, OPT) &&
                                   string.Equals(item, INSTITUTIONAL_SAVINGS, OPT);
            var isMortgage = string.Equals(grp, AssetGroupNames.REAL_PROPERTY, OPT) &&
                             string.Equals(item, REAL_PROPERTY_HOME_OWNERSHIP, OPT);
            var isCarLoan = string.Equals(grp, AssetGroupNames.PERSONAL_PROPERTY, OPT) &&
                            string.Equals(item, PERSONAL_PROPERTY_MOTOR_VEHICLES, OPT);

            Pondus p;
            if (isCheckingAccount)
            {
                var checkingAmt = amt == null || amt == Pecuniam.Zero
                    ? _randCheckingAcctAmt
                    : _checkingAccountRate * amt.ToDouble();
                p = DepositAccount.RandomCheckingAccount(options.PersonsName, startDate, $"{Etx.RandomInteger(1, 9999):0000}");
                p.Push(startDate.AddDays(-1), checkingAmt.ToPecuniam());
                p.My.ExpectedValue = checkingAmt.ToPecuniam();
            }
            else if (isSavingsAccount)
            {
                var savingAmt = amt == null || amt == Pecuniam.Zero
                    ? _randSavingsAcctAmt
                    : _savingsAccountRate * amt.ToDouble();
                p = DepositAccount.RandomSavingAccount(options.PersonsName, startDate);
                p.Push(startDate.AddDays(-1), savingAmt.ToPecuniam());
                p.My.ExpectedValue = savingAmt.ToPecuniam();
            }
            else if (isMortgage || isCarLoan)
            {
                var homeDebtAmt = amt == null || amt == Pecuniam.Zero 
                    ? _randHomeDebt 
                    : _homeDebtRate * amt.ToDouble();
                var homeEquityAmt = amt == null || amt == Pecuniam.Zero
                    ? _randHomeEquity
                    : _homeEquityRate * amt.ToDouble();
                var carDebtAmt = amt == null || amt == Pecuniam.Zero 
                    ? _randCarDebt 
                    : _carDebtRate * amt.ToDouble();
                var carEquityAmt = amt == null || amt == Pecuniam.Zero
                    ? _randCarEquity
                    : _carEquityRate * amt.ToDouble();

                var baseRate = isMortgage ? FED_RATE : FED_RATE + 3.2;
                var randRate =
                    (float) CreditScore.GetRandomInterestRate(null, baseRate) * 0.01f;

                var id = isMortgage
                    ? new PostalAddress()
                    : (Identifier) new Vin();

                var remainingCost = isMortgage
                    ? homeDebtAmt.ToPecuniam()
                    : carDebtAmt.ToPecuniam();

                var totalValue = isMortgage
                    ? (homeDebtAmt + homeEquityAmt).ToPecuniam()
                    : (carDebtAmt + carEquityAmt).ToPecuniam();

                var buyer = options.Personality;

                var termInYears = isMortgage ? 30 : 5;

                Func<bool> irresp = (() => false);
                if (buyer != null)
                    irresp = buyer.GetRandomActsIrresponsible;

                p = SecuredFixedRateLoan.RandomSecuredFixedRateLoanWithHistory(remainingCost,
                    totalValue,
                    randRate,
                    termInYears,
                    id, irresp);

                var loan = (SecuredFixedRateLoan) p;
                if (isMortgage)
                {
                    p.My.ExpectedValue = homeEquityAmt.ToPecuniam();
                    HousePayment = new Mereo(item, grp) {ExpectedValue = loan.MonthlyPayment, Interval = Interval.Monthly};
                }
                else
                {
                    p.My.ExpectedValue = carEquityAmt.ToPecuniam();
                    CarPayment = new Mereo(item, grp) {ExpectedValue = loan.MonthlyPayment, Interval = Interval.Monthly};
                }
            }
            else
            {
                p = new Pondus(item, options.Interval)
                {
                    Inception = startDate,
                    Terminus = options.Terminus,
                };
            }

            p.My.Name = item;
            p.My.UpsertName(KindsOfNames.Group, grp);
            return p;
        }

        /// <summary>
        /// Produces the item names to rates for the Real Property Assets
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetRealPropertyName2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.DerivativeSlope = -0.2D;

            if (options.IsRenting)
            {
                options.GivenDirectly.Add(
                    new Mereo(REAL_PROPERTY_HOME_OWNERSHIP, AssetGroupNames.REAL_PROPERTY) { ExpectedValue = Pecuniam.Zero });
            }
            options.PossibleZeroOuts.AddRange(new []{ "Time Shares", "Land", "Mineral Rights" });
            var d = GetItemNames2Portions(AssetGroupNames.REAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Personal Property Assets.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetPersonalPropertyAssetNames2Rates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            options.PossibleZeroOuts.AddRange(new[] { "Art", "Firearms", "Collections", "Antiques" });

            //remove obvious rural related items for everyone except those who are way out in the country
            var livesInCountry = options.GeoLocation is UsCityStateZip usCityState &&
                                 usCityState.Msa?.MsaType >= UrbanCentric.Fringe;
            if (!livesInCountry)
            {
                options.GivenDirectly.Add(
                    new Mereo("Crops", AssetGroupNames.PERSONAL_PROPERTY) { ExpectedValue = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Livestock", AssetGroupNames.PERSONAL_PROPERTY) { ExpectedValue = Pecuniam.Zero });
            }

            var d = GetItemNames2Portions(AssetGroupNames.PERSONAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for Institutionally Held Assets (e.g. Money Market).
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetInstitutionalAssetName2Rates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            options.PossibleZeroOuts.AddRange(new[]
            {
                "Certificate of Deposit", "Insurance Policies",
                "Money Market", "Annuity",
                "Credit Union", "Profit Sharing",
                "Safe Deposit Box", "Trusts",
                "Brokerage", "Partnerships",
                "Fellowships", "Escrow",
                "Stipends", "Royalties"
            });
            var d = GetItemNames2Portions(AssetGroupNames.INSTITUTIONAL, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for Tradable Security Assets (e.g. Stock and Bonds).
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetSecuritiesAssetNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            var tOptions = options.GetClone();
            tOptions.PossibleZeroOuts.AddRange(new[] { "Derivatives" });
            var d = GetItemNames2Portions(AssetGroupNames.SECURITIES, tOptions);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        #endregion

    }
}
