using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Criminal.US.Terms;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPublic
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
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsUnderInfluence(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsUnderInfluence)} is false");
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
