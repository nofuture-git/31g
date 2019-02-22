using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    [Note("rapere, latin for steal or seize")]
    public class Rape : CriminalBase, IActusReus
    {
        /// <summary>
        /// loosely defined as vaginal, anal or oral penetration of by somebody else body part (penis, finger, etc.)
        /// </summary>
        public Predicate<ILegalPerson> IsSexualIntercourse { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByForce { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByThreatOfForce { get; set; } = lp => false;

        public Consent Consent { get; set; } = new Consent();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsSexualIntercourse)} is false");
                return false;
            }

            var isByForce = IsByForce(defendant);
            var isByThreatOfForce = IsByThreatOfForce(defendant);
            if (isByForce || isByThreatOfForce)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByForce)} is {isByForce}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfForce)} is {isByThreatOfForce}");
                return true;
            }
            var isConsented = Consent?.IsValid(persons) ?? false;

            AddReasonEntryRange(Consent?.GetReasonEntries());
            return !isConsented;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
