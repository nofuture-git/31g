using System;
using System.Linq;

namespace NoFuture.Rand.Law.US.Contracts.Litigate
{
    /// <summary>
    /// <![CDATA[
    /// is an act or event, other than a lapse of time, which, unless 
    /// the condition is excused, must occur before a duty to perform 
    /// a promise in the agreement arises
    /// ]]>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConditionsPrecedent<T> : DilemmaBase<T>
    {
        public ConditionsPrecedent(IContract<T> contract) : base(contract) { }

        /// <summary>
        /// The implementation that person X failed to meet the conditional term Y
        /// </summary>
        public Func<Term<object>, ILegalPerson, bool> IsNotConditionMet { get; set; } = (t, lp) => true;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            foreach (var ct in AgreedTerms)
            {
                var isNotMet = IsNotConditionMet(ct, offeror) || IsNotConditionMet(ct, offeree);
                if (isNotMet)
                {
                    AddReasonEntry($"there is a conditional precedent between {offeror.Name} " +
                        $"and {offeree.Name}, '{ct.ToString()}', which has not been met.");
                    return false;
                }
            }
            return true;
        }
    }
}
