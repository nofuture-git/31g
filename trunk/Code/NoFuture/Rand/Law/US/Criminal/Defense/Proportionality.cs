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
        }

        public Func<ILegalPerson, T> GetContribution { get; set; } = lp => default(T);

        public Func<ITermCategory, ITermCategory, bool> IsProportional { get; set; } = (t1, t2) =>
            (t1?.GetCategoryRank() ?? 0) == (t2?.GetCategoryRank() ?? -1);

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
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsProportional)} is false " +
                                   $"for defendant's {defendantContribution.ToString()} to {otherPartyContribution.ToString()}");
                    return false;
                }
            }

            return true;
        }
    }
}
