using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Criminal.Elements;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent;

namespace NoFuture.Rand.Law.US.Criminal
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

            if (!Concurrence.IsValid(persons))
            {
                AddReasonEntry($"{nameof(Concurrence)} is invalid");
                AddReasonEntryRange(Concurrence.GetReasonEntries());
                AddPersonsReasonEntries(persons);

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

        public Concurrence Concurrence { get; set; } = new Concurrence();

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
        public Func<IEnumerable<ILegalPerson>> OtherParties { get; set; } = () => new List<ILegalPerson>();

        protected internal void AddPersonsReasonEntries(params ILegalPerson[] persons)
        {
            foreach(var person in persons)
                AddReasonEntryRange(person.GetReasonEntries());
        }
    }
}
