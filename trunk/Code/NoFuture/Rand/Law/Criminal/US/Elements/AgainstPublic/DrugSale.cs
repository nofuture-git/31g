using System;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.Criminal.US.Terms;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPublic
{
    /// <summary>
    /// The illegal sale of drugs
    /// </summary>
    public class DrugSale: LegalConcept, IControlledSubstance, IBargain<ILegalProperty, IDrugSchedule>
    {
        public IDrugSchedule Offer { get; set; } = new ScheduleI();
        public Func<IDrugSchedule, ILegalProperty> Acceptance { get; set; }
        public IAssent Assent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (Offer == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(Offer)} is null");
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
            var isValidIntent = criminalIntent is Purposely || criminalIntent is SpecificIntent;
            if (!isValidIntent)
            {
                AddReasonEntry($"{nameof(DrugSale)} requires intent of {nameof(Purposely)} or {nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }
    }
}
