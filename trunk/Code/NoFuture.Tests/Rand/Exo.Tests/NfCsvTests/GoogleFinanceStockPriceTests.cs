using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NoFuture.Rand.Exo.Tests.NfCsvTests
{
    [TestFixture]
    public class GoogleFinanceStockPriceTests
    {
        [Test]
        public void TestGetUri()
        {
            var testResult = NoFuture.Rand.Exo.NfCsv.GoogleFinanceStockPrice.GetUri("SUCH");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.ToString().Contains("q=SUCH"));
            Console.WriteLine(testResult.ToString());
        }
    }
}
