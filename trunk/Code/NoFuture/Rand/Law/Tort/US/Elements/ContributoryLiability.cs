using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.US.Elements
{
    /// <summary>
    /// by intentionally inducing or encouraging direct infringement,
    /// see Gershwin Pub. Corp. v. Columbia Artists Management, Inc., 443 F.2d 1159, 1162 (C.A.2 1971)
    /// </summary>
    public class ContributoryLiability : AdiunctusLiability
    {
        public ContributoryLiability(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsInduceOthersAct { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsEncourageOthersAct { get; set; } = lp => false;

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

            if (!IsInduceOthersAct(subj) && !IsEncourageOthersAct(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsInduceOthersAct)} " +
                               $"and {IsEncourageOthersAct} are both false");
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

            throw new NotImplementedException();
        }
    }
}
