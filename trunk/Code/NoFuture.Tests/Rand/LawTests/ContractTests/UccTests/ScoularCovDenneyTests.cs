﻿using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.UccTests
{
    /// <summary>
    /// SCOULAR CO. v. DENNEY Court of Appeals of Colorado 151 P.3d 615 (Colo.Ct.App. 2006)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, UCC 2-205 removes one of the four ways to terminate 
    /// an offer; namely, "revocation by the offeror" under certian conditions:
    /// 1. in writing
    /// 2. assurance it won't be revoked
    /// even when there is no consideration
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ScoularCovDenneyTests
    {
        [Test]
        public void ScoularCovDenney()
        {
            var testSubject = new UccContract<Goods>
            {
                Agreement = new FowardContractForMillet(),
                Offer = new BushelsOfMillet(),
                Acceptance = o => new AcceptanceArranged2SellMillet()
            };
            var testResult = testSubject.IsValid(new Denney(), new ScoularCo());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class BushelsOfMillet : Goods
    {
        public int Count => 15000;
        public Tuple<decimal,int> PriceKilogram => new Tuple<decimal, int>(5m,50);

    }

    public class AcceptanceArranged2SellMillet : Goods
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            AddReasonEntry("court found that this was ambiguous form of acceptance");
            return false;
        }
    }

    public class FowardContractForMillet : Agreement
    {
        public override Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp =>
        {
            if (lp is ScoularCo || lp is Denney)
                return true;
            return false;
        };
    }

    public class ScoularCo : VocaBase, ILegalPerson
    {
        public bool IsGrainCompany => true;
    }

    public class Denney : VocaBase, ILegalPerson
    {
        public bool IsGrowerOfMillet => true;
    }
}