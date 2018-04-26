using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US.Sec;

namespace NoFuture.Rand.Exo.Tests
{
    [TestFixture]
    public class PublicCompanyTests
    {

        [Test]
        public void TestTryMergeXbrl()
        {
            var testUri = new Uri("https://www.sec.gov/Archives/edgar/data/1593936/000155837016004206/mik-20160130.xml");
            var testSubject = new NoFuture.Rand.Com.PublicCompany
            {
                CIK = new CentralIndexKey {Value = "0000768899"}
            };
            testSubject.UpsertName(KindsOfNames.Legal, "TrueBlue, Inc.");
            testSubject.AddSecReport(new Form10K {XmlLink = testUri});
            var testContent =
                System.IO.File.ReadAllText(TestAssembly.TestDataDir + @"\ExampleSecXbrl.xml");
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

            Assert.IsNotNull(tenK2015.GetTextBlocks());
            Assert.AreNotEqual(0, tenK2015.GetTextBlocks().Count);

            var lastTb = tenK2015.GetTextBlocks().Last();
            var firstTb = tenK2015.GetTextBlocks().First();

            Assert.IsTrue(lastTb.Item2.Length > firstTb.Item2.Length);

            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.GetForm10KDescriptionOfBiz()));
        }
    }
}
