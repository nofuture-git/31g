using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Excuse
{
    /// <inheritdoc cref="IFrustrationOfPurpose" />
    public class FrustrationOfPurpose<T> : ImpracticabilityOfPerformance<T>, IFrustrationOfPurpose where T : ILegalConcept
    {
        public FrustrationOfPurpose(IContract<T> contract) : base(contract)
        {
        }

        public Predicate<ILegalPerson> IsPrincipalPurposeFrustrated { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsFrustrationSubstantial { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            var ps = new[] {IsPrincipalPurposeFrustrated, IsBasicAssumptionGone, IsFrustrationSubstantial};
            return IsPerformanceDischarged(offeror, ps) || IsPerformanceDischarged(offeree, ps);
        }
    }
}
