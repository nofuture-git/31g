﻿using NUnit.Framework;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Tests.TeleTests
{
    [TestFixture]
    public class PhoneTests
    {
        [Test]
        public void AmericanPhoneTests()
        {
            var testResult = Phone.RandomAmericanPhone();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.AreaCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.CentralOfficeCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.SubscriberNumber));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Formatted));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));

            Assert.IsTrue(testResult.ToString().Contains("-"));

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);
            System.Diagnostics.Debug.WriteLine(testResult.CentralOfficeCode);
            System.Diagnostics.Debug.WriteLine(testResult.SubscriberNumber);
        }

        [Test]
        public void AmericanPhoneByStateTests()
        {
            const string TEST_STATE_WITH_MANY = "CA";
            const string TEST_STATE_WITH_ONE = "DC";

            var testResult = Phone.RandomAmericanPhone(TEST_STATE_WITH_MANY);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

            testResult = Phone.RandomAmericanPhone(TEST_STATE_WITH_ONE);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

        }

        [Test]
        public void CanadianPhoneByProvidenceTests()
        {
            const string TEST_STATE_WITH_MANY = "ON";
            const string TEST_STATE_WITH_ONE = "YT";

            var testResult = Phone.RandomCanadianPhone(TEST_STATE_WITH_MANY);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

            testResult = Phone.RandomCanadianPhone(TEST_STATE_WITH_ONE);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AreaCode);

            System.Diagnostics.Debug.WriteLine(testResult.AreaCode);

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
        }
    }
}
