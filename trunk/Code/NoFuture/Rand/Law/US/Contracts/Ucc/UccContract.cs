using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <inheritdoc cref="ObjectiveLegalConcept"/>
    /// <inheritdoc cref="IContract{T}"/>
    /// <summary>
    /// <![CDATA[
    /// the total legal obligation that results from 
    /// the parties' agreement as determined by the UCC
    /// ]]>
    /// </summary>
    public class UccContract<T> : ObjectiveLegalConcept, IUccItem, IContract<T> where T : IUccItem
    {
        [Note("the bargain of the parties")]
        public virtual IAssent Agreement { get; set; }

        public IObjectiveLegalConcept Offer { get; set; }
        public Func<IObjectiveLegalConcept, T> Acceptance { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (Agreement == null)
            {
                AddReasonEntry("There is no agreement.");
                return false;
            }

            if (!Agreement.IsValid(offeree, offeror))
            {
                AddReasonEntry("The agreement is invalid");
                AddReasonEntryRange(Agreement.GetReasonEntries());
                return false;
            }

            if (!IsEnforceableInCourt)
            {
                AddReasonEntry("The contract is not enforceable in court and is therefore void.");
                return false;
            }

            if (Offer != null && Offer.IsValid(offeror, offeree) == false)
            {
                AddReasonEntry($"The {Offer.GetType().Name} is invalid");
                AddReasonEntryRange(Offer.GetReasonEntries());
                return false;
            }
            
            if(Acceptance == null)
                return true;

            var acceptance = Acceptance(Offer);
            if (acceptance != null && acceptance.IsValid(offeror, offeree) == false)
            {
                AddReasonEntry($"The {acceptance.GetType().Name} is invalid");
                AddReasonEntryRange(acceptance.GetReasonEntries());
                return false;
            }

            return true;
        }

        public override bool IsEnforceableInCourt => true;
    }
}
