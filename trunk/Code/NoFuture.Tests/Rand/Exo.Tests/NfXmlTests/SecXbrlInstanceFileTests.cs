using System;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Exo.NfXml;
using NoFuture.Rand.Exo.Tests;

namespace NoFuture.Tests.Rand.NfXmlTests
{
    [TestClass]
    public class SecXbrlInstanceFileTests
    {


        [TestMethod]
        public void TestParseContent()
        {
            var testXmlFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl.xml";
            var testSubject = new SecXbrlInstanceFile(new Uri("http://localhost"));

            var testResult = testSubject.ParseContent(System.IO.File.ReadAllText(testXmlFile));
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            var testResultItem = testResult.First();
            Assert.IsFalse(testResultItem.IsAmended);
            Assert.AreNotEqual(0, testResultItem.EndOfYear);
            Assert.AreEqual("0000768899", testResultItem.Cik);
            Assert.AreEqual(42029009, testResultItem.NumOfShares);
            Assert.AreEqual("TrueBlue, Inc.", testResultItem.Name);
            Assert.AreEqual("TBI", testResultItem.Ticker);

            Assert.AreNotEqual(0, testResultItem.Assets.Count);

            foreach (var asset in testResultItem.Assets)
            {
                var tAsset = asset as Tuple<int, decimal>;
                Assert.IsNotNull(tAsset);
                if(tAsset.Item1 == 2014)
                    Assert.AreEqual(1066671M, tAsset.Item2);
                else if(tAsset.Item1 == 2015)
                    Assert.AreEqual(1266835M, tAsset.Item2);
            }

            Assert.AreNotEqual(0, testResultItem.Liabilities.Count);
            foreach (var lia in testResultItem.Liabilities)
            {
                var tLia = lia as Tuple<int, decimal>;
                Assert.IsNotNull(tLia);
                if(tLia.Item1 == 2014)
                    Assert.AreEqual(597337M, tLia.Item2);
                if(tLia.Item1 == 2015)
                    Assert.AreEqual(731262M, tLia.Item2);
            }

            Assert.AreNotEqual(0, testResultItem.NetIncome.Count);
            foreach (var ni in testResultItem.NetIncome)
            {
                var tNi = ni as Tuple<int, decimal>;
                Assert.IsNotNull(tNi);
                if(tNi.Item1 == 2013)
                    Assert.AreEqual(44924M, tNi.Item2);
                if (tNi.Item1 == 2014)
                    Assert.AreEqual(65675M, tNi.Item2);
                if (tNi.Item1 == 2015)
                    Assert.AreEqual(71247M, tNi.Item2);
            }

            Assert.AreNotEqual(0, testResultItem.OperatingIncome.Count);
            foreach (var oi in testResultItem.OperatingIncome)
            {
                var tOi = oi as Tuple<int, decimal>;
                Assert.IsNotNull(tOi);
                if(tOi.Item1 == 2013)
                    Assert.AreEqual(59583M, tOi.Item2);
                if (tOi.Item1 == 2014)
                    Assert.AreEqual(81728M, tOi.Item2);
                if (tOi.Item1 == 2015)
                    Assert.AreEqual(97842M, tOi.Item2);
            }

            Assert.AreNotEqual(0, testResultItem.Revenue.Count);
            foreach (var r in testResultItem.Revenue)
            {
                var tR = r as Tuple<int, decimal>;
                Assert.IsNotNull(tR);
                if (tR.Item1 == 2013)
                    Assert.AreEqual(1668929M, tR.Item2);
                if (tR.Item1 == 2014)
                    Assert.AreEqual(2174045M, tR.Item2);
                if (tR.Item1 == 2015)
                    Assert.AreEqual(2695680M, tR.Item2);
            }
        }

        [TestMethod]
        public void TestGetXmlAndNsMgr()
        {
            var content =
                System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl.xml");
            var testResult = SecXbrlInstanceFile.GetXmlAndNsMgr(content);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Item1);
            Assert.IsNotNull(testResult.Item2);
            Assert.AreEqual("http://xbrl.sec.gov/dei/2014-01-31",
                testResult.Item2.LookupNamespace(SecXbrlInstanceFile.XmlNs.DEI));
            Assert.AreEqual("http://fasb.org/us-gaap/2015-01-31",
                testResult.Item2.LookupNamespace(SecXbrlInstanceFile.XmlNs.US_GAAP));
            Assert.AreEqual("http://www.xbrl.org/2003/instance",
                testResult.Item2.LookupNamespace(SecXbrlInstanceFile.XmlNs.ROOTNS));

            content =System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl2.xml");
            testResult = SecXbrlInstanceFile.GetXmlAndNsMgr(content);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Item1);
            Assert.IsNotNull(testResult.Item2);
            Assert.AreEqual("http://xbrl.sec.gov/dei/2014-01-31",
                testResult.Item2.LookupNamespace(SecXbrlInstanceFile.XmlNs.DEI));
            Assert.AreEqual("http://fasb.org/us-gaap/2015-01-31",
                testResult.Item2.LookupNamespace(SecXbrlInstanceFile.XmlNs.US_GAAP));
            Assert.AreEqual("http://www.xbrl.org/2003/instance",
                testResult.Item2.LookupNamespace(SecXbrlInstanceFile.XmlNs.ROOTNS));
        }

        [TestMethod]
        public void TestTryParseDollar()
        {
            var xml = new System.Xml.XmlDocument();
            xml.LoadXml(@"
            <xbrl>
                <SalesRevenueServicesNet contextRef=""FD2013Q4YTD"" decimals=""-3"" id=""Fact-15D9A15CC4357F62E26EAAD1EC523789"" unitRef=""usd"">1668929000</SalesRevenueServicesNet>
            </xbrl>");

            var testInput = xml.SelectSingleNode("//SalesRevenueServicesNet");
            Assert.IsNotNull(testInput);
            var testResultOut = 0M;
            var testResult = SecXbrlInstanceFile.TryParseDollar(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual(1668929M, testResultOut);
        }

        [TestMethod]
        public void TestTryGetYear()
        {
            var content =
                System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl.xml");
            var testSubject = SecXbrlInstanceFile.GetXmlAndNsMgr(content);
            int testResultOut;
            var testResult = SecXbrlInstanceFile.TryGetYear("FD2013Q4YTD", testSubject.Item1, testSubject.Item2, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual(2013, testResultOut);

            content =
                System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl2.xml");
            testSubject = SecXbrlInstanceFile.GetXmlAndNsMgr(content);
            testResult = SecXbrlInstanceFile.TryGetYear("FYCurrentYearM12", testSubject.Item1, testSubject.Item2, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual(2015, testResultOut);
        }

        [TestMethod]
        public void TestGetNodeDollarYear()
        {
            var content =
                System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl.xml");
            var testSubject = SecXbrlInstanceFile.GetXmlAndNsMgr(content);
            var testResult = SecXbrlInstanceFile.GetNodeDollarYear(testSubject.Item1, "//us-gaap:SalesRevenueServicesNet", testSubject.Item2);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            foreach(var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);

            content =
                System.IO.File.ReadAllText(TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecXbrl2.xml");
            testSubject = SecXbrlInstanceFile.GetXmlAndNsMgr(content);
            testResult = SecXbrlInstanceFile.GetNodeDollarYear(testSubject.Item1, "//us-gaap:Revenues", testSubject.Item2);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            foreach (var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);

        }
    }
}

