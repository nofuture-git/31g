using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// Represents the concept of picking one <see cref="T"/> amoung many possible choices thereof
    /// </summary>
    public class ChoiceThereof<T> : Proportionality<T>, IElement where T: ITermCategory
    {
        public ChoiceThereof()
        {
            IsProportional = (t1, t2) => TermCategory.IsRank(TermCategoryBoolOps.Lt, t1, t2);
        }

        /// <summary>
        /// The collection of all other possible choices of <see cref="T"/> NOT made by the given person
        /// </summary>
        public Func<ILegalPerson, IEnumerable<T>> GetOtherPossibleChoices { get; set; } = lp => new List<T>();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
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

            //for duress the choice of the defendant is less than the choice of other parties
            return persons.Length <= 1 || base.IsValid(persons);
        }
    }
}
