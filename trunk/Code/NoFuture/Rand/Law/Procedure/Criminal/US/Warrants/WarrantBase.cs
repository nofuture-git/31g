using System;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <inheritdoc cref="IWarrant{T}"/>
    public abstract class WarrantBase<T> : LegalConcept, IWarrant<T>
    {
        public Func<ILegalPerson[], ILegalPerson> GetIssuerOfWarrant { get; set; } = lps => null;

        public abstract Func<T> GetObjectiveOfSearch { get; set; }

        public abstract Predicate<T> IsObjectiveDescribedWithParticularity { get; set; }

        public ILegalConcept ProbableCause { get; set; }

        public Predicate<ILegalPerson> IsIssuerNeutralAndDetached { get; set; } = lp => lp is ICourtOfficial;

        public Predicate<ILegalPerson> IsIssuerCapableDetermineProbableCause { get; set; } = lp => lp is ICourtOfficial;

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

            if (!IsIssuerNeutralAndDetached(magistrate))
            {
                AddReasonEntry($"{magistrateTitle} {magistrate.Name}, " +
                               $"{nameof(IsIssuerNeutralAndDetached)} is false");
                return false;
            }

            if (!IsIssuerCapableDetermineProbableCause(magistrate))
            {
                AddReasonEntry($"{magistrateTitle} {magistrate.Name}, " +
                               $"{nameof(IsIssuerCapableDetermineProbableCause)} is false");
                return false;
            }

            return true;
        }

    }
}
