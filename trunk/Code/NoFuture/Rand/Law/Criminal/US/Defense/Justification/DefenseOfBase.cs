using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    public abstract class DefenseOfBase : DefenseBase
    {
        protected DefenseOfBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            Provocation = new Provocation(GetSubjectPerson);
            Imminence = new Imminence(GetSubjectPerson);
            Proportionality = new Proportionality<ITermCategory>(GetSubjectPerson);
        }

        /// <summary>
        /// (1) an unprovoked attack
        /// </summary>
        public Provocation Provocation { get; set; }

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
            var legalPerson = GetSubjectPerson(persons);
            if (legalPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var lpTypeName = legalPerson.GetLegalPersonTypeName();
            if (Imminence != null && !Imminence.IsValid(persons))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            if (Provocation != null && !Provocation.IsValid(persons))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(Provocation)} is false");
                AddReasonEntryRange(Provocation.GetReasonEntries());
                return false;
            }
            if (Proportionality != null && !Proportionality.IsValid(persons))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(Proportionality)} is false");
                AddReasonEntryRange(Proportionality.GetReasonEntries());
                return false;
            }
            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Provocation?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());
            return true;
        }
    }
}
