using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// Trespass to personal or real property
    /// </summary>
    public abstract class TrespassToProperty : TortTrespass
    {
        protected TrespassToProperty(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }
        protected TrespassToProperty() : base(ExtensionMethods.Tortfeasor) { }

        protected internal Damage PropertyDamage => Injury as Damage;

        protected internal virtual bool IsPhysicalDamage(ILegalPerson[] persons)
        {
            if (PropertyDamage == null)
            {
                AddReasonEntry($"The {nameof(Injury)} is not of type {nameof(Damage)}");
                return false;
            }

            PropertyDamage.GetSubjectPerson = PropertyDamage.GetSubjectPerson ?? ExtensionMethods.Tortfeasor;
            PropertyDamage.SubjectProperty = PropertyDamage.SubjectProperty ?? SubjectProperty;
            var rslt = PropertyDamage.IsValid(persons);
            AddReasonEntryRange(PropertyDamage.GetReasonEntries());

            if (Causation == null)
            {
                AddReasonEntry($"{nameof(Causation)} is unassigned");
                return rslt;
            }

            Causation.GetSubjectPerson = Causation.GetSubjectPerson ?? ExtensionMethods.Tortfeasor;
            rslt = rslt && Causation.IsValid(persons);
            AddReasonEntryRange(Causation.GetReasonEntries());

            return rslt;
        }
    }
}
