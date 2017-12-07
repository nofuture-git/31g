using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Any type which is tied to a discrete range of time
    /// </summary>
    public interface ITempore
    {
        DateTime Inception { get; set; }
        DateTime? Terminus { get; set; }
        bool IsInRange(DateTime dt);
    }
}
