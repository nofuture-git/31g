﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.RemedyTests
{
    /// <summary>
    /// ROCKINGHAM COUNTY v. LUTEN BRIDGE CO. Circuit Court of Appeals, Fourth Circuit 35 F.2d 301 (4th Cir. 1929)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, a plaintiff cannot hold a defendant liable for damages which need not have been incurred
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class RockinghamvLutenBridgeTests
    {
        [Test]
        public void RockinghamvLutenBridge()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferCtorBridge(),
                Acceptance = o => o is OfferCtorBridge ? new AcceptanctCtorBridge() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Rockingham _:
                                return ((Rockingham)lp).GetTerms();
                            case LutenBridge _:
                                return ((LutenBridge)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Rockingham(), new LutenBridge());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                //Luten Bridge kept building a useless bridge after being told not to
                CalcLossToInjured = lp => 100m
            };

            testSubject.Limits.CalcAvoidable = lp => 50m;
            testResult = testSubject.IsValid(new Rockingham(), new LutenBridge());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferCtorBridge : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Rockingham || offeror is LutenBridge)
                   && (offeree is Rockingham || offeree is LutenBridge);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferCtorBridge;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctCtorBridge : OfferCtorBridge
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctCtorBridge;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Rockingham : LegalPerson
    {
        public Rockingham(): base("ROCKINGHAM COUNTY") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("bridge", DBNull.Value),
            };
        }
    }

    public class LutenBridge : LegalPerson
    {
        public LutenBridge(): base("LUTEN BRIDGE CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("bridge", DBNull.Value),
            };
        }
    }
}