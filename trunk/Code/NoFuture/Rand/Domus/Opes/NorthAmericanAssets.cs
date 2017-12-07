using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IRebus" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    public class NorthAmericanAssets : WealthBase, IRebus
    {
        #region constants

        internal const string ASSET_GRP_REAL_PROPERTY = "Real Property";
        internal const string ASSET_GRP_PERSONAL_PROPERTY = "Personal Property";
        internal const string ASSET_GRP_INSTITUTIONAL = "Institutional";
        internal const string ASSET_GRP_SECURITIES = "Securities";
        #endregion

        #region fields
        private readonly HashSet<Pondus> _assets = new HashSet<Pondus>();
        private readonly DateTime _startDate;

        private readonly double _randCheckingAcctAmt;
        private readonly double _randSavingsAcctAmt;
        private readonly double _randHomeEquity;
        private readonly double _randCarEquity;

        private readonly double _randCcDebt;
        private readonly double _randHomeDebt;
        private readonly double _randCarDebt;

        private readonly double _randNetWorth;
        private readonly double _diffInNetWorth;

        #endregion

        #region ctors
        public NorthAmericanAssets(NorthAmerican american, AssestOptions options, DateTime? startDate = null) : base(
            american, options?.IsRenting ?? false)
        {
            options = options ?? new AssestOptions();

            _startDate = startDate ?? GetYearNeg(-1);
            _randCheckingAcctAmt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.CheckingAccount,
                Factors.CheckingAcctFactor, DF_STD_DEV_PERCENT);
            _randSavingsAcctAmt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.SavingsAccount,
                Factors.SavingsAcctFactor, DF_STD_DEV_PERCENT);

            _randHomeEquity = options.IsRenting
                    ? 0D
                    : NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeEquity, Factors.HomeEquityFactor,
                        DF_STD_DEV_PERCENT);

            _randCarEquity = options.HasVehicles
                    ? NorthAmericanFactors.GetRandomFactorValue(FactorTables.VehicleEquity,
                        Factors.VehicleEquityFactor, DF_STD_DEV_PERCENT)
                    : 0D;

            _randCcDebt = options.HasCreditCards
                    ? NorthAmericanFactors.GetRandomFactorValue(FactorTables.CreditCardDebt,
                        Factors.CreditCardDebtFactor,
                        DF_STD_DEV_PERCENT)
                    : 0D;

            _randHomeDebt = options.IsRenting
                    ? 0D
                    : NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, Factors.HomeDebtFactor,
                        DF_STD_DEV_PERCENT);

            _randCarDebt = options.HasVehicles
                    ? NorthAmericanFactors.GetRandomFactorValue(FactorTables.VehicleDebt,
                        Factors.VehicleDebtFactor, DF_STD_DEV_PERCENT)
                    : 0D;

            _randNetWorth =
                NorthAmericanFactors.GetRandomFactorValue(FactorTables.NetWorth, Factors.NetWorthFactor,
                    DF_STD_DEV_PERCENT);

            var calcNetWorth = _randCheckingAcctAmt + _randSavingsAcctAmt + _randHomeEquity + _randCarEquity -
                               Math.Abs(_randCcDebt + _randHomeDebt + _randCarDebt);

            //this is what would be spread across other assets
            _diffInNetWorth = _randNetWorth - calcNetWorth;
        }
        #endregion

        #region inner types
        public class AssestOptions
        {
            public bool IsRenting { get; set; }
            public int NumberOfVehicles { get; set; }
            public int NumberOfCreditCards { get; set; }

            public bool HasCreditCards => NumberOfCreditCards > 0;
            public bool HasVehicles => NumberOfCreditCards > 0;
        }
        #endregion

        #region properties

        public Pondus[] CurrentAssets => GetCurrent(Assets);

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

        protected virtual List<Pondus> Assets
        {
            get
            {
                var e = _assets.ToList();
                e.Sort(Comparer);
                return e.ToList();
            }
        }

        #endregion

    }
}
