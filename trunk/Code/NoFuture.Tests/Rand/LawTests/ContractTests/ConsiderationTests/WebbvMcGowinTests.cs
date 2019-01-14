﻿using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// WEBB v. McGOWIN Court of Appeals of Alabama 27 Ala.App. 82, 168 So. 196 (1935), aff’d 232 Ala. 374, 168 So. 199 (1936)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, direct contradiction of Mills v Wyman.  Only difference is in the terms of what
    /// was promised where here there is some explicit rate and duration.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WebbvMcGowinTests
    {
        [Test]
        public void WebbvMcGowin()
        {
            var testSubject = new ComLawContract<Promise>()
            {
                Offer = new OfferSavedMcGowinLife(),
                Acceptance = o => o is OfferSavedMcGowinLife ? new AcceptancePayForWebbsInjuries(): null,
            };
            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testResult = testSubject.IsValid(new Webb(), new McGowin());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferSavedMcGowinLife : Performance
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => true;
    }

    /// <summary>
    /// This is crux of the contradiction with 
    /// Mills v Wyman - court sees this as a enforceable promise
    /// </summary>
    public class AcceptancePayForWebbsInjuries : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }
    }

    public class Webb : VocaBase, ILegalPerson
    {
        public Webb() : base("Joe Webb") { }
    }

    public class McGowin : VocaBase, ILegalPerson
    {
        public McGowin() : base("J. Greeley McGowin") { }
    }
}