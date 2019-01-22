using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.AcceptanceTests
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
            public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
            {
                AddReasonEntry("this was withdrawn before Beehee was aware of any acceptance");
                return false;
            }
        }

        public class AcceptanceSmiths2Behee : Promise
        {
            public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
            {
                return offeror is Behee && offeree is Smiths;
            }
        }

        public class Behee : VocaBase, ILegalPerson
        {
            public Behee(): base("Behee") { }
        }

        public class Smiths : VocaBase, ILegalPerson
        {
            public Smiths() : base("the Smiths") { }
        }
    }
}
