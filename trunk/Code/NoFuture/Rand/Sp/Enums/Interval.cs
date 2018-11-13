using System;

namespace NoFuture.Rand.Sp.Enums
{
    [Serializable]
    public enum Interval
    {
        None,
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
