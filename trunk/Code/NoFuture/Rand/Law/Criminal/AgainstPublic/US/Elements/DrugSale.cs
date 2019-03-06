using System;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// The illegal sale of drugs
    /// </summary>
    public class DrugSale: CriminalBase, IControlledSubstance
    {
        public IDrugSchedule SubjectDrug { get; set; } = new ScheduleI();

        public Predicate<ILegalPerson> IsSeller { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsSeller(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsSeller)} is false");
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
