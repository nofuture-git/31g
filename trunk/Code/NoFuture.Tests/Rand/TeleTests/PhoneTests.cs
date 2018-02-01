using System;
using NoFuture.Rand.Core.Enums;
using NUnit.Framework;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Tests.TeleTests
{
    [TestFixture]
    public class PhoneTests
    {
        [Test]
        public void AmericanPhoneTests()
        {
            var testResult = NorthAmericanPhone.RandomAmericanPhone();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.AreaCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.CentralOfficeCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.SubscriberNumber));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Formatted));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));

            Assert.IsTrue(testResult.ToString().Contains("-"));

            Console.WriteLine(testResult.AreaCode);
            Console.WriteLine(testResult.CentralOfficeCode);
            Console.WriteLine(testResult.SubscriberNumber);
        }

        [Test]
        public void AmericanPhoneByStateTests()
        {
            const string TEST_STATE_WITH_MANY = "CA";
            const string TEST_STATE_WITH_ONE = "DC";

            var testResult = NorthAmericanPhone.RandomAmericanPhone(TEST_STATE_WITH_MANY);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            Console.WriteLine(testResult.AreaCode);

            testResult = NorthAmericanPhone.RandomAmericanPhone(TEST_STATE_WITH_ONE);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            Console.WriteLine(testResult.AreaCode);

        }

        [Test]
        public void CanadianPhoneByProvidenceTests()
        {
            const string TEST_STATE_WITH_MANY = "ON";
            const string TEST_STATE_WITH_ONE = "YT";

            var testResult = NorthAmericanPhone.RandomCanadianPhone(TEST_STATE_WITH_MANY);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            Console.WriteLine(testResult.AreaCode);

            testResult = NorthAmericanPhone.RandomCanadianPhone(TEST_STATE_WITH_ONE);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            Console.WriteLine(testResult.AreaCode);

        }

        [Test]
        public void TestTryParse()
        {
            var testInput = "5184154299";
            var testResult = NorthAmericanPhone.TryParse(testInput, out var testResultOut);
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

            testInput = "(518)-415-4299";
            testResult = NorthAmericanPhone.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreEqual("518", testResultOut.AreaCode);
            Assert.AreEqual("415", testResultOut.CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.SubscriberNumber);
        }

        [Test]
        public void TestToUri()
        {
            var testSubject = new NorthAmericanPhone();
            testSubject.Value = "707-855-4512";
            testSubject.Descriptor = KindsOfLabels.Mobile;
            var testResult = testSubject.ToUri();
            Assert.IsNotNull(testResult);
            Assert.AreEqual("tel:7078554512;phone-context=Mobile.KindsOfLabels.Enums.Core.Rand.NoFuture", testResult.ToString());
        }

        [Test]
        public void TestTryParseUri()
        {
            var testInput = "tel:7078554512;phone-context=Mobile.KindsOfLabels.Enums.Core.Rand.NoFuture";
            NorthAmericanPhone testOutput;
            var testResult = NorthAmericanPhone.TryParse(new Uri(testInput), out testOutput);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOutput);
            Assert.AreEqual("7078554512", testOutput.Unformatted);
            Assert.AreEqual(KindsOfLabels.Mobile, testOutput.Descriptor);

        }
    }
}
