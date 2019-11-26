using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Judgment
{
    /// <summary>
    /// For when material facts already resolve to a conclusion 
    /// </summary>
    public class SummaryJudgment : CivilProcedureBase
    {
        public Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; set; } = lps => null;

        /// <summary>
        /// An element which is essential to prove true or false
        /// </summary>
        [Aka("material fact")]
        public Predicate<ILegalConcept> IsEssentialElement { get; set; } = lp => false;

        /// <summary>
        /// The truth-value the essential element must have
        /// </summary>
        public Predicate<ILegalConcept> RequiredTruthValue { get; set; } = lc => true;

        /// <summary>
        /// The truth-value the essential element actually has
        /// </summary>
        public Predicate<ILegalConcept> ActualTruthValue { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subjectPerson = GetSubjectPerson(persons);
            if (subjectPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }

            var title = subjectPerson.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(subjectPerson, out var subject))
                return false;

            //when the claim is based on something which is not a question of law
            if (!subject.IsEnforceableInCourt)
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(GetAssertion)} " +
                               $"returned instance where {IsEnforceableInCourt} is false");
                return true;
            }

            if (!IsEssentialElement(subject))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsEssentialElement)} is false");
                return false;
            }

            //whatever is essential is required this but is actually that
            var requiredValue = RequiredTruthValue(subject);
            var actualValue = ActualTruthValue(subject);

            if (requiredValue == actualValue)
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(RequiredTruthValue)} " +
                               $"and {nameof(ActualTruthValue)} are both {actualValue}");
                return false;
            }

            return true;
        }
    }
}
