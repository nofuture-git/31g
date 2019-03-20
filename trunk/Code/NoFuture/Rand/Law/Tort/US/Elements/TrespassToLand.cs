using System;
using NoFuture.Rand.Law.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class TrespassToLand : TrespassBase
    {
        /// <summary>
        /// E.G. dust, noise, vibrations, sound waves, electromagnetic radiation, etc
        /// </summary>
        public Predicate<ILegalPerson> IsIntangibleEntry { get; set; } = lp => false;

        public Damage PropertyDamage { get; set; }

        public override void ClearReasons()
        {
            PropertyDamage?.ClearReasons();
            base.ClearReasons();
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeasor = persons.Tortfeasor();
            if (tortfeasor == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            var obviousEntry = IsTangibleEntry(tortfeasor);

            var subtleEntry = IsIntangibleEntry(tortfeasor) && IsPhysicalDamage(persons);

            if (!obviousEntry && !subtleEntry)
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(IsTangibleEntry)} is false");
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(IsIntangibleEntry)} " +
                               $"and {nameof(PropertyDamage)} are both false");
                return false;
            }

            return true;
        }

        protected virtual bool IsPhysicalDamage(ILegalPerson[] persons)
        {
            if (PropertyDamage == null)
                return false;
            PropertyDamage.GetSubjectPerson = PropertyDamage.GetSubjectPerson ?? ExtensionMethods.Tortfeasor;
            PropertyDamage.SubjectProperty = PropertyDamage.SubjectProperty ?? SubjectProperty;
            var rslt = PropertyDamage.IsValid(persons);
            AddReasonEntryRange(PropertyDamage.GetReasonEntries());
            return rslt;
        }
    }
}
