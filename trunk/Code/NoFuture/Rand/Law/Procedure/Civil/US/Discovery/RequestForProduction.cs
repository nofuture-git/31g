using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Discovery
{
    /// <summary>
    /// Authorize one party to required an opponent to produce
    /// &quot;documents&quot; (i.e. thing(s) related to the facts)
    /// for inspection and copying.
    /// From Fed.R.Civ.P. 34
    /// </summary>
    public class RequestForProduction : ScopeOfDiscovery
    {
        /// <summary>
        /// When the information is difficult to get such as being somewhere in 800 backup tapes
        /// Fed.R.Civ.P. 26(b)(2)(B)
        /// </summary>
        public Predicate<ILegalConcept> IsReasonablyAccessible { get; set; } = lc => true;

        /// <summary>
        /// Whenever a privileged document is sent to an opponent by mistake - the opponent can be limited from using it.
        /// Fed.R.Civ.P. 26(b)(5)(B)
        /// </summary>
        public Predicate<ILegalConcept> IsInadvertentDisclosure { get; set; } = lc => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            var subjectPerson = GetSubjectPerson(persons);

            if (subjectPerson == null && !this.TryGetSubjectPerson(persons, out subjectPerson))
                return false;

            var subjPersonTitle = subjectPerson.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(subjectPerson, out var toBeDiscovered))
                return false;

            if (!IsReasonablyAccessible(toBeDiscovered))
            {
                AddReasonEntry($"{subjPersonTitle} {subjectPerson.Name}, {nameof(IsReasonablyAccessible)} is false");
                return false;
            }

            if (IsInadvertentDisclosure(toBeDiscovered))
            {
                AddReasonEntry($"{subjPersonTitle} {subjectPerson.Name}, {nameof(IsInadvertentDisclosure)} is false");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
