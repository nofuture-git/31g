using System;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Domus.Sp //Sequere pecuniam
{
    public interface ILoan
    {
        Identifier Id { get; set; }
        TradeLine TradeLine { get; set; }
        float Rate { get; set; }
        IFirm Lender { get; set; }
    }

    [Serializable]
    public class Loan : ILoan
    {
        public Identifier Id { get; set; }
        public TradeLine TradeLine { get; set; }
        public float Rate { get; set; }
        public IFirm Lender { get; set; }
    }

    [Flags]
    [Serializable]
    public enum FormOfCredit : short
    {
        None = 0,
        Revolving = 1,
        Installment = 2,
        Mortgage = 4,
    }

    [Serializable]
    public enum LoanStatus
    {
        Closed,
        Current,
        Late,
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
