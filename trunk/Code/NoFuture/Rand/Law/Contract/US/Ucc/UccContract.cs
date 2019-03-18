using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Ucc
{
    /// <inheritdoc cref="LegalConcept"/>
    /// <inheritdoc cref="IContract{T}"/>
    /// <summary>
    /// <![CDATA[
    /// the total legal obligation that results from 
    /// the parties' agreement as determined by the UCC
    /// ]]>
    /// </summary>
    public class UccContract<T> : LegalConcept, IUccItem, IContract<T> where T : IUccItem
    {
        public virtual IAssent Assent { get; set; }

        public ILegalConcept Offer { get; set; }

        public Func<ILegalConcept, T> Acceptance { get; set; }

        /// <summary>
        /// the Perfect Tender rule does not apply to UCC installment contracts
        /// </summary>
        [Aka("UCC 2-612")]
        public virtual bool IsInstallmentContract { get; set; }

        public ILegalPerson GetOfferor(ILegalPerson[] persons)
        {
            return persons.Offeror();
        }

        public ILegalPerson GetOfferee(ILegalPerson[] persons)
        {
            return persons.Offeree();
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = GetOfferor(persons);
            var offeree = GetOfferee(persons);

            if (Assent == null)
            {
                AddReasonEntry("There is no agreement.");
                return false;
            }

            if (!Assent.IsValid(offeror, offeree))
            {
                AddReasonEntry("The agreement is invalid");
                AddReasonEntryRange(Assent.GetReasonEntries());
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
