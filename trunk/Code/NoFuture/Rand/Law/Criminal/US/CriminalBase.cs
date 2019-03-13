using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Law.Criminal.US
{
    public abstract class CriminalBase : LegalConcept, IProsecution
    {
        public ILegalPerson GetDefendant(params ILegalPerson[] persons)
        {
            var defendant = persons.FirstOrDefault(p => p is IDefendant);
            if (defendant == null)
            {
                AddReasonEntry("it is not clear who the " +
                               $"defendant is amoung {string.Join(", ", persons.Select(p => p.Name))}");
            }

            return defendant;
        }

        public virtual IEnumerable<ILegalPerson> GetVictims(params ILegalPerson[] persons)
        {
            var victims = new HashSet<ILegalPerson>();
            if (persons == null || !persons.Any())
            {
                AddReasonEntry($"{nameof(persons)} is null or empty");
                return victims;
            }

            foreach (var legalPerson in persons)
            {
                var victim = legalPerson as IVictim;
                if(victim == null)
                    continue;
                victims.Add(victim);
            }

            return victims;
        }

    }
}
