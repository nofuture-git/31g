using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    [Aka("conduct")]
    public class Concurrence : ObjectiveLegalConcept, IElement
    {
        private MensRea _mens;

        /// <summary>
        /// see <![CDATA[
        /// For some crimes this is not required.  Therefore it can be nulled out.  
        /// see United States v. Freed, 401 U.S. 601 (1971) 
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
