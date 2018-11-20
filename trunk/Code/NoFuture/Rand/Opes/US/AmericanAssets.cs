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
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Opes.US
{
    /// <inheritdoc cref="DomusOpesBase" />
    /// <summary>
    /// Represents the assets of a North American over some span of time
    /// </summary>
    [Serializable]
    public class AmericanAssets : DomusOpesBase, IDeinde
    {
        #region fields
        internal const string REAL_PROPERTY_HOME_OWNERSHIP = "Home Ownership";
        internal const string PERSONAL_PROPERTY_MOTOR_VEHICLES = "Motor Vehicles";
        internal const string INSTITUTIONAL_CHECKING = "Checking";
        internal const string INSTITUTIONAL_SAVINGS = "Savings";

        private readonly HashSet<NamedReceivable> _assets = new HashSet<NamedReceivable>();

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

        public Pecuniam HousePayment { get; private set; }

        public Pecuniam CarPayment { get; private set; }

        protected internal override List<NamedReceivable> MyItems
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
        public static AmericanAssets RandomAssets(DomusOpesOptions options = null)
        {
            options = options ?? DomusOpesOptions.RandomOpesOptions();
            var assets = new AmericanAssets();
            assets.RandomizeAllItems(options);
            return assets;
        }

        public override void AddItem(NamedReceivable item)
        {
            if (item == null)
                return;

            _assets.Add(item);
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            var itemData = new Dictionary<string, object>();

            foreach (var p in CurrentItems)
            {
                if (p.Value == Pecuniam.Zero)
                    continue;
                AddOrReplace(itemData, p.ToData(txtCase));
            }

            return itemData;
        }

        protected override Dictionary<string, Func<DomusOpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<DomusOpesOptions, Dictionary<string, double>>>
            {
                {AssetGroupNames.REAL_PROPERTY, GetRealPropertyName2RandomRates},
                {AssetGroupNames.PERSONAL_PROPERTY, GetPersonalPropertyAssetNames2Rates},
                {AssetGroupNames.INSTITUTIONAL, GetInstitutionalAssetName2Rates},
                {AssetGroupNames.SECURITIES, GetSecuritiesAssetNames2RandomRates}
            };
        }

        protected internal override void RandomizeAllItems(DomusOpesOptions options)
        {
            options = options ?? DomusOpesOptions.RandomOpesOptions();
            AssignFactorValues(options.FactorOptions);
            options.DueFrequency = Constants.TropicalYear;
            var items = GetItemsForRange(options);
            foreach (var item in items)
                AddItem(item);
        }

        protected internal override List<Tuple<string, double>> GetGroupNames2Portions(DomusOpesOptions options)
        {
            options = options ?? DomusOpesOptions.RandomOpesOptions();

            var amt = options.SumTotal == null || options.SumTotal == Pecuniam.Zero
                ? _totalEquity.ToPecuniam()
                : options.SumTotal;

            var givenDirectly = new List<Tuple<string, string, Pecuniam>>();
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
                givenDirectly.Add(new Tuple<string, string, Pecuniam>(REAL_PROPERTY_HOME_OWNERSHIP,
                    AssetGroupNames.REAL_PROPERTY, (_homeEquityRate * amt.ToDouble()).ToPecuniam()));
            }

            if (options.NumberOfVehicles > 0 && !assignedVehicleDirectly)
            {
                givenDirectly.Add(new Tuple<string, string, Pecuniam>(PERSONAL_PROPERTY_MOTOR_VEHICLES,
                    AssetGroupNames.PERSONAL_PROPERTY, (_carEquityRate * amt.ToDouble()).ToPecuniam()));
            }
            if (!assignedCheckingDirectly)
            {
                givenDirectly.Add(new Tuple<string, string, Pecuniam>(INSTITUTIONAL_CHECKING,
                    AssetGroupNames.INSTITUTIONAL, (_checkingAccountRate * amt.ToDouble()).ToPecuniam()));
            }
            if (!assignedSavingDirectly)
            {
                givenDirectly.Add(new Tuple<string, string, Pecuniam>(INSTITUTIONAL_SAVINGS, AssetGroupNames.INSTITUTIONAL,
                    (_savingsAccountRate * amt.ToDouble()).ToPecuniam()));
            }

            if (options.SumTotal == null || options.SumTotal == Pecuniam.Zero)
            {
                options.DerivativeSlope = -0.2D;
                options.AddGivenDirectlyRange(givenDirectly);
            }
            return base.GetGroupNames2Portions(options);
        }

        protected internal override NamedReceivable GetNamedReceivableForItemAndGroup(string item, string grp, DomusOpesOptions options, double rate)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            const float FED_RATE = RiskFreeInterestRate.DF_VALUE;

            options = options ?? DomusOpesOptions.RandomOpesOptions();

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

            NamedReceivable p;
            if (isCheckingAccount)
            {
                var checkingAmt = amt == null || amt == Pecuniam.Zero
                    ? _randCheckingAcctAmt
                    : _checkingAccountRate * amt.ToDouble();
                p = DepositAccount.RandomCheckingAccount(options.PersonsName, startDate,
                    $"{Etx.RandomInteger(1, 9999):0000}");
                ((DepositAccount)p).Deposit(startDate.AddDays(-1), checkingAmt.ToPecuniam());
            }
            else if (isSavingsAccount)
            {
                var savingAmt = amt == null || amt == Pecuniam.Zero
                    ? _randSavingsAcctAmt
                    : _savingsAccountRate * amt.ToDouble();
                p = DepositAccount.RandomSavingAccount(options.PersonsName, startDate);
                ((DepositAccount)p).Deposit(startDate.AddDays(-1), savingAmt.ToPecuniam());
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
                    HousePayment = loan.MonthlyPayment;
                }
                else
                {
                    CarPayment =loan.MonthlyPayment;
                }
            }
            else
            {
                p = new NamedReceivable(item)
                {
                    Inception = startDate,
                    Terminus = options.Terminus,
                    DueFrequency =  options.DueFrequency
                };
            }

            p.Name = item;
            p.AddName(KindsOfNames.Group, grp);
            return p;
        }

        /// <summary>
        /// Produces the item names to rates for the Real Property Assets
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetRealPropertyName2RandomRates(DomusOpesOptions options)
        {
            options = options ?? DomusOpesOptions.RandomOpesOptions();

            options.DerivativeSlope = -0.2D;

            if (options.IsRenting)
            {
                options.AddGivenDirectlyZero(REAL_PROPERTY_HOME_OWNERSHIP, AssetGroupNames.REAL_PROPERTY);
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
        protected internal virtual Dictionary<string, double> GetPersonalPropertyAssetNames2Rates(DomusOpesOptions options)
        {
            options = options ?? DomusOpesOptions.RandomOpesOptions();
            options.PossibleZeroOuts.AddRange(new[] { "Art", "Firearms", "Collections", "Antiques" });

            //remove obvious rural related items for everyone except those who are way out in the country
            var livesInCountry = options.HomeLocation is UsCityStateZip usCityState &&
                                 usCityState.Msa?.MsaType >= UrbanCentric.Fringe;
            if (!livesInCountry)
            {
                options.AddGivenDirectlyZero("Crops", AssetGroupNames.PERSONAL_PROPERTY);

                options.AddGivenDirectlyZero("Livestock", AssetGroupNames.PERSONAL_PROPERTY);
            }

            var d = GetItemNames2Portions(AssetGroupNames.PERSONAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for Institutionally Held Assets (e.g. Money Market).
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetInstitutionalAssetName2Rates(DomusOpesOptions options)
        {
            options = options ?? DomusOpesOptions.RandomOpesOptions();
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
        protected internal Dictionary<string, double> GetSecuritiesAssetNames2RandomRates(DomusOpesOptions options)
        {
            options = options ?? DomusOpesOptions.RandomOpesOptions();
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
