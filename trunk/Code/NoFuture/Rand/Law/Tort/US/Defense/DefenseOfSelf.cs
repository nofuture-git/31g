using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Tort.US.Defense
{
    /// <inheritdoc cref="IDefenseOfSelf"/>
    public class DefenseOfSelf : LegalConcept, IDefenseOfSelf
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeaser = persons.Tortfeasor();
            if (tortfeaser == null)
                return false;
            if (Imminence != null && !Imminence.IsValid(persons))
            {
                AddReasonEntry($"tortfeaser, {tortfeaser.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            if (Provocation != null && !Provocation.IsValid(persons))
            {
                AddReasonEntry($"tortfeaser, {tortfeaser.Name}, {nameof(Provocation)} is false");
                AddReasonEntryRange(Provocation.GetReasonEntries());
                return false;
            }
            if (Proportionality != null && !Proportionality.IsValid(persons))
            {
                AddReasonEntry($"tortfeaser, {tortfeaser.Name}, {nameof(Proportionality)} is false");
                AddReasonEntryRange(Proportionality.GetReasonEntries());
                return false;
            }
            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Provocation?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());
            return true;
        }

        public ObjectivePredicate<ILegalPerson> IsReasonableFearOfInjuryOrDeath { get; set; }
        public Provocation Provocation { get; set; }
        public Imminence Imminence { get; set; }
        public Proportionality<ITermCategory> Proportionality { get; set; }
    }
}
