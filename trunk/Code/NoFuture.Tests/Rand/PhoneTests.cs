using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class PhoneTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.Root = @"C:\Projects\31g\trunk\Code\NoFuture\bin";
        }
        [TestMethod]
        public void AmericanPhoneTests()
        {
            var testResult = NoFuture.Rand.Phone.American();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.AreaCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.CentralOfficeCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.SubscriberNumber));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Formatted));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));

            Assert.IsFalse(testResult.ToString().Contains("-"));

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);
            System.Diagnostics.Debug.WriteLine(testResult.CentralOfficeCode);
            System.Diagnostics.Debug.WriteLine(testResult.SubscriberNumber);
        }

        [TestMethod]
        public void AmericanPhoneByStateTests()
        {
            const string TEST_STATE_WITH_MANY = "CA";
            const string TEST_STATE_WITH_ONE = "DC";

            var testResult = NoFuture.Rand.Phone.American(TEST_STATE_WITH_MANY);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

            testResult = NoFuture.Rand.Phone.American(TEST_STATE_WITH_ONE);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

        }

        [TestMethod]
        public void CanadianPhoneByProvidenceTests()
        {
            const string TEST_STATE_WITH_MANY = "ON";
            const string TEST_STATE_WITH_ONE = "YT";

            var testResult = NoFuture.Rand.Phone.Canadian(TEST_STATE_WITH_MANY);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

            testResult = NoFuture.Rand.Phone.Canadian(TEST_STATE_WITH_ONE);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

        }

        [TestMethod]
        public void TestTryParse()
        {
            var testInput = "5184154299";
            NorthAmericanPhone testResultOut;
            var testResult = NorthAmericanPhone.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreEqual("518",testResultOut.AreaCode);
            Assert.AreEqual("415",testResultOut.CentralOfficeCode);
            Assert.AreEqual("4299",testResultOut.SubscriberNumber);

            testInput = "4154299";
            testResult = NorthAmericanPhone.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreEqual("415", testResultOut.CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.SubscriberNumber);

            testInput = "15184154299";
            testResult = NorthAmericanPhone.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreEqual("518", testResultOut.AreaCode);
            Assert.AreEqual("415", testResultOut.CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.SubscriberNumber);

            testInput = "1 518 415 4299";
            testResult = NorthAmericanPhone.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreEqual("518", testResultOut.AreaCode);
            Assert.AreEqual("415", testResultOut.CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.SubscriberNumber);

            testInput = "1-518-415-4299";
            testResult = NorthAmericanPhone.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreEqual("518", testResultOut.AreaCode);
            Assert.AreEqual("415", testResultOut.CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.SubscriberNumber);

            testInput = "(518)415-4299";
            testResult = NorthAmericanPhone.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreEqual("518", testResultOut.AreaCode);
            Assert.AreEqual("415", testResultOut.CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.SubscriberNumber);
        }
    }
}
