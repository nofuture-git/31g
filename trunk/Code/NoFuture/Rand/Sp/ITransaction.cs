using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a single one-time exchange at an exact moment in-time.
    /// </summary>
    public interface ITransaction : ITransactionId
    {
        Pecuniam Cash { get; }

        /// <summary>
        /// Composes a trace id version of this instance
        /// </summary>
        /// <param name="atTime">
        /// Optional, allows for overriding the property of like name.
        /// </param>
        /// <param name="journalName">
        /// Optional, allow the caller to set the trace in the context of a journal
        /// </param>
        /// <returns></returns>
        TraceTransactionId GetThisAsTraceId(DateTime? atTime = null, IVoca journalName = null);
    }
}