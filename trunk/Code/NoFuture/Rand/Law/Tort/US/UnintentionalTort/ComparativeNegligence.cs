using System;
using System.Linq;
using NoFuture.Law.Tort.US.Elements;

namespace NoFuture.Law.Tort.US.UnintentionalTort
{
    /// <summary>
    /// allocated liability &quot;in direct proportion to the extent of
    /// the parties&apos; causal responsibility (a.k.a. fault).&quot;
    /// </summary>
    public class ComparativeNegligence<T> : ContributoryNegligence<T> where T : IRankable
    {
        public ComparativeNegligence(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }

            var subjContribution = GetContributionWithReasonEntry(subj);
            var result = subjContribution > 0;
            
            foreach (var person in persons.Where(p => !ReferenceEquals(subj, p)))
            {
                var pContribution = GetContributionWithReasonEntry(person);

                if (pContribution > subjContribution)
                    result = false;
            }

            return result;
        }
    }
}
