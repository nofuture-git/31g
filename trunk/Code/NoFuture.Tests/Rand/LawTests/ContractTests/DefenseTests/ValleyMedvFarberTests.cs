using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.DefenseTests
{
    /// <summary>
    /// VALLEY MEDICAL SPECIALISTS v. FARBER Supreme Court of Arizona 194 Ariz. 363, 982 P.2d 1277 (en banc 1999)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, objective crit. concerning restrictive competition
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class ValleyMedvFarberTests
    {
        [Test]
        public void ValleyMedvFarber()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferVmsEmployment(),
                Acceptance = o => o is OfferVmsEmployment ? new AcceptVmsEmployment() : null,
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

            var testSubject = new ByRestrictCompetition<Promise>(testContract)
            {
                IsInjuriousToPublic = lp => lp is ValleyMed,
                IsRestraintSelfServing = lp => lp is ValleyMed
            };

            var testResult = testSubject.IsValid(new ValleyMed(), new Farber());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("employment", DBNull.Value)
            };
        }
    }

    public class OfferVmsEmployment : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is ValleyMed || offeror is Farber)
                   && (offeree is ValleyMed || offeree is Farber);
        }
    }

    public class AcceptVmsEmployment : OfferVmsEmployment { }

    public class ValleyMed : LegalPerson, IOfferor
    {
        public ValleyMed(): base("VALLEY MEDICAL SPECIALISTS") { }
    }

    public class Farber : LegalPerson, IOfferee
    {
        public Farber() : base("Steven S. Farber, D.O.") { }
    }
}
