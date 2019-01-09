using System;

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
        private readonly Agreement _agreement;
        public Contract(Agreement agreement)
        {
            _agreement = agreement;
        }

        public virtual Agreement Agreement => _agreement;
        public T SaleOf { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (_agreement == null)
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

            if (!_agreement.IsValid(offeree, offeror))
            {
                AddAuditEntry("The agreement is invalid");
                AddAuditEntryRange(_agreement.GetAuditEntries());
                return false;
            }

            if (!IsEnforceableInCourt)
            {
                AddAuditEntry("The contract is not enforceable in court.");
                return false;
            }

            return _agreement?.IsValid(offeree, offeror) ?? false;
        }

        public override bool IsEnforceableInCourt => true;
    }
}
