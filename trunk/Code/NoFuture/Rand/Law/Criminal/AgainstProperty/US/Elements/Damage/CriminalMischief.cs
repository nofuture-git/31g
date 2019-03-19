using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Elements;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Damage
{
    /// <summary>
    /// general case for damaging other property being less heinous than <see cref="Arson"/>
    /// </summary>
    [Aka("vandalism")]
    public class CriminalMischief : AgainstPropertyBase, IActusReus
    {
        public Predicate<ILegalPerson> IsCauseOfDamage { get; set; } = lp => false;

        /// <summary>
        /// damaging, destroying, interfering with, tampering with, etc.
        /// </summary>
        public Predicate<ILegalProperty> IsDamaged { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            if (PropertyOwnerIsDefendant(persons))
                return false;

            if (!IsCauseOfDamage(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsCauseOfDamage)} is false");
                return false;
            }

            if (!IsDamaged(SubjectProperty))
            {
                AddReasonEntry($"property {SubjectProperty?.GetType().Name}, '{SubjectProperty?.Name}', " +
                               $"{nameof(IsDamaged)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
