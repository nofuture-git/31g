using System;

namespace NoFuture.Rand.Data.Sp
{
    [Flags]
    [Serializable]
    public enum FormOfCredit : short
    {
        None = 0,
        Revolving = 1,
        Installment = 2,
        Mortgage = 4,
        Fixed = 8,
    }
    /// <summary>
    /// ISO 4217 Currency Codes
    /// </summary>
    [Serializable]
    public enum CurrencyAbbrev
    {
        USD,
        EUR,
        GBP,
        JPY,
        AUD,
        CAD,
        BRL,
        MXN,
        CNY,
    }
    [Serializable]
    public enum SpStatus
    {
        Closed,
        Current,
        Late,
        NoHistory,
        Overdrawn
    }
    [Serializable]
    public enum PastDue
    {
        Thirty,
        Sixty,
        Ninety,
        HundredAndEighty
    }
    [Serializable]
    public enum ClosedCondition
    {
        ClosedWithZeroBalance,
        VoluntarySurrender,
        ClosureSurrender,
        Repossession,
        ChargeOff,
        Foreclosure
    }

    [Serializable]
    public struct TradelineClosure
    {
        public DateTime ClosedDate;
        public ClosedCondition Condition;
    }
}
