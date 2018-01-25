using System;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    [Flags]
    public enum PropertyCrime : short
    {
        Burglary = 1,
        Theft = 2,
        GTA = 4
    }
}