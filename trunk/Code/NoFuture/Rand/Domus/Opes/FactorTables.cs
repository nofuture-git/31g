using System;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// src http://www.census.gov/people/wealth/files/Debt_Tables_2011.xlsx
    ///     http://www.census.gov/people/wealth/files/Wealth_Tables_2011.xlsx
    /// </summary>
    [Serializable]
    public enum FactorTables
    {
        HomeDebt,
        VehicleDebt,
        CreditCardDebt,
        CheckingAccount,
        SavingsAccount,
        NetWorth,
        VehicleEquity,
        HomeEquity
    }
}