using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc cref="IContract{T}"/>
    [Aka("Enforceable Promise")]
    public class ComLawContract<T> : ObjectiveLegalConcept, IContract<T> where T : IObjectiveLegalConcept
    {
        [Note("bargained for: if it is sought by one and given by the other")]
        public virtual Consideration<T> Consideration { get; set; }

        [Note("this is what distinguishes a common (donative) promise from a legal one")]
        public override bool IsEnforceableInCourt => true;

        public virtual IObjectiveLegalConcept Offer { get; set; }

        public virtual Func<IObjectiveLegalConcept, T> Acceptance { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {

            if (Consideration == null)
            {
                AddAuditEntry($"{nameof(Consideration)} is null");
                return false;
            }

            if (!Consideration.IsValid(offeror, offeree))
            {
                AddAuditEntry($"{nameof(Consideration)}.{nameof(IsValid)} returned false");
                AddAuditEntryRange(Consideration.GetAuditEntries());
                return false;
            }

            if (Offer == null)
            {
                AddAuditEntry($"{nameof(Offer)} is null");
                return false;
            }

            if (!Offer.IsValid(offeror, offeree))
            {
                AddAuditEntry("the offer in invalid");
                AddAuditEntryRange(Offer.GetAuditEntries());
                return false;
            }

            //short-circuit since this allows for no return promise 
            var promissoryEstoppel = Consideration as PromissoryEstoppel<T>;
            if (promissoryEstoppel != null)
            {
                return true;
            }

            if (!Offer.IsEnforceableInCourt)
            {
                AddAuditEntry("the offer is not enforceable in court");
                AddAuditEntryRange(Offer.GetAuditEntries());
                return false;
            }

            if (Acceptance == null)
            {
                AddAuditEntry($"{nameof(Acceptance)} is null");
                return false;
            }

            var returnPromise = Acceptance(Offer);
            if (returnPromise == null)
            {
                AddAuditEntry($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!returnPromise.IsEnforceableInCourt)
            {
                AddAuditEntry("the return promise is not enforceable in court");
                AddAuditEntryRange(returnPromise.GetAuditEntries());
                return false;
            }

            if (!returnPromise.IsValid(offeror, offeree))
            {
                AddAuditEntry("the return promise is invalid");
                AddAuditEntryRange(returnPromise.GetAuditEntries());
                return false;
            }


            return true;
        }
    }
}