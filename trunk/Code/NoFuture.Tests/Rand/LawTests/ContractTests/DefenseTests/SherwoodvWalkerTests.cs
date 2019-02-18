using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToAssent;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.DefenseTests
{
    /// <summary>
    /// SHERWOOD v. WALKER Supreme Court of Michigan 66 Mich. 568, 33 N.W. 919 (1887)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, there is no defense since there is no contract;
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SherwoodvWalkerTests
    {
        [Test]
        public void SherwoodvWalker()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSellCow(),
                Acceptance = o => o is OfferSellCow ? new AcceptSellCow() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => (lp as Sherwood)?.GetTerms() ?? (lp as Walker)?.GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testSubject = new ByFraud<Promise>(testContract);
            var testResult = testSubject.IsValid(new Sherwood(), new Walker());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferSellCow : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Sherwood || offeror is Walker)
                   && (offeree is Sherwood || offeree is Walker);
        }
    }

    public class AcceptSellCow : OfferSellCow { }

    public class Sherwood : LegalPerson
    {
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("Rose 2d of Aberlone",80)
            };
        }
    }

    public class Walker : LegalPerson
    {
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("Rose 2d of Aberlone",700)
            };
        }
    }
}
