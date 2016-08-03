using System;
using System.Collections.Generic;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Domus
{
    /// <summary>
    /// http://www.census.gov/people/wealth/files/Debt_Tables_2011.xlsx
    /// http://www.census.gov/people/wealth/files/Wealth_Tables_2011.xlsx
    /// </summary>
    public class Opes
    {
        public IEnumerable<Savings> SavingAccounts { get; } = new List<Savings>();
        public IEnumerable<Checking> CheckingAccounts { get; } = new List<Checking>();
        public IEnumerable<IAsset> OtherAssets { get; } = new List<IAsset>();

        public IEnumerable<IReceivable> HomeDebt { get; } = new List<IReceivable>();
        public IEnumerable<IReceivable> VehicleDebt { get; } = new List<IReceivable>();
        public IEnumerable<IReceivable> CreditCardDebt { get; } = new List<IReceivable>();
        public IEnumerable<ILoan> Loans { get; } = new List<ILoan>();
        public IEnumerable<IReceivable> OtherDebts { get; } = new List<IReceivable>();
    }

    public class NorthAmericanWealth : Opes
    {
        private readonly NorthAmerican _amer;
        public NorthAmericanWealth(NorthAmerican american)
        {
            _amer = american;
            CreditScore = new PersonalCreditScore(american);
        }
        public CreditScore CreditScore { get; }

        /// <summary>
        /// Get the linear eq of the city if its found otherwise
        /// defaults to the state.
        /// </summary>
        /// <returns></returns>
        protected internal LinearEquation GetAvgEarningPerYear()
        {
            var ca = _amer.Address?.HomeCityArea as UsCityStateZip;
            return ca?.AverageEarnings ?? ca?.State?.GetStateData()?.AverageEarnings;
        }
    }

}
