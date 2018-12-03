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
    /// <inheritdoc cref="AmericanDomusOpesBase" />
    /// <summary>
    /// Represents the assets of a North American over some span of time
    /// </summary>
    [Serializable]
    public class AmericanAssets : AmericanDomusOpesBase, IDeinde
    {
        #region fields
        internal const string REAL_PROPERTY_HOME_OWNERSHIP = "Home Ownership";
        internal const string PERSONAL_PROPERTY_MOTOR_VEHICLES = "Motor Vehicles";
        internal const string INSTITUTIONAL_CHECKING = "Checking";
        internal const string INSTITUTIONAL_SAVINGS = "Savings";

        private readonly HashSet<NamedTradeline> _assets = new HashSet<NamedTradeline>();

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

        protected internal override List<NamedTradeline> MyItems
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
        public static AmericanAssets RandomAssets(AmericanDomusOpesOptions options = null)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();
            var assets = new AmericanAssets();
            assets.RandomizeAllItems(options);
            return assets;
        }

        public override void AddItem(NamedTradeline item)
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

        protected override Dictionary<string, Func<AmericanDomusOpesOptions, Dictionary<string, double>>> GetItems2Functions()
        {
            return new Dictionary<string, Func<AmericanDomusOpesOptions, Dictionary<string, double>>>
            {
                {AssetGroupNames.REAL_PROPERTY, GetRealPropertyName2RandomRates},
                {AssetGroupNames.PERSONAL_PROPERTY, GetPersonalPropertyAssetNames2Rates},
                {AssetGroupNames.INSTITUTIONAL, GetInstitutionalAssetName2Rates},
                {AssetGroupNames.SECURITIES, GetSecuritiesAssetNames2RandomRates}
            };
        }

        protected internal override void RandomizeAllItems(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();
            AssignFactorValues(options.FactorOptions);
            options.DueFrequency = Constants.TropicalYear;
            var items = GetItemsForRange(options);
            foreach (var item in items)
                AddItem(item);
        }

        protected internal override List<Tuple<string, double>> GetGroupNames2Portions(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var amtR = options.SumTotal == null || options.SumTotal == 0
                ? _totalEquity
                : (double) options.SumTotal;

            var givenDirectly = new List<Tuple<string, string, double>>();
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
                givenDirectly.Add(new Tuple<string, string, double>(REAL_PROPERTY_HOME_OWNERSHIP,
                    AssetGroupNames.REAL_PROPERTY, (_homeEquityRate * amtR)));
            }

            if (options.NumberOfVehicles > 0 && !assignedVehicleDirectly)
            {
                givenDirectly.Add(new Tuple<string, string, double>(PERSONAL_PROPERTY_MOTOR_VEHICLES,
                    AssetGroupNames.PERSONAL_PROPERTY, (_carEquityRate * amtR)));
            }
            if (!assignedCheckingDirectly)
            {
                givenDirectly.Add(new Tuple<string, string, double>(INSTITUTIONAL_CHECKING,
                    AssetGroupNames.INSTITUTIONAL, (_checkingAccountRate * amtR)));
            }
            if (!assignedSavingDirectly)
            {
                givenDirectly.Add(new Tuple<string, string, double>(INSTITUTIONAL_SAVINGS, AssetGroupNames.INSTITUTIONAL,
                    (_savingsAccountRate * amtR)));
            }

            if (options.SumTotal == null || options.SumTotal == 0)
            {
                options.Rate = RandPortions.DiminishingRate.VeryFast;
                options.AddGivenDirectlyRange(givenDirectly);
            }

            return base.GetGroupNames2Portions(options);
        }

        protected internal override NamedTradeline GetNamedReceivableForItemAndGroup(string item, string grp, AmericanDomusOpesOptions options, double rate)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            const float FED_RATE = RiskFreeInterestRate.DF_VALUE;

            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var startDate = options.Inception;
            var amtR = options.SumTotal;

            var isCheckingAccount = string.Equals(grp, AssetGroupNames.INSTITUTIONAL, OPT) &&
                                    string.Equals(item, INSTITUTIONAL_CHECKING, OPT);
            var isSavingsAccount = string.Equals(grp, AssetGroupNames.INSTITUTIONAL, OPT) &&
                                   string.Equals(item, INSTITUTIONAL_SAVINGS, OPT);
            var isMortgage = !options.IsRenting && string.Equals(grp, AssetGroupNames.REAL_PROPERTY, OPT) &&
                             string.Equals(item, REAL_PROPERTY_HOME_OWNERSHIP, OPT);
            var isCarLoan = options.NumberOfVehicles > 0 &&
                            string.Equals(grp, AssetGroupNames.PERSONAL_PROPERTY, OPT) &&
                            string.Equals(item, PERSONAL_PROPERTY_MOTOR_VEHICLES, OPT);

            NamedTradeline p;
            if (isCheckingAccount)
            {
                var checkingAmt = amtR == null || amtR == 0
                    ? _randCheckingAcctAmt
                    : _checkingAccountRate * amtR;
                p = DepositAccount.RandomCheckingAccount(options.PersonsName, startDate,
                    $"{Etx.RandomInteger(1, 9999):0000}");
                ((DepositAccount)p).Deposit(startDate.AddDays(-1), (checkingAmt ?? 0).ToPecuniam());
            }
            else if (isSavingsAccount)
            {
                var savingAmt = amtR == null || amtR == 0
                    ? _randSavingsAcctAmt
                    : _savingsAccountRate * amtR;
                p = DepositAccount.RandomSavingAccount(options.PersonsName, startDate);
                ((DepositAccount)p).Deposit(startDate.AddDays(-1), (savingAmt ?? 0).ToPecuniam());
            }
            else if (isMortgage || isCarLoan)
            {
                var homeDebtAmt = amtR == null || amtR == 0
                    ? _randHomeDebt 
                    : _homeDebtRate * amtR;
                var homeEquityAmt = amtR == null || amtR == 0
                    ? _randHomeEquity
                    : _homeEquityRate * amtR;
                var carDebtAmt = amtR == null || amtR == 0
                    ? _randCarDebt 
                    : _carDebtRate * amtR;
                var carEquityAmt = amtR == null || amtR == 0
                    ? _randCarEquity
                    : _carEquityRate * amtR;

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
                    ? (homeDebtAmt ?? 0).ToPecuniam()
                    : (carDebtAmt ?? 0).ToPecuniam();

                var totalValue = isMortgage
                    ? ((homeDebtAmt ?? 0) + (homeEquityAmt ?? 0)).ToPecuniam()
                    : ((carDebtAmt ?? 0) + (carEquityAmt ?? 0)).ToPecuniam();

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

                p.Name = item;
                p.AddName(KindsOfNames.Group, grp);
            }
            else
            {
                p = base.GetNamedReceivableForItemAndGroup(item, grp, options, rate);
            }

            return p;
        }

        /// <summary>
        /// Produces the item names to rates for the Real Property Assets
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetRealPropertyName2RandomRates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            options.Rate = RandPortions.DiminishingRate.VeryFast;

            if (options.IsRenting)
            {
                options.AddZeroPortion(REAL_PROPERTY_HOME_OWNERSHIP, AssetGroupNames.REAL_PROPERTY);
            }
            options.AddPossibleZeroOuts(GetAllowZeroNames(Division, AssetGroupNames.REAL_PROPERTY));
            var d = GetItemNames2Portions(AssetGroupNames.REAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for the Personal Property Assets.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetPersonalPropertyAssetNames2Rates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();
            options.AddPossibleZeroOuts(GetAllowZeroNames(Division, AssetGroupNames.PERSONAL_PROPERTY));

            //remove obvious rural related items for everyone except those who are way out in the country
            var livesInCountry = options.HomeLocation is UsCityStateZip usCityState &&
                                 usCityState.Msa?.MsaType >= UrbanCentric.Fringe;
            if (!livesInCountry)
            {
                options.AddZeroPortion("Crops", AssetGroupNames.PERSONAL_PROPERTY);

                options.AddZeroPortion("Livestock", AssetGroupNames.PERSONAL_PROPERTY);
            }

            var d = GetItemNames2Portions(AssetGroupNames.PERSONAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for Institutionally Held Assets (e.g. Money Market).
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetInstitutionalAssetName2Rates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();
            options.AddPossibleZeroOuts(GetAllowZeroNames(Division, AssetGroupNames.INSTITUTIONAL));
            var d = GetItemNames2Portions(AssetGroupNames.INSTITUTIONAL, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        /// Produces the item names to rates for Tradable Security Assets (e.g. Stock and Bonds).
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> GetSecuritiesAssetNames2RandomRates(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();
            var tOptions = options.GetClone();
            tOptions.AddPossibleZeroOuts(GetAllowZeroNames(Division, AssetGroupNames.SECURITIES));
            var d = GetItemNames2Portions(AssetGroupNames.SECURITIES, tOptions);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal void AssignFactorValues(AmericanFactorOptions factorOptions, double stdDev = DF_STD_DEV_PERCENT)
        {
            if (_factorsSet)
                return;
            var factors = new AmericanFactors(factorOptions);

            _randCheckingAcctAmt = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.CheckingAccount,
                factors.CheckingAcctFactor, stdDev);
            _randSavingsAcctAmt = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.SavingsAccount,
                factors.SavingsAcctFactor, stdDev);

            _randHomeEquity = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.HomeEquity,
                factors.HomeEquityFactor,
                DF_STD_DEV_PERCENT);

            _randCarEquity = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.VehicleEquity,
                factors.VehicleEquityFactor, stdDev);

            var randCcDebt = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.CreditCardDebt,
                factors.CreditCardDebtFactor,
                DF_STD_DEV_PERCENT);

            _randHomeDebt = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.HomeDebt, factors.HomeDebtFactor,
                DF_STD_DEV_PERCENT);

            _randCarDebt = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.VehicleDebt,
                factors.VehicleDebtFactor, stdDev);

            _totalEquity = _randCheckingAcctAmt + _randSavingsAcctAmt + _randHomeEquity + _randCarEquity;

            var randNetWorth = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.NetWorth,
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
