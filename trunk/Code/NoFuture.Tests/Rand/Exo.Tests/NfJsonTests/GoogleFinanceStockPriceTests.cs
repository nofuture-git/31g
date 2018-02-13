using System;
using System.Linq;
using NoFuture.Rand.Exo.NfJson;
using NUnit.Framework;

namespace NoFuture.Rand.Exo.Tests.NfCsvTests
{
    [TestFixture]
    public class GoogleFinanceStockPriceTests
    {
        [Test]
        public void TestGetUri()
        {
            var testResult = GoogleFinanceStockPrice.GetUri("SUCH");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.ToString().Contains("q=SUCH"));
            Console.WriteLine(testResult.ToString());
        }

        [Test]
        public void TestParseContent()
        {
            var testFile = TestAssembly.TestDataDir + @"\GoogleFinanceData.txt";
            var testInput = System.IO.File.ReadAllText(testFile);
            var testSubject = new GoogleFinanceStockPrice(null);
            var testResult = testSubject.ParseContent(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());
        }
    }
}
