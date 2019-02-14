using System;
using System.Collections.Generic;

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
            IsProportional = (t1, t2) => TermCategory.IsRank(TermCategoryBoolOps.Ge, t1, t2);
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

            return true;
        }
    }
}
