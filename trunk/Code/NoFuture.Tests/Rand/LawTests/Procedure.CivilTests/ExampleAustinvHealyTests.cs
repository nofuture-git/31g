using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    /// <summary>
    /// Civil Procedure Examples &amp; Explanations 8th Ed. Joseph W. Glannon pg 11
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ExampleAustinvHealyTests
    {
        [Test]
        public void AustinvHealy01()
        {
            var testSubject = new PersonalJurisdiction("North Dakota")
            {
                Consent = Consent.NotGiven(),
                MinimumContact = new MinimumContact("North Dakota")
                {
                    GetCommerciallyEngagedLocation = lp =>
                        lp is Austin 
                            ? new[] {new VocaBase("North Dakota"), new VocaBase("South Dakota"), new VocaBase("Minnesota")} 
                            : null,
                    GetDirectContactTo = lp => lp is Austin ? new Healy() : null
                },
                GetDomicileLocation = GetState,
                GetPhysicalLocation = GetState
            };

            var testResult = testSubject.IsValid(new AustinAsPlaintiff(), new HealyAsDefendant());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void AustinvHealy03()
        {
            var testSubject = new PersonalJurisdiction("Minnesota")
            {
                Consent = Consent.NotGiven(),
                MinimumContact = new MinimumContact("Minnesota")
                {
                    GetCommerciallyEngagedLocation = lp =>
                        lp is Austin
                            ? new[] { new VocaBase("North Dakota"), new VocaBase("South Dakota"), new VocaBase("Minnesota") }
                            : null,
                    GetDirectContactTo = lp => lp is Austin ? new Healy() : null,
                },
                GetDomicileLocation = GetState,
                GetPhysicalLocation = GetState
            };

            var testResult = testSubject.IsValid(new HealyAsPlaintiff(), new AustinAsDefendant());
            //Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        public IVoca[] GetState(ILegalPerson lp)
        {
            if (lp is Austin)
                return new[] {new  VocaBase("North Dakota")};
            if (lp is Healy)
                return new[] {new VocaBase("Minnesota")};
            return null;
        }
    }

    public class Austin : LegalPerson
    {
        public Austin() : base("Austin") { }
    }

    public class Healy : LegalPerson
    {
        public Healy() : base("Healy") { }
    }

    public class AustinAsPlaintiff : Austin, IPlaintiff { }

    public class HealyAsDefendant : Healy, IDefendant { }

    public class AustinAsDefendant : Austin, IDefendant { }

    public class HealyAsPlaintiff : Healy, IPlaintiff { }
}
