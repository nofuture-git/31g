using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Smith v. Sneller, 26 A.2d 452 (Pa. 1942)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, its not a greater duty of physical disability to meet the rational care standard
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SmithvSnellerTests
    {
        [Test]
        public void SmithvSneller()
        {
            var test = new OfPhysicalDeficiency(ExtensionMethods.Plaintiff)
            {
                IsAction = lp => lp is Smith,
                IsVoluntary = lp => lp is Smith,
                IsUsingCompensatoryDevice = lp => !(lp is Smith),
                IsAfflictedWith = lp => lp is Smith
            };

            var testResult = test.IsValid(new Smith(), new Sneller());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Smith : LegalPerson, IPlaintiff
    {
        public Smith(): base("Smith") { }
    }

    public class Sneller : LegalPerson, ITortfeasor
    {
        public Sneller(): base("Sneller") { }
    }
}
