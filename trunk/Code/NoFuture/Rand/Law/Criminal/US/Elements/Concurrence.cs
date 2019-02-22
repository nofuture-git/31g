using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// concurrence (i.e. at-the-same-time) of an evil-meaning mind with an evil-doing hand
    /// </summary>
    [Aka("conduct")]
    public class Concurrence : CriminalBase, IElement
    {
        private IMensRea _mens;

        /// <summary>
        /// the willful intent to do harm
        /// </summary>
        /// <remarks>
        /// Setting this to null implies that intent is not required. 
        /// </remarks>
        [Aka("intent")]
        public IMensRea MensRea
        {
            get => _mens;
            set
            {
                _mens = value;
                if(_mens == null)
                    AddReasonEntry("mens rea is not required for this crime");
            }
        }
        public IActusReus ActusReus { get; set; } = new ActusReus();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var criminalAct = ActusReus ?? new ActusReus();

            if (!criminalAct.IsValid(persons))
            {
                AddReasonEntry("actus rea is invalid");
                AddReasonEntryRange(criminalAct.GetReasonEntries());
                return false;
            }

            if (MensRea == null)
                return true;

            var criminalIntent = MensRea;
            if (!criminalIntent.IsValid(persons))
            {
                AddReasonEntry("mens rea is invalid");
                AddReasonEntryRange(criminalIntent.GetReasonEntries());
                return false;
            }

            var intent2ActCompare = criminalIntent.CompareTo(criminalAct, persons);
            var act2IntentCompare = criminalAct.CompareTo(criminalIntent, persons);

            AddReasonEntryRange(criminalIntent.GetReasonEntries());
            AddReasonEntryRange(criminalAct.GetReasonEntries());

            //test if implementor has some kind of x-ref rules in place
            if (!intent2ActCompare)
            {
                AddReasonEntry($"{nameof(MensRea)} {nameof(MensRea.CompareTo)} to this {nameof(ActusReus)} is false");
                return false;
            }
            if (!act2IntentCompare)
            {
                AddReasonEntry($"{nameof(ActusReus)} {nameof(ActusReus.CompareTo)} to this {nameof(MensRea)} is false");
                return false;
            }

            return true;
        }
    }
}
