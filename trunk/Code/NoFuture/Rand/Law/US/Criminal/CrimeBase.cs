using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Criminal.Elements;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent;

namespace NoFuture.Rand.Law.US.Criminal
{
    public abstract class CrimeBase : LegalConcept, ICrime
    {
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            ClearReasons();
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (!IsChargedWith(defendant))
            {
                AddReasonEntry($"there are no charges against {defendant.Name}");
                AddPersonsReasonEntries(offeror, offeree);
                return false;
            }

            if (Concurrence == null)
            {
                AddReasonEntry($"{nameof(Concurrence)} is missing");
                AddPersonsReasonEntries(offeror, offeree);
                return false;
            }

            if (!Concurrence.IsValid(offeror, offeree))
            {
                AddReasonEntry($"{nameof(Concurrence)} is invalid");
                AddReasonEntryRange(Concurrence.GetReasonEntries());
                AddPersonsReasonEntries(offeror, offeree);

                return false;
            }

            foreach (var elem in AdditionalElements)
            {
                if (!elem.IsValid(offeror, offeree))
                {
                    AddReasonEntryRange(elem.GetReasonEntries());
                    AddPersonsReasonEntries(offeror, offeree);
                    return false;
                }
            }

            return true;
        }

        public abstract int CompareTo(object obj);

        public Concurrence Concurrence { get; set; } = new Concurrence();

        /// <summary>
        /// A enclosure to describe the specific charges
        /// </summary>
        public virtual Predicate<ILegalPerson> IsChargedWith { get; set; } = lp => true;

        /// <summary>
        /// A short hand property to the same stack-residing variable in <see cref="Concurrence"/>
        /// </summary>
        public ActusReus ActusReus
        {
            get => Concurrence.ActusReus;
            set => Concurrence.ActusReus = value;
        }

        /// <summary>
        /// A short hand property to the same stack-residing variable in <see cref="Concurrence"/>
        /// </summary>
        [Aka("intent")]
        public MensRea MensRea
        {
            get => Concurrence.MensRea;
            set => Concurrence.MensRea = value;
        }

        public IList<IElement> AdditionalElements { get; } = new List<IElement>();

        protected internal void AddPersonsReasonEntries(ILegalPerson offeror, ILegalPerson offeree)
        {
            AddReasonEntryRange(offeror?.GetReasonEntries());
            AddReasonEntryRange(offeree?.GetReasonEntries());
        }
    }
}
