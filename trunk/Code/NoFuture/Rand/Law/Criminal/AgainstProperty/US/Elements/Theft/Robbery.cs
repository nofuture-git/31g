using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    public class Robbery : ByTaking, IBattery, IAssault
    {
        public Predicate<ILegalPerson> IsByForce { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsByThreatOfForce { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;
            var byForce = IsByForce(defendant);
            var byThreat = IsByThreatOfForce(defendant);

            if (!byForce && !byThreat)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByForce)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfForce)} is false");
                return false;
            }

            return true;
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntend = criminalIntent is Purposely || criminalIntent is SpecificIntent;

            if (!validIntend)
            {
                AddReasonEntry($"{nameof(ByTaking)} requires intent " +
                               $"of {nameof(Purposely)}, {nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }
    }
}
