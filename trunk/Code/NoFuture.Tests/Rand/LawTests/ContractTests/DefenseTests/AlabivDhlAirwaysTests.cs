using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToAssent;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// ALABI v. DHL AIRWAYS, INC. Superior Court of Delaware, New Castle 583 A.2d 1358 (Del. Super. 1990)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class AlabivDhlAirwaysTests
    {
        [Test]
        public void AlabivDhlAirways()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferShipCashUndeclared(),
                Acceptance = o => o is OfferShipCashUndeclared ? new AcceptShipCashUndeclared() : null,
                MutualAssent = new MutualAssent
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
                IsMisrepresentationPresent = lp => (lp as Alabi)?.IsCashShippedAsSchoolBills ?? false,

            };

            var testResult = testSubject.IsValid(new Alabi(), new DhlAirways());

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
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
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
