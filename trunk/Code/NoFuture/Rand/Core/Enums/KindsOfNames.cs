using System;

namespace NoFuture.Rand.Core.Enums
{
    [Serializable]
    [Flags]
    public enum KindsOfNames : UInt32
    {
        None = 0,
        Legal = 1,
        First = 2,
        Surname = 4,
        Abbrev = 8,
        Group = 16,
        Colloquial = 32,
        Mother = 64,
        Father = 128,
        Adopted = 256,
        Biological = 512,
        Spouse = 1024,
        Middle = 2048,
        Former = 4096,
        Step = 8192,
        Maiden = 16384,
        Technical = 32768
    }
}