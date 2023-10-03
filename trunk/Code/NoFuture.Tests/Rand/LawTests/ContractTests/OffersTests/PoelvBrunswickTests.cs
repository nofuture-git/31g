using System;
using System.Linq;
using NoFuture.Law.Contract.US;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.OffersTests
{
    /// <summary>
    /// POEL v. BRUNSWICK-BALKE-COLLENDER CO. Court of Appeals of New York 216 N.Y. 310, 110 N.E. 619 (1915)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// The doctrine point of this case what may appear as acceptance of a previous offer is 
    /// infact a counter-offer followed by no acceptance (rejection).
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PoelvBrunswickTests
    {
        [Test]
        public void PoelvBrunswick()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new Apr6Bruns2Poel(),
                Acceptance = o => o is Apr6Bruns2Poel ? new Jan7Bruns2Poel() : null
            };
            var testResult = testSubject.IsValid(new Brunswick(), new Poel());
            Console.WriteLine(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    /// <summary>
    /// Mr Kelly with Poel is contacting Brunswick about confirming an offer
    /// </summary>
    public class Apr2Poel2Bruns : Promise
    {

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Brunswick || offeror is Poel)
                   && (offeree is Brunswick || offeree is Poel);
        }
    }

    /// <summary>
    /// Mr. Kelly with Poel is again contacting Brunswick with a contract (offer) enclosed
    /// </summary>
    public class Apr4Poel2Bruns : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Brunswick || offeror is Poel)
                   && (offeree is Brunswick || offeree is Poel);
        }
    }

    /// <summary>
    /// Mr. Rogers with Brunswick writing to Poel - the court finds that this 
    /// is, in fact, a counter-offer, not an acceptance
    /// </summary>
    public class Apr6Bruns2Poel : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Brunswick || offeror is Poel)
                   && (offeree is Brunswick || offeree is Poel);
        }
    }

    /// <summary>
    /// Mr. Miller with Brunswick to Poel advising that no acceptance is made
    /// </summary>
    public class Jan7Bruns2Poel : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            AddReasonEntry("Brunswick never agreed to the counter offer from Mr. Rogers on Apr. 6th ");
            return false;
        }
    }

    public class Poel : LegalPerson, IOfferee
    {
        public Poel() : base("Poel & Arnold was a rubber importer") { }
    }

    public class Brunswick : LegalPerson, IOfferor
    {
        public Brunswick(): base("BRUNSWICK-BALKE-COLLENDER CO. manufacturer of various items made from rubber") { }
    }
}
