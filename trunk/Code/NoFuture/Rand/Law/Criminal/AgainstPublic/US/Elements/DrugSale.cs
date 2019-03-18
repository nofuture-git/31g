using System;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
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
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (Offer == null)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Offer)} is null");
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
