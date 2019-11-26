using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Judgment
{
    /// <summary>
    /// While <see cref="JudgmentAsMatterOfLaw"/> is to keep jury&apos;s from
    /// making irrational verdicts - this when a jury, in fact, has rendered an irrational verdict.
    /// </summary>
    [Aka("judgment non obstante veredicto", "j.n.o.v.")]
    public class JudgmentNotwithstandingVerdict : JudgmentAsMatterOfLaw
    {
        /// <summary>
        /// Fed.R.Civ.P. 50(b) means the party already request
        /// a <see cref="JudgmentAsMatterOfLaw"/> before any verdict was made.
        /// </summary>
        /// <remarks>
        /// To preserve the idea that a judge has not &quot;reexamined&quot;
        /// a jury&apos;s findings which against Seventh Amendment of U.S. Constitution
        /// </remarks>
        public Predicate<ILegalPerson> IsMadeMotionPriorToVerdict { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subjectPerson = GetSubjectPerson(persons);
            if (subjectPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subjectPerson.GetLegalPersonTypeName();

            if (!IsMadeMotionPriorToVerdict(subjectPerson))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsMadeMotionPriorToVerdict)} is false");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
