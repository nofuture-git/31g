﻿using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Exo.NfXml;

namespace NoFuture.Rand.Exo.Tests.NfXmlTests
{
    [TestFixture]
    public class SecForm13fInfoTableTests
    {
        [Test]
        public void TestParseContent()
        {
            var contentFile = TestAssembly.TestDataDir + @"\form13fInfoTable.xml";
            var content = System.IO.File.ReadAllText(contentFile);

            var testSubject = new SecForm13FInfoTable(null);
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
