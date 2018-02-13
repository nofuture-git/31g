using System;
using System.Linq;
using NoFuture.Rand.Exo.NfJson;
using NUnit.Framework;

namespace NoFuture.Rand.Exo.Tests.NfJsonTests
{
    [TestFixture]
    public class IexKeyStatsTests
    {
        [Test]
        public void TestParseContent()
        {
            var testFile = TestAssembly.TestDataDir + @"\IexKeyStatsResponse.json";
            var testInput = System.IO.File.ReadAllText(testFile);
            var testSubject = new IexKeyStats(null);
            var testResult = testSubject.ParseContent(testInput);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());
            var singleResult = testResult.First();
            var someValue = singleResult.peRatioHigh;
            Assert.AreEqual(114.1D, someValue.Value);
            Console.WriteLine(someValue.ToString());

        }
    }
}
