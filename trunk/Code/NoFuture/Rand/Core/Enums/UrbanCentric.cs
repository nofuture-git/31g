using System;

namespace NoFuture.Rand.Core.Enums
{
    [Flags]
    [Serializable]
    public enum UrbanCentric : short
    {
        City = 0,
        Suburb = 1,
        Town = 2,
        Rural = 4,
        Large = 8,
        Midsize = 16,
        Small = 32,
        Distant = 64,
        Fringe = 128,
        Remote = 256
    }
}