using System;
using System.Linq;
using NoFuture.Rand.Core.Enums;
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

        [Test]
        public void TestRandomUniversity()
        {
            var testResult = AmericanUniversity.RandomUniversity("CA");
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StateName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StateAbbrev));
            Assert.IsNotNull(testResult.PercentOfStateStudents);
            Assert.IsFalse(testResult.PercentOfStateStudents == 0.0f);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = new AmericanUniversity
            {
                Name = "University of California",
                CampusName = "Irvine",
                StateAbbrev = "CA",
                StateName = "California"
            };

            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
