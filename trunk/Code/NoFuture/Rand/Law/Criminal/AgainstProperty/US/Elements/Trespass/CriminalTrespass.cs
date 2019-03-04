using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Trespass
{
    public class CriminalTrespass : AgitPropertyBase, IActusReus
    {
        /// <summary>
        /// partial or complete intrusion of either the defendant, the defendant&apos;s body part or a tool or instrument
        /// </summary>
        public Predicate<ILegalPerson> IsEntry { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            if (!IsEntry(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsEntry)} is false");
                return false;
            }

            return true;
        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntend = criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!validIntend)
            {
                AddReasonEntry($"{nameof(CriminalTrespass)} requires intent " +
                               $"{nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
