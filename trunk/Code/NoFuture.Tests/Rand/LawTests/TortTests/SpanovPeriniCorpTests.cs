using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Spano v. Perini Corp., 250 N.E.2d 31 (N.Y. 1969)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, illustrate concept of abnormally dangerous activity
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SpanovPeriniCorpTests
    {
        [Test]
        public void SpanovPeriniCorp()
        {
            var property = new LegalProperty("garage in Brooklyn");
            var test = new AbnormallyDangerousActivity(ExtensionMethods.Tortfeasor)
            {
                IsExplosives = p => p.Equals(property),
                SubjectProperty = property,
                PropertyDamage = new Damage(ExtensionMethods.Tortfeasor)
                {
                    SubjectProperty = property,
                    ToValue = p => true,
                },
            };

            var testResult = test.IsValid(new Spano(), new PeriniCorp());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Spano : LegalPerson, IPlaintiff
    {
        public Spano(): base("Spano") { }
    }

    public class PeriniCorp : LegalPerson, ITortfeasor
    {
        public PeriniCorp(): base("Perini Corp.") { }
    }
}
