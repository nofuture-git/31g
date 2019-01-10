using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// the total legal obligation that results from 
    /// the parties' agreement as determined by the UCC
    /// ]]>
    /// </summary>
    public class Contract<T> : ObjectiveLegalConcept, IUccItem where T : IUccItem
    {
        [Note("the bargain of the parties")]
        public virtual Agreement Agreement { get; set; }

        [Note("the scope of the commercial context")]
        public T SaleOf { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (Agreement == null)
            {
                AddAuditEntry("There is no agreement.");
                return false;
            }

            if (SaleOf != null && SaleOf.IsValid(offeror, offeree) == false)
            {
                AddAuditEntry($"The {SaleOf.GetType().Name} is invalid");
                AddAuditEntryRange(SaleOf.GetAuditEntries());
                return false;
            }

            if (!Agreement.IsValid(offeree, offeror))
            {
                AddAuditEntry("The agreement is invalid");
                AddAuditEntryRange(Agreement.GetAuditEntries());
                return false;
            }

            if (!IsEnforceableInCourt)
            {
                AddAuditEntry("The contract is not enforceable in court.");
                return false;
            }

            return Agreement?.IsValid(offeree, offeror) ?? false;
        }

        public override bool IsEnforceableInCourt => true;
    }
}
