using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// McINERNEY v. CHARTER GOLF, INC. Supreme Court of Illinois 176 Ill. 2d 482, 680 N.E.2d 1347 (1997)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, once found to be within scope of statute then written 
    /// and signed agreement is needed otherwise its a valid defense against enforcement
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class McInerneyvCharterGolfTests
    {
        [Test]
        public void McInerneyvCharterGolf()
        {
            var testContract = new BilateralContract
            {
                Offer = new CharterGolfCounterOfferEmpl(),
                Acceptance = o => o is CharterGolfCounterOfferEmpl ? new McInerneyContEmplAtCharter() : null,
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

            var testSubject = new StatuteOfFrauds<Promise>(testContract);
            //court finds it is in scope to statute of frauds
            testSubject.Scope.IsYearsInPerformance = c => true;

            var testResult = testSubject.IsValid(new CharterGolf(), new McInerney());
            //so it is a valid defense because it lacks sufficient writing (was oral agreement)
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("employment", DBNull.Value)
            };
        }
    }

    public class HickeyFreemanOfferEmpl : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeree is McInerney;
        }
    }

    public class McInerneyContEmplAtCharter : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is CharterGolf && offeree is McInerney;
        }
    }

    public class CharterGolfCounterOfferEmpl : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is CharterGolf && offeree is McInerney;
        }
    }

    public class McInerney : LegalPerson
    {
        public McInerney() : base("Dennis McInerney") { }
    }

    public class CharterGolf : LegalPerson
    {
        public CharterGolf() : base("Charter Golf, Inc.") {}
    }
}
