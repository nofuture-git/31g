using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.OffersTests
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
                IsSoughtByPromisor = (lp, promise) => lp is HelenSevertson && promise is AcceptancePriceInSignedDoc,
                IsGivenByPromisee = (lp, promise) => lp is MarkAndKellyThorson && promise is OfferToPurchaseFarmsite
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
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
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
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
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

    public class HelenSevertson : LegalPerson
    {
        public HelenSevertson() : base("Helen Severtson") { }
        public bool HasDied { get; set; }
    }

    public class MarkAndKellyThorson : LegalPerson
    {
        public MarkAndKellyThorson() : base("Mark & KellyThorson") { }
    }
}
