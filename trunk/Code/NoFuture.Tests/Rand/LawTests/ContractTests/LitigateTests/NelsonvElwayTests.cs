﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Litigate;
using NoFuture.Rand.Law.US.Contracts.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.LitigateTests
{
    /// <summary>
    /// NELSON v. ELWAY Supreme Court of Colorado 908 P.2d 102 (Colo. 1995)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    ///  doctrine issue, a term which states that no oral terms are allowed means just that
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class NelsonvElwayTests
    {
        [Test]
        public void NelsonvElway()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSellMetroCarDealership(),
                Acceptance = o => o is OfferSellMetroCarDealership ? new AcceptanceSellMetroCarDealership() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Nelson _:
                                return ((Nelson)lp).GetTerms();
                            case Elway _:
                                return ((Elway)lp).GetTerms();
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

            var testSubject = new ParolEvidenceRule<Promise>(testContract)
            {
                IsCollateralInForm = t => true,
                IsNotContradictWritten = t => true,
                IsNotExpectedWritten = t => true,
            };

            var testResult = testSubject.IsValid(new Nelson(), new Elway());
            Console.WriteLine(testSubject.ToString());
            //is still false since the terms include the Expressly Conditional term
            Assert.IsFalse(testResult);
        }
    }

    public class OfferSellMetroCarDealership : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Nelson || offeror is Elway)
                   && (offeree is Nelson || offeree is Elway);
        }
    }

    public class AcceptanceSellMetroCarDealership : OfferSellMetroCarDealership
    {

    }

    public class Nelson : LegalPerson
    {
        public Nelson(): base("Mel Nelson") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell dealership", 1) {Source = TermSource.Written},
                TermExpresslyConditional.Value,
                new ContractTerm<object>("$50-per-car payment", 2) {Source = TermSource.Oral},
            };
        }
    }

    public class Elway : LegalPerson
    {
        public Elway(): base("John Elway") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell dealership", 1) {Source = TermSource.Written},
                TermExpresslyConditional.Value,
            };
        }
    }
}
