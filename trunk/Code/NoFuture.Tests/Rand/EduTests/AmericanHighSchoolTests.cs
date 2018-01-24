using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Edu.US;

namespace NoFuture.Rand.Tests.EduTests
{
    [TestFixture]
    public class AmericanHighSchoolTests
    {
        [Test]
        public void TestNatlGradRate()
        {
            var testResults = AmericanHighSchool.NatlGradRate();
            Assert.IsNotNull(testResults);
            Assert.IsFalse(testResults.IsEmpty());
        }


        [Test]
        public void TestGetHighSchoolsByState()
        {
            var testResults = AmericanHighSchool.GetHighSchoolsByState("Arizona");
            System.Diagnostics.Debug.WriteLine("----");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            testResults = AmericanHighSchool.GetHighSchoolsByState("NorthCarolina");
            System.Diagnostics.Debug.WriteLine("----");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            testResults = AmericanHighSchool.GetHighSchoolsByState("MO");
            System.Diagnostics.Debug.WriteLine("----");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);


            testResults = AmericanHighSchool.GetHighSchoolsByState();
            System.Diagnostics.Debug.WriteLine("----");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

        }


        [Test]
        public void TestGetHighSchoolsByZipCode()
        {
            var hs = AmericanHighSchool.GetHighSchoolsByZipCode("62644");

            Assert.IsNotNull(hs);
            Assert.AreNotEqual(0, hs.Length);

            Assert.IsFalse(hs.All(h => h.Equals(AmericanHighSchool.GetDefaultHs())));

            hs = AmericanHighSchool.GetHighSchoolsByZipCode(null);

            Assert.IsNotNull(hs);
            Assert.AreNotEqual(0, hs.Length);

            Assert.IsFalse(hs.All(h => h.Equals(AmericanHighSchool.GetDefaultHs())));
        }

        [Test]
        public void TestRandomHighSchool()
        {
            var testResult = AmericanHighSchool.RandomHighSchool();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StateName));
        }
    }
}
