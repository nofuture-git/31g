using System;
using System.Linq;
using NoFuture.Law.Contract.US;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.AcceptanceTests
{
    /// <summary>
    /// HENDRICKS v. BEHEE Court of Appeals of Missouri, Southern District 786 S.W.2d 610 (Mo.Ct.App. 1990)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine appears to be that acceptance is invalid if the offer gets withdrawn.
    /// Implies that consideration is optional...(?)
    /// Lots of good comments otherwise:
    /// [   An uncommunicated intention to accept an offer is not an acceptance. When an offer 
    /// calls for a promise, as distinguished from an act, on the part of the offeree, notice 
    /// of acceptance is always essential. A mere private act of the offeree does not 
    /// constitute an acceptance. Communication of acceptance of a contract to an agent of the 
    /// offeree is not sufficient and does not bind the offeror.
    ///     Unless the offer is supported by consideration, an offeror may withdraw his offer at 
    /// any time "before acceptance and communication of that fact to him." To be effective, 
    /// revocation of an offer must be communicated to the offeree before he has accepted.  
    ///     Notice to the agent, within the scope of the agent’s authority, is notice to the 
    /// principal, and the agent’s knowledge is binding on the principal.]
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HendricksvBeheeTests
    {
        [Test]
        public void HendricksvBehee()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferFromBehee2Smiths(),
                Acceptance = o => o is OfferFromBehee2Smiths ? new AcceptanceSmiths2Behee() : null,

            };

            var testResult = testSubject.IsValid(new Behee(), new Smiths());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        public class OfferFromBehee2Smiths : Promise
        {
            public override bool IsValid(params ILegalPerson[] persons)
            {
                var offeror = persons.FirstOrDefault();
                var offeree = persons.Skip(1).Take(1).FirstOrDefault();
                AddReasonEntry("this was withdrawn before Beehee was aware of any acceptance");
                return false;
            }
        }

        public class AcceptanceSmiths2Behee : Promise
        {
            public override bool IsValid(params ILegalPerson[] persons)
            {
                var offeror = persons.FirstOrDefault();
                var offeree = persons.Skip(1).Take(1).FirstOrDefault();
                return offeror is Behee && offeree is Smiths;
            }
        }

        public class Behee : LegalPerson, IOfferor
        {
            public Behee(): base("Behee") { }
        }

        public class Smiths : LegalPerson, IOfferee
        {
            public Smiths() : base("the Smiths") { }
        }
    }
}
