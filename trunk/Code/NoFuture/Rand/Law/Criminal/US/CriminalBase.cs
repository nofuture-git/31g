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

        public Func<ILegalPerson[], ILegalPerson> GetDefendant
        {
            get => _presecution.GetDefendant;
            set => _presecution.GetDefendant = value;
        }
    }
}
