using System;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// The illegal production or manufacturing of drugs
    /// </summary>
    public class DrugManufacture : CriminalBase, IControlledSubstance
    {
        public IDrugSchedule Offer { get; set; } = new ScheduleI();

        public Predicate<ILegalPerson> IsManufacturer { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsManufacturer(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsManufacturer)} is false");
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
