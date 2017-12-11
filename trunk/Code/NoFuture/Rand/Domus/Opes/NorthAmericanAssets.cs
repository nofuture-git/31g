using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Opes.Options;

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
        private readonly DateTime _startDate;

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
        public NorthAmericanAssets(NorthAmerican american, OpesOptions options, DateTime? startDate = null) : base(
            american, options)
        {
            _startDate = startDate ?? GetYearNeg(-1);


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

        public Pondus[] CurrentAssets => GetCurrent(Assets);

        public Pecuniam TotalCurrentExpectedValue => Pondus.GetExpectedSum(CurrentAssets);

        public IMereo HousePayment => _housePayment;
        public IMereo CarPayment => _carPayment;

        protected internal virtual List<Pondus> Assets
        {
            get
            {
                var e = _assets.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        #endregion

        #region methods

        public Pondus[] GetAssetsAt(DateTime? dt)
        {
            return GetAt(dt, Assets);
        }

        protected virtual void AddAsset(Pondus asset)
        {
            if (asset == null)
                return;

            _assets.Add(asset);
        }

        /// <summary>
        /// Resolves all assets for last year and this year
        /// </summary>
        protected internal void ResolveAssets()
        {
            var amt = MyOptions.SumTotal ?? _totalEquity.ToPecuniam();
            var pondus = GetAssetItemsForRange(amt, _startDate);
            foreach(var p in pondus)
                AddAsset(p);
        }

        /// <summary>
        /// Gets a manifold of <see cref="Pondus"/> items based on the 
        /// names from GetAssetItemNames assigning random values as a portion of <see cref="amt"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <param name="explicitAmounts">
        /// Allows calling assembly direct control over the created rates.
        /// </param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetAssetItemsForRange(Pecuniam amt, DateTime startDate,
            DateTime? endDate = null, Interval interval = Interval.Annually)
        {
            var itemsout = new List<Pondus>();

            startDate = startDate == DateTime.MinValue ? _startDate : startDate;
            amt = amt ?? _totalEquity.ToPecuniam();

            //get just the group names of assets
            var grpNames = GetAssetItemNames().Select(x => x.GetName(KindsOfNames.Group)).Distinct().ToList();

            //determine a portion or each group
            var portions = Etx.DiminishingPortions(grpNames.Count, -0.2D);
            var grp2Rates = grpNames.Zip(portions, (n, v) => new Tuple<string, double>(n, v)).ToList();

            //when calling assembly doesn't define exact amounts the use the values from the Factor Tables
            if (!MyOptions.GivenDirectly.Any())
            {
                grp2Rates = GetGroupPortionsByFactorTables(amt);
            }
            //map the groups name to a function 
            var name2Op = new Dictionary<string, Func<RatesDictionaryArgs, Dictionary<string, double>>>
            {
                {AssetGroupNames.REAL_PROPERTY, GetRealPropertyName2RandomRates},
                {AssetGroupNames.PERSONAL_PROPERTY, GetPersonalPropertyName2Rates},
                {AssetGroupNames.INSTITUTIONAL, GetInstitutionalName2Rates},
                {AssetGroupNames.SECURITIES, GetSecuritiesName2Rates}
            };

            foreach (var g2r in grp2Rates)
            {
                var grp = g2r.Item1;
                var portion = g2r.Item2;

                //convert explicit amount into rates
                var directAssignRates = ConvertToExplicitRates(amt, MyOptions.GivenDirectly, grp);

                System.Diagnostics.Debug.WriteLine($"{directAssignRates.Select(kv => kv.Value).Sum()}\t{portion}");

                //get the name-to-rates dictionary
                var grpRates = name2Op.ContainsKey(grp)
                    ? name2Op[grp](new RatesDictionaryArgs
                    {
                        SumOfRates = portion,
                        DerivativeSlope = -1.0D,
                        DirectAssignNames2Rates = directAssignRates
                    })
                    : GetNames2RandomRates(DomusOpesDivisions.Assets, grp, portion, null);

                //each item of this group
                foreach (var item in grpRates.Keys)
                {
                    //create the pondus for this group\name pa
                    var p = GetPondusForItemAndGroup(item, grp, startDate, endDate, interval);
                    p.My.ExpectedValue = CalcValue(amt, grpRates[item]);
                    p.My.Interval = Interval.Annually;
                    itemsout.Add(p);
                }
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// Creates a groupName-to-Portion list which matches the factor table values of the given instance.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="explicitAmounts"></param>
        /// <returns></returns>
        protected internal List<Tuple<string, double>> GetGroupPortionsByFactorTables(Pecuniam amt)
        {
            var givenDirectly = new List<IMereo>();
            if (!IsRenting)
            {
                givenDirectly.Add(new Mereo("Real Estate", AssetGroupNames.REAL_PROPERTY)
                {
                    ExpectedValue = (_homeEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }

            if (MyOptions.HasVehicles)
            {
                givenDirectly.Add(new Mereo("Motor Vehicles", AssetGroupNames.PERSONAL_PROPERTY)
                {
                    ExpectedValue = (_carEquityRate * amt.ToDouble()).ToPecuniam()
                });
            }

            givenDirectly.Add(new Mereo("Checking", AssetGroupNames.INSTITUTIONAL)
            {
                ExpectedValue = (_checkingAccountRate * amt.ToDouble()).ToPecuniam()
            });
            givenDirectly.Add(new Mereo("Savings", AssetGroupNames.INSTITUTIONAL)
            {
                ExpectedValue = (_savingsAccountRate * amt.ToDouble()).ToPecuniam()
            });

            var opesOptions = new OpesOptions {SumTotal = _totalEquity.ToPecuniam(), DerivativeSlope = -0.2D};
            opesOptions.GivenDirectly.AddRange(givenDirectly);
            MyOptions = opesOptions;
            return GetGroupNames2Portions(DomusOpesDivisions.Assets, opesOptions);
        }

        /// <summary>
        /// Factory method to create a <see cref="Pondus"/> based on the given values
        /// </summary>
        /// <param name="item"></param>
        /// <param name="grp"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <param name="amt">
        /// Optional, forces the actual amounts used when generating a random history to scale and fit this amount
        /// </param>
        /// <returns></returns>
        protected Pondus GetPondusForItemAndGroup(string item, string grp, DateTime startDate, DateTime? endDate,
            Interval interval = Interval.Annually, Pecuniam amt = null)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            const float FED_RATE = Gov.Fed.RiskFreeInterestRate.DF_VALUE;

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
                p = new Pondus(item, interval)
                {
                    Inception = startDate,
                    Terminus = endDate,
                };
            }

            p.My.Name = item;
            p.My.UpsertName(KindsOfNames.Group, grp);
            return p;
        }

        /// <summary>
        /// Produces a dictionary of Real Property <see cref="WealthBase.DomusOpesDivisions.Assets"/> 
        /// items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetRealPropertyName2RandomRates(
        RatesDictionaryArgs args = null)
        {
            var sumOfRates = args?.SumOfRates ?? Math.Round(Math.Abs(_randHomeEquity / _totalEquity), 5);
            var derivativeSlope = args?.DerivativeSlope ?? -0.2D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            //ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            if (IsRenting)
            {
                var d = new Dictionary<string, double>();
                foreach (var name in GetAssetItemNames().Where(e =>
                    string.Equals(e.GetName(KindsOfNames.Group), AssetGroupNames.REAL_PROPERTY,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    d.Add(name.Name, 0D);
                }
                return d;
            }

            var dk = GetNames2RandomRates(DomusOpesDivisions.Assets, AssetGroupNames.REAL_PROPERTY, sumOfRates, null,
                derivativeSlope);

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Personal Property <see cref="WealthBase.DomusOpesDivisions.Assets"/>
        ///  items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetPersonalPropertyName2Rates(
            RatesDictionaryArgs args = null)
        {

            var sumOfRates = args?.SumOfRates ?? Math.Round(Math.Abs(_randCarDebt / _totalEquity), 5);
            var derivativeSlope = args?.DerivativeSlope ?? -0.4D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            if (!MyOptions.HasVehicles)
            {
                //now we have to have this
                directAssignNames2Rates = directAssignNames2Rates ?? new Dictionary<string, double>();

                if (!directAssignNames2Rates.ContainsKey("Motor Vehicles"))
                {
                    directAssignNames2Rates.Add("Motor Vehicles", 0D);
                }
            }

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Assets, AssetGroupNames.PERSONAL_PROPERTY, sumOfRates,
                null,
                derivativeSlope, "Art", "Firearms", "Collections", "Antiques");

            var zeroOuts = new List<string> {"Crops", "Livestock"};

            dk = ZeroOutRates(dk, zeroOuts.ToArray());

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Institutional <see cref="WealthBase.DomusOpesDivisions.Assets"/> 
        /// items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetInstitutionalName2Rates(
            RatesDictionaryArgs args = null)
        {
            //half of the diff half there
            var sumOfRates = args?.SumOfRates ?? Math.Round(Math.Abs(_allOtherAssetEquity / 2 / _totalEquity), 5);
            var derivativeSlope = args?.DerivativeSlope ?? -0.2D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Assets, AssetGroupNames.INSTITUTIONAL, sumOfRates, null,
                derivativeSlope, "Certificate of Deposit", "Insurance Policies", "Money Market", "Annuity",
                "Credit Union", "Profit Sharing", "Safe Deposit Box", "Trusts", "Brokerage", "Partnerships",
                "Fellowships", "Escrow", "Stipends", "Royalties");

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }

        /// <summary>
        /// Produces a dictionary of Securities <see cref="WealthBase.DomusOpesDivisions.Assets"/> 
        /// items by names to a portion of the total sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetSecuritiesName2Rates(RatesDictionaryArgs args = null)
        {
            //half of the diff half there
            var sumOfRates = args?.SumOfRates ?? Math.Round(Math.Abs(_allOtherAssetEquity / 2 / _totalEquity), 5);
            var derivativeSlope = args?.DerivativeSlope ?? -0.4D;
            var directAssignNames2Rates = args?.DirectAssignNames2Rates;

            ReconcileDiffsInSums(ref sumOfRates, directAssignNames2Rates);

            var dk = GetNames2RandomRates(DomusOpesDivisions.Assets, AssetGroupNames.SECURITIES, sumOfRates, null,
                derivativeSlope, "Derivatives");

            if (directAssignNames2Rates != null && directAssignNames2Rates.Any())
                dk = ReassignRates(dk, directAssignNames2Rates, derivativeSlope);

            return dk;
        }
        #endregion

    }
}
