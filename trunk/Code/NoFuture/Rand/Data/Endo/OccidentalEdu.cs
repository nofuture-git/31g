using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    [Flags]
    public enum OccidentalEdu : short
    {
        None = 0,
        Some = 1,
        Grad = 2,
        HighSchool = 16,
        Assoc = 32,
        Bachelor = 64,
        Master = 128,
        Doctorate = 256
    }
}