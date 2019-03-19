using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.DefenseTests
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
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSnowTubing(),
                Acceptance = o => o is OfferSnowTubing ? new AcceptSnowTubing() : null,
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
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Hanks || offeror is PowderRidge)
                   && (offeree is Hanks || offeree is PowderRidge);
        }

        public int MinAgeRequirement => 6;
        public Tuple<int, string> MinHeightRequirement = new Tuple<int, string>(44, "inches");

    }

    public class AcceptSnowTubing : OfferSnowTubing { }

    public class Hanks : LegalPerson, IOfferee
    {

    }

    public class PowderRidge : LegalPerson, IOfferor
    {
        public PowderRidge(): base("POWDER RIDGE RESTAURANT CORP.") { }
    }
}
