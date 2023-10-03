using System;

namespace NoFuture.Law.Procedure.Criminal
{
    /// <summary>
    /// to interpose a disinterest magistrate between the police and the
    /// individual whom they seek to search or seize.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWarrant<T> : ILegalConcept
    {
        Func<ILegalPerson[], ILegalPerson> GetIssuerOfWarrant { get; set; }

        /// <summary>
        /// The place and items of the person(s) being seized
        /// </summary>
        Func<T> GetObjectiveOfSearch { get; set; }

        /// <summary>
        /// Required reasoning for the search
        /// </summary>
        ILegalConcept ProbableCause { get; set; }

        /// <summary>
        /// The issuer must be a neutral and detached magistrate.
        /// Who is part of the judicial apparatus and not a member of
        /// law enforcement.
        /// </summary>
        Predicate<ILegalPerson> IsIssuerNeutralAndDetached { get; set; }

        /// <summary>
        /// There must be present to the magistrate an adequate
        /// showing probable cause (either to search or arrest)
        /// supported by oath or affirmation.
        /// </summary>
        Predicate<ILegalPerson> IsIssuerCapableDetermineProbableCause { get; set; }

        /// <summary>
        /// The warrant must describe with particularity the place to be
        /// searched and the items or persons to be seized
        /// </summary>
        Predicate<T> IsObjectiveDescribedWithParticularity { get; set; }
    }
}
