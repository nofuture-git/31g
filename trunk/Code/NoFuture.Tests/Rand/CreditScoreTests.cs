using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class CreditScoreTests
    {
        [TestMethod]
        public void TestGetScore()
        {
            var testInput = NoFuture.Rand.Domus.Person.American();

            var testSubject = new NoFuture.Rand.Data.Sp.PersonalCreditScore(testInput);

            System.Diagnostics.Debug.WriteLine($"test subject age {testInput.GetAgeAt(null)}");

            var testAgeRstl = testSubject.GetAgePenalty(null);
            System.Diagnostics.Debug.WriteLine($"age penalty {testAgeRstl}");
            var testDispRslt = testSubject.GetUndisciplinedPenalty();
            System.Diagnostics.Debug.WriteLine($"discipline penalty {testDispRslt}");
            var testInconRslt = testSubject.GetInconsistentPenalty();
            System.Diagnostics.Debug.WriteLine($"inconsistent penalty {testInconRslt}");

            var baseScore = testSubject.FicoBaseValue;
            System.Diagnostics.Debug.WriteLine($"base score {baseScore}");

            var expected = (int) Math.Ceiling(baseScore + testAgeRstl + testDispRslt + testInconRslt);
            
            Assert.IsTrue(expected >= testSubject.GetScore(null));
        }
    }
}
