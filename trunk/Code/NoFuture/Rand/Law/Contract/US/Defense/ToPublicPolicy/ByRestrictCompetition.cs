using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy
{
    /// <summary>
    /// contracts that restrict competition
    /// </summary>
    public class ByRestrictCompetition<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByRestrictCompetition(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[
        /// A covenant not to compete is invalid unless it protects some legitimate 
        /// interest beyond the employer's desire to protect itself from competition.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsRestraintSelfServing { get; set; } = lp => false;

        /// <summary>
        /// restriction against choice of fiduciaries outweights commercial interest
        /// </summary>
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
