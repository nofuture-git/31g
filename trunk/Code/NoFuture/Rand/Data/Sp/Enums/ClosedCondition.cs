using System;

namespace NoFuture.Rand.Data.Sp.Enums
{
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
}