using System;

namespace NoFuture.Rand.Sp.Enums
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
        SemiQuarterly,
        Quarterly,
        SemiAnnually,
        Annually,
        Varied,
    }
}
