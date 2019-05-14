using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToAssent
{
    /// <inheritdoc cref="IMisrepresentation"/>
    public class Misrepresentation<T> : DefenseBase<T>, IMisrepresentation where T : ILegalConcept
    {
        public Misrepresentation(IContract<T> contract) : base(contract) { }

        public Predicate<ILegalPerson> IsAssertionToInduceAssent { get; set; } = llp => false;

        public Predicate<ILegalPerson> IsNotAssertionInAccord2Facts { get; set; } = llp => false;

        public Predicate<ILegalPerson> IsNotAssertionInConfidence2Truth { get; set; } = llp => false;

        public Predicate<ILegalPerson> IsNotAssertionInBasis2ImpliedStatement { get; set; } = llp => false;

        public virtual bool IsFraudulent(ILegalPerson lp)
        {
            if (!IsAssertionToInduceAssent(lp))
                return false;

            AddReasonEntry($"{lp?.Name} intends his/her assertion to " +
                           "induce a party to manifest assent");
            if (IsNotAssertionInAccord2Facts(lp))
            {
                AddReasonEntry($"and, {lp?.Name} knows or believes that the " +
                               "assertion is not in accord with the facts");
                return true;
            }
            if (IsNotAssertionInConfidence2Truth(lp))
            {
                AddReasonEntry($"and, {lp?.Name} does not have the confidence " +
                               "that he/she states or implies in the truth of the assertion");
                return true;
            }
            if (IsNotAssertionInBasis2ImpliedStatement(lp))
            {
                AddReasonEntry($"and, {lp?.Name} knows that he/she does not have " +
                               "the basis that he/she states or implies for the assertion");
                return true;
            }

            return false;
        }

        public virtual bool IsMaterial(ILegalPerson lp)
        {
            return IsAssertionToInduceAssent(lp);
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            return IsFraudulent(offeror) || IsFraudulent(offeree) || IsMaterial(offeror) || IsMaterial(offeree);
        }

    }
}
