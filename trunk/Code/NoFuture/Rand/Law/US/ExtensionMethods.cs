using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
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

        public static IEnumerable<IVictim> Victims(this IEnumerable<ILegalPerson> persons)
        {
            return persons.Where(p => p is IVictim).Cast<IVictim>().ToList();
        }

        public static IVictim Victim(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IVictim) as IVictim;
        }

        public static ILegalPerson Offeror(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IOfferor);
        }

        public static ILegalPerson Offeree(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IOfferee);
        }

        public static ILegalPerson Lessee(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is ILessee);
        }

        public static ILegalPerson Lessor(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is ILessor);
        }

        public static ILegalPerson Grantor(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IGrantor);
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

        public static ILegalPerson Employer(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IEmployer);
        }

        public static ILegalPerson Employee(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IEmployee);
        }

        public static ILegalPerson Expert<T>(this IEnumerable<ILegalPerson> persons) where T : ILegalConcept
        {
            return persons.FirstOrDefault(p => p is IExpert<T>);
        }

        public static ILegalPerson Disseisor(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IDisseisor);
        }

        public static ILegalPerson CourtOfficial(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is ICourtOfficial);
        }

        public static INotaryPublic NotaryPublic(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is INotaryPublic) as INotaryPublic;
        }

        public static ILawEnforcement LawEnforcement(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is ILawEnforcement) as ILawEnforcement;
        }

        public static ISuspect Suspect(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is ISuspect) as ISuspect;
        }

        public static IAffiant Affiant(this IEnumerable<ILegalPerson> persons)
        {
            return persons.FirstOrDefault(p => p is IAffiant) as IAffiant;
        }

        public static IPlaintiff Plaintiff(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var plaintiff = ppersons.Plaintiff() as IPlaintiff;
            if (plaintiff == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IPlaintiff)} in {nameTitles}");
                return null;
            }

            return plaintiff;
        }

        public static IDefendant Defendant(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var defendant = ppersons.Defendant() as IDefendant;
            if (defendant == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IDefendant)} in {nameTitles}");
                return null;
            }

            return defendant;
        }

        public static IVictim Victim(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var victim = ppersons.Victims().ToList();
            if (!victim.Any())
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IVictim)} in {nameTitles}");
                return null;
            }

            return victim.FirstOrDefault();
        }

        public static ITortfeasor Tortfeasor(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var tortfeasor = ppersons.Tortfeasor() as ITortfeasor;
            if (tortfeasor == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(ITortfeasor)} in {nameTitles}");
                return null;
            }

            return tortfeasor;
        }

        public static IThirdParty ThirdParty(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var thirdParty = ppersons.ThirdParty() as IThirdParty;
            if (thirdParty == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IThirdParty)} in {nameTitles}");
                return null;
            }

            return thirdParty;
        }

        public static IEmployer Employer(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var employer = ppersons.Employer() as IEmployer;
            if (employer == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IEmployer)} in {nameTitles}");
                return null;
            }

            return employer;
        }

        public static IEnumerable<ILegalPerson> Employees(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();
            if (lc == null || !ppersons.Any())
                return new List<ILegalPerson>();

            var employees = ppersons.Where(p => p is IEmployee).ToList();

            if (!employees.Any())
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IEmployee)} in {nameTitles}");
            }

            return employees;
        }

        public static IOfferee Offeree(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var offeree = ppersons.Offeree() as IOfferee;
            if (offeree == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IOfferee)} in {nameTitles}");
                return null;
            }

            return offeree;
        }

        public static IOfferor Offeror(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var offeror = ppersons.Offeror() as IOfferor;
            if (offeror == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IOfferor)} in {nameTitles}");
                return null;
            }

            return offeror;
        }

        public static ILessee Lessee(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var lessee = ppersons.Lessee() as ILessee;
            if (lessee == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(ILessee)} in {nameTitles}");
                return null;
            }

            return lessee;
        }

        public static ILessor Lessor(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();

            if (lc == null || !ppersons.Any())
                return null;

            var lessor = ppersons.Lessor() as ILessor;
            if (lessor == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(ILessor)} in {nameTitles}");
                return null;
            }

            return lessor;
        }

        public static IEnumerable<ILegalPerson> Cotenants(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();
            if (lc == null || !ppersons.Any())
                return new List<ILegalPerson>();

            var cotenants = ppersons.Where(p => p is ICotenant).ToList();

            if (!cotenants.Any())
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(ICotenant)} in {nameTitles}");
            }

            return cotenants;
        }

        public static IEnumerable<IAbsentee> Absentees(this IRationale lc, IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();
            if (lc == null || !ppersons.Any())
                return new List<IAbsentee>();

            var absentees = ppersons.Where(p => p is IAbsentee).Cast<IAbsentee>().ToList();

            if (!absentees.Any())
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(IAbsentee)} in {nameTitles}");
            }

            return absentees;
        }

        public static ISuspect Suspect(this IRationale lc, IEnumerable<ILegalPerson> persons, Func<ILegalPerson[], ILegalPerson> func)
        {
            func = func ?? (lps => lps.Suspect());
            var ppersons = persons == null ? new ILegalPerson[]{} : persons.ToArray();

            if (lc == null || !ppersons.Any())
                return null;

            var suspect = func(ppersons) as ISuspect;
            if (suspect == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(ISuspect)} in {nameTitles}");
                return null;
            }

            return suspect;
        }

        public static ILawEnforcement LawEnforcement(this IRationale lc, IEnumerable<ILegalPerson> persons, Func<ILegalPerson[], ILegalPerson> func)
        {
            func = func ?? (lps => lps.LawEnforcement());
            var ppersons = persons == null ? new ILegalPerson[] { } : persons.ToArray();

            if (lc == null || !ppersons.Any())
                return null;

            var lawEnforcement = func(ppersons) as ILawEnforcement;
            if (lawEnforcement == null)
            {
                var nameTitles = ppersons.GetTitleNamePairs();
                lc.AddReasonEntry($"No one is the {nameof(ILawEnforcement)} in {nameTitles}");
                return null;
            }

            return lawEnforcement;
        }

        public static bool PropertyOwnerIsSubjectPerson(this IRationale lc, ILegalProperty property, ILegalPerson person)
        {
            if (person == null)
                return false;
            var title = person.GetLegalPersonTypeName();
            if (property == null)
            {
                lc.AddReasonEntry($"{title} {person.Name}, {nameof(property)} is unassigned");
                return false;
            }

            var isOwner = property.IsEntitledTo != null && property.IsEntitledTo(person);
            var isIsNot = isOwner ? "is owner" : "is not owner";
            lc.AddReasonEntry(
                $"{title} {person.Name}, {isIsNot} " +
                $"of {property.GetType().Name} " +
                $"named '{property.Name}'");

            return isOwner;
        }

        /// <summary>
        /// Gets the subset of terms that have both the same name and meaning 
        /// </summary>
        public static ISet<Term<object>> GetAgreedTerms(this IAssentTerms lc, ILegalPerson offeror,
            ILegalPerson offeree)
        {
            if(lc == null)
                return new HashSet<Term<object>>();

            //the shared terms between the two
            var sorTerms = lc.TermsOfAgreement?.Invoke(offeror);
            if (sorTerms == null || !sorTerms.Any())
            {
                lc.AddReasonEntry($"{offeror.Name} has no terms");
                return new HashSet<Term<object>>();
            }

            var seeTerms = lc.TermsOfAgreement(offeree);
            if (seeTerms == null || !seeTerms.Any())
            {
                lc.AddReasonEntry($"{offeree.Name} has no terms");
                return new HashSet<Term<object>>();
            }
            var agreedTerms = Term<object>.GetAgreedTerms(sorTerms, seeTerms);
            if (!agreedTerms.Any())
            {
                lc.AddReasonEntry("there are no terms, with the same name," +
                               $" shared between {offeror.Name} and {offeree.Name}");
            }

            return agreedTerms;
        }

        /// <summary>
        /// Gets the symmetric difference of the terms between offeror and offeree
        /// </summary>
        public static ISet<Term<object>> GetAdditionalTerms(this IAssentTerms lc, ILegalPerson offeror,
            ILegalPerson offeree)
        {
            if (lc == null)
                return new HashSet<Term<object>>();

            var sorTerms = lc.TermsOfAgreement?.Invoke(offeror);
            if (sorTerms == null || !sorTerms.Any())
            {
                lc.AddReasonEntry($"{offeror.Name} has no terms");
                return new HashSet<Term<object>>();
            }

            var seeTerms = lc.TermsOfAgreement(offeree);
            if (seeTerms == null || !seeTerms.Any())
            {
                lc.AddReasonEntry($"{offeree.Name} has no terms");
                return new HashSet<Term<object>>();
            }
            var additionalTerms = Term<object>.GetAdditionalTerms(sorTerms, seeTerms);
            if (!additionalTerms.Any())
            {
                lc.AddReasonEntry("there are no additional terms between " +
                               $" {offeror.Name} and {offeree.Name}");
            }
            return additionalTerms;
        }

        /// <summary>
        /// Gets the subset of terms which have the same name.
        /// </summary>
        public static ISet<Term<object>> GetInNameAgreedTerms(this IAssentTerms lc, ILegalPerson offeror, ILegalPerson offeree)
        {
            if (lc == null)
                return new HashSet<Term<object>>();

            var sorTerms = lc.TermsOfAgreement?.Invoke(offeror);
            if (sorTerms == null || !sorTerms.Any())
            {
                lc.AddReasonEntry($"{offeror.Name} has no terms");
                return new HashSet<Term<object>>();
            }

            var seeTerms = lc.TermsOfAgreement(offeree);
            if (seeTerms == null || !seeTerms.Any())
            {
                lc.AddReasonEntry($"{offeree.Name} has no terms");
                return new HashSet<Term<object>>();
            }

            var agreedTerms = Term<object>.GetInNameAgreedTerms(sorTerms, seeTerms);
            if (!agreedTerms.Any())
            {
                lc.AddReasonEntry($"there are no terms shared between {offeror.Name} and {offeree.Name}");
                return new HashSet<Term<object>>();
            }
            return agreedTerms;
        }

        public static bool PropertyOwnerIsInPossession(this IRationale lc, ILegalProperty SubjectProperty, ILegalPerson subj)
        {
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();
            if (SubjectProperty == null)
            {
                lc.AddReasonEntry($"{title} {subj.Name}, {nameof(SubjectProperty)} is unassigned");
                return false;
            }

            var hasPossession = SubjectProperty.IsInPossessionOf != null && SubjectProperty.IsInPossessionOf(subj);
            var isIsNot = hasPossession ? "is in possession" : "is not in possession";
            lc.AddReasonEntry(
                $"{title} {subj.Name}, {isIsNot} " +
                $"of {SubjectProperty.GetType().Name} " +
                $"named '{SubjectProperty.Name}'");

            return hasPossession;
        }

        public static string GetTitleNamePairs(this IEnumerable<ILegalPerson> persons)
        {
            var ppersons = persons == null ? new List<ILegalPerson>() : persons.ToList();
            var titleNamePairs = ppersons.Where(p => p != null).Select(p => Tuple.Create(p.GetLegalPersonTypeName(), p.Name));
            return string.Join(" ", titleNamePairs);
        }

        public static string GetLegalPersonTypeName(this ILegalPerson person)
        {
            if (person == null)
                return string.Empty;
            var titles = new List<string>();

            if (person is ICourtOfficial)
                titles.Add("court official");
            if (person is ILessee)
                titles.Add("lessee");
            if (person is IDonee)
                titles.Add("donee");
            if (person is IGrantee)
                titles.Add("grantee");
            if (person is IOfferee)
                titles.Add("offeree");
            if(person is ILessor)
                titles.Add("lessor");
            if (person is IDonor)
                titles.Add("donor");
            if (person is IGrantor)
                titles.Add("grantor");
            if (person is IOfferor)
                titles.Add("offeror");
            if (person is IDisseisor)
                titles.Add("disseisor");
            if (person is ITortfeasor)
                titles.Add("tortfeasor");
            if (person is IDefendant)
                titles.Add("defendant");
            if (person is IPlaintiff)
                titles.Add("plaintiff");
            if (person is IChild)
                titles.Add("child");
            if (person is IVictim)
                titles.Add("victim");
            if (person is IInvitee)
                titles.Add("invitee");
            if (person is ILicensee)
                titles.Add("licensee");
            if (person is IThirdParty)
                titles.Add("third party");
            if (person is IEmployee)
                titles.Add("employee");
            if (person is IEmployer)
                titles.Add("employer");
            if (person.Equals(Government.Value))
                titles.Add("the government");
            if(person is ICorporation)
                titles.Add("corporation");
            if(person is ICotenant)
                titles.Add("cotenant");
            if(person is IForeigner)
                titles.Add("foreigner");
            if(person is IAbsentee)
                titles.Add("absentee");
            if(person is INotaryPublic)
                titles.Add("U.S. Notary");
            if(person is ILawEnforcement)
                titles.Add("law enforcement");
            if(person is IJudge)
                titles.Add("judge");
            if (person is ISuspect)
                titles.Add("suspect");
            if (person is IInformant)
                titles.Add("informant");
            if(person is IAffiant)
                titles.Add("affiant");

            return string.Join("|", titles);
        }

        #region REPL ease

        public static Func<ILegalPerson[], ILegalPerson> FirstOne => (lps => lps.FirstOrDefault());
        public static Func<ILegalPerson[], ILegalPerson> DefendantFx => Defendant;
        public static Func<ILegalPerson[], ILegalPerson> OfferorFx => Offeror;
        public static Func<ILegalPerson[], ILegalPerson> OffereeFx => Offeree;
        public static Func<ILegalPerson[], ILegalPerson> GrantorFx => Grantor;
        public static Func<ILegalPerson[], ILegalPerson> TortfeasorFx => Tortfeasor;
        public static Func<ILegalPerson[], ILegalPerson> PlaintiffFx => Plaintiff;
        public static Func<ILegalPerson[], ILegalPerson> ThirdPartyFx => ThirdParty;
        public static Func<ILegalPerson[], ILegalPerson> EmployerFx => Employer;
        public static Func<ILegalPerson[], ILegalPerson> DisseisorFx => Disseisor;

        public static Predicate<ILegalPerson> TruePredicateFx = lp => true;
        public static Predicate<ILegalPerson> FalsePredicateFx = lp => false;

        #endregion
    }
}
