using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Discovery
{

    /// <summary>
    /// Based on Fed.R.Civ.P. 26(b)(1)
    /// </summary>
    public abstract class ScopeOfDiscovery : CivilProcedureBase, ILinkedLegalConcept
    {
        /// <summary>
        /// The pleading/answer upon which the requested discovery is sought
        /// </summary>
        public ILegalConcept LinkedTo { get; set; }

        /// <summary>
        /// That which is being discovered for the given person
        /// </summary>
        public override Func<ILegalPerson, ILegalConcept> GetCauseOfAction { get; set; } = lp => null;

        public Predicate<ILegalConcept> IsLimitedByCourtOrder { get; set; } = lc => false;

        public Predicate<ILegalConcept> IsPrivilegedMatter { get; set; } = lc => false;

        public Func<ILegalPerson, ILegalConcept, bool> IsIrrelevantToPartyClaimOrDefense { get; set; } =
            (lc, lp) => false;

        /// <summary>
        /// As a reason to exclude the discovery because it is an expense or burden beyond likely benefit
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// (1) importance of the issues at stake
        /// (2) the amount in controvery
        /// (3) the parties' relative access to relevant information
        /// (4) the parties' resources
        /// (5) the importance of the discovery in resolving the issues
        /// ]]>
        /// </remarks>
        public Predicate<ILegalConcept> IsUnbalancedToNeedsOfCase { get; set; } = lc => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!this.TryGetSubjectPerson(persons, out var subjectPerson))
                return false;

            var subjPersonTitle = subjectPerson.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(subjectPerson, out var toBeDiscovered))
                return false;

            if (IsLimitedByCourtOrder(toBeDiscovered))
            {
                AddReasonEntry($"{subjPersonTitle} {subjectPerson.Name}, {nameof(IsLimitedByCourtOrder)} is true");
                return false;
            }

            if (IsPrivilegedMatter(toBeDiscovered))
            {
                AddReasonEntry($"{subjPersonTitle} {subjectPerson.Name}, {nameof(IsPrivilegedMatter)} is true");
                return false;
            }

            if (IsUnbalancedToNeedsOfCase(toBeDiscovered))
            {
                AddReasonEntry($"{subjPersonTitle} {subjectPerson.Name}, {nameof(IsUnbalancedToNeedsOfCase)} is true");
                return false;
            }

            if (!this.TryGetOppositionPerson(persons, subjectPerson, out var opposition))
                return true;

            var oppositionTitle = opposition.GetLegalPersonTypeName();

            var irrelevant2Subj = IsIrrelevantToPartyClaimOrDefense(subjectPerson, toBeDiscovered);
            var irrelevant2Opp = IsIrrelevantToPartyClaimOrDefense(opposition, toBeDiscovered);

            if (irrelevant2Subj && irrelevant2Opp)
            {
                AddReasonEntry($"{subjPersonTitle} {subjectPerson.Name} and {oppositionTitle} " +
                               $"{opposition.Name}, {nameof(IsIrrelevantToPartyClaimOrDefense)} are both false");
                return false;
            }

            return true;
        }
    }
}
