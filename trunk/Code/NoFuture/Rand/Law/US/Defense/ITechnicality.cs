using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// Assumes the government asserted some incorrect fact in their paperwork submitted to the court
    /// </summary>
    public interface ITechnicality : ILegalConcept
    {
        /// <summary>
        /// What the government asserted
        /// </summary>
        ITermCategory AssertedFact { get; set; }

        /// <summary>
        /// What the actual fact-of-the-matter turned out to be.
        /// </summary>
        ITermCategory ActualFact { get; set; }

        /// <summary>
        /// The enclosure that determines if what the <see cref="AssertedFact"/> equals <see cref="ActualFact"/>
        /// </summary>
        Func<ITermCategory, ITermCategory, bool> IsMistaken { get; set; }
    }
}