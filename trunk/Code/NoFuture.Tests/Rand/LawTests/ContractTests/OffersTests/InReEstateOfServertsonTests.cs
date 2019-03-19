using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.OffersTests
{
    /// <summary>
    /// IN RE ESTATE OF SEVERTSON Court of Appeals of Minnesota 1998 Minn.App.LEXIS 243 (March 3, 1998)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// What happens if the Offeree dies but seemed to know 
    /// that was possible and intended an offer to still 
    /// remain despite it?
    /// 
    /// The court seems to think that the will to assent to 
    /// an offer may only exist within the vessel of a body 
    /// and so dies if the body dies.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class InReEstateOfServertsonTests
    {
        [Test]
        public void InReEstateOfServertson()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferToPurchaseFarmsite(),
                Acceptance = o => o is OfferToPurchaseFarmsite ? new AcceptancePriceInSignedDoc() : null,
                Assent = new TypedAndSignedDocument()
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByOfferor = (lp, promise) => lp is HelenSevertson && promise is AcceptancePriceInSignedDoc,
                IsGivenByOfferee = (lp, promise) => lp is MarkAndKellyThorson && promise is OfferToPurchaseFarmsite
            };

            var helen = new HelenSevertson();
            var markAndKelly = new MarkAndKellyThorson();

            var testResult = testSubject.IsValid(helen, markAndKelly);
            Assert.IsTrue(testResult);
            helen.HasDied = true;
            testResult = testSubject.IsValid(helen, markAndKelly);
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferToPurchaseFarmsite : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            var helen = offeror as HelenSevertson;
            if (helen == null)
                return false;
            var theThorsons = offeree as MarkAndKellyThorson;
            if (theThorsons == null)
                return false;

            var isHelenStillAlive = !helen.HasDied;
            if(!isHelenStillAlive)
                AddReasonEntry($"{helen.Name} has died.");
            return isHelenStillAlive;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptancePriceInSignedDoc : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            var helen = offeror as HelenSevertson;
            if (helen == null)
                return false;
            var theThorsons = offeree as MarkAndKellyThorson;
            if (theThorsons == null)
                return false;
            return true;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class TypedAndSignedDocument : MutualAssent
    {
        public TypedAndSignedDocument()
        {
            TermsOfAgreement = lp => GetTerms();
            IsApprovalExpressed = lp =>
                (lp is HelenSevertson || lp is MarkAndKellyThorson) && IsSignedByBothParties;
        }
        private static object _term00 = new object();

        //everyone knows what property we are talking about
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("farmsite", _term00)
            };
        }

        public bool IsSignedByBothParties => true;
    }

    public class HelenSevertson : LegalPerson, IOfferor
    {
        public HelenSevertson() : base("Helen Severtson") { }
        public bool HasDied { get; set; }
    }

    public class MarkAndKellyThorson : LegalPerson, IOfferee
    {
        public MarkAndKellyThorson() : base("Mark & KellyThorson") { }
    }
}
