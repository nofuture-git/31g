using System;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.NfXmlTests
{
    [TestClass]
    public class SecXbrlInstanceFileTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }

        [TestMethod]
        public void TestParseContent()
        {
            var testXmlFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\ExampleSecXbrl.xml";
            var testSubject = new NoFuture.Rand.Data.NfXml.SecXbrlInstanceFile(new Uri("http://localhost"));

            var testResult = testSubject.ParseContent(System.IO.File.ReadAllText(testXmlFile));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

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
        public void TestGetNodeDollarYear()
        {
            var xml = new XmlDocument();
            xml.Load(@"C:\Projects\31g\trunk\temp\bco-20151231.xml");
            var nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("us-gaap", "http://fasb.org/us-gaap/2015-01-31");
            nsMgr.AddNamespace("xbrli", "http://www.xbrl.org/2003/instance");
            var rootElem = xml.DocumentElement;
            Assert.IsNotNull(rootElem);
            foreach (var attr in rootElem.Attributes.Cast<XmlAttribute>())
            {
                 System.Diagnostics.Debug.WriteLine(string.Join(" ", attr.Name, attr.Value));
            }


        }
    }
}
