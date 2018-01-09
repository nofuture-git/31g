using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Exo;
using NoFuture.Rand.Exo.Tests;
using NoFuture.Rand.Gov.Sec;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class PublicCorporationTests
    {

        [TestMethod]
        public void TestMergeTickerLookupFromJson()
        {
            var testInput =
                "YAHOO.Finance.SymbolSuggest.ssCallback({\"ResultSet\":{\"Query\":\"jpmorgan chase\",\"Result\":[{\"symbol\":\"JPM\",\"name\":\"JPMorgan Chase & Co.\",\"exch\":\"NYQ\",\"type\":\"S\",\"exchDisp\":\"NYSE\",\"typeDisp\":\"Equity\"},{\"symbol\":\"JPM-WT\",\"name\":\"JPMORGAN CHASE & CO. WARRANTS \",\"exch\":\"NYQ\",\"type\":\"S\",\"exchDisp\":\"NYSE\",\"typeDisp\":\"Equity\"},{\"symbol\":\"JPM.MX\",\"name\":\"JPMorgan Chase & Co.\",\"exch\":\"MEX\",\"type\":\"S\",\"exchDisp\":\"Mexico\",\"typeDisp\":\"Equity\"},{\"symbol\":\"CMC.DE\",\"name\":\"JPMorgan Chase & Co.\",\"exch\":\"GER\",\"type\":\"S\",\"exchDisp\":\"XETRA\",\"typeDisp\":\"Equity\"},{\"symbol\":\"JPM-PD\",\"name\":\"JPMorgan Chase Bank N A London \",\"exch\":\"NYQ\",\"type\":\"S\",\"exchDisp\":\"NYSE\",\"typeDisp\":\"Equity\"},{\"symbol\":\"CMC.DU\",\"name\":\"JPMORGAN CHASE\",\"exch\":\"DUS\",\"type\":\"S\",\"exchDisp\":\"Dusseldorf Stock Exchange \",\"typeDisp\":\"Equity\"},{\"symbol\":\"CMC.HA\",\"name\":\"JPMORGAN CHASE\",\"exch\":\"HAN\",\"type\":\"S\",\"exchDisp\":\"Hanover\",\"typeDisp\":\"Equity\"},{\"symbol\":\"JPM.TI\",\"name\":\"JPMORGAN CHASE\",\"exch\":\"TLO\",\"type\":\"S\",\"exchDisp\":\"TLX Exchange \",\"typeDisp\":\"Equity\"},{\"symbol\":\"JPMPP\",\"name\":\"JPMORGAN CHASE\",\"exch\":\"PNK\",\"type\":\"S\",\"exchDisp\":\"OTC Markets\",\"typeDisp\":\"Equity\"},{\"symbol\":\"CMC.MU\",\"name\":\"JPMORGAN CHASE\",\"exch\":\"MUN\",\"type\":\"S\",\"exchDisp\":\"Munich\",\"typeDisp\":\"Equity\"}]}})";
            var testSubject = new NoFuture.Rand.Com.PublicCorporation();
            testSubject.UpsertName(KindsOfNames.Legal, "JPMorgan Chase & Co.");
            var testResult = Copula.TryMergeTickerLookup(testInput, new Uri("http://www.bloomberg.com/markets/symbolsearch"), ref testSubject);

            Assert.IsTrue(testResult);
            Assert.IsNotNull(testSubject.TickerSymbols);
            Assert.AreNotEqual(0, testSubject.TickerSymbols.Count);
        }

        [TestMethod]
        public void TestTryMergeXbrl()
        {
            var testUri = new Uri("https://www.sec.gov/Archives/edgar/data/1593936/000155837016004206/mik-20160130.xml");
            var testSubject = new NoFuture.Rand.Com.PublicCorporation
            {
                CIK = new CentralIndexKey {Value = "0000768899"}
            };
            testSubject.UpsertName(KindsOfNames.Legal, "TrueBlue, Inc.");
            testSubject.SecReports.Add(new Form10K {XmlLink = testUri});
            var testContent =
                System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl.xml");
            var testResult = Copula.TryMergeXbrlInto10K(testContent,
                testUri,
                ref testSubject);

            Assert.IsTrue(testResult);
            Assert.IsTrue(testSubject.TickerSymbols.Any(x => x.Symbol == "TBI"));

            var tenK2015 =
                testSubject.SecReports.FirstOrDefault(x => x is Form10K && ((Form10K) x).XmlLink == testUri) as
                    Form10K;
            Assert.IsNotNull(tenK2015);

            Assert.AreEqual(42029009, tenK2015.NumOfShares);

            Assert.AreEqual(1266835M, tenK2015.TotalAssets);

            Assert.AreEqual(731262M, tenK2015.TotalLiabilities);

            Assert.AreEqual(71247M, tenK2015.NetIncome);

            Assert.AreEqual(97842M, tenK2015.OperatingIncome);

            Assert.AreEqual(2695680M, tenK2015.Revenue);

        }
    }
}
