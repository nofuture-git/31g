using System;

namespace NoFuture.Rand.Data.Sp.Enums
{
    [Serializable]
    public enum Interval
    {
        OnceOnly,
        Hourly,
        Daily,
        Weekly,
        BiWeekly,
        SemiMonthly,
        Monthly,
        Quarterly,
        SemiAnnually,
        Annually
    }
}
