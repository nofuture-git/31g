using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Com;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class TickerTests
    {
        [TestMethod]
        public void TestEquals()
        {
            var t0 = new Ticker {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"};
            var t1 = new Ticker {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"};

            Assert.IsTrue(t0.Equals(t1));
        }

        [TestMethod]
        public void TestTickerCompare()
        {
            var testTickers = new Ticker[]
            {
                new Ticker {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"},
                new Ticker {InstrumentType = "BDR", Symbol = "NFLX34:BZ", Country = "Brazil"},
                new Ticker {InstrumentType = "Common Stock", Symbol = "NFC:GR", Country = "Germany"},
                new Ticker {InstrumentType = "Common Stock", Symbol = "NFLX:SW", Country = "Switzerland"},
                new Ticker {InstrumentType = "Common Stock", Symbol = "NFLX*:MM", Country = "Mexico"},
            };

            var testSubject = new TickerComparer("NETFLIX INC");

            var testInput = testTickers.ToList();

            testInput.Sort(testSubject);

            var testResult = testInput.First();

            Assert.IsTrue(testResult.Equals(new Ticker {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"}));


        }
    }
}
