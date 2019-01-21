using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToAssent;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// WOOD v. BOYNTON Supreme Court of Wisconsin 64 Wis. 265, 25 N.W. 42 (1885)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, fraud requires misrepresentation
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class WoodvBoyntonTests
    {
        [Test]
        public void WoodvBoynton()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferSmallStone(),
                Acceptance = o => o is OfferSmallStone ? new AcceptSmallStone() : null,
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

            //court found there was no misrepresentation
            var testSubject = new ByFraud<Promise>(testContract);

            var testResult = testSubject.IsValid(new Boynton(), new Wood());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("small stone", DBNull.Value)
            };
        }
    }

    public class OfferSmallStone : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Wood || offeror is Boynton)
                   && (offeree is Wood || offeree is Boynton);
        }

        public decimal AskingPrice => 1m; //37 in 2018 dollars
        public decimal ActualWorth => 700m; //26223 in 2018 dollars, bummer
    }

    public class AcceptSmallStone : OfferSmallStone { }

    public class Wood : LegalPerson
    {

    }

    public class Boynton : LegalPerson
    {
        public Boynton(): base("Mr. Samuel B. Boynton") { }
    }
}
