using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Semiosis
{
    /// <summary>
    /// <![CDATA[
    /// is an act or event, other than a lapse of time, which, unless 
    /// the condition is excused, must occur before a duty to perform 
    /// a promise in the agreement arises
    /// ]]>
    /// </summary>
    [Aka("modus pones", "if...then")]
    public class ConditionsPrecedent<T> : DilemmaBase<T> where T : ILegalConcept
    {
        public ConditionsPrecedent(IContract<T> contract) : base(contract) { }

        /// <summary>
        /// Predicate logic applied to each term to distinguish it as a conditional one
        /// </summary>
        public Predicate<Term<object>> IsConditionalTerm { get; set; } = t => false;

        /// <summary>
        /// The implementation that person X failed to meet the conditional term Y
        /// </summary>
        public Func<Term<object>, ILegalPerson, bool> IsNotConditionMet { get; set; } = (t, lp) => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            var conditionalTerms = AgreedTerms.Where(t => IsConditionalTerm(t)).ToList();
            if (!conditionalTerms.Any())
            {
                AddReasonEntry($"There are no conditional precedent terms between {offeror.Name} and {offeree.Name}");
                return false;
            }

            foreach (var ct in conditionalTerms)
            {
                var isNotMet = IsNotConditionMet(ct, offeror) || IsNotConditionMet(ct, offeree);
                if (isNotMet)
                {
                    AddReasonEntry($"there is a conditional precedent between {offeror.Name} " +
                        $"and {offeree.Name}, '{ct}', which has not been met.");
                    return false;
                }
            }
            return true;
        }
    }
}
