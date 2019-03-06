using System;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// The illegal sale of drugs
    /// </summary>
    public class DrugSale: CriminalBase, IControlledSubstance
    {
        public IDrugSchedule SubjectDrug { get; set; } = new ScheduleI();
        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
