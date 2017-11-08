using System;

namespace NoFuture.Rand.Core.Enums
{
    [Serializable]
    [Flags]
    public enum KindsOfNames : short
    {
        None = 0,
        Former = 1,
        First = 2,
        Surname = 4,
        Abbrev = 8,
        Maiden = 16,
        Mother = 32,
        Father = 64,
        Adopted = 128,
        Biological = 264,
        Spouse = 512,
        Middle = 1024,
        Legal = 2048,
        Step = 4098
    }
}