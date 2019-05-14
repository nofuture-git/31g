using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Excuse
{
    /// <inheritdoc cref="IImpracticabilityOfPerformance"/>
    public class ImpracticabilityOfPerformance<T> : ExcuseBase<T>, IImpracticabilityOfPerformance where T : ILegalConcept
    {
        public ImpracticabilityOfPerformance(IContract<T> contract) : base(contract)
        {
        }

        public Predicate<ILegalPerson> IsBasicAssumptionGone { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            return IsPerformanceDischarged(offeror, IsBasicAssumptionGone) || IsPerformanceDischarged(offeree, IsBasicAssumptionGone);
        }

    }
}
