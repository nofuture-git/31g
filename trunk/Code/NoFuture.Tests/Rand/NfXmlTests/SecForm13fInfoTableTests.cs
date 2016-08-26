using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.NfXmlTests
{
    [TestClass]
    public class SecForm13fInfoTableTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }

        [TestMethod]
        public void TestParseContent()
        {
            var contentFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Rand\form13fInfoTable.xml";
            var content = System.IO.File.ReadAllText(contentFile);

            var testSubject = new NoFuture.Rand.Data.NfXml.SecForm13FInfoTable(null);
            var testResult = testSubject.ParseContent(content);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

            var firstEntry = testResult.FirstOrDefault();
            Assert.IsNotNull(firstEntry);

            var trCusipId = firstEntry.CusipId as string;
            var trUsd = Convert.ToInt64(firstEntry.MarketValue);
            var trCount = Convert.ToInt64(firstEntry.TotalNumberOf);
            var trAbbrev = firstEntry.SecurityAbbrev as string;

            Assert.IsNotNull(trCusipId);
            Assert.IsNotNull(trAbbrev);
            Assert.AreNotEqual(0, trUsd);
            Assert.AreNotEqual(0, trCount);
        }
    }
}
