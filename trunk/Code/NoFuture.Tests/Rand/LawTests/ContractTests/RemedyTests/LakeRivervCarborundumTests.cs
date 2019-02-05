﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Remedy;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.RemedyTests
{
    /// <summary>
    /// LAKE RIVER CORPORATION v. CARBORUNDUM COMPANY United States Court of Appeals, Seventh Circuit 769 F.2d 1284 (7th Cir. 1985).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, liquidated damages which give the full price of the contract are not enforceable since the are just pure profit
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class LakeRivervCarborundumTests
    {
        [Test]
        public void LakeRivervCarborundum()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferProvideDistribution(),
                Acceptance = o => o is OfferProvideDistribution ? new AcceptanctProvideDistribution() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case LakeRiver _:
                                return ((LakeRiver)lp).GetTerms();
                            case Carborundum _:
                                return ((Carborundum)lp).GetTerms();
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

            var testResult = testContract.IsValid(new LakeRiver(), new Carborundum());
            Assert.IsTrue(testResult);

            var testSubject = new LiquidatedDmg<Promise>(testContract)
            {
                IsDisproportionateToActual = lp => lp is LakeRiver
            };

            testResult = testSubject.IsValid(new LakeRiver(), new Carborundum());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }

    public class OfferProvideDistribution : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is LakeRiver || offeror is Carborundum)
                   && (offeree is LakeRiver || offeree is Carborundum);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferProvideDistribution;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctProvideDistribution : OfferProvideDistribution
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctProvideDistribution;
            if (o == null)
                return false;
            return true;
        }
    }

    public class LakeRiver : LegalPerson
    {
        public LakeRiver(): base("LAKE RIVER CORPORATION") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("Ferro Carbo", DBNull.Value),
            };
        }
    }

    public class Carborundum : LegalPerson
    {
        public Carborundum(): base("CARBORUNDUM COMPANY") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("Ferro Carbo", DBNull.Value),
            };
        }
    }
}