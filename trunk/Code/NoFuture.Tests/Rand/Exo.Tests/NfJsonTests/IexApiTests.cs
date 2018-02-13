using System.Linq;
using NoFuture.Rand.Exo.NfJson;
using NUnit.Framework;

namespace NoFuture.Rand.Exo.Tests.NfJsonTests
{
    [TestFixture]
    public class IexApiTests
    {
        [Test]
        public void TestParseContent()
        {
            var testFile = TestAssembly.TestDataDir + @"\IexApiDataResponse.json";
            var testInput = System.IO.File.ReadAllText(testFile);
            var testSubject = new IexApi();
            var testResult = testSubject.ParseContent(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());
        }
    }
}
