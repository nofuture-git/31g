using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Exo.NfXml;

namespace NoFuture.Rand.Exo.Tests.NfXmlTests
{
    [TestFixture]
    public class SecFullTxtSearchTests
    {

        [Test]
        public void TestMethod1()
        {
            var testXmlFile = TestAssembly.TestDataDir + @"\SecByFullText.xml";
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
