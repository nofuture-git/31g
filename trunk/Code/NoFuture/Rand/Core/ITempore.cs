using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Any type which is tied to a discrete range of time
    /// </summary>
    public interface ITempore
    {
        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; }
        bool IsInRange(DateTime dt);
    }
}
