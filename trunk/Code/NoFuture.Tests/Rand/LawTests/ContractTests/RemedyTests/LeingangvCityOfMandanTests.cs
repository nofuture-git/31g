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
    /// LEINGANG v. CITY OF MANDAN WEED BOARD Supreme Court of North Dakota 468 N.W.2d 397 (N.D. 1991)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, calc of damages may require an understanding of accounting, econ, finance, etc.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class LeingangvCityOfMandanTests
    {
        [Test]
        public void LeingangvCityOfMandan()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferTrimLargeLots(),
                Acceptance = o => o is OfferTrimLargeLots ? new AcceptanctTrimLargeLots() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Leingang _:
                                return ((Leingang)lp).GetTerms();
                            case CityOfMandan _:
                                return ((CityOfMandan)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Leingang(), new CityOfMandan());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                CalcLossAvoided = lp => lp is Leingang ? 211.18m : 0m,
                CalcLossToInjured = lp => lp is Leingang ? 1933.78m : 0m,
            };

            testResult = testSubject.IsValid(new Leingang(), new CityOfMandan());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferTrimLargeLots : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Leingang || offeror is CityOfMandan)
                   && (offeree is Leingang || offeree is CityOfMandan);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferTrimLargeLots;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctTrimLargeLots : OfferTrimLargeLots
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctTrimLargeLots;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Leingang : LegalPerson
    {
        public Leingang(): base("ROBERT LEINGANG") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("10,000 square feet", DBNull.Value),
            };
        }
    }

    public class CityOfMandan : LegalPerson
    {
        public CityOfMandan(): base("CITY OF MANDAN WEED BOARD") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("10,000 square feet", DBNull.Value),
            };
        }
    }
}