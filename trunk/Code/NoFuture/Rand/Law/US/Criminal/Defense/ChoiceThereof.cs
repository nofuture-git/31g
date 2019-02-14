using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
    /// <summary>
    /// Represents the concept of picking one <see cref="T"/> amoung many possible choices thereof
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChoiceThereof<T> : Proportionality<T> where T: ITermCategory
    {
        public ChoiceThereof(ICrime crime) : base(crime)
        {
            IsProportional = (t1, t2) => TermCategory.IsRank(TermCategoryBoolOps.Lt, t1, t2);
        }

        /// <summary>
        /// The collection of all other possible choices of <see cref="T"/> NOT made by the given person
        /// </summary>
        public Func<ILegalPerson, IEnumerable<T>> GetOtherPossibleChoices { get; set; } = lp => new List<T>();

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            var actualChoice = GetChoice(defendant);

            var otherChoices = GetOtherPossibleChoices(defendant);

            foreach (var otherChoice in otherChoices)
            {
                if (TestIsProportional(defendant, defendant, actualChoice, otherChoice))
                    continue;
                return false;
            }

            var getOtherParties = Crime?.OtherParties ?? (() => new List<ILegalPerson>());
            var otherParties = getOtherParties();

            //for duress the choice of the defendant is less than the choice of other parties
            return !otherParties.Any() || base.IsValid(offeror, offeree);
        }
    }
}
