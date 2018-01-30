using System;

namespace NoFuture.Rand.Sp.Enums
{
    [Serializable]
    public enum ClosedCondition
    {
        None,
        ClosedWithZeroBalance,
        VoluntarySurrender,
        ClosureSurrender,
        Repossession,
        ChargeOff,
        Foreclosure
    }
}