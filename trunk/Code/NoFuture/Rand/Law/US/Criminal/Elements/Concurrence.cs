using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    /// <summary>
    /// concurrence (i.e. at-the-same-time) of an evil-meaning mind with an evil-doing hand
    /// </summary>
    [Aka("conduct")]
    public class Concurrence : CriminalBase, IElement
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
            var criminalAct = ActusReus ?? new ActusReus();
            var criminalIntent = MensRea ?? new GeneralIntent();

            if (!criminalAct.IsValid(persons))
            {
                AddReasonEntry("actus rea is invalid");
                AddReasonEntryRange(criminalAct.GetReasonEntries());
                return false;
            }

            if (!criminalIntent.IsValid(persons))
            {
                AddReasonEntry("mens rea is invalid");
                AddReasonEntryRange(criminalIntent.GetReasonEntries());
                return false;
            }

            //test if implementor has some kind of x-ref rules in place
            if (!criminalIntent.CompareTo(criminalAct))
            {
                AddReasonEntry($"{nameof(MensRea)} {nameof(MensRea.CompareTo)} to this {nameof(ActusReus)} is false");
                AddReasonEntryRange(criminalIntent.GetReasonEntries());
                return false;
            }
            if (!criminalAct.CompareTo(criminalIntent))
            {
                AddReasonEntry($"{nameof(ActusReus)} {nameof(ActusReus.CompareTo)} to this {nameof(MensRea)} is false");
                AddReasonEntryRange(criminalIntent.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
