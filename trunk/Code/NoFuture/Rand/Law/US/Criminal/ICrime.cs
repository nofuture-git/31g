using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US.Criminal.Elements;

namespace NoFuture.Rand.Law.US.Criminal
{
    /// <inheritdoc cref="ILegalConcept"/>
    /// <summary>
    /// an act committed in violation of a law prohibiting it, or omitted in violation of a law ordering it
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Nullum crimen sine lege, nulla poena sine crimen
    /// Criminal Law concerns the rights and duties of individuals in society.
    /// Criminal Procedure concerns individual's rights during the criminal process
    /// ]]>
    /// </remarks>
    public interface ICrime : ILegalConcept, IComparable
    {
        /// <summary>
        /// operation of an act or omission to act and intention or criminal negligence
        /// </summary>
        Concurrence Concurrence { get; }

        IList<IElement> AdditionalElements { get; }

    }
}
