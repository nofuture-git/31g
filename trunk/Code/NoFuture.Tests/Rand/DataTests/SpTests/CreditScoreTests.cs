using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class CreditScoreTests
    {

        [TestMethod]
        public void TestGetScore()
        {
            var testInput = new American(UsState.GetWorkingAdultBirthDate(), Gender.Female);

            var testSubject = new Data.Sp.PersonalCreditScore
            {
                GetAgeAt = testInput.GetAgeAt,
                OpennessZscore = testInput.Personality?.Openness?.Value?.Zscore ?? 0D,
                ConscientiousnessZscore = testInput.Personality?.Conscientiousness?.Value?.Zscore ?? 0D
            };

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
            var testInput = new American(UsState.GetWorkingAdultBirthDate(), Gender.Female);

            var testSubject = new Data.Sp.PersonalCreditScore
            {
                GetAgeAt = testInput.GetAgeAt,
                OpennessZscore = testInput.Personality?.Openness?.Value?.Zscore ?? 0D,
                ConscientiousnessZscore = testInput.Personality?.Conscientiousness?.Value?.Zscore ?? 0D
            };
            Debug.WriteLine(testSubject.GetScore(new DateTime(DateTime.Today.Year, 1, 1)));
            var testResult = testSubject.GetRandomInterestRate(new DateTime(DateTime.Today.Year, 1,1));
            Assert.IsTrue(testResult > 3.0D);
            Debug.WriteLine(testResult);

        }

        [TestMethod]
        public void TestGetRandomMax()
        {
            var testInput = new American(UsState.GetWorkingAdultBirthDate(), Gender.Female);

            var testSubject = new Data.Sp.PersonalCreditScore()
            {
                GetAgeAt = testInput.GetAgeAt,
                OpennessZscore = testInput.Personality?.Openness?.Value?.Zscore ?? 0D,
                ConscientiousnessZscore = testInput.Personality?.Conscientiousness?.Value?.Zscore ?? 0D
            };
            var testResult = testSubject.GetRandomMax(null);
            Debug.WriteLine(testResult);
        }
    }
}
