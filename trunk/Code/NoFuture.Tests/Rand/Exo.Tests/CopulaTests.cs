using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Com;
using NoFuture.Rand.Exo.NfJson;

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

        [Test]
        public void TestTryParseIexCompanyJson()
        {
            var testContent = System.IO.File.ReadAllText(TestAssembly.TestDataDir + @"\IexCompanyResponse.json");

            var corp = new PublicCompany();
            var testResult = Copula.TryParseIexCompanyJson(testContent, IexCompany.GetUri("AAPL"), ref corp);
            Assert.IsTrue( testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(corp.Description));
            var ticker = corp.TickerSymbols.FirstOrDefault();
            Assert.IsNotNull(ticker);
            Assert.AreEqual("AAPL", ticker.Symbol);
            Assert.IsTrue(ticker.Exchange.Contains("Nasdaq"));
            var uri = corp.NetUris.FirstOrDefault();
            Assert.IsNotNull(uri);
            Assert.AreEqual("http://www.apple.com/", uri.ToString());
            Assert.IsFalse(string.IsNullOrWhiteSpace(corp.Description));
            Assert.IsFalse(string.IsNullOrWhiteSpace(corp.Name));
        }

        [Test]
        public void TestTryParseIexKeyStatsJson()
        {
            var testContent = System.IO.File.ReadAllText(TestAssembly.TestDataDir + @"\IexKeyStatsResponse.json");

            var corp = new PublicCompany();
            var testResult = Copula.TryParseIexKeyStatsJson(testContent, IexKeyStats.GetUri("AAPL"), ref corp);
            Assert.IsTrue(testResult);

            Assert.IsTrue(corp.NetUris.Any());

            foreach(var uri in corp.NetUris)
                Console.WriteLine(uri.ToString());
        }
    }
}
