using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// The use of a drug illegally
    /// </summary>
    [Aka("under the influence of a controlled substance")]
    public class DrugUse : CriminalBase, IControlledSubstance
    {
        public IDrugSchedule SubjectDrug { get; set; } = new ScheduleI();
        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new System.NotImplementedException();
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            throw new System.NotImplementedException();
        }
    }
}
