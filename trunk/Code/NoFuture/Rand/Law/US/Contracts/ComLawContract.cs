﻿using System;
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
        public virtual IObjectiveLegalConcept Offer { get; set; }

        /// <summary>
        /// A function which resolves what the offer gets in return.
        /// </summary>
        /// <remarks>
        /// when an offer has indicated the mode and means of acceptance, 
        /// an acceptance in accordance with that mode or means is binding 
        /// on the offeror
        /// </remarks>
        public virtual Func<IObjectiveLegalConcept, T> Acceptance { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {

            if (Consideration == null)
            {
                AddReasonEntry($"{nameof(Consideration)} is null");
                return false;
            }

            if (!Consideration.IsValid(offeror, offeree))
            {
                AddReasonEntry($"{nameof(Consideration)}.{nameof(IsValid)} returned false");
                AddReasonEntryRange(Consideration.GetReasonEntries());
                return false;
            }

            if (Offer == null)
            {
                AddReasonEntry($"{nameof(Offer)} is null");
                return false;
            }

            if (!Offer.IsValid(offeror, offeree))
            {
                AddReasonEntry("the offer in invalid");
                AddReasonEntryRange(Offer.GetReasonEntries());
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
                AddReasonEntry("the offer is not enforceable in court");
                AddReasonEntryRange(Offer.GetReasonEntries());
                return false;
            }

            if (Acceptance == null)
            {
                AddReasonEntry($"{nameof(Acceptance)} is null");
                return false;
            }

            var returnPromise = Acceptance(Offer);
            if (returnPromise == null)
            {
                AddReasonEntry($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!returnPromise.IsEnforceableInCourt)
            {
                AddReasonEntry("the return promise is not enforceable in court");
                AddReasonEntryRange(returnPromise.GetReasonEntries());
                return false;
            }

            if (!returnPromise.IsValid(offeror, offeree))
            {
                AddReasonEntry("the return promise is invalid");
                AddReasonEntryRange(returnPromise.GetReasonEntries());
                return false;
            }


            return true;
        }
    }
}