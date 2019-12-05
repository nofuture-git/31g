using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// to interpose a disinterest magistrate between the police and the
    /// individual whom they seek to search or seize.
    /// </summary>
    public class SearchWarrant : LegalConcept
    {
        public Func<ILegalPerson[], ILegalPerson> GetIssuerOfWarrant { get; set; } = lps => null;

        /// <summary>
        /// The place and items of the person(s) being seized
        /// </summary>
        public Func<IVoca> GetObjectiveOfSearch { get; set; } = () => null;

        /// <summary>
        /// Required reasoning for the search
        /// </summary>
        public ILegalConcept ProbableCause { get; set; }

        /// <summary>
        /// The issuer must be a neutral and detached magistrate.
        /// Who is part of the judicial apparatus and not a member of
        /// law enforcement.
        /// </summary>
        public Predicate<ILegalPerson> IsNeutralAndDetached { get; set; } = lp => lp is ICourtOfficial;

        /// <summary>
        /// There must be present to the magistrate an adequate
        /// showing probable cause (either to search or arrest)
        /// supported by oath or affirmation.
        /// </summary>
        public Predicate<ILegalPerson> IsCapableDetermineProbableCause { get; set; } = lp => lp is ICourtOfficial;

        /// <summary>
        /// The warrant must describe with particularity the place to be
        /// searched and the items or persons to be seized
        /// </summary>
        public Predicate<IVoca> IsDescribedWithParticularity { get; set; } = r => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (ProbableCause == null)
            {
                AddReasonEntry($"{nameof(ProbableCause)} is unassigned");
                return false;
            }

            if (!ProbableCause.IsValid(persons))
            {
                AddReasonEntry($"{nameof(ProbableCause)} {nameof(IsValid)} is false");
                AddReasonEntryRange(ProbableCause.GetReasonEntries());
                return false;
            }

            var magistrate = GetIssuerOfWarrant(persons);
            if (magistrate == null)
            {
                AddReasonEntry($"{nameof(GetIssuerOfWarrant)} returned nothing");
                return false;
            }

            var magistrateTitle = magistrate.GetAllKindsOfNames();

            var isCourtOfficial = magistrate is ICourtOfficial;
            if (!isCourtOfficial)
            {
                AddReasonEntry($"{magistrateTitle} {magistrate.Name}, is not of " +
                               $"type {nameof(ICourtOfficial)}");
                return false;
            }

            if (!IsNeutralAndDetached(magistrate))
            {
                AddReasonEntry($"{magistrateTitle} {magistrate.Name}, " +
                               $"{nameof(IsNeutralAndDetached)} is false");
                return false;
            }

            if (!IsCapableDetermineProbableCause(magistrate))
            {
                AddReasonEntry($"{magistrateTitle} {magistrate.Name}, " +
                               $"{nameof(IsCapableDetermineProbableCause)} is false");
                return false;
            }

            var objective = GetObjectiveOfSearch();
            if (objective == null)
            {
                AddReasonEntry($"{nameof(GetObjectiveOfSearch)} returned nothing");
                return false;
            }

            if (!IsDescribedWithParticularity(objective))
            {
                AddReasonEntry($"{objective.Name}, {nameof(IsDescribedWithParticularity)} " +
                               $"is false");
                return false;
            }

            return true;
        }

    }
}
