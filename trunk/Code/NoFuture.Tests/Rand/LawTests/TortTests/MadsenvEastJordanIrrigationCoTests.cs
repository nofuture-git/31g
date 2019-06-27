using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Madsen v. East Jordan Irrigation Co., 125 P.2d 794 (Utah 1942)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, when the loose requirements of cause in abnormal danger acts is just too loose
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MadsenvEastJordanIrrigationCoTests
    {
        [Test]
        public void MadsenvEastJordanIrrigationCo()
        {
            var property = new LegalProperty("cannibal minks");

            var test = new AbnormallyDangerousActivity(ExtensionMethods.Tortfeasor)
            {
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                    {
                        IsButForCaused = lp => lp is EastJordanIrrigationCo
                    },
                    ProximateCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsDirectCause = lp => false,
                        IsForeseeable = lp => false,
                    }
                },
                IsExplosives = p => true,
                SubjectProperty = property,
                PropertyDamage = new Damage(ExtensionMethods.Tortfeasor)
                {
                    SubjectProperty = property,
                    ToValue = p => true //they are dead
                }
            };

            var testResult = test.IsValid(new Madsen(), new EastJordanIrrigationCo());
            Console.WriteLine(test.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Madsen : LegalPerson, IPlaintiff
    {
        public Madsen(): base("Madsen") { }
    }

    public class EastJordanIrrigationCo : LegalPerson, ITortfeasor
    {
        public EastJordanIrrigationCo(): base("East Jordan Irrigation Co.") { }
    }
}
