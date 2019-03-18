using System;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    public abstract class DefenseOfBase : DefenseBase
    {
        protected DefenseOfBase(ICrime crime) : base(crime)
        {
            Provacation = new Provacation(ExtensionMethods.Defendant);
            Imminence = new Imminence(ExtensionMethods.Defendant);
            Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Defendant);
        }

        /// <summary>
        /// (1) an unprovoked attack
        /// </summary>
        public Provacation Provacation { get; set; }

        /// <summary>
        /// (2) an attack which threatens imminent injury or death 
        ///     to a person or or damage, destruction, or theft to real or personal property
        /// </summary>
        public Imminence Imminence { get; set; }

        /// <summary>
        /// (3) an objectively reasonable degree of force, used in response
        /// </summary>
        public Proportionality<ITermCategory> Proportionality { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            if (Imminence != null && !Imminence.IsValid(persons))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            if (Provacation != null && !Provacation.IsValid(persons))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Provacation)} is false");
                AddReasonEntryRange(Provacation.GetReasonEntries());
                return false;
            }
            if (Proportionality != null && !Proportionality.IsValid(persons))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Proportionality)} is false");
                AddReasonEntryRange(Proportionality.GetReasonEntries());
                return false;
            }
            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Provacation?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());
            return true;
        }
    }
}
