using System;
using NUnit.Framework;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Exo.Tests
{
    [TestFixture]
    public class CopulaTests
    {
        [Test]
        public void TestTryParseFfiecInstitutionProfileAspxHtml()
        {
            var testContent = System.IO.File.ReadAllText(TestAssembly.TestDataDir + @"\ffiecHtml.html");

            Bank firmOut = new Bank();
            var testResult = Copula.TryParseFfiecInstitutionProfileAspxHtml(testContent, new Uri(UsGov.Links.Ffiec.SEARCH_URL_BASE),
                ref firmOut);
            System.Console.WriteLine(firmOut.RoutingNumber);
            Assert.IsTrue(testResult);

        }
    }
}
