using System;

namespace NoFuture.Rand.Opes.US
{
    /// <summary>
    /// Defines the kinds of factors effecting wealth.
    /// </summary>
    [Serializable]
    public enum AmericanFactorTables
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