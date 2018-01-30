using NUnit.Framework;

namespace NoFuture.Rand.Tests.ComTests
{
    [TestFixture]
    public class FirmTests
    {
        [Test]
        public void TestGetSearchCompanyName()
        {
            var testResult = Com.Firm.GetSearchCompanyName("HNI Corporation");
            Assert.AreEqual("HNI CORP", testResult);
            testResult = Com.Firm.GetSearchCompanyName("ZION OIL & GAS INC.");
            Assert.AreEqual("ZION OIL & GAS INC", testResult);
            testResult = Com.Firm.GetSearchCompanyName("The Saint Louis Glass Company");
            Assert.AreEqual("ST LOUIS GLASS CO", testResult);
            testResult = Com.Firm.GetSearchCompanyName("Twenty-Nine Palms, California");
            Assert.AreEqual("TWENTY NINE PALMS CALIF", testResult);
            testResult = Com.Firm.GetSearchCompanyName("B/G Foods Company");
            Assert.AreEqual("B G FOODS CO", testResult);
            testResult = Com.Firm.GetSearchCompanyName("A. & C. Company Mortgage");
            Assert.AreEqual("A & C CO MTG", testResult);

            testResult = Com.Firm.GetSearchCompanyName("M&T Bank Corporation");
            Assert.AreEqual("M&T BK CORP", testResult);

            testResult = Com.Firm.GetSearchCompanyName("Parsons and Company Incorporated");
            Assert.AreEqual("PARSONS & CO INC", testResult);
        }

        [Test]
        public void TestGetNameFull()
        {
            var testResult = Com.Firm.GetNameFull("BRAND BKG CO");
            Assert.AreEqual("Brand Banking Company", testResult);

        }
    }
}
