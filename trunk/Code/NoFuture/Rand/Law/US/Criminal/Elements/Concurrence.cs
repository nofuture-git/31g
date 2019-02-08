using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    [Aka("conduct")]
    public class Concurrence : ObjectiveLegalConcept, IElement
    {
        private MensRea _mens;

        /// <summary>
        /// see <![CDATA[
        /// strict liability is a crime that requires no intent in which case this would be null
        /// e.g. speeding requires no intent
        /// ]]>
        /// </summary>
        public MensRea MensRea
        {
            get => _mens;
            set
            {
                _mens = value;
                if(_mens == null)
                    AddReasonEntry("mens rea is not required for this crime");
            }
        }
        public ActusReus ActusReus { get; set; } = new ActusReus();

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (ActusReus != null && !ActusReus.IsValid(offeror, offeree))
            {
                AddReasonEntry("actus rea is required for this crime");
                AddReasonEntryRange(ActusReus.GetReasonEntries());
                return false;
            }

            if (MensRea != null && !MensRea.IsValid(offeror, offeree))
            {
                AddReasonEntry("mens rea is required for this crime");
                AddReasonEntryRange(MensRea.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
