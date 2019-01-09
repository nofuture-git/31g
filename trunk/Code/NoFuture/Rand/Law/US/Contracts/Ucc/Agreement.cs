using System;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// the bargain of the parties in fact, as found in their 
    /// language or inferred from other circumstances, including 
    /// course of performance, course of dealing, or usage of trade
    /// ]]>
    /// </summary>
    public abstract class Agreement : ObjectiveLegalConcept, IUccItem, IAssent
    {
        public override bool IsEnforceableInCourt => true;

        /// <inheritdoc />
        public virtual Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => true;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            var intent2Contrx = IsApprovalExpressed ?? (lp => true);

            if (!intent2Contrx(offeror))
            {
                AddAuditEntry($"{offeror?.Name} did not intend this " +
                              "agreement as a binding contract.");
                return false;
            }

            if (!intent2Contrx(offeree))
            {
                AddAuditEntry($"{offeree?.Name} did not intend this " +
                              "agreement as a binding contract.");
                return false;
            }

            return true;
        }
    }
}
