using NUnit.Framework;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Tests.OrgTests
{
    [TestFixture]
    public class NorthAmericanIndustryTests
    {
        [Test]
        public void TestSuperSectors()
        {
            var testResult = NorthAmericanIndustryClassification.AllSectors;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach (var ss in testResult)
            {
                Assert.IsInstanceOf<NaicsSuperSector>(ss);
                System.Diagnostics.Debug.WriteLine($"{ss.Value} {ss.Description}");
                foreach (var s in ss.Divisions)
                {
                    System.Diagnostics.Debug.WriteLine(s.Description);
                }
            }
        }

        [Test]
        public void TestRandomMarket()
        {
            var testResult = NaicsMarket.RandomMarket();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

        [Test]
        public void TestRandomSuperSector()
        {
            var testResult = NaicsSuperSector.RandomSuperSector();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

        [Test]
        public void TestRandomPrimarySector()
        {
            var testResult = NaicsPrimarySector.RandomPrimarySector();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

        [Test]
        public void TestRandomSector()
        {
            var testResult = NaicsSector.RandomSector();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

    }
}
