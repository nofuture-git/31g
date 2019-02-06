using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    [Aka("criminal intent")]
    public class ActusReus : ObjectiveLegalConcept, IElement
    {
        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsStatus { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (IsStatus(defendant))
            {
                AddReasonEntry($"actus reus cannot be specified as the status of {defendant.Name}");
                return false;
            }

            if (!IsVoluntary(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, was not acting voluntarily");
                return false;
            }


            return true;
        }
    }
}
