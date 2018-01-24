using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Edu.US;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.EduTests
{
    [TestFixture]
    public class AmericanUniversityTests
    {
        [Test]
        public void TestNatlGradRate()
        {
            var testResults = AmericanUniversity.NatlGradRate();
            Assert.IsNotNull(testResults);
            Assert.IsFalse(testResults.IsEmpty());
        }

        [Test]
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


        [Test]
        public void TestGetAmericanUniversity()
        {
            var testResult = AmericanEducation.GetAmericanUniversity(null);
            Assert.IsNotNull(testResult);
            testResult = AmericanEducation.GetAmericanUniversity("AZ");
            Assert.IsNotNull(testResult);
        }
    }
}
