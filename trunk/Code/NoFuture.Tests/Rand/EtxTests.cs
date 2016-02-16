using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class EtxTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.Root = @"C:\Projects\31g\trunk\Code\NoFuture\bin";
        }
        [TestMethod]
        public void RandomUSZipWithRespectToPopTest()
        {
            var testResult = NoFuture.Rand.Etx.RandomAmericanZipWithRespectToPop();
            Assert.IsNotNull(testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }
        [TestMethod]
        public void AmericanRaceRatioByZipCodeTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = NoFuture.Rand.Etx.RandomAmericanRaceWithRespectToZip(TEST_ZIP);
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
    }
}
