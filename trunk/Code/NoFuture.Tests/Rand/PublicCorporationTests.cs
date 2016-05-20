using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var testSubject = new NoFuture.Rand.Com.PublicCorporation() {Name = "JPMorgan Chase & Co."};
            var testResult = NoFuture.Rand.Com.PublicCorporation.TryMergeTickerLookup(testInput, new Uri("http://www.bloomberg.com/markets/symbolsearch"), ref testSubject);

            Assert.IsTrue(testResult);
            Assert.IsNotNull(testSubject.TickerSymbols);
            Assert.AreNotEqual(0, testSubject.TickerSymbols.Count);
        }
    }
}
