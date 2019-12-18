using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
{
    public class Pimping : LegalConcept, IPossession
    {
        public Predicate<ILegalPerson> IsKnowinglyProcured { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsKnowinglyReceived { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsFromProstitute { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var procured = IsKnowinglyProcured(defendant);
            var received = IsKnowinglyReceived(defendant);

            if (!procured && !received)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsKnowinglyProcured)} is false");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsKnowinglyReceived)} is false");
                return false;
            }

            if (!IsFromProstitute(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsFromProstitute)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is Knowingly || criminalIntent is GeneralIntent;
            if (!isValid)
            {
                AddReasonEntry($"{nameof(Pimping)} requries intent {nameof(Knowingly)} or {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }

    }
}
