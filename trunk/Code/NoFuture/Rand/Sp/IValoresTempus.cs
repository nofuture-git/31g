using System;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Latin for values of time.
    /// Considers time as a discrete set of indivisible units (&quot;ticks&quot;).
    /// <see cref="Value"/> is participatory in-time and is, therefore, a function of time.
    /// </summary>
    public interface IValoresTempus
    {
        /// <summary>
        /// The value at the current system time
        /// </summary>
        Pecuniam Value { get; }

        /// <summary>
        /// Get the current value of this asset at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetValueAt(DateTime dt);

    }
}