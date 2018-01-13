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
        public void TestGetHighSchools()
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

        }
    }
}
