using System;

namespace NoFuture.Rand.Core.Enums
{
    [Serializable]
    [Flags]
    public enum KindsOfNames : UInt32
    {
        None = 1,
        Legal = 2,
        First = 4,
        Surname = 8,
        Abbrev = 16,
        Group = 32,
        Colloquial = 64,
        Mother = 128,
        Father = 256,
        Adopted = 512,
        Biological = 1024,
        Spouse = 2048,
        Middle = 4096,
        Former = 8192,
        Step = 16384,
        Maiden = 32768,
        Technical = 65536
    }
}