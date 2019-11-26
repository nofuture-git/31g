using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Judgment
{
    /// <summary>
    /// Some procedural flaw has made due-process of law impossible then start things over (re-do)
    /// </summary>
    public class NewTrial : CivilProcedureBase
    {
        public Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; set; } = lps => null;

        public Predicate<ILegalPerson> IsCommittedProceduralError { get; set; } = lp => false;

        public Predicate<ILegalConcept> IsJurySeriouslyErroneousResult { get; set; } = lc => false;

        public Predicate<ILegalConcept> IsNeededToPreventInjustice { get; set; } = lc => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            string title;
            foreach (var person in persons)
            {
                if (IsCommittedProceduralError(person))
                {
                    title = person.GetLegalPersonTypeName();
                    AddReasonEntry($"{title} {person.Name}, {nameof(IsCommittedProceduralError)} is true");
                    return true;
                }
            }

            var subjectPerson = GetSubjectPerson(persons);
            if (subjectPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            title = subjectPerson.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(subjectPerson, out var subjectConcept))
                return false;

            if (IsJurySeriouslyErroneousResult(subjectConcept))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsJurySeriouslyErroneousResult)} is true");
                return true;
            }

            if (IsNeededToPreventInjustice(subjectConcept))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsNeededToPreventInjustice)} is true");
                return true;
            }

            return false;
        }
    }
}
