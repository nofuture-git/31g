using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.UnintentionalTort;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Cooley v. Public Service Co., 10 A.2d 673 (N.H. 1940)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, defendent cannot have mutually exclusive duty
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CooleyvPublicServiceCoTests
    {
        [Test]
        public void CooleyvPublicServiceCo()
        {
            var test = new CritiqueLearnedHandsFormula(ExtensionMethods.Tortfeasor)
            {
                IsNegateExistingDuty = lp => lp is PublicServiceCo
            };

            var testResult = test.IsValid(new PublicServiceCo(), new Cooley());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Cooley : LegalPerson, IPlaintiff
    {
        public Cooley(): base("Cooley") { }
    }

    public class PublicServiceCo : LegalPerson, ITortfeasor
    {
        public PublicServiceCo(): base("Public Service Co.") { }
    }
}
