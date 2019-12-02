using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Judgment
{
    /// <summary>
    /// Capacity of a judge to withdraw a case from a jury when there is no meaningful question of law
    /// </summary>
    /// <remarks>
    /// <![CDATA[ "a method for protecting neutral principles of law from powerful forces
    /// outside the scope of law - compassion and prejudice" Rutherford v. Illinois Central R.R.(1960) ]]>
    /// </remarks>
    /// <remarks>
    /// Is not a verdict since its not rendered by a jury.
    /// </remarks>
    [Aka("directed verdict", "motion for judgment as a matter of law")]
    public class JudgmentAsMatterOfLaw : CivilProcedureBase, IJudgment
    {
        public Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; set; } = lps => null;

        /// <summary>
        /// Where unlikely fades into the impossible
        /// </summary>
        public Predicate<ILegalConcept> IsCaseWeakBeyondReason { get; set; } = lc => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            var subjectPerson = GetSubjectPerson(persons);
            if (subjectPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subjectPerson.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(subjectPerson, out var subject))
                return false;

            if (IsCaseWeakBeyondReason(subject))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsCaseWeakBeyondReason)} is true");
                return true;
            }

            return false;
        }
    }
}
