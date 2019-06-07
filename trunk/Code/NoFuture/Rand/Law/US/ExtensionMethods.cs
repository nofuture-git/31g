using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// https://www.cali.org/
    /// </summary>
    public static class ExtensionMethods
    {
        [EtymologyNote("Latin", "res ipsa loquitur", "thing itself speaks")]
        public static bool ResIpsaLoquitur(this ILegalPerson person)
        {
            return true;
        }

        public static bool IsSamePerson(this ILegalPerson p0, ILegalPerson p1)
        {
            return VocaBase.Equals(p0, p1) || ReferenceEquals(p0, p1);
        }

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

        public static ILegalPerson Tortfeasor(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is ITortfeasor || p is IDefendant);
        }

        public static ILegalPerson Plaintiff(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IPlaintiff);
        }

        public static ILegalPerson ThirdParty(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IThirdParty);
        }

        public static ILegalPerson Expert<T>(this IEnumerable<ILegalPerson> persons) where T : ILegalConcept
        {
            return persons.FirstOrDefault(p => p is IExpert<T>);
        }

        public static IPlaintiff Plaintiff(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var plaintiff = ppersons.Plaintiff() as IPlaintiff;
            if (plaintiff == null)
            {
                var nameTitles = ppersons.Select(p => Tuple.Create(p.GetLegalPersonTypeName(), p.Name));
                lc.AddReasonEntry($"No one is the {nameof(IPlaintiff)} in {nameTitles}");
                return null;
            }

            return plaintiff;
        }

        public static IThirdParty ThirdParty(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var thirdParty = ppersons.ThirdParty() as IThirdParty;
            if (thirdParty == null)
            {
                var nameTitles = ppersons.Select(p => Tuple.Create(p.GetLegalPersonTypeName(), p.Name));
                lc.AddReasonEntry($"No one is the {nameof(IThirdParty)} in {nameTitles}");
                return null;
            }

            return thirdParty;
        }

        public static string GetLegalPersonTypeName(this ILegalPerson person)
        {
            if (person == null)
                return string.Empty;
            if (person is IDefendant)
                return "defendant";
            if (person is IPlaintiff)
                return "plaintiff";
            if (person is ITortfeasor)
                return "tortfeasor";
            if (person is IOfferee)
                return "offeree";
            if (person is IOfferor)
                return "offeror";
            if (person is IChild)
                return "child";
            if (person is IVictim)
                return "victim";
            if (person is IInvitee)
                return "invitee";
            if (person is ILicensee)
                return "licensee";
            if (person is IThirdParty)
                return "third party";
            if (person.Equals(Government.Value))
                return "the government";
            return "legal person";

        }
    }
}
