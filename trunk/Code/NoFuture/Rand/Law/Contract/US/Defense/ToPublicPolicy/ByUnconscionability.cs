using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy
{
    /// <inheritdoc cref="IUnconscionability"/>
    public class ByUnconscionability<T> : DefenseBase<T>, IUnconscionability where T : ILegalConcept
    {
        public ByUnconscionability(IContract<T> contract) : base(contract)
        {
        }
        public Predicate<ILegalPerson> IsAbsenceOfMeaningfulChoice { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsUnreasonablyFavorableTerms { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

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
