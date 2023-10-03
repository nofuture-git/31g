using System;
using System.Linq;
using NoFuture.Law;
using NoFuture.Law.Procedure.Civil.US;
using NoFuture.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Law.US;
using NoFuture.Law.US.Courts;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests
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
        /// <summary>
        /// Defendant has nothing to do with North Dakota, that&apos;s the plaintiff&apos;s home state...
        /// </summary>
        [Test]
        public void AustinvHealy01()
        {
            var testSubject = new PersonalJurisdiction(new StateCourt("North Dakota"))
            {
                Consent = Consent.NotGiven(),
                MinimumContact = new MinimumContact
                {
                    GetCommerciallyEngagedLocation = lp =>
                        lp is Austin 
                            ? new[] {new VocaBase("North Dakota"), new VocaBase("South Dakota"), new VocaBase("Minnesota")} 
                            : null,
                    
                },
                GetInjuryLocation = lp => new VocaBase("Minnesota"),
                GetDomicileLocation = GetState,
                GetCurrentLocation = GetState
            };

            var testResult = testSubject.IsValid(new AustinAsPlaintiff(), new HealyAsDefendant());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        /// <summary>
        /// doing business is not enough - needs to be connected to the plaintiff's claim in some way
        /// </summary>
        [Test]
        public void AustinvHealy02()
        {
            var testSubject = new PersonalJurisdiction(new StateCourt("South Dakota"))
            {
                Consent = Consent.NotGiven(),
                MinimumContact = new MinimumContact
                {
                    GetCommerciallyEngagedLocation = lp =>
                        lp is Austin
                            ? new[] { new VocaBase("North Dakota"), new VocaBase("South Dakota"), new VocaBase("Minnesota") }
                            : null
                    
                },
                GetInjuryLocation = lp => new VocaBase("Minnesota"),
                GetDomicileLocation = GetState,
                GetCurrentLocation = GetState
            };

            var testResult = testSubject.IsValid(new HealyAsPlaintiff(), new AustinAsDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }

        /// <summary>
        /// Valid because plaintiff is in Minnesota on biz and that is the location of the injury
        /// </summary>
        [Test]
        public void AustinvHealy03()
        {
            var testSubject = new PersonalJurisdiction(new StateCourt("Minnesota"))
            {
                Consent = Consent.NotGiven(),
                MinimumContact = new MinimumContact
                {
                    GetCommerciallyEngagedLocation = lp =>
                        lp is Austin
                            ? new[] { new VocaBase("North Dakota"), new VocaBase("South Dakota"), new VocaBase("Minnesota") }
                            : null,
                },
                GetInjuryLocation = lp => new VocaBase("Minnesota"),
                GetDomicileLocation = GetState,
                GetCurrentLocation = GetState
            };

            var testResult = testSubject.IsValid(new HealyAsPlaintiff(), new AustinAsDefendant());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        public IVoca GetState(ILegalPerson lp)
        {
            if (lp is Austin)
                return new DomicileLocation("North Dakota", lps => lps.FirstOrDefault(l => l is Austin));
            if (lp is Healy)
                return new DomicileLocation("Minnesota", lps => lps.FirstOrDefault(l => l is Healy));
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
