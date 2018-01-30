using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Exo.NfXml;
using NoFuture.Rand.Exo.Tests;

namespace NoFuture.Tests.Rand.NfXmlTests
{
    [TestClass]
    public class SecFullTxtSearchTests
    {

        [TestMethod]
        public void TestMethod1()
        {
            var testXmlFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\srch-edgar.xml";
            var testSubject = new SecFullTxtSearch(new Uri("http://localhost"));

            var testResult = testSubject.ParseContent(System.IO.File.ReadAllText(testXmlFile));
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());
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
