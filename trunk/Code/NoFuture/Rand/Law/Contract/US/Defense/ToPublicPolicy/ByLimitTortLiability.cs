using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy
{
    /// <inheritdoc cref="ILimitTortLiability"/>
    public class ByLimitTortLiability<T> : DefenseBase<T>, ILimitTortLiability where T : ILegalConcept
    {
        public ByLimitTortLiability(IContract<T> contract) : base(contract)
        {
        }
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;

            var cnt = 0;
            foreach (var kvp in GetPredicate2ReasonString())
            {
                var p = kvp.Key;
                if (p(offeror) || p(offeree))
                {
                    AddReasonEntry(kvp.Value);
                    cnt += 1;
                }
            }

            return cnt >= NumberNeededToTestTrue;
        }

        public int NumberNeededToTestTrue { get; set; } = 5;

        public Predicate<ILegalPerson> IsSuitableForPublicRegulation { get; set; } = c => false;

        public Predicate<ILegalPerson> IsImportantPublicService { get; set; } = c => false;

        public Predicate<ILegalPerson> IsOfferToAnyMemberOfPublic { get; set; } = c => false;

        public Predicate<ILegalPerson> IsAdvantageOverMemberOfPublic { get; set; } = c => false;

        public Predicate<ILegalPerson> IsStandardizedAdhesion { get; set; } = c => false;

        public Predicate<ILegalPerson> IsSubjectToSellerCarelessness { get; set; } = c => false;

        protected internal virtual Dictionary<Predicate<ILegalPerson>, string> GetPredicate2ReasonString()
        {
            return new Dictionary<Predicate<ILegalPerson>, string>
            {
                {IsSuitableForPublicRegulation, "the agreement suitable for public regulation"},
                {IsImportantPublicService, "the service is of importance to the public"},
                {IsOfferToAnyMemberOfPublic, "the offer is open to any member of public"},
                {IsAdvantageOverMemberOfPublic, "there is a decisive advantage over any member of the public"},
                {IsStandardizedAdhesion, "the agreement involves a standardized exculpation form"},
                {IsSubjectToSellerCarelessness, "purchaser is subject to risk by seller carelessness"},
            };
        }
    }
}
