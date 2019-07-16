using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Acquisition
{
    public class AdversePossession : PropertyConsent
    {
        public AdversePossession(): base(persons => persons.Disseisor()) { }

        public AdversePossession(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// must physically use the land as a property owner would  with some form of physical change upon the it
        /// </summary>
        public IAct ActualPossession { get; set; }

        /// <summary>
        /// the use of the property is visible and apparent so the true owner is aware of potential conflict
        /// </summary>
        /// <remarks>
        /// Some secret use or taking of something implied guilt and therefore would fail this test
        /// </remarks>
        public IAct OpenNotoriousUse { get; set; }

        /// <summary>
        /// the use must have been continuous for the amount of time so that statue of limitations has expired
        /// </summary>
        public Predicate<ILegalProperty> IsContinuousUse { get; set; } = t => false;

        /// <summary>
        /// use of the land to the exclusion of the true owner
        /// </summary>
        public Predicate<ILegalProperty> IsExclusiveUse { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();
            if (PropertyOwnerIsSubjectPerson(persons))
            {
                return false;
            }
            if (PropertyOwnerIsInPossession(persons))
            {
                return false;
            }

            if (!WithoutConsent(persons))
                return false;

            if (ActualPossession == null || !ActualPossession.IsValid(persons))
            {
                AddReasonEntryRange(ActualPossession?.GetReasonEntries());
                return false;
            }

            if (OpenNotoriousUse == null || !OpenNotoriousUse.IsValid(persons))
            {
                AddReasonEntryRange(OpenNotoriousUse?.GetReasonEntries());
                return false;
            }

            if (!IsContinuousUse(SubjectProperty))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsContinuousUse)} is " +
                               $"false for {SubjectProperty.GetType().Name} '{SubjectProperty.Name}'");
                return false;
            }

            if (!IsExclusiveUse(SubjectProperty))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsExclusiveUse)} is " +
                               $"false for {SubjectProperty.GetType().Name} '{SubjectProperty.Name}'");
                return false;
            }

            //TODO - add unit test for this from text (at page 691)
            return true;
        }
    }
}
