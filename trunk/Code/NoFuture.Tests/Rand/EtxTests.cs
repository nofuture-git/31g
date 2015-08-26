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
    }
}
