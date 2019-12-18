using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US
{
    /// <inheritdoc cref="IContract{T}"/>
    /// <remarks>
    /// <![CDATA[
    /// It is axiomatic in modern contracts law that all contracts 
    /// are, in some fashion, "incomplete."  Meaning, no contract 
    /// has addressed every possible contingency.
    /// ]]>
    /// </remarks>
    [Aka("Enforceable Promise")]
    public class ComLawContract<T> : LegalConcept, IContract<T> where T : ILegalConcept
    {
        [Note("bargained for: if it is sought by one and given by the other")]
        public virtual IConsideration<T> Consideration { get; set; }

        public virtual IAssent Assent { get; set; }

        [Note("this is what distinguishes a common (donative) promise from a legal one")]
        public override bool IsEnforceableInCourt => true;

        public virtual ILegalConcept Offer { get; set; }

        public virtual Func<ILegalConcept, T> Acceptance { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();
            if (!IsEnforceableInCourt)
            {
                AddReasonEntry("The contract is not enforceable in court and is therefore void.");
                return false;
            }

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

            if (Assent != null && !Assent.IsValid(offeror, offeree))
            {
                AddReasonEntry($"{nameof(Assent)}.{nameof(IsValid)} returned false");
                AddReasonEntryRange(Assent.GetReasonEntries());
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            AddReasonEntryRange(Assent?.GetReasonEntries());
            AddReasonEntryRange(Consideration?.GetReasonEntries());
            AddReasonEntryRange(Offer?.GetReasonEntries());
            return base.ToString();
        }
    }
}