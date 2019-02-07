using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    [Aka("criminal act", "guilty mind")]
    public class MensRea : ObjectiveLegalConcept, IElement
    {
        public Predicate<ILegalPerson> IsIntentOnWrongdoing { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsKnowledgeOfWrongdoing { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;
            var intent = IsIntentOnWrongdoing(defendant);
            var knowledge = IsKnowledgeOfWrongdoing(defendant);

            if (!intent && !knowledge)
            {
                AddReasonEntry($"defendant {defendant.Name} did not " +
                               "have intent of wrong doing nor knowledge of wrong doing");
                return false;
            }


            return true;
        }
    }
}
