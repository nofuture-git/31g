using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <summary>
    /// The type which from the reciprocal of an offer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// <![CDATA[
    /// - origin from "quid pro quo",
    /// is it offered in order to get something in return,
    /// something that has value in the eyes of the law.  
    /// Illusionary promise: words in promissory form that promise nothing
    /// ]]>
    /// </remarks>
    [Note("is performance or return promise bargained for", 
          "is not just a choice (illusionary promise)",
          "is not already obligated to do")]
    public class Consideration<T> : ObjectiveLegalConcept where T : ObjectiveLegalConcept
    {
        private readonly LegalContract<T> _contract;
        public override bool IsEnforceableInCourt => true;

        public Consideration(LegalContract<T> contract)
        {
            _contract = contract;
            _contract.Consideration = this;
        }

        /// <summary>
        /// A test for if Acceptance is actually what the promisor wants in return.
        /// </summary>
        public virtual Func<ILegalPerson, T, bool> IsSoughtByPromisor { get; set; }

        /// <summary>
        /// A test for if Offer is actually what the promisee wants in return.
        /// </summary>
        public virtual Func<ILegalPerson, ObjectiveLegalConcept, bool> IsGivenByPromisee { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (IsSoughtByPromisor == null)
            {
                AddAuditEntry($"{nameof(IsSoughtByPromisor)} is null");
                return false;
            }

            if (IsGivenByPromisee == null)
            {
                AddAuditEntry($"{nameof(IsGivenByPromisee)} is null");
                return false;
            }

            var returnPromise = _contract.Acceptance(_contract.Offer);
            if (returnPromise == null)
            {
                AddAuditEntry($"{nameof(returnPromise)} is null");
                return false;
            }

            if (!IsSoughtByPromisor(offeror, returnPromise))
            {
                AddAuditEntry($"the return promise is not what {offeror.Name} wants");
                return false;
            }

            if (!IsGivenByPromisee(offeree, _contract.Offer))
            {
                AddAuditEntry($"the offer is not what the {offeree.Name} wants");
                return false;
            }

            return true;
        }

    }
}