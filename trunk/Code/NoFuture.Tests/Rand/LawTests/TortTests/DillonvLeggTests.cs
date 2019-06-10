using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Dillon v. Legg, 441 P.2d 912 (Cal. 1968)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, defines a predicate test for emotional trauma
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DillonvLeggTests
    {
        [Test]
        public void DillonvLegg()
        {
            var test = new EmotionalDistress(ExtensionMethods.Tortfeasor)
            {
                IsByViolence = lp => lp is Legg,
                IsCloselyRelated = (v, lp) => v is DaughterOfDillon && lp is Dillon,
                IsDirectPrimaryWitness = lp => lp is Dillon,
                IsNearSceneOfAccident = lp => lp is Dillon
            };
            var testResult = test.IsValid(new Legg(), new Dillon(), new DaughterOfDillon());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Dillon : LegalPerson, IPlaintiff
    {
        public Dillon(): base("Dillon") { }
    }

    public class DaughterOfDillon : LegalPerson, IVictim
    {
        public DaughterOfDillon() : base("Erin Dillon") { }

    }

    public class Legg : LegalPerson, ITortfeasor
    {
        public Legg(): base("Legg") { }
    }
}
