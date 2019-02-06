using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    [Aka("conduct")]
    public class Concurrence : ObjectiveLegalConcept, IElement
    {
        public MensRea MensRea { get; } = new MensRea();
        public ActusReus ActusReus { get; } = new ActusReus();

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (!ActusReus.IsValid(offeror, offeree))
            {
                AddReasonEntry("actus rea is required for any crime");
                AddReasonEntryRange(ActusReus.GetReasonEntries());
                return false;
            }

            if (!MensRea.IsValid(offeror, offeree))
            {
                AddReasonEntry("mens rea is required for any crime");
                AddReasonEntryRange(MensRea.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
