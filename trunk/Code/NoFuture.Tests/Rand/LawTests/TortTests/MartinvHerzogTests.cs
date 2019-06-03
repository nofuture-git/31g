using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Martin v. Herzog, 19 N.E.2d 987 (N.Y. 1920)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, failure to follow command of statue means negligence by its standard
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MartinvHerzogTests
    {
        [Test]
        public void MartinvHerzog()
        {
            var test = new NegligenceByStatute(ExtensionMethods.Tortfeasor)
            {
                IsObeyStatute = lp => !(lp is Herzog),
            };
            var testResult = test.IsValid(new Martin(), new Herzog());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Martin : LegalPerson, IPlaintiff
    {
        public Martin(): base("Martin") { }
    }

    public class Herzog : LegalPerson, ITortfeasor
    {
        public Herzog(): base("Herzog") { }
    }
}
