using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// Offering, agreeing, or engaging in sexual conduct for money, property, or anything of value
    /// </summary>
    public class Prostitution : CriminalBase, IActusReus, IBargain<ILegalProperty, ISexBipartitie>
    {
        public ISexBipartitie Offer { get; set; }
        public Func<ISexBipartitie, ILegalProperty> Acceptance { get; set; }
        public IAssent Assent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!Offer.IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Offer)} " +
                               $"{nameof(Offer.IsSexualIntercourse)} is false");
                return false;
            }

            var acceptance = Acceptance(Offer);
            if (acceptance?.PropretyValue == null)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Acceptance)} " +
                               $"or {nameof(acceptance.PropretyValue)} is null");
                return false;
            }

            var isAgreed = Assent?.IsValid(persons) ?? false;
            if (!isAgreed)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Assent)} is invalid");
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
