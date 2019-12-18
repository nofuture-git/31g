using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
{
    /// <summary>
    /// Offering, agreeing, or engaging in sexual conduct for money, property, or anything of value
    /// </summary>
    public class Prostitution : LegalConcept, IActusReus, IBargain<ILegalProperty, ISexBilateral>
    {
        public ISexBilateral Offer { get; set; }
        public Func<ISexBilateral, ILegalProperty> Acceptance { get; set; }
        public IAssent Assent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!Offer.IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(Offer)} " +
                               $"{nameof(Offer.IsSexualIntercourse)} is false");
                return false;
            }

            var acceptance = Acceptance(Offer);
            if (acceptance?.CurrentPropertyValue == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(Acceptance)} " +
                               $"or {nameof(acceptance.CurrentPropertyValue)} is null");
                return false;
            }

            var isAgreed = Assent?.IsValid(persons) ?? false;
            if (!isAgreed)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(Assent)} is invalid");
                AddReasonEntryRange(Assent?.GetReasonEntries());
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is StrictLiability || criminalIntent is Purposely ||
                          criminalIntent is Knowingly || criminalIntent is GeneralIntent;
            if (!isValid)
            {
                AddReasonEntry($"{nameof(Prostitution)} requires intent of " +
                               $"{nameof(StrictLiability)}, {nameof(Purposely)}, " +
                               $"{nameof(Knowingly)} or {nameof(GeneralIntent)}");
                return false;
            }
            return true;
        }
    }
}
