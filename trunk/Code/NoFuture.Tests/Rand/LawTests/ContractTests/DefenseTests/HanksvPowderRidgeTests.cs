using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{

    /// <summary>
    /// HANKS v. POWDER RIDGE RESTAURANT CORP. Supreme Court of Connecticut 885 A.2d 734 (Conn. 2005)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, exculpatory provisions undermine the policy considerations governing our 
    /// tort system - with the public bearing the cost of the resulting injuries
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HanksvPowderRidgeTests
    {
        [Test]
        public void HanksvPowderRidge()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferSnowTubing(),
                Acceptance = o => o is OfferSnowTubing ? new AcceptSnowTubing() : null,
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

            var testSubject = new ByLimitTortLiability<Promise>(testContract)
            {
                IsOfferToAnyMemberOfPublic = lp => lp is PowderRidge,
                IsStandardizedAdhesion = lp => lp is PowderRidge,
                IsSubjectToSellerCarelessness = lp => lp is PowderRidge,
                IsSuitableForPublicRegulation = lp => lp is PowderRidge,
                IsAdvantageOverMemberOfPublic = lp => lp is PowderRidge
            };

            var testResult = testSubject.IsValid(new PowderRidge(), new Hanks());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("snow tubing", DBNull.Value)
            };
        }
    }

    public class OfferSnowTubing : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Hanks || offeror is PowderRidge)
                   && (offeree is Hanks || offeree is PowderRidge);
        }

        public int MinAgeRequirement => 6;
        public Tuple<int, string> MinHeightRequirement = new Tuple<int, string>(44, "inches");

    }

    public class AcceptSnowTubing : OfferSnowTubing { }

    public class Hanks : LegalPerson
    {

    }

    public class PowderRidge : LegalPerson
    {
        public PowderRidge(): base("POWDER RIDGE RESTAURANT CORP.") { }
    }
}
