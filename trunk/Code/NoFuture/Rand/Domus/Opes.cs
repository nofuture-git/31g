using System.Collections.Generic;
using NoFuture.Rand.Data.Sp;

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
        public NorthAmericanWealth(NorthAmerican american)
        {
            CreditScore = new PersonalCreditScore(american);
        }
        public CreditScore CreditScore { get; }

    }

}
