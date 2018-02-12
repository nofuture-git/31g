using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Exo.NfHtml;

namespace NoFuture.Rand.Exo.Tests.NfHtmlTests
{
    [TestFixture]
    public class WikepediaInsComTests
    {

        [Test]
        public void TestParseContent()
        {
            var testFile = TestAssembly.TestDataDir + @"\wikipedia_ListInsComs.html";
            Assert.IsTrue(System.IO.File.Exists(testFile));
            var testContent = System.IO.File.ReadAllText(testFile);
            Assert.IsNotNull(testContent);

            var testSubject = new WikipediaInsCom(new Uri("http://localhost"));
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
                System.Console.WriteLine(i);

        }
    }
}
