using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy
{
    /// <inheritdoc cref="IRestrictCompetition"/>
    public class ByRestrictCompetition<T> : DefenseBase<T>, IRestrictCompetition where T : ILegalConcept
    {
        public ByRestrictCompetition(IContract<T> contract) : base(contract)
        {
        }

        public Predicate<ILegalPerson> IsRestraintSelfServing { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsInjuriousToPublic { get; set; } = lp => false;
            
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;
            var rslt = false;
            if (IsRestraintSelfServing(offeror) || IsRestraintSelfServing(offeree))
            {
                AddReasonEntry("the restraint is greater than necessary to protect a legitimate interest");
                rslt = true;
            }

            if (IsInjuriousToPublic(offeror) || IsInjuriousToPublic(offeree))
            {
                AddReasonEntry("interest is outweighed by the hardship and the likely injury to the public.");
                rslt = true;
            }

            return rslt;
        }
    }
}
