using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var cD = testResult.First();
            var dictTestResult = testResult.First() as Dictionary<string, long>;

            Assert.IsNotNull(dictTestResult);

            foreach(var k in dictTestResult.Keys)
                System.Diagnostics.Debug.WriteLine($"{k} {dictTestResult[k]}");

        }
    }
}
