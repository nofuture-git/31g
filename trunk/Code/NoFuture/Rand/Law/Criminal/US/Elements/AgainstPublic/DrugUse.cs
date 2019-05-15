using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
{
    /// <summary>
    /// The use of a drug illegally
    /// </summary>
    [Aka("under the influence of a controlled substance")]
    public class DrugUse : LegalConcept, IControlledSubstance
    {
        public IDrugSchedule Offer { get; set; } = new ScheduleI();

        public Predicate<ILegalPerson> IsUnderInfluence { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsUnderInfluence(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUnderInfluence)} is false");
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
