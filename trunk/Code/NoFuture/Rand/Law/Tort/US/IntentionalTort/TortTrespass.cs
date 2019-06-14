using System;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Trespass;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    public abstract class TortTrespass : TrespassBase
    {
        protected TortTrespass(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

        protected TortTrespass() : this(ExtensionMethods.Tortfeasor) { }

        public Damage PropertyDamage { get; set; }

        public Causation Causation { get; set; }

        public override void ClearReasons()
        {
            PropertyDamage?.ClearReasons();
            base.ClearReasons();
        }

        protected internal virtual bool IsPhysicalDamage(ILegalPerson[] persons)
        {
            if (PropertyDamage == null)
            {
                AddReasonEntry($"{nameof(PropertyDamage)} is unassigned");
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
