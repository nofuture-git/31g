using System;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <inheritdoc cref="ObjectiveLegalConcept"/>
    /// <inheritdoc cref="IAssent"/>
    /// <summary>
    /// <![CDATA[
    /// the bargain of the parties in fact, as found in their 
    /// language or inferred from other circumstances, including 
    /// course of performance, course of dealing, or usage of trade
    /// ]]>
    /// </summary>
    public abstract class Agreement<T> : ObjectiveLegalConcept, IUccItem, IAssent where T : IUccItem
    {
        public override bool IsEnforceableInCourt => true;

        public virtual Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => true;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            var intent2Contrx = IsApprovalExpressed ?? (lp => true);

            if (!intent2Contrx(offeror))
            {
                AddReasonEntry($"{offeror?.Name} did not intend this " +
                              "agreement as a binding contract.");
                return false;
            }

            if (!intent2Contrx(offeree))
            {
                AddReasonEntry($"{offeree?.Name} did not intend this " +
                              "agreement as a binding contract.");
                return false;
            }

            return true;
        }
    }
}
