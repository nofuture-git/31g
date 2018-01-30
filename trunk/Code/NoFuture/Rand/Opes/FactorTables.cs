using System;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// Defines the kinds of factors effecting wealth.
    /// </summary>
    [Serializable]
    public enum FactorTables
    {
        HomeDebt,
        VehicleDebt,
        OtherDebt,
        CreditCardDebt,
        CheckingAccount,
        SavingsAccount,
        NetWorth,
        VehicleEquity,
        HomeEquity
    }
}