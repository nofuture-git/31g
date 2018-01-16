using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Edu.US;

namespace NoFuture.Rand.Tests.EduTests
{
    [TestClass]
    public class AmericanHighSchoolTests
    {
        [TestMethod]
        public void TestNatlGradRate()
        {
            var testResults = AmericanHighSchool.NatlGradRate();
            Assert.IsNotNull(testResults);
            Assert.IsFalse(testResults.IsEmpty());
        }


        [TestMethod]
        public void TestGetHighSchoolsByState()
        {
            var testResults = AmericanHighSchool.GetHighSchoolsByState("Arizona");

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            testResults = AmericanHighSchool.GetHighSchoolsByState("NorthCarolina");

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            testResults = AmericanHighSchool.GetHighSchoolsByState("MO");

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);


            testResults = AmericanHighSchool.GetHighSchoolsByState();
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

        }


        [TestMethod]
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

        [TestMethod]
        public void TestRandomHighSchool()
        {
            var testResult = AmericanHighSchool.RandomHighSchool();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StateName));
        }
    }
}
