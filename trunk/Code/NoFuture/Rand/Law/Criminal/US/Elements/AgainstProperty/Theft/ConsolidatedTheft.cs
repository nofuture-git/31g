using System;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Theft
{
    /// <summary>
    /// Explanatory Note for Sections 223.1-223.9 of Model Penal Code
    /// </summary>
    public abstract class ConsolidatedTheft : PropertyConsent, IActusReus
    {
        public ConsolidatedTheft() : base(ExtensionMethods.Defendant) { }

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

            if (SubjectProperty.IsEntitledTo(defendant))
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
                SubjectProperty.IsInPossessionOf = lp => lp.IsSamePerson(defendant);

            var isTitled = IsAcquiredTitle(defendant);
            if (isTitled)
                SubjectProperty.IsEntitledTo = lp => lp.IsSamePerson(defendant);

            if (!isPossess && !isTitled)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsTakenPossession)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsAcquiredTitle)} is false");
                return false;
            }

            return true;
        }
    }
}
