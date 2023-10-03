using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.US.Elements
{
    /// <summary>
    /// Liability received through association to some other wrongdoer.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// infringes vicariously by profiting from direct infringement while declining to
    /// exercise a right to stop or limit it. 
    /// Shapiro, Bernstein & Co. v. H.L. Green Co., 316 F.2d 304, 307 (C.A.2 1963)
    /// ]]>
    /// </remarks>
    public class  VicariousLiability : AdiunctusLiability
    {
        public VicariousLiability(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsAttempt2LimitOthersAct { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            if (!Is3rdPartyBeneficialRelationship(persons))
                return false;

            if (IsAttempt2LimitOthersAct(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsAttempt2LimitOthersAct)} is true");
                return false;
            }

            if (!IsVoluntary(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsVoluntary)} is false");
                return false;
            }

            if (!IsAction(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsAction)} is false");
                return false;
            }

            return true;
        }
    }
}
