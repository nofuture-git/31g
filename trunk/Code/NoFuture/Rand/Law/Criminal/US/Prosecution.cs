using System;
using System.Linq;

namespace NoFuture.Rand.Law.Criminal.US
{
    public class Prosecution : Rationale, IProsecution
    {
        public Predicate<ILegalPerson> IsVictim { get; set; } = lp => lp is IVictim;

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
    }
}
