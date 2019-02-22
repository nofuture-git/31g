using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.US
{
    public abstract class CrimeBase : CriminalBase, ICrime
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            ClearReasons();
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (Concurrence == null)
            {
                AddReasonEntry($"{nameof(Concurrence)} is missing");
                AddPersonsReasonEntries(persons);
                return false;
            }

            var isConcurranceValid = Concurrence.IsValid(persons);
            AddReasonEntryRange(Concurrence.GetReasonEntries());
            AddPersonsReasonEntries(persons);

            if (!isConcurranceValid)
            {
                AddReasonEntry($"{nameof(Concurrence)} is invalid");
                return false;
            }

            foreach (var elem in AdditionalElements)
            {
                if (!elem.IsValid(persons))
                {
                    AddReasonEntryRange(elem.GetReasonEntries());
                    AddPersonsReasonEntries(persons);
                    return false;
                }
            }

            return true;
        }

        public abstract int CompareTo(object obj);

        public virtual Concurrence Concurrence { get; set; } = new Concurrence();

        /// <summary>
        /// A short hand property to the same stack-residing variable in <see cref="Concurrence"/>
        /// </summary>
        protected internal virtual IActusReus ActusReus
        {
            get => Concurrence.ActusReus;
            set => Concurrence.ActusReus = value;
        }

        /// <summary>
        /// A short hand property to the same stack-residing variable in <see cref="Concurrence"/>
        /// </summary>
        [Aka("intent")]
        protected internal virtual IMensRea MensRea
        {
            get => Concurrence.MensRea;
            set => Concurrence.MensRea = value;
        }

        public virtual IList<IElement> AdditionalElements { get; } = new List<IElement>();

        protected internal void AddPersonsReasonEntries(params ILegalPerson[] persons)
        {
            foreach(var person in persons)
                AddReasonEntryRange(person.GetReasonEntries());
        }
    }
}
