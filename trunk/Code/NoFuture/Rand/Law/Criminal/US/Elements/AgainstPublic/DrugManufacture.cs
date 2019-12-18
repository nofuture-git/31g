using System;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
{
    /// <summary>
    /// The illegal production or manufacturing of drugs
    /// </summary>
    public class DrugManufacture : LegalConcept, IControlledSubstance
    {
        public IDrugSchedule Offer { get; set; } = new ScheduleI();

        public Predicate<ILegalPerson> IsManufacturer { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsManufacturer(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsManufacturer)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
