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
    /// PARKER v. TWENTIETH CENTURY-FOX FILM CORP. Supreme Court of California 3 Cal. 3d 176, 474 P.2d 689, 89 Cal.Rptr. 737 (1970) (in bank)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the nature of what is uncertian, unavoidable and the like is per-case kinda specific
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ParkervTwentiethFoxTests
    {
        [Test]
        public void ParkervTwentiethFox()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferPayActress(),
                Acceptance = o => o is OfferPayActress ? new AcceptanctPayActress() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Parker _:
                                return ((Parker)lp).GetTerms();
                            case TwentiethFox _:
                                return ((TwentiethFox)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Parker(), new TwentiethFox());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                CalcLossToInjured = lp => lp is Parker ? 750000m : 0
            };

            //this is what the dissenting judge noted, 
            // seems like speculation to apply value judgements about anything other than money
            testSubject.Limits.CalcUncertianty = lp => lp is Parker ? 749999m : 0;

            testResult = testSubject.IsValid(new Parker(), new TwentiethFox());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class OfferPayActress : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Parker || offeror is TwentiethFox)
                   && (offeree is Parker || offeree is TwentiethFox);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferPayActress;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctPayActress : OfferPayActress
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctPayActress;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Parker : LegalPerson
    {
        public Parker(): base("SHIRLEY MAC LAINE") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("Bloomer Girl", DBNull.Value),
            };
        }
    }

    public class TwentiethFox : LegalPerson
    {
        public TwentiethFox(): base("TWENTIETH CENTURY-FOX FILM CORP.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("Bloomer Girl", DBNull.Value),
            };
        }
    }
}
