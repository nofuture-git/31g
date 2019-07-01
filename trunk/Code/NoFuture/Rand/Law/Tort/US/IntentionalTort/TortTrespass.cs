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

        public Causation Causation { get; set; }

        public IInjury Injury { get; set; }

        public override void ClearReasons()
        {
            Injury?.ClearReasons();
            Causation?.ClearReasons();
            base.ClearReasons();
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            if (PropertyOwnerIsSubjectPerson(persons))
                return false;

            if (!WithoutConsent(persons))
                return false;

            if (Injury == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(Injury)} is unassigned");
                return false;
            }

            var rslt = Injury.IsValid(persons);
            AddReasonEntryRange(Injury.GetReasonEntries());

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
