using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Contract.US.Defense.ToPublicPolicy
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

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

            if (!base.IsValid(offeror, offeree))
                return false;

            var offerorTitle = offeror.GetLegalPersonTypeName();
            var offereeTitle = offeree.GetLegalPersonTypeName();

            if (IsAbsenceOfMeaningfulChoice(offeror))
            {
                AddReasonEntry($"{offerorTitle} {offeror.Name}, {nameof(IsAbsenceOfMeaningfulChoice)} is true");
                return true;
            }
            if (IsAbsenceOfMeaningfulChoice(offeree))
            {
                AddReasonEntry($"{offereeTitle} {offeree.Name}, {nameof(IsAbsenceOfMeaningfulChoice)} is true");
                return true;
            }

            if (IsUnreasonablyFavorableTerms(offeror))
            {
                AddReasonEntry($"{offerorTitle} {offeror.Name}, {nameof(IsUnreasonablyFavorableTerms)} is true");
                return true;
            }

            if (IsUnreasonablyFavorableTerms(offeree))
            {
                AddReasonEntry($"{offereeTitle} {offeree.Name}, {nameof(IsUnreasonablyFavorableTerms)} is true");
                return true;
            }
            return false;
        }
    }
}
