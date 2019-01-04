using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Enforceable Promise")]
    public class LegalContract<T> : ObjectiveLegalConcept where T : ObjectiveLegalConcept
    {

        [Note("bargained for: if it is sought by one and given by the other")]
        public virtual Consideration<T> Consideration { get; set; }

        [Note("this is what distinguishes a common (donative) promise from a legal one")]
        public override bool IsEnforceableInCourt => true;

        /// <summary>
        /// What the promisor is putting out there.
        /// </summary>
        /// <remarks>
        /// May be terminated by
        /// (a) rejection or counter-offer by the offeree, or
        /// (b) lapse of time, or
        /// (c) revocation by the offeror, or
        /// (d) death or incapacity of the offeror or offeree.
        /// </remarks>
        [Note("Is the manifestation of willingness to enter into a bargain")]
        public virtual ObjectiveLegalConcept Offer { get; set; }

        /// <summary>
        /// A function which resolves what the offer gets in return.
        /// </summary>
        public virtual Func<ObjectiveLegalConcept, T> Acceptance { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (Offer == null)
            {
                AddAuditEntry($"{nameof(Offer)} is null");
                return false;
            }

            if (Acceptance == null)
            {
                AddAuditEntry($"{nameof(Acceptance)} is null");
                return false;
            }

            if (!Offer.IsEnforceableInCourt)
            {
                AddAuditEntry("the offer is not enforceable in court");
                AddAuditEntryRange(Offer.GetAuditEntries());
                return false;
            }

            if (!Offer.IsValid(offeror, offeree))
            {
                AddAuditEntry("the offer in invalid");
                AddAuditEntryRange(Offer.GetAuditEntries());
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

            if (Consideration == null)
            {
                AddAuditEntry($"{nameof(Consideration)} is null");
                return false;
            }

            if (!Consideration.IsValid(offeror, offeree))
            {
                AddAuditEntry($"{nameof(Consideration)}.{nameof(IsValid)} returned false");
                return false;
            }

            return true;
        }
    }
}