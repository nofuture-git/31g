using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Act
{
    /// <summary>
    /// To be a crime there must have been an action which was voluntary
    /// </summary>
    [Aka("criminal act")]
    public class ActusReus : LegalConcept, IActusReus, IElement
    {
        /// <summary>
        /// There must always be something willfully
        /// </summary>
        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;

        /// <summary>
        /// There must be some outward act or failure to act 
        /// (thoughts, plans, labels, status are not actus reus).
        /// </summary>
        public Predicate<ILegalPerson> IsAction { get; set; } = lp => false;

        /// <summary>
        /// There must be some outward act or failure to act 
        /// (thoughts, plans, labels, status are not actus reus).
        /// </summary>
        public Duty Omission { get; set; } = new Duty();

        /// <inheritdoc cref="IActusReus"/>
        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsVoluntary(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, did not perform voluntarily");
                return false;
            }

            if (Omission != null && Omission.IsValid(persons))
            {
                AddReasonEntryRange(Omission.GetReasonEntries());
                return true;
            }

            if (!IsAction(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, has not operation of an act");
                return false;
            }

            return true;
        }
    }
}
