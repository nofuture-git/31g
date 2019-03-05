using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// when a peace officer or public servant orders a group
    /// participating in disorderly conduct likely to cause
    /// substantial harm, serious annoyance, or alarm to do so
    /// </summary>
    public class FailureToDisperse : UnlawfulAssembly
    {
        //TODO is this only applicable to groups? should this extend DisorderlyConduct directly?
        public Predicate<ILegalPerson> IsOrderedToDisperse { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsOrderedToDisperse(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsOrderedToDisperse)} is false");
                return false;
            }

            return base.IsValid(persons);
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntent = criminalIntent is Knowingly || criminalIntent is GeneralIntent;
            if (!validIntent)
            {
                AddReasonEntry($"{nameof(FailureToDisperse)} requires intent " +
                               $"of {nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
