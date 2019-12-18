using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.UnintentionalTort
{
    /// <summary>
    /// is founded upon the [...] impossibility of assigning all effects to their respective causes
    /// </summary>
    [EtymologyNote("Latin", "volenti non fit injuria", "willing no injury is")]
    public class ContributoryNegligence<T> : UnoHomine, INegligence where T : IRankable
    {
        public ContributoryNegligence(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// The value such-and-such person produced which
        /// is comparable to the value everyone else produced
        /// </summary>
        /// <remarks>
        /// Tautological use of: assuming-risk, negligence,
        /// fault and responsibility - all get used in language in the similar ways.
        /// </remarks>
        [Aka("primary assumption of risk", "secondary assumption of risk", "causal responsibility", "fault")]
        public Func<ILegalPerson, T> GetContribution { get; set; } = lp => default(T);

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }

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
