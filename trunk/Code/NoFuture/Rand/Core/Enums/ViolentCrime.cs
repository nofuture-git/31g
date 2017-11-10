using System;

namespace NoFuture.Rand.Core.Enums
{
    [Serializable]
    [Flags]
    public enum ViolentCrime : short
    {
        Murder = 1,
        Rape = 2,
        Robbery = 4,
        Assault = 8
    }
}