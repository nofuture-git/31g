using System.Linq;
using NoFuture.Rand.Com;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.ComTests
{
    [TestFixture]
    public class TickerTests
    {
        [Test]
        public void TestEquals()
        {
            var t0 = new TickerSymbol {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"};
            var t1 = new TickerSymbol {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"};

            Assert.IsTrue(t0.Equals(t1));
        }

        [Test]
        public void TestTickerCompare()
        {
            var testTickers = new[]
            {
                new TickerSymbol {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"},
                new TickerSymbol {InstrumentType = "BDR", Symbol = "NFLX34:BZ", Country = "Brazil"},
                new TickerSymbol {InstrumentType = "Common Stock", Symbol = "NFC:GR", Country = "Germany"},
                new TickerSymbol {InstrumentType = "Common Stock", Symbol = "NFLX:SW", Country = "Switzerland"},
                new TickerSymbol {InstrumentType = "Common Stock", Symbol = "NFLX*:MM", Country = "Mexico"},
            };

            var testSubject = new TickerComparer("NETFLIX INC");

            var testInput = testTickers.ToList();

            testInput.Sort(testSubject);

            var testResult = testInput.First();

            Assert.IsTrue(testResult.Equals(new TickerSymbol {InstrumentType = "Common Stock", Symbol = "NFLX:US", Country = "USA"}));


        }
    }
}
