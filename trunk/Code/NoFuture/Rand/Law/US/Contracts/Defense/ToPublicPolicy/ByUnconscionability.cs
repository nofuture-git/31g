using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy
{
    [Aka("no honest man would offer and no sane man would sign")]
    public class ByUnconscionability<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByUnconscionability(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[determined by consideration of all the circumstances surrounding the transaction]]>
        /// </summary>
        public Predicate<ILegalPerson> IsAbsenceOfMeaningfulChoice { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[contract terms which are unreasonably favorable to the other party]]>
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// The terms are to be considered "in the light of the general commercial 
        /// background and the commercial needs of the particular trade or case."
        /// ]]>
        /// </remarks>
        public Predicate<ILegalPerson> IsUnreasonablyFavorableTerms { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            var rslt = false;

            if (IsAbsenceOfMeaningfulChoice(offeror) || IsAbsenceOfMeaningfulChoice(offeree))
            {
                AddReasonEntry("there is an absence of meaningful choice " +
                               "on the part of one of the parties");
                rslt = true;
            }

            if (IsUnreasonablyFavorableTerms(offeror) || IsUnreasonablyFavorableTerms(offeree))
            {
                AddReasonEntry("the contract terms are unreasonably favorable to the one party");
                rslt = true;
            }

            return rslt;
        }
    }
}
