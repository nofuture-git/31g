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
            IsProportional = (t1, t2) => TermCategory.IsRank(TermCategoryBoolOps.Eq, t1, t2);
        }

        /// <summary>
        /// The enclosure to get a portion-per-person
        /// </summary>
        public Func<ILegalPerson, T> GetContribution { get; set; } = lp => default(T);

        /// <summary>
        /// The test of each portion-per-person compared to the portion-per-defendant using 
        /// the numerical value returned from <see cref="ITermCategory.GetCategoryRank"/>
        /// </summary>
        public Func<ITermCategory, ITermCategory, bool> IsProportional { get; set; } 

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            var getOtherParties = Crime?.OtherParties ?? (() => new List<ILegalPerson>());
            var otherParties = getOtherParties();


            var defendantContribution = GetContribution(defendant);

            if (otherParties == null || !otherParties.Any())
            {
                AddReasonEntry($"defendant, {defendant.Name}, there are no other " +
                               $"parties with which to compare {defendantContribution.ToString()}");
                return false;
            }

            foreach (var otherParty in otherParties)
            {
                var otherPartyContribution = GetContribution(otherParty);
                if(otherPartyContribution == null)
                    continue;

                if (!IsProportional(defendantContribution, otherPartyContribution)
                    || !IsProportional(otherPartyContribution, defendantContribution))
                {
                    AddReasonEntry($"{nameof(IsProportional)} is false " +
                                   $"for {defendant.Name}'s {defendantContribution.ToString()} " +
                                   $"to {otherParty.Name}'s {otherPartyContribution.ToString()}");
                    return false;
                }
            }

            return true;
        }
    }
}
