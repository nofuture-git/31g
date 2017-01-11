using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.NfHtmlTests
{
    [TestClass]
    public class WikepediaInsComTests
    {

        [TestMethod]
        public void TestParseContent()
        {
            var testFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\wikipedia_ListInsComs.html";
            Assert.IsTrue(System.IO.File.Exists(testFile));
            var testContent = System.IO.File.ReadAllText(testFile);
            Assert.IsNotNull(testContent);

            var testSubject = new NoFuture.Rand.Data.NfHtml.WikipediaInsCom(new Uri("http://localhost"));
            var testResults = testSubject.ParseContent(testContent);
            Assert.IsNotNull(testResults);
            Assert.IsTrue(testResults.Any());

            var testResultItem = testResults.First();
            var testResultProp = testResultItem.UsInsComNames;
            Assert.IsNotNull(testResultProp);

            var testResultArray = testResultProp as string[];
            Assert.IsNotNull(testResultArray);

            Assert.AreNotEqual(0, testResultArray.Length);
            foreach(var i in testResultArray)
                System.Diagnostics.Debug.WriteLine(i);

        }
    }
}
