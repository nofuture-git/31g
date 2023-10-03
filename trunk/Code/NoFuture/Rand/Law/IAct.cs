using System;

namespace NoFuture.Law
{
    /// <summary>
    /// a duty in acting to behave reasonably in the actions one does undertake
    /// </summary>
    public interface IAct : ILegalConcept
    {
        /// <summary>
        /// There must always be something willfully
        /// </summary>
        Predicate<ILegalPerson> IsVoluntary { get; set; }

        /// <summary>
        /// There must be some outward act or failure to act 
        /// (thoughts, plans, labels, status are not actions).
        /// </summary>
        Predicate<ILegalPerson> IsAction { get; set; }
    }
}
