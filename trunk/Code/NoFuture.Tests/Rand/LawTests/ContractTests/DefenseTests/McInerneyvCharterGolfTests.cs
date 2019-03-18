using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.DefenseTests
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
            var testContract = new ComLawContract<Promise>
            {
                Offer = new CharterGolfCounterOfferEmpl(),
                Acceptance = o => o is CharterGolfCounterOfferEmpl ? new McInerneyContEmplAtCharter() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
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
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeree is McInerney;
        }
    }

    public class McInerneyContEmplAtCharter : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is CharterGolf && offeree is McInerney;
        }
    }

    public class CharterGolfCounterOfferEmpl : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is CharterGolf && offeree is McInerney;
        }
    }

    public class McInerney : LegalPerson, IOfferee
    {
        public McInerney() : base("Dennis McInerney") { }
    }

    public class CharterGolf : LegalPerson, IOfferor
    {
        public CharterGolf() : base("Charter Golf, Inc.") {}
    }
}
