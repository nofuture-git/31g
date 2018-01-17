using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Domus.Pneuma;

namespace NoFuture.Rand.Tests.DomusTests.PneumaTests
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

        [TestMethod]
        public void TestGetRandomActsIrresponsible()
        {
            var testSubject = Personality.RandomPersonality();

            testSubject.Conscientiousness.Value = new Dimension(0.99,0.10);

            //given normal dist it should always be false
            for (var i = 0; i < 100; i++)
            {
                Assert.IsFalse(testSubject.GetRandomActsIrresponsible());
            }

            testSubject.Conscientiousness.Value = new Dimension(-0.99, 0.10);
            //now the same only always true
            for (var i = 0; i < 100; i++)
            {
                Assert.IsTrue(testSubject.GetRandomActsIrresponsible());
            }
        }

        [TestMethod]
        public void TestEquals()
        {
            var testSubject00 = Personality.RandomPersonality();
            var testSubject01 = Personality.RandomPersonality();

            testSubject01.Agreeableness.Value = testSubject00.Agreeableness.Value;
            testSubject01.Conscientiousness.Value = testSubject00.Conscientiousness.Value;
            testSubject01.Extraversion.Value = testSubject00.Extraversion.Value;
            testSubject01.Neuroticism.Value = testSubject00.Neuroticism.Value;
            testSubject01.Openness.Value = testSubject00.Openness.Value;

            Assert.IsTrue(testSubject00.Equals(testSubject01));
        }
    }
}
