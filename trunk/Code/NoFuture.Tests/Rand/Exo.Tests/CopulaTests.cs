using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Exo;

namespace NoFuture.Rand.Exo.Tests
{
    [TestClass]
    public class CopulaTests
    {
        [TestMethod]
        public void TestTryParseFfiecInstitutionProfileAspxHtml()
        {
            var testContent = System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\Rand\ffiecHtml.html");

            Bank firmOut = new Bank();
            var testResult = Copula.TryParseFfiecInstitutionProfileAspxHtml(testContent, new Uri(NoFuture.Rand.Data.Exo.UsGov.Links.Ffiec.SEARCH_URL_BASE),
                ref firmOut);
            System.Diagnostics.Debug.WriteLine(firmOut.RoutingNumber);
            Assert.IsTrue(testResult);

        }
    }
}
