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
            Assert.AreNotEqual(NoFuture.Rand.Domus.Pneuma.Dimension.Null, testSubject.Openness);
            Assert.AreNotEqual(NoFuture.Rand.Domus.Pneuma.Dimension.Null, testSubject.Conscientiousness);
            Assert.AreNotEqual(NoFuture.Rand.Domus.Pneuma.Dimension.Null, testSubject.Extraversion);
            Assert.AreNotEqual(NoFuture.Rand.Domus.Pneuma.Dimension.Null, testSubject.Agreeableness);
            Assert.AreNotEqual(NoFuture.Rand.Domus.Pneuma.Dimension.Null, testSubject.Neuroticism);
        }
    }
}
