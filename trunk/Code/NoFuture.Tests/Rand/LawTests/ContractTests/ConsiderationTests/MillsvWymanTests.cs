﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// MILLS v. WYMAN Supreme Court of Massachusetts 20 Mass. (3 Pick.) 207 (1825)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, still can't enforce a donative promise even when morally ought to.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MillsvWymanTests
    {
        [Test]
        public void MillsvWyman()
        {
            var testSubject = new BilateralContract
            {
                Offer =  new OfferMedicalTreatment(),
                Acceptance = o => o is OfferMedicalTreatment ? new AcceptancePayForItAfterWords() : null,
                MutualAssent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>> { new Term<object>("undefined", DBNull.Value) }
                }
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByPromisor = (lp, p) => true,
                IsGivenByPromisee = (lp, p) => true
            };
            var testResult = testSubject.IsValid(new Mills(), new Wyman());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferMedicalTreatment : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }
    }

    public class AcceptancePayForItAfterWords : DonativePromise
    {
    }

    public class Mills : VocaBase, ILegalPerson
    {
        public Mills() : base("MILLS") { }
    }

    public class Wyman : VocaBase, ILegalPerson
    {
        public Wyman() :base("Levi Wyman") { }
    }
}