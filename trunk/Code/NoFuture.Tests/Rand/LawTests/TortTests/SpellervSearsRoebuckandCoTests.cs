using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Speller v. Sears, Roebuck and Co., 790 N.E.2d 252 (N.Y. 2003)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, proving defect based on circumstantial evidence is enough for product strict liability
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SpellervSearsRoebuckandCoTests
    {
        [Test]
        public void SpellervSearsRoebuckandCo()
        {
            var test = new ProductStrictLiability(ExtensionMethods.Tortfeasor)
            {
                SubjectProperty = new Refrigerator(),
                Injury = new Harm(ExtensionMethods.Plaintiff)
                {
                    IsDeath = lp => lp is Speller
                },
                //case doesn't specify this 
                IsDirectCause = p => p is Refrigerator,
                //only that circumstances are enough to proof this 
                IsDefectiveAtTimeOfSale = p => p is Refrigerator
            };

            var testResult = test.IsValid(new Speller(), new SearsRoebuckandCo());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Refrigerator : LegalProperty
    {

    }

    public class Speller : LegalPerson, IPlaintiff
    {
        public Speller(): base("Speller") { }
    }

    public class SearsRoebuckandCo : LegalPerson, ITortfeasor
    {
        public SearsRoebuckandCo(): base("Sears, Roebuck and Co.") { }
    }
}
