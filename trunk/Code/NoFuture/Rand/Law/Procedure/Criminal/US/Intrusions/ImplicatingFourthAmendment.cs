using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions
{
    public abstract class ImplicatingFourthAmendment<T> : LegalConcept, IIntrusion
    {
        public virtual Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; } = lps => lps.Suspect();
        public virtual Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        public ILegalConcept ProbableCause { get; set; }
        public IWarrant<T> Warrant { get; set; }

        /// <summary>
        /// Tests if probable cause is both valid and is of considered an emergency exception
        /// </summary>
        protected internal virtual bool IsProbableCauseExigentCircumstances(ILegalPerson[] persons)
        {
            var police = this.LawEnforcement(persons, GetLawEnforcement);
            if (police == null)
                return false;

            var policeTitle = police.GetLegalPersonTypeName();

            ProbableCause = ProbableCause ?? Warrant?.ProbableCause;
            if (ProbableCause == null)
            {
                AddReasonEntry($"{nameof(ProbableCause)} is unassigned");
                return false;
            }

            var isExigentCircumstances = ProbableCause is ExigentCircumstances;
            if (isExigentCircumstances && ProbableCause.IsValid(persons))
            {
                AddReasonEntry($"{policeTitle} {police.Name}, {nameof(ProbableCause)} " +
                               $"is type {nameof(ExigentCircumstances)}");
                AddReasonEntryRange(ProbableCause.GetReasonEntries());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tests if the Warrant is applicable to the suspect and is valid
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        protected internal virtual bool IsWarrantValid(ILegalPerson[] persons)
        {
            var suspect = this.Suspect(persons, GetSuspect);
            if (suspect == null)
                return false;

            var suspectTitle = suspect.GetLegalPersonTypeName();

            if (Warrant == null)
            {
                AddReasonEntry($"{nameof(Warrant)} is unassigned");
                return false;
            }

            if (Warrant.ProbableCause == null)
                Warrant.ProbableCause = ProbableCause;

            if (!Warrant.IsValid(persons))
            {
                AddReasonEntry($"{nameof(Warrant)} {nameof(IsValid)} is false");
                AddReasonEntryRange(Warrant.GetReasonEntries());
                return false;
            }

            var objectiveOfSearch = Warrant.GetObjectiveOfSearch();

            if (objectiveOfSearch == null)
            {
                AddReasonEntry($"{nameof(Warrant)} {nameof(Warrant.GetObjectiveOfSearch)} returned nothing");
                return false;
            }

            var personOfSearch = objectiveOfSearch as ILegalPerson;
            if (personOfSearch == null)
            {
                AddReasonEntry($"{nameof(Warrant)} {nameof(Warrant.GetObjectiveOfSearch)} returned " +
                               $"type {objectiveOfSearch.GetType().Name} expected type {nameof(ILegalPerson)}");

            }

            var lpOnWarrantTitle = personOfSearch.GetLegalPersonTypeName();

            if (!NamesEqual(suspect, personOfSearch))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name} to {lpOnWarrantTitle} {personOfSearch.Name}, " +
                               $"{nameof(NamesEqual)} is false");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Allows for class level overrides -default is the static VocaBase.Equals
        /// </summary>
        public virtual bool NamesEqual(IVoca voca1, IVoca voca2)
        {
            return VocaBase.Equals(voca1, voca2);
        }
    }
}
