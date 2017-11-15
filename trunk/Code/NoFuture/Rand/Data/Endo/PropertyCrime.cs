using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    [Flags]
    public enum PropertyCrime : short
    {
        Burglary = 1,
        Theft = 2,
        Gta = 4
    }
}