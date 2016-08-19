﻿using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.NfXmlTests
{
    [TestClass]
    public class SecFullTxtSearchTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }
        [TestMethod]
        public void TestMethod1()
        {
            var testXmlFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\srch-edgar.xml";
            var testSubject = new NoFuture.Rand.Data.NfXml.SecFullTxtSearch(new Uri("http://localhost"));

            var testResult = testSubject.ParseContent(System.IO.File.ReadAllText(testXmlFile));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultItem = testResult.First();
            var title = testResultItem.Title;
            var id = testResultItem.Id;
            var link = testResultItem.Link;
            var update = testResultItem.Update;
            var summary = testResultItem.Summary;
            Debug.WriteLine($"{title}");
            Debug.WriteLine($"{id}");
            Debug.WriteLine($"{link}");
            Debug.WriteLine($"{link}");
            Debug.WriteLine($"{update}");
            Debug.WriteLine($"{summary}");
        }
    }
}