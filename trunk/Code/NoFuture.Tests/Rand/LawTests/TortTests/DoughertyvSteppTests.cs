using System;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Dougherty v. Stepp, 18 N.C. 371 (1835)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DoughertyvSteppTests
    {
        [Test]
        public void DoughertyvStepp()
        {
            var testSubject = new TrespassToLand
            {
                Consent = new Consent
                {
                    IsApprovalExpressed = lp => false,
                    IsCapableThereof = lp => lp is Dougherty
                },
                SubjectProperty = new RealProperty
                {
                    Name = "unenclosed land of plaintiff",
                    EntitledTo = new Dougherty(),
                    InPossessionOf = new Dougherty()
                },
                IsTangibleEntry = lp => lp is Stepp
            };

            var testResult = testSubject.IsValid(new Dougherty(), new Stepp());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestIntangibleEntry()
        {
            var testSubject = new TrespassToLand
            {
                Consent = new Consent
                {
                    IsApprovalExpressed = lp => false,
                    IsCapableThereof = lp => lp is Dougherty
                },
                SubjectProperty = new RealProperty
                {
                    Name = "unenclosed land of plaintiff",
                    EntitledTo = new Dougherty(),
                    InPossessionOf = new Dougherty()
                },
                IsIntangibleEntry = lp => lp is Stepp,
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                    {
                        IsButForCaused = lp => lp is Stepp
                    },
                    ProximateCause =  new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsForeseeable = lp => lp is Stepp
                    }
                },
            };

            var testResult = testSubject.IsValid(new Dougherty(), new Stepp());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
            testSubject.ClearReasons();

            testSubject.PropertyDamage = new Damage(ExtensionMethods.Tortfeasor)
            {
                ToUsefulness = p => true
            };

            testResult = testSubject.IsValid(new Dougherty(), new Stepp());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Dougherty : LegalPerson, IPlaintiff
    {

    }
    public class Stepp : LegalPerson, ITortfeasor
    {

    }

}
