using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense.ToAssent;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.DefenseTests
{
    /// <summary>
    /// ALABI v. DHL AIRWAYS, INC. Superior Court of Delaware, New Castle 583 A.2d 1358 (Del. Super. 1990)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, objective tests to fraud defense against contract enforcement
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class AlabivDhlAirwaysTests
    {
        [Test]
        public void AlabivDhlAirways()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferShipCashUndeclared(),
                Acceptance = o => o is OfferShipCashUndeclared ? new AcceptShipCashUndeclared() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testSubject = new ByFraud<Promise>(testContract)
            {
                Misrepresentation = new Misrepresentation<Promise>(testContract)
                {
                    /* plaintiff had reason to know DHL would not accept 
                       cash and describing the shipment as otherwise, he 
                       induced DHL to enter into a contract to which it 
                       would not otherwise assent to */
                    IsAssertionToInduceAssent = lp => lp is Alabi
                },
                /*the misrepresentation was a substantial factor in DHL's
                 decision to enter into the contracts at issue*/
                IsRecipientInduced = lp => lp is DhlAirways,

                IsRecipientRelianceReasonable = lp => lp is DhlAirways
            };

            var testResult = testSubject.IsValid(new Alabi(), new DhlAirways());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());

        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("express package delivery", DBNull.Value)
            };
        }
    }

    public class OfferShipCashUndeclared : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Alabi || offeror is DhlAirways)
                   && (offeree is Alabi || offeree is DhlAirways);
        }
    }

    public class AcceptShipCashUndeclared : OfferShipCashUndeclared { }

    public class Alabi : LegalPerson
    {
        public Alabi():base("Mabayomije Alabi") { }

        /// <summary>
        /// <![CDATA[
        /// The term “documents regarding school bills” does not indicate that the contents are inherently valuable to third-party interlopers or others in DHL’s position.
        /// ]]>
        /// </summary>
        public bool IsCashShippedAsSchoolBills => true;
    }

    public class DhlAirways : LegalPerson
    {
        public DhlAirways(): base("DHL AIRWAYS, INC.") {}
    }
}
