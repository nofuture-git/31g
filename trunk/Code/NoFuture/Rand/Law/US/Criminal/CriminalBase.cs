using System.Linq;

namespace NoFuture.Rand.Law.US.Criminal
{
    public abstract class CriminalBase : LegalConcept, IProsecution
    {
        public ILegalPerson GetDefendant(ILegalPerson[] persons)
        {
            var defendant = persons.FirstOrDefault();
            if (defendant == null)
            {
                AddReasonEntry("it is not clear who the " +
                               $"defendant is amoung {string.Join(", ", persons.Select(p => p.Name))}");
            }

            return defendant;
        }
    }
}
