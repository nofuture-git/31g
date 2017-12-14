using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IRebus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// Represents the assets of a North American over some span of time
    /// </summary>
    [Serializable]
    public class NorthAmericanAssets : WealthBase, IRebus
    {
        #region fields
        private readonly HashSet<Pondus> _assets = new HashSet<Pondus>();
        //private readonly DateTime _startDate;

        private readonly double _randCheckingAcctAmt;
        private readonly double _randSavingsAcctAmt;
        private readonly double _randHomeEquity;
        private readonly double _randCarEquity;
        private readonly double _allOtherAssetEquity;

        private readonly double _randCcDebt;
        private readonly double _randHomeDebt;
        private readonly double _randCarDebt;

        private readonly double _totalEquity;

        private IMereo _carPayment;
        private IMereo _housePayment;
        private readonly double _homeEquityRate;
        private readonly double _carEquityRate;
        private readonly double _checkingAccountRate;
        private readonly double _savingsAccountRate;
        private readonly double _homeDebtRate;
        private readonly double _carDebtRate;

        #endregion

        #region ctors
        public NorthAmericanAssets(NorthAmerican american, OpesOptions options) : base(
            american, options)
        {
            if(MyOptions.StartDate == DateTime.MinValue)
                MyOptions.StartDate = GetYearNeg(-1);

            _randCheckingAcctAmt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.CheckingAccount,
                Factors.CheckingAcctFactor, DF_STD_DEV_PERCENT);
            _randSavingsAcctAmt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.SavingsAccount,
                Factors.SavingsAcctFactor, DF_STD_DEV_PERCENT);

            _randHomeEquity  = NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeEquity, Factors.HomeEquityFactor,
                        DF_STD_DEV_PERCENT);

            _randCarEquity = NorthAmericanFactors.GetRandomFactorValue(FactorTables.VehicleEquity,
                        Factors.VehicleEquityFactor, DF_STD_DEV_PERCENT);

            _randCcDebt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.CreditCardDebt,
                        Factors.CreditCardDebtFactor,
                        DF_STD_DEV_PERCENT);

            _randHomeDebt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, Factors.HomeDebtFactor,
                        DF_STD_DEV_PERCENT);

            _randCarDebt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.VehicleDebt,
                        Factors.VehicleDebtFactor, DF_STD_DEV_PERCENT);

            _totalEquity = _randCheckingAcctAmt + _randSavingsAcctAmt + _randHomeEquity + _randCarEquity;


            var randNetWorth = NorthAmericanFactors.GetRandomFactorValue(FactorTables.NetWorth,
                Factors.NetWorthFactor, DF_STD_DEV_PERCENT);

            _allOtherAssetEquity = _totalEquity - randNetWorth;

            _totalEquity += _allOtherAssetEquity;

            _homeEquityRate = _randHomeEquity / _totalEquity;
            _carEquityRate = _randCarEquity / _totalEquity;
            _checkingAccountRate = _randCheckingAcctAmt / _totalEquity;
            _savingsAccountRate = _randSavingsAcctAmt / _totalEquity;

            var totalDebt = _randHomeDebt + _randCarDebt + _randCcDebt;
            _homeDebtRate = _randHomeDebt / totalDebt;
            _carDebtRate = _randCarDebt / totalDebt;
        }
        #endregion

        #region properties

        public Pondus[] CurrentAssets => GetCurrent(MyItems);

        public Pecuniam TotalCurrentExpectedValue => Pondus.GetExpectedSum(CurrentAssets);

        public IMereo HousePayment => _housePayment;
        public IMereo CarPayment => _carPayment;

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

        protected internal override void ResolveItems(OpesOptions options)
        {
            options = options ?? MyOptions;
            var stDt = options.StartDate == DateTime.MinValue ? GetYearNeg(-1) : options.StartDate;
            var ranges = GetYearsInDates(stDt);

            foreach (var range in ranges)
            {
                var cloneOptions = options.GetClone();
                cloneOptions.StartDate = range.Item1;
                cloneOptions.EndDate = range.Item2;
                cloneOptions.Interval = Interval.Annually;

                var items = GetItemsForRange(cloneOptions);
                foreach(var item in items)
                    AddItem(item);
            }
        }

        public override List<Tuple<string, double>> GetGroupNames2Portions(OpesOptions options)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            options = options ?? MyOptions;

            if (options.GivenDirectly.Any())
                return base.GetGroupNames2Portions(options);

            var amt = options.SumTotal ?? _totalEquity.ToPecuniam();
            var givenDirectly = new List<IMereo>();
            var assignedRealEstateDirectly = options.GivenDirectly.Any(g =>
                string.Equals(g.Name, "Real Estate", OPT) && string.Equals(g.GetName(KindsOfNames.Group),
                    AssetGroupNames.REAL_PROPERTY, OPT));
            var assignedVehicleDirectly = options.GivenDirectly.Any(g =>
                string.Equals(g.Name, "Motor Vehicles", OPT) && string.Equals(g.GetName(KindsOfNames.Group),
                    AssetGroupNames.PERSONAL_PROPERTY, OPT));

            var assignedCheckingDirectly = options.GivenDirectly.Any(g =>
                string.Equals(g.Name, "Checking", OPT) && string.Equals(g.GetName(KindsOfNames.Group),
                    AssetGroupNames.INSTITUTIONAL, OPT));

            var assignedSavingDirectly = options.GivenDirectly.Any(g =>
                string.Equals(g.Name, "Savings", OPT) && string.Equals(g.GetName(KindsOfNames.Group),
                    AssetGroupNames.INSTITUTIONAL, OPT));

            if (!options.IsRenting && !assignedRealEstateDirectly)
            {
                givenDirectly.Add(new Mereo("Real Estate", AssetGroupNames.REAL_PROPERTY)
                {
                    ExpectedValue = (_homeEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }

            if (MyOptions.HasVehicles && !assignedVehicleDirectly)
            {
                givenDirectly.Add(new Mereo("Motor Vehicles", AssetGroupNames.PERSONAL_PROPERTY)
                {
                    ExpectedValue = (_carEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }
            if (!assignedCheckingDirectly)
            {
                givenDirectly.Add(new Mereo("Checking", AssetGroupNames.INSTITUTIONAL)
                {
                    ExpectedValue = (_checkingAccountRate * amt.ToDouble()).ToPecuniam()
                });
            }
            if (!assignedSavingDirectly)
            {
                givenDirectly.Add(new Mereo("Savings", AssetGroupNames.INSTITUTIONAL)
                {
                    ExpectedValue = (_savingsAccountRate * amt.ToDouble()).ToPecuniam()
                });
            }

            if (options.SumTotal == Pecuniam.Zero)
            {
                options.SumTotal = _totalEquity.ToPecuniam();
                options.DerivativeSlope = -0.2D;
                options.GivenDirectly.AddRange(givenDirectly);
                MyOptions = options;
            }
            return base.GetGroupNames2Portions(options);
        }

        /// <summary>
        /// Factory method to create a <see cref="Pondus"/> based on the given values
        /// </summary>
        /// <param name="item"></param>
        /// <param name="grp"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal override Pondus GetPondusForItemAndGroup(string item, string grp, OpesOptions options)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            const float FED_RATE = Gov.Fed.RiskFreeInterestRate.DF_VALUE;

            options = (options ?? MyOptions) ?? new OpesOptions();

            var startDate = options.StartDate;
            var amt = options.SumTotal;

            var isCheckingAccount =
                string.Equals(grp, AssetGroupNames.INSTITUTIONAL, OPT) && string.Equals(item, "Checking", OPT);
            var isSavingsAccount =
                string.Equals(grp, AssetGroupNames.INSTITUTIONAL, OPT) && string.Equals(item, "Savings", OPT);
            var isMortgage = string.Equals(grp, AssetGroupNames.REAL_PROPERTY, OPT) &&
                             string.Equals(item, "Real Estate", OPT);
            var isCarLoan = string.Equals(grp, AssetGroupNames.PERSONAL_PROPERTY, OPT) &&
                            string.Equals(item, "Motor Vehicles", OPT);

            Pondus p;
            if (isCheckingAccount)
            {
                var checkingAmt = amt == null ? _randCheckingAcctAmt : _checkingAccountRate * amt.ToDouble();
                p = CheckingAccount.GetRandomCheckingAcct(Person, startDate, $"{Etx.IntNumber(1, 9999):0000}");
                p.Push(startDate.AddDays(-1), checkingAmt.ToPecuniam());
            }
            else if (isSavingsAccount)
            {
                var savingAmt = amt == null ? _randSavingsAcctAmt : _savingsAccountRate * amt.ToDouble();
                p = SavingsAccount.GetRandomSavingAcct(Person, startDate);
                p.Push(startDate.AddDays(-1), savingAmt.ToPecuniam());
            }
            else if (isMortgage || isCarLoan)
            {
                var homeDebtAmt = amt == null ? _randHomeDebt : _homeDebtRate * amt.ToDouble();
                var homeEquityAmt = amt == null ? _randHomeEquity : _homeEquityRate * amt.ToDouble();
                var carDebtAmt = amt == null ? _randCarDebt : _carDebtRate * amt.ToDouble();
                var carEquityAmt = amt == null ? _randCarEquity : _carEquityRate * amt.ToDouble();

                var baseRate = isMortgage ? FED_RATE : FED_RATE + 3.2;
                var randRate =
                    (float) CreditScore.GetRandomInterestRate(null, baseRate) * 0.01f;

                var id = isMortgage
                    ? (Person?.Address ?? new ResidentAddress())
                    : (Identifier) new Gov.Nhtsa.Vin();

                var remainingCost = isMortgage
                    ? homeDebtAmt.ToPecuniam()
                    : carDebtAmt.ToPecuniam();

                var totalValue = isMortgage
                    ? (homeDebtAmt + homeEquityAmt).ToPecuniam()
                    : (carDebtAmt + carEquityAmt).ToPecuniam();

                var buyer = Person?.Personality;

                var termInYears = isMortgage ? 30 : 5;

                p = SecuredFixedRateLoan.GetRandomLoanWithHistory(
                    id,
                    remainingCost,
                    totalValue,
                    randRate,
                    termInYears,
                    out var outPayment,
                    buyer);

                if (isMortgage)
                {
                    _housePayment = new Mereo(item, grp) {ExpectedValue = outPayment, Interval = Interval.Monthly};
                }
                else
                    _carPayment = new Mereo(item, grp) { ExpectedValue = outPayment, Interval = Interval.Monthly };


            }
            else
            {
                p = new Pondus(item, options.Interval)
                {
                    Inception = startDate,
                    Terminus = options.EndDate,
                };
            }

            p.My.Name = item;
            p.My.UpsertName(KindsOfNames.Group, grp);
            return p;
        }

        protected internal virtual Dictionary<string, double> GetRealPropertyName2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();

            options.DerivativeSlope = -0.2D;

            if (options.IsRenting)
            {
                options.GivenDirectly.Add(
                    new Mereo("Home Ownership", AssetGroupNames.REAL_PROPERTY) { ExpectedValue = Pecuniam.Zero });
            }
            options.PossiableZeroOuts.AddRange(new []{ "Time Shares", "Land", "Mineral Right" });
            var d = GetItemNames2Portions(AssetGroupNames.REAL_PROPERTY, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal virtual Dictionary<string, double> GetPersonalPropertyAssetNames2Rates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            options.PossiableZeroOuts.AddRange(new[] { "Art", "Firearms", "Collections", "Antiques" });

            //remove these for everyone except those who are way out in the country
            var livesInCountry = Person?.Address?.HomeCityArea is UsCityStateZip usCityState &&
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

        protected internal Dictionary<string, double> GetInstitutionalAssetName2Rates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            options.PossiableZeroOuts.AddRange(new[]
            {
                "Certificate of Deposit", "Insurance Policies", "Money Market", "Annuity",
                "Credit Union", "Profit Sharing", "Safe Deposit Box", "Trusts", "Brokerage", "Partnerships",
                "Fellowships", "Escrow", "Stipends", "Royalties"
            });
            var d = GetItemNames2Portions(AssetGroupNames.INSTITUTIONAL, options);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        protected internal Dictionary<string, double> GetSecuritiesAssetNames2RandomRates(OpesOptions options)
        {
            options = (options ?? MyOptions) ?? new OpesOptions();
            var tOptions = options.GetClone();
            tOptions.PossiableZeroOuts.AddRange(new[] { "Derivatives" });
            var d = GetItemNames2Portions(AssetGroupNames.SECURITIES, tOptions);
            return d.ToDictionary(t => t.Item1, t => t.Item2);
        }

        #endregion

    }
}
