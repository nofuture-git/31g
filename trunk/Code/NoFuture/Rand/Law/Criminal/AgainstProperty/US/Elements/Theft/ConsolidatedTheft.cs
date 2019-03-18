using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// Explanatory Note for Sections 223.1-223.9 of Model Penal Code
    /// </summary>
    public abstract class ConsolidatedTheft : AgitPropertyBase, IActusReus
    {

        /// <summary>
        /// The typical idea of theft as grab and run stealing, or, more 
        /// generally as the idea that control of the property has been taken unlawfully
        /// </summary>
        public Predicate<ILegalPerson> IsTakenPossession { get; set; } = lp => false;

        /// <summary>
        /// When the thief has acquired some kind of legal entitlement over the property
        /// </summary>
        public Predicate<ILegalPerson> IsAcquiredTitle { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (SubjectProperty == null)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(SubjectProperty)} is null");
                return false;
            }

            if (VocaBase.Equals(SubjectProperty.EntitledTo, defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, is the owner of property {SubjectProperty}");
                return false;
            }

            if (!WithoutConsent(persons))
                return false;

            if (!PossessOrEntitle(persons))
                return false;

            return true;
        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }

        protected internal bool PossessOrEntitle(ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            var isPossess = IsTakenPossession(defendant);
            if (isPossess)
                SubjectProperty.InPossessionOf = defendant;

            var isTitled = IsAcquiredTitle(defendant);
            if (isTitled)
                SubjectProperty.EntitledTo = defendant;

            if (!isPossess && !isTitled)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsTakenPossession)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsAcquiredTitle)} is false");
                return false;
            }

            return true;
        }

        protected virtual bool TryGetPossesorOfProperty(out ILegalPerson possessor)
        {
            possessor = SubjectProperty?.InPossessionOf;
            if (possessor == null)
            {
                AddReasonEntry($"the {nameof(SubjectProperty)}, {nameof(SubjectProperty.InPossessionOf)} is null");
                return false;
            }
            return true;
        }
    }
}
