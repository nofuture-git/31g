using System.Linq;
using NoFuture.Rand.Exo.NfJson;
using NUnit.Framework;

namespace NoFuture.Rand.Exo.Tests.NfJsonTests
{
    [TestFixture]
    public class IexCompanyTests
    {

        [Test]
        public void TestParseContent()
        {
            var testFile = TestAssembly.TestDataDir + @"\IexCompanyResponse.json";
            var testInput = System.IO.File.ReadAllText(testFile);
            var testSubject = new IexCompany(null);
            var testResult = testSubject.ParseContent(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());
            var singleResult = testResult.First();
            var someValue = singleResult.companyName;
            Assert.AreEqual("Apple Inc.", someValue.Value);
            someValue = singleResult.Src;
            Assert.AreEqual(testSubject.Src, someValue.Value);

        }
    }
}
