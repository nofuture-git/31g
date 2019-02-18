using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
    /// <summary>
    /// Template for the concept of reasonable proportionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Proportionality<T> : DefenseBase where T: ITermCategory
    {
        public Proportionality(ICrime crime) : base(crime)
        {
            IsProportional = (t1, t2) =>
                TermCategory.IsRank(TermCategoryBoolOps.Eq, t1, t2) &&
                TermCategory.IsRank(TermCategoryBoolOps.Eq, t2, t1);
        }

        /// <summary>
        /// The enclosure to get a portion-per-person
        /// </summary>
        /// <remarks>
        /// Use the <see cref="ICrime.OtherParties"/> to place other parties into scope
        /// </remarks>
        public Func<ILegalPerson, T> GetChoice { get; set; } = lp => default(T);

        /// <summary>
        /// The test of one <see cref="T"/> compared to another <see cref="T"/>
        /// </summary>
        public Func<T, T, bool> IsProportional { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;

            var getOtherParties = Crime?.OtherParties ?? (() => new List<ILegalPerson>());
            var otherParties = getOtherParties();

            var defendantContribution = GetChoice(defendant);

            if (otherParties == null || !otherParties.Any())
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
