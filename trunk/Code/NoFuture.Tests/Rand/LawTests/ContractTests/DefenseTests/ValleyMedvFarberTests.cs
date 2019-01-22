﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// VALLEY MEDICAL SPECIALISTS v. FARBER Supreme Court of Arizona 194 Ariz. 363, 982 P.2d 1277 (en banc 1999)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, objective crit. concerning restrictive competition
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class ValleyMedvFarberTests
    {
        [Test]
        public void ValleyMedvFarber()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferVmsEmployment(),
                Acceptance = o => o is OfferVmsEmployment ? new AcceptVmsEmployment() : null,
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

            var testSubject = new ByRestrictCompetition<Promise>(testContract)
            {
                IsInjuriousToPublic = lp => lp is ValleyMed,
                IsRestraintSelfServing = lp => lp is ValleyMed
            };

            var testResult = testSubject.IsValid(new ValleyMed(), new Farber());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("employment", DBNull.Value)
            };
        }
    }

    public class OfferVmsEmployment : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is ValleyMed || offeror is Farber)
                   && (offeree is ValleyMed || offeree is Farber);
        }
    }

    public class AcceptVmsEmployment : OfferVmsEmployment { }

    public class ValleyMed : LegalPerson
    {
        public ValleyMed(): base("VALLEY MEDICAL SPECIALISTS") { }
    }

    public class Farber : LegalPerson
    {
        public Farber() : base("Steven S. Farber, D.O.") { }
    }
}