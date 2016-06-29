using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class EtxTests
    {
        [TestMethod]
        public void RandomUSZipWithRespectToPopTest()
        {
            var testResult = NAmerUtil.RandomAmericanZipWithRespectToPop();
            Assert.IsNotNull(testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }
        [TestMethod]
        public void AmericanRaceRatioByZipCodeTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = NAmerUtil.RandomAmericanRaceWithRespectToZip(TEST_ZIP);
            Assert.IsNotNull(testResult);
            
        }

        [TestMethod]
        public void TestGetMagnitudeAdjustment()
        {
            var testResult = NoFuture.Rand.Etx.GetMagnitudeAdjustment(5.0D);
            Assert.AreEqual(100D, testResult);

            testResult = NoFuture.Rand.Etx.GetMagnitudeAdjustment(0.5);
            Assert.AreEqual(100D, testResult);

            testResult = NoFuture.Rand.Etx.GetMagnitudeAdjustment(500);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestRandomValueInNormalDist()
        {
            var mean = 500;
            var stdDev = 100;

            for (var i = 0; i < 256; i++)
            {
                var testResult = NoFuture.Rand.Etx.RandomValueInNormalDist(mean, stdDev);

                System.Diagnostics.Debug.WriteLine(testResult);
                
            }
        }

        [TestMethod]
        public void TestGetRandomRChars()
        {
            var testResult = NoFuture.Rand.Etx.GetRandomRChars();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.IsTrue(testResult.Length >= 5);
            Assert.IsTrue(testResult.Length <= 15);

            var example = new System.Text.StringBuilder();
            foreach (var r in testResult)
                example.Append(r.Rand);

            System.Diagnostics.Debug.WriteLine(example);

            testResult = NoFuture.Rand.Etx.GetRandomRChars(true);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.IsTrue(testResult.Length >= 5);
            Assert.IsTrue(testResult.Length <= 15);

            example = new System.Text.StringBuilder();
            foreach (var r in testResult)
                example.Append(r.Rand);

            Assert.IsTrue(example.ToString().ToCharArray().All(char.IsNumber));

            System.Diagnostics.Debug.WriteLine(example);
            
        }

        [TestMethod]
        public void TestRandomDouble()
        {
            var testResult = NoFuture.Rand.Etx.RationalNumber(0, 3);
            Assert.IsTrue(testResult >= 0);
            Assert.IsTrue(testResult < 4);
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}
