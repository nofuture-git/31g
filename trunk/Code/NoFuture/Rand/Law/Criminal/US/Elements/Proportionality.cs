using System;
using System.Linq;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// Template for the concept of reasonable proportionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Proportionality<T> : CriminalBase, IElement where T: ITermCategory
    {
        public Proportionality()
        {
            IsProportional = (t1, t2) =>
                TermCategory.IsRank(TermCategoryBoolOps.Eq, t1, t2) &&
                TermCategory.IsRank(TermCategoryBoolOps.Eq, t2, t1);
        }

        /// <summary>
        /// The enclosure to get a portion-per-person
        /// </summary>
        public Func<ILegalPerson, T> GetChoice { get; set; } = lp => default(T);

        /// <summary>
        /// The test of one <see cref="T"/> compared to another <see cref="T"/>
        /// </summary>
        public Func<T, T, bool> IsProportional { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var otherParties = persons.Where(p => !ReferenceEquals(defendant, p)).ToList();

            var defendantContribution = GetChoice(defendant);

            if (!otherParties.Any())
            {
                AddReasonEntry($"defendant, {defendant.Name}, there are no other " +
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
