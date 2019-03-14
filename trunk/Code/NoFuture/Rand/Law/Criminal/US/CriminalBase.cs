using System;

namespace NoFuture.Rand.Law.Criminal.US
{
    public abstract class CriminalBase : LegalConcept, IProsecution
    {
        private readonly IProsecution _presecution = new Prosecution();

        public Predicate<ILegalPerson> IsVictim
        {
            get => _presecution.IsVictim;
            set => _presecution.IsVictim = value;
        }

        public ILegalPerson GetDefendant(params ILegalPerson[] persons)
        {
            var defendant = _presecution.GetDefendant(persons);
            AddReasonEntryRange(_presecution.GetReasonEntries());
            return defendant;
        }
    }
}
