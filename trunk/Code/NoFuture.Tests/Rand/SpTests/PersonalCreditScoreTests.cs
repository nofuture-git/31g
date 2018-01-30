using System;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class PersonalCreditScoreTests
    {
        [Test]
        public void TestGetAgePenalty()
        {
            var testInput = new PersonalCreditScore(DateTime.Today.AddYears(-36));
            var testResult = testInput.GetAgePenalty(null);

            Assert.IsTrue(testResult >= -2.3D && testResult <= 2.27D);
        }
    }
}
