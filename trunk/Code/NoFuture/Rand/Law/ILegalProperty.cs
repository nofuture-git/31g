using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// Everything which is subject to ownership and possess an exchangeable value
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// For future value, it must exist presently as an enforceable contractual right.
    /// A future "mere expectancy" (e.g. anticipated inheritance, advanced degree) falls
    /// outside the limits of ownership and exchangeable value.
    /// ]]>
    /// </remarks>
    public interface ILegalProperty : IVoca, IRationale, IRankable
    {
        /// <summary>
        /// Who is lawfully entitled to the property
        /// </summary>
        Predicate<ILegalPerson> IsEntitledTo { get; set; }

        /// <summary>
        /// Who actually possess control over the property - more than just physical possession.
        /// </summary>
        /// <remarks>
        /// Meaning of possession differs by context where it ranges between communicated intent
        /// to a physical act.  However, once established (by whatever means) it becomes a legally protected interest.
        /// </remarks>
        [Aka("seisin")]
        Predicate<ILegalPerson> IsInPossessionOf { get; set; }

        /// <summary>
        /// The value at the current system time
        /// </summary>
        decimal? CurrentPropertyValue { get; }

        /// <summary>
        /// The value at the given date-time
        /// </summary>
        Func<DateTime?, decimal?> PropertyValue { get; set; }
    }
}
