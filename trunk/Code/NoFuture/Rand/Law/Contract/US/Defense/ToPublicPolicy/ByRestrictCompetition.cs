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
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

            if (!base.IsValid(offeror, offeree))
                return false;

            var offerorTitle = offeror.GetLegalPersonTypeName();
            var offereeTitle = offeree.GetLegalPersonTypeName();

            if (IsRestraintSelfServing(offeror))
            {
                AddReasonEntry($"{offerorTitle} {offeror.Name}, {nameof(IsRestraintSelfServing)} is true");
                return true;
            }
            if (IsRestraintSelfServing(offeree))
            {
                AddReasonEntry($"{offereeTitle} {offeree.Name}, {nameof(IsRestraintSelfServing)} is true");
                return true;
            }
            if (IsInjuriousToPublic(offeror))
            {
                AddReasonEntry($"{offerorTitle} {offeror.Name}, {nameof(IsInjuriousToPublic)} is true");
                return true;
            }
            if (IsInjuriousToPublic(offeree))
            {
                AddReasonEntry($"{offereeTitle} {offeree.Name}, {nameof(IsInjuriousToPublic)} is true");
                return true;
            }

            return false;
        }
    }
}
