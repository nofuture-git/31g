using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US.Fed;
using NoFuture.Rand.Gov.US.Nhtsa;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Opes.US
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

        private bool _factorsSet;
        private double _randCheckingAcctAmt;
        private double _randSavingsAcctAmt;
        private double _randHomeEquity;
        private double _randCarEquity;
        private double _randHomeDebt;
        private double _randCarDebt;
        private double _totalEquity;
        private double _checkingAccountRate;
        private double _savingsAccountRate;
        private double _homeEquityRate;
        private double _carEquityRate;
        private double _homeDebtRate;
        private double _carDebtRate;

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
            options = options ?? OpesOptions.RandomOpesOptions();
            var assets = new AmericanAssets();
            assets.RandomizeAllItems(options);
            return assets;
        }

        public Pondus[] GetAssetsAt(DateTime? dt)
        {
            return GetAt(dt, MyItems);
        }

        public override void AddItem(Pondus item)
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

        protected internal override void RandomizeAllItems(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();
            AssignFactorValues(options.FactorOptions);
            options.Interval = Interval.Annually;
            var items = GetItemsForRange(options);
            foreach (var item in items)
                AddItem(item);
        }

        protected internal override List<Tuple<string, double>> GetGroupNames2Portions(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

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
                    Value = (_homeEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }

            if (options.NumberOfVehicles > 0 && !assignedVehicleDirectly)
            {
                givenDirectly.Add(new Mereo(PERSONAL_PROPERTY_MOTOR_VEHICLES, AssetGroupNames.PERSONAL_PROPERTY)
                {
                    Value = (_carEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }
            if (!assignedCheckingDirectly)
            {
                givenDirectly.Add(new Mereo(INSTITUTIONAL_CHECKING, AssetGroupNames.INSTITUTIONAL)
                {
                    Value = (_checkingAccountRate * amt.ToDouble()).ToPecuniam()
                });
            }
            if (!assignedSavingDirectly)
            {
                givenDirectly.Add(new Mereo(INSTITUTIONAL_SAVINGS, AssetGroupNames.INSTITUTIONAL)
                {
                    Value = (_savingsAccountRate * amt.ToDouble()).ToPecuniam()
                });
            }

            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
            {
                options.DerivativeSlope = -0.2D;
                options.GivenDirectly.AddRange(givenDirectly);
            }
            return base.GetGroupNames2Portions(options);
        }

        protected internal override Pondus GetPondusForItemAndGroup(string item, string grp, OpesOptions options)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            const float FED_RATE = RiskFreeInterestRate.DF_VALUE;

            options = options ?? OpesOptions.RandomOpesOptions();

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
                p = DepositAccount.RandomCheckingAccount(options.PersonsName, startDate,
                    $"{Etx.RandomInteger(1, 9999):0000}");
                ((DepositAccount)p).Deposit(startDate.AddDays(-1), checkingAmt.ToPecuniam());
                p.Expectation.Value = checkingAmt.ToPecuniam();
            }
            else if (isSavingsAccount)
            {
                var savingAmt = amt == null || amt == Pecuniam.Zero
                    ? _randSavingsAcctAmt
                    : _savingsAccountRate * amt.ToDouble();
                p = DepositAccount.RandomSavingAccount(options.PersonsName, startDate);
                ((DepositAccount)p).Deposit(startDate.AddDays(-1), savingAmt.ToPecuniam());
                p.Expectation.Value = savingAmt.ToPecuniam();
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
                var creditScore = new PersonalCreditScore(options.FactorOptions.DateOfBirth)
                {
                    OpennessZscore = options.Personality?.Openness?.Value?.Zscore ?? 0D,
                    ConscientiousnessZscore = options.Personality?.Conscientiousness?.Value?.Zscore ?? 0D
                };
                var randRate = (float) creditScore.GetRandomInterestRate(null, baseRate) * 0.01f;

                var id = isMortgage
                    ? (Identifier) PostalAddress.RandomAmericanAddress()
                    : Vin.RandomVin();

                var remainingCost = isMortgage
                    ? homeDebtAmt.ToPecuniam()
                    : carDebtAmt.ToPecuniam();

                var totalValue = isMortgage
                    ? (homeDebtAmt + homeEquityAmt).ToPecuniam()
                    : (carDebtAmt + carEquityAmt).ToPecuniam();

                var buyer = options.Personality;

                var termInYears = isMortgage ? 30 : 5;

                Func<bool> irresp = () => false;
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
                    p.Expectation.Value = homeEquityAmt.ToPecuniam();
                    HousePayment =
                        new Mereo(item, grp) {Value = loan.MonthlyPayment, DueFrequency = new TimeSpan(30, 0, 0, 0)};
                }
                else
                {
                    p.Expectation.Value = carEquityAmt.ToPecuniam();
                    CarPayment =
                        new Mereo(item, grp) {Value = loan.MonthlyPayment, DueFrequency = new TimeSpan(30, 0, 0, 0) };
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

            p.Expectation.Name = item;
            p.Expectation.AddName(KindsOfNames.Group, grp);
            return p;
        }

        /// <summary>
        /// Produces the item names to rates for the Real Property Assets
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetRealPropertyName2RandomRates(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            options.DerivativeSlope = -0.2D;

            if (options.IsRenting)
            {
                options.GivenDirectly.Add(
                    new Mereo(REAL_PROPERTY_HOME_OWNERSHIP, AssetGroupNames.REAL_PROPERTY)
                    {
                        Value = Pecuniam.Zero
                    });
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
            options = options ?? OpesOptions.RandomOpesOptions();
            options.PossibleZeroOuts.AddRange(new[] { "Art", "Firearms", "Collections", "Antiques" });

            //remove obvious rural related items for everyone except those who are way out in the country
            var livesInCountry = options.HomeLocation is UsCityStateZip usCityState &&
                                 usCityState.Msa?.MsaType >= UrbanCentric.Fringe;
            if (!livesInCountry)
            {
                options.GivenDirectly.Add(
                    new Mereo("Crops", AssetGroupNames.PERSONAL_PROPERTY) { Value = Pecuniam.Zero });

                options.GivenDirectly.Add(
                    new Mereo("Livestock", AssetGroupNames.PERSONAL_PROPERTY) {Value = Pecuniam.Zero});
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
            options = options ?? OpesOptions.RandomOpesOptions();
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
            options = options ?? OpesOptions.RandomOpesOptions();
            var tOptions = options.GetClone();
            tOptions.PossibleZeroOuts.AddRange(new[] { "Derivatives" });
            var d = GetItemNames2Portions(AssetGroupNames.SECURITIES, tOptions);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal void AssignFactorValues(AmericanFactorOptions factorOptions, double stdDev = DF_STD_DEV_PERCENT)
        {
            if (_factorsSet)
                return;
            var factors = new AmericanFactors(factorOptions);

            _randCheckingAcctAmt = AmericanFactors.GetRandomFactorValue(FactorTables.CheckingAccount,
                factors.CheckingAcctFactor, stdDev);
            _randSavingsAcctAmt = AmericanFactors.GetRandomFactorValue(FactorTables.SavingsAccount,
                factors.SavingsAcctFactor, stdDev);

            _randHomeEquity = AmericanFactors.GetRandomFactorValue(FactorTables.HomeEquity,
                factors.HomeEquityFactor,
                DF_STD_DEV_PERCENT);

            _randCarEquity = AmericanFactors.GetRandomFactorValue(FactorTables.VehicleEquity,
                factors.VehicleEquityFactor, stdDev);

            var randCcDebt = AmericanFactors.GetRandomFactorValue(FactorTables.CreditCardDebt,
                factors.CreditCardDebtFactor,
                DF_STD_DEV_PERCENT);

            _randHomeDebt = AmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, factors.HomeDebtFactor,
                DF_STD_DEV_PERCENT);

            _randCarDebt = AmericanFactors.GetRandomFactorValue(FactorTables.VehicleDebt,
                factors.VehicleDebtFactor, stdDev);

            _totalEquity = _randCheckingAcctAmt + _randSavingsAcctAmt + _randHomeEquity + _randCarEquity;

            var randNetWorth = AmericanFactors.GetRandomFactorValue(FactorTables.NetWorth,
                factors.NetWorthFactor, stdDev);

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

            _factorsSet = true;
        }

        #endregion

    }
}
