using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Property.US.Acquisition.Donative
{
    /// <summary>
    /// A gift given between two living people
    /// </summary>
    [EtymologyNote("Latin", "inter vivos", "between the living")]
    public class InterVivos : LegalConcept, IBargain<ILegalProperty, ILegalProperty>
    {
        public ILegalProperty Offer { get; set; }

        [Aka("delivery")]
        public Func<ILegalProperty, ILegalProperty> Acceptance { get; set; }

        public IAssent Assent { get; set; } = Consent.IsGiven();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            //get the two parties 
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);

            if (offeree == null || offeror == null)
            {
                AddReasonEntry($"{nameof(InterVivos)} requires both an " +
                               $"{nameof(IOfferee)} and an {nameof(IOfferor)}");
                return false;
            }

            var ooTitle = offeror.GetLegalPersonTypeName();

            //check the parts are all here
            if (Offer == null)
            {
                AddReasonEntry($"{nameof(Offer)} is unassigned");
                return false;
            }

            if (Acceptance == null)
            {
                AddReasonEntry($"{nameof(Acceptance)} is unassigned");
                return false;
            }

            //make sure the offer is from the owner
            var isOfferorCurrentOwner = this.PropertyOwnerIsSubjectPerson(Offer, offeror);
            if (!isOfferorCurrentOwner)
            {
                return false;
            }

            //make sure some kind of exchange occurs
            var performExchange = Acceptance(Offer);
            if (performExchange == null)
            {
                AddReasonEntry($"{ooTitle} {offeror.Name}, {nameof(Acceptance)} returned nothing");
                return false;
            }

            //only care that the offeree is the owner of it
            var isOffereeOwnerOfExchangeResult = this.PropertyOwnerIsSubjectPerson(performExchange, offeree);
            if (!isOffereeOwnerOfExchangeResult)
            {
                return false;
            }

            //and the offeror is not the owner of the Offer
            isOfferorCurrentOwner = this.PropertyOwnerIsSubjectPerson(Offer, offeror);
            if (isOfferorCurrentOwner)
            {
                return false;
            }

            //don't require this but test it if its here
            if (Assent != null && !Assent.IsValid(persons))
            {
                AddReasonEntryRange(Assent.GetReasonEntries());
                return false;
            }

            return true;
        }

    }
}
