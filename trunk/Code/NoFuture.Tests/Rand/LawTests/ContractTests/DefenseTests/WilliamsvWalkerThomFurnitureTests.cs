﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// WILLIAMS v. WALKER-THOMAS FURNITURE CO. United States Court of Appeals for the District of Columbia Circuit 121 U.S.App.D.C. 315, 350 F.2d 445 (1965)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, unconsionable can be on unreasonably favorable terms alone
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class WilliamsvWalkerThomFurnitureTests
    {
        [Test]
        public void WilliamsvWalkerThom()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferPurchaseOnCredit(),
                Acceptance = o => o is OfferPurchaseOnCredit ? new AcceptPurchaseOnCredit() : null,
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

            var testSubject = new ByUnconscionability<Promise>(testContract)
            {
                IsUnreasonablyFavorableTerms = lp => lp is WalkerThom
            };

            var testResult = testSubject.IsValid(new WalkerThom(), new Williams());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("deceptive legal jargon", "money sweet money"),
            };
        }
    }

    public class OfferPurchaseOnCredit : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Williams || offeror is WalkerThom)
                   && (offeree is Williams || offeree is WalkerThom);
        }

        public string UnconscionableClause => "allow repossess all items, even " +
                                              "those long since paid for in full, " +
                                              "if even one item is every late";
    }

    public class AcceptPurchaseOnCredit : OfferPurchaseOnCredit { }

    public class Williams : LegalPerson
    {
        public Williams():base("WILLIAMS") {}
    }

    public class WalkerThom : LegalPerson
    {
        public WalkerThom(): base("WALKER-THOMAS FURNITURE CO.") {}
    }
}