using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
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
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsOrderedToDisperse(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsOrderedToDisperse)} is false");
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
