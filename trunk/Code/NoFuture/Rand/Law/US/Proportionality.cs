using System;
using System.Linq;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// Template for the concept of reasonable proportionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Proportionality<T> : UnoHomine where T: IRankable
    {
        public Proportionality(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            IsProportional = (t1, t2) =>
                TermCategory.IsRank(TermCategoryBoolOps.Eq, t1, t2) &&
                TermCategory.IsRank(TermCategoryBoolOps.Eq, t2, t1);
        }

        /// <summary>
        /// The test of one <see cref="T"/> compared to another <see cref="T"/>
        /// </summary>
        public Func<T, T, bool> IsProportional { get; set; }

        /// <summary>
        /// The enclosure to get a term category-per-person
        /// </summary>
        public virtual Func<ILegalPerson, T> GetChoice { get; set; } = lp => default(T);

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;

            var otherParties = persons.Where(p => !defendant.IsSamePerson(p)).ToList();

            var defendantContribution = GetChoice(defendant);
            var title = defendant.GetLegalPersonTypeName();
            if (!otherParties.Any())
            {
                AddReasonEntry($"{title}, {defendant.Name}, there are no other " +
                               $"parties with which to compare {defendantContribution.ToString()}");
                return false;
            }

            foreach (var otherParty in otherParties)
            {
                var otherPartyContribution = GetChoice(otherParty);
                if(otherPartyContribution == null)
                    continue;

                if (TestIsProportional(defendant, otherParty, defendantContribution, otherPartyContribution))
                    continue;
                return false;
            }

            return true;
        }

        protected internal bool TestIsProportional(ILegalPerson defendant, ILegalPerson otherParty, T defendantContribution, T otherPartyContribution)
        {
            if (IsProportional(defendantContribution, otherPartyContribution))
                return true;

            AddReasonEntry($"{nameof(IsProportional)} is false " +
                           $"for {defendant.Name}'s {defendantContribution.ToString()} " +
                           $"to {otherParty.Name}'s {otherPartyContribution.ToString()}");
            return false;

        }
    }
}
