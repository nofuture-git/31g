using System;

namespace NoFuture.Rand.Gov
{
    [Flags]
    [Serializable]
    public enum NorthAmericanRace : byte
    {
        White = 1,
        Hispanic = 2,
        Black = 4,
        Asian = 8,
        AmericanIndian = 16,
        Pacific = 32,
        Mixed = 64
    }
}