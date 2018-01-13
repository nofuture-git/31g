using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Edu.US;

namespace NoFuture.Rand.Tests.EduTests
{
    [TestClass]
    public class AmericanUniversityTests
    {
        [TestMethod]
        public void TestNatlGradRate()
        {
            var testResults = AmericanUniversity.NatlGradRate();
            Assert.IsNotNull(testResults);
            Assert.IsFalse(testResults.IsEmpty());
        }

        [TestMethod]
        public void TestGetUniversities()
        {
            var testResults = AmericanUniversity.GetUniversitiesByState("Arizona");

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);
            Assert.IsTrue(testResults.Any(univ => univ.Name == "Arizona State University"));

            //test deals with naming problems
            testResults = AmericanUniversity.GetUniversitiesByState("NewYork");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            testResults = AmericanUniversity.GetUniversitiesByState("NC");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);
        }
    }
}
