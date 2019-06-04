using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// is founded upon the [...] impossibility of assigning all effects to their respective causes
    /// </summary>
    /// <remarks>
    /// source: CHARLES FISK BEACH, JR., THE LAW OF CONTRIBUTORY NEGLIGENCE 11-13 (1885).
    /// </remarks>
    public class ContributoryNegligence<T> : UnoHomine where T : IRankable
    {
        public ContributoryNegligence(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Func<ILegalPerson, T> GetContribution { get; set; } = lp => default(T);

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var subjContribution = GetContributionWithReasonEntry(subj);

            if (subjContribution <= 0)
            {
                AddReasonEntry(
                    $"{subj.GetLegalPersonTypeName()} {subj.Name}, {nameof(GetContribution)} is zero or less");
                return false;
            }

            foreach (var person in persons.Where(p => !ReferenceEquals(subj, p) && p is IPlaintiff))
            {
                var pContribution = GetContributionWithReasonEntry(person);

                if (pContribution > 0)
                {
                    AddReasonEntry($"{person.GetLegalPersonTypeName()} {person.Name}, " +
                                   $"{nameof(GetContribution)} is greater-than-zero");
                    return false;
                }
            }

            return true;
        }

        protected internal virtual int GetContributionWithReasonEntry(ILegalPerson person)
        {
            if (person == null)
                return 0;

            var contribution = GetContribution(person)?.GetRank() ?? 0;

            AddReasonEntry($"{person.GetLegalPersonTypeName()} {person.Name}, " +
                           $"{nameof(GetContribution)} {nameof(IRankable.GetRank)} " +
                           $"return value of {contribution}");

            return contribution;
        }
    }
}
