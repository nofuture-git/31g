﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.RemedyTests
{
    /// <summary>
    /// HAWKINS v. McGEE Supreme Court of New Hampshire 84 N.H. 114, 146 A. 641 (1929)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the calc of expected money dmg is about the 
    /// diff in expected and actual value - calc of such a value is 
    /// difficult when it was something like, "the value of a perfect hand..."
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HawkinsvMcGeeTests
    {
        [Test]
        public void HawkinsvMcGee()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSkinGraft(),
                Acceptance = o => o is OfferSkinGraft ? new AcceptanctSkinGraft() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Hawkins _:
                                return ((Hawkins)lp).GetTerms();
                            case McGee _:
                                return ((McGee)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testResult = testContract.IsValid(new Hawkins(), new McGee());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                CalcLossToInjured = lp => lp is Hawkins ? 500m : 0m,
            };
            testResult = testSubject.IsValid(new Hawkins(), new McGee());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSkinGraft : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Hawkins || offeror is McGee)
                   && (offeree is Hawkins || offeree is McGee);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferSkinGraft;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctSkinGraft : OfferSkinGraft
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctSkinGraft;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Hawkins : LegalPerson
    {
        public Hawkins(): base("GEORGE HAWKINS") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("hundred per cent good hand", DBNull.Value),
            };
        }
    }

    public class McGee : LegalPerson
    {
        public McGee(): base("DR. MCGEE") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("hundred per cent good hand", DBNull.Value),
            };
        }
    }
}
