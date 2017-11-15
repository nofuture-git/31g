using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class CreditScoreTests
    {

        [TestMethod]
        public void TestGetScore()
        {
            var testInput = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);

            var testSubject = new Data.Sp.PersonalCreditScore(testInput);

            Debug.WriteLine($"test subject age {testInput.GetAgeAt(null)}");

            var testAgeRstl = testSubject.GetAgePenalty(null);
            Debug.WriteLine($"age penalty {testAgeRstl}");
            var testDispRslt = testSubject.GetUndisciplinedPenalty();
            Debug.WriteLine($"discipline penalty {testDispRslt}");
            var testInconRslt = testSubject.GetInconsistentPenalty();
            Debug.WriteLine($"inconsistent penalty {testInconRslt}");

            var baseScore = testSubject.FicoBaseValue;
            Debug.WriteLine($"base score {baseScore}");

            var expected = (int) Math.Ceiling(baseScore + testAgeRstl + testDispRslt + testInconRslt);
            
            Assert.IsTrue(expected >= testSubject.GetScore(null));
        }

        [TestMethod]
        public void TestGetRandomInterestRate()
        {
            var testInput = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);

            var testSubject = new Data.Sp.PersonalCreditScore(testInput);
            Debug.WriteLine(testSubject.GetScore(new DateTime(DateTime.Today.Year, 1, 1)));
            var testResult = testSubject.GetRandomInterestRate(new DateTime(DateTime.Today.Year, 1,1));
            Assert.IsTrue(testResult > 3.0D);
            Debug.WriteLine(testResult);

        }

        [TestMethod]
        public void TestGetRandomMax()
        {
            var testInput = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);

            var testSubject = new Data.Sp.PersonalCreditScore(testInput);
            var testResult = testSubject.GetRandomMax(null);
            Debug.WriteLine(testResult);
        }
    }
}
