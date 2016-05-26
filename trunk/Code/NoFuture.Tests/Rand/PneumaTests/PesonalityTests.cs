using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.PneumaTests
{
    [TestClass]
    public class PesonalityTests
    {
        [TestMethod]
        public void TestCtor()
        {
            var testSubject = new NoFuture.Rand.Domus.Pneuma.Personality();
            Assert.IsNotNull(testSubject.Openness.Value);
            Assert.IsNotNull(testSubject.Conscientiousness.Value);
            Assert.IsNotNull(testSubject.Extraversion.Value);
            Assert.IsNotNull(testSubject.Agreeableness.Value);
            Assert.IsNotNull(testSubject.Neuroticism.Value);
        }
    }
}
