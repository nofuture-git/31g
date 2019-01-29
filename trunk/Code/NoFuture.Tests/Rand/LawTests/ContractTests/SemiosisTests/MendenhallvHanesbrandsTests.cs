﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.SemiosisTests
{
    /// <summary>
    /// MENDENHALL v. HANESBRANDS, INC. U.S.District Court for the Middle District of North Carolina 856 F.Supp. 2d 717 (M.D.N.C. 2012)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, default implied terms of good faith and fair dealings 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MendenhallvHanesbrandsTests
    {
        [Test]
        public void MendenhallvHanesBrands()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferPromoteChampion(),
                Acceptance = o => o is OfferPromoteChampion ? new AcceptancePromoteChampion() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Mendenhall _:
                                return ((Mendenhall)lp).GetTerms();
                            case HanesBrands _:
                                return ((HanesBrands)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Mendenhall(), new HanesBrands());
            Console.WriteLine(testContract.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferPromoteChampion : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Mendenhall || offeror is HanesBrands)
                && (offeree is Mendenhall || offeree is HanesBrands);
        }
    }

    public class AcceptancePromoteChampion : OfferPromoteChampion
    {
    }

    public class Mendenhall : LegalPerson
    {
        public Mendenhall() : base("RASHARD MENDENHALL") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("morals clause", DBNull.Value),
                new ContractTerm<object>("covenant of good faith", 2)
            };
        }
    }

    public class HanesBrands : LegalPerson
    {
        public HanesBrands() : base("HANESBRANDS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("morals clause", DBNull.Value),
                new ContractTerm<object>("covenant of good faith", 2)
            };
        }
    }
}