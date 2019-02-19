using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.DefenseTests
{
    /// <summary>
    /// STEVENS v. PUBLICIS S.A. Supreme Court of New York, Appellate Division, First Department 50 A.D.3d 253, 854 N.Y.S.2d 690 (Sup.Ct.App.Div. 2008)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, with telecom tech, it is very easy to satisfy the two requirements of the statute of frauds
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class StevensvPublicisTests
    {
        [Test]
        public void StevensvPublicis()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new SomeEmail(),
                Acceptance = o => o is SomeEmail ? new SomeEmailResponse() : null,
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


            var testSubject = new StatuteOfFrauds<Promise>(testContract);
            testSubject.Scope.IsYearsInPerformance = c => true;
            testSubject.IsSufficientWriting = c => c.Offer is SomeEmail && c.Acceptance(c.Offer) is SomeEmailResponse;
            testSubject.IsSigned = c => c.Offer is SomeEmail && c.Acceptance(c.Offer) is SomeEmailResponse;
            var testResult = testSubject.IsValid(new Stevens(), new Publicis());

            //the statute of frauds written\signed bits are very easy with electronic emails and such
            Assert.IsFalse(testResult);
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

    public class IgnoreContract : ComLawContract<Promise>
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }

    public class SomeEmail : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Stevens || offeror is Publicis)
                   && (offeree is Stevens || offeree is Publicis);
        }
    }

    public class SomeEmailResponse : SomeEmail { }

    public class Stevens : LegalPerson
    {
        public Stevens() : base("Lobsenz-Stevens (L-S)") { }
    }

    public class Publicis : LegalPerson
    {
        public Publicis() : base("Publicis S.A.") {}
    }
}
