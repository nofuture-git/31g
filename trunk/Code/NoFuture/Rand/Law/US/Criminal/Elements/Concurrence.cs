using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    /// <summary>
    /// concurrence (i.e. at-the-same-time) of an evil-meaning mind with an evil-doing hand
    /// </summary>
    [Aka("conduct")]
    public class Concurrence : LegalConcept, IElement
    {
        private MensRea _mens;

        /// <summary>
        /// the willful intent to do harm
        /// </summary>
        /// <remarks>
        /// Setting this to null implies that intent is not required. 
        /// </remarks>
        [Aka("intent")]
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

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (ActusReus != null && !ActusReus.IsValid(persons))
            {
                AddReasonEntry("actus rea is invalid");
                AddReasonEntryRange(ActusReus.GetReasonEntries());
                return false;
            }

            if (MensRea != null && !MensRea.IsValid(persons))
            {
                AddReasonEntry("mens rea is invalid");
                AddReasonEntryRange(MensRea.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
