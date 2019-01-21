﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToAssent;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// ODORIZZI v. BLOOMFIELD SCHOOL DISTRICT Court of Appeal of California, Second Appellate District 246 Cal.App. 2d 123, 54 Cal.Rptr. 533 (2d Dist. 1966)    
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, objective test for undue influence
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class OdorizzivBloomfieldSchoolTests
    {
        [Test]
        public void OrdorizzivBloomfieldSchool()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferEmploymentBloomfieldSchool(),
                Acceptance = o => o is OfferEmploymentBloomfieldSchool ? new AcceptEmploymentBloomfieldSchool() : null,
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

            var testSubject = new ByUndueInfluence<Promise>(testContract)
            {
                IsInsistentOnNoTime4Advisors = lp => lp is BloomfieldSchool,
                IsInsistentOnImmediateCompletion = lp => lp is BloomfieldSchool,
                IsAbsentAdvisorsToServientParty = lp => lp is Odorizzi,
                IsMultiPersuadersOnServientParty = lp => lp is BloomfieldSchool,
                IsExtremeEmphasisOnConsequencesOfDelay = lp => lp is BloomfieldSchool,
                IsUnusualLocation = lp => lp is BloomfieldSchool || lp is Odorizzi
            };

            var testResult = testSubject.IsValid(new BloomfieldSchool(), new Odorizzi());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());

        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("elementary school teacher", DBNull.Value)
            };
        }
    }

    public class OfferEmploymentBloomfieldSchool : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Odorizzi || offeror is BloomfieldSchool)
                && (offeree is Odorizzi || offeree is BloomfieldSchool);
        }
    }

    public class AcceptEmploymentBloomfieldSchool : OfferEmploymentBloomfieldSchool
    {

    }

    public class Odorizzi : LegalPerson
    {
        public Odorizzi(): base("Donald Odorizzi") {}
    }

    public class BloomfieldSchool : LegalPerson
    {
        public BloomfieldSchool(): base("BLOOMFIELD SCHOOL DISTRICT") { }
    }
}
