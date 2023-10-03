using System;

namespace NoFuture.Law
{
    /// <summary>
    /// Any type which is tied to a discrete range of linear time
    /// </summary>
    public interface ITempore
    {
        /// <summary>
        /// Marks the beginning point in time.
        /// </summary>
        DateTime Inception { get; set; }

        /// <summary>
        /// Marks the end point in time where null is assumed as its on-going.
        /// </summary>
        DateTime? Terminus { get; set; }

        /// <summary>
        /// Test if the given <see cref="dt"/> is within this instance.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        bool IsInRange(DateTime dt);
    }
}
