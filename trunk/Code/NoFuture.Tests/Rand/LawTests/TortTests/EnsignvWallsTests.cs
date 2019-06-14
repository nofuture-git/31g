using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Ensign v. Walls, 34 N.W.2d 549 (Mich. 1948)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, private nuisance still responsible even when
    /// new, adjoining, property owners are aware of nuisance at-time-of-purchase
    /// (aka priority in time)
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class EnsignvWallsTests
    {
        [Test]
        public void EnsignvWalls()
        {
            var property = new LegalProperty("13949 Dacosta Street")
            {
                InPossessionOf = new Walls(),
                EntitledTo = new Walls()
            };
            var test = new PrivateNuisance(ExtensionMethods.Tortfeasor)
            {
                SubjectProperty = property,
                IsInvasionOfProtectableInterest = lp => lp is Ensign,
                IsNegligentInvasion = lp => lp is Walls
            };
            var testResult = test.IsValid(new Ensign(), new Walls());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Ensign : LegalPerson, IPlaintiff
    {
        public Ensign(): base("Ensign") { }
    }

    public class Walls : LegalPerson, ITortfeasor
    {
        public Walls(): base("Walls") { }
    }
}
