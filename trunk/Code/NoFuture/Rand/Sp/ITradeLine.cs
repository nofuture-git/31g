using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="ITransactionable" />
    /// <inheritdoc cref="ITempore" />
    /// <inheritdoc cref="IObviate" />
    /// <inheritdoc cref="IValoresTempus" />
    /// <summary>
    /// Represents the item reported to a Credit Bureau
    /// </summary>
    public interface ITradeLine : ITempore, ITransactionable, IObviate, IValoresTempus
    {
        /// <summary>
        /// The kind of credit associated to this tradeline, if any.
        /// </summary>
        FormOfCredit? FormOfCredit { get; set; }

        /// <summary>
        /// The complete history of the tradeline.
        /// </summary>
        IBalance Balance { get; }

        /// <summary>
        /// The recurring frequency of activity of the tradeline, if any.
        /// </summary>
        TimeSpan? DueFrequency { get; set; }

        /// <summary>
        /// The condition on which the tradeline was closed, if any.
        /// </summary>
        ClosedCondition? Closure { get; set; }

        /// <summary>
        /// The number of days in a year used to calc per-diem interest
        /// </summary>
        double DaysPerYear { get; set; }

        /// <summary>
        /// Gets the average value per <see cref="duration"/> or <see cref="DueFrequency"/> or Annual
        /// </summary>
        /// <returns></returns>
        Pecuniam AveragePerDueFrequency(TimeSpan? duration = null);
    }
}