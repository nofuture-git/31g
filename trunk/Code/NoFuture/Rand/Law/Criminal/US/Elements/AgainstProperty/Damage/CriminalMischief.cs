using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Property.US;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstProperty.Damage
{
    /// <summary>
    /// general case for damaging other property being less heinous than <see cref="Arson"/>
    /// </summary>
    [Aka("vandalism")]
    public class CriminalMischief : PropertyConsent, IActusReus
    {
        public CriminalMischief() : base(ExtensionMethods.Defendant) { }

        public Predicate<ILegalPerson> IsCauseOfDamage { get; set; } = lp => false;

        /// <summary>
        /// damaging, destroying, interfering with, tampering with, etc.
        /// </summary>
        public Predicate<ILegalProperty> IsDamaged { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!WithoutConsent(persons))
                return false;

            if (PropertyOwnerIsSubjectPerson(persons))
                return false;

            if (!IsCauseOfDamage(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsCauseOfDamage)} is false");
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
