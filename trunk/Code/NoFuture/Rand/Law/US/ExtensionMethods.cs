using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Law.US
{
    public static class ExtensionMethods
    {
        public static ILegalPerson Defendant(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IDefendant);
        }

        public static IEnumerable<ILegalPerson> Victims(this IEnumerable<ILegalPerson> persons)
        {
            return persons.Where(p => p is IVictim).ToList();
        }

        public static ILegalPerson Offeror(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IOfferor);
        }

        public static ILegalPerson Offeree(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IOfferee);
        }

        public static ILegalPerson Tortfeasor(IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is ITortfeasor);
        }
    }
}
