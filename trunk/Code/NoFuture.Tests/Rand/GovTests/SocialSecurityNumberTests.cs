using System;
using NoFuture.Rand.Core.Enums;
using NUnit.Framework;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestFixture]
    public class SocialSecurityNumberTests
    {
        [Test]
        public void TestRandomSsn()
        {
            var testResult = SocialSecurityNumber.RandomSsn();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("", testResult.ToString());
            Assert.IsNotNull(testResult.AreaNumber);
            Assert.IsNotNull(testResult.GroupNumber);
            Assert.IsNotNull(testResult.SerialNumber);
            Console.Write(testResult);
        }

        [Test]
        public void TestSetValue()
        {
            var testSubject = new SocialSecurityNumber();
            testSubject.Value = "361-09-1010";
            Assert.IsNotNull(testSubject.AreaNumber);
            Assert.AreEqual("361", testSubject.AreaNumber);
            Assert.IsNotNull(testSubject.GroupNumber);
            Assert.AreEqual("09", testSubject.GroupNumber);
            Assert.IsNotNull(testSubject.SerialNumber);
            Assert.AreEqual("1010", testSubject.SerialNumber);

            testSubject.Value = "385-54-8845";
            Assert.IsNotNull(testSubject.AreaNumber);
            Assert.AreEqual("385", testSubject.AreaNumber);
            Assert.IsNotNull(testSubject.GroupNumber);
            Assert.AreEqual("54", testSubject.GroupNumber);
            Assert.IsNotNull(testSubject.SerialNumber);
            Assert.AreEqual("8845", testSubject.SerialNumber);
        }

        [Test]
        public void TestValidate()
        {
            var testSubject = new SocialSecurityNumber();
            var testResult = testSubject.Validate("361-09-1010");
            Assert.IsTrue(testResult);

            testResult = testSubject.Validate("361091010");
            Console.Write(testResult);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = new SocialSecurityNumber {Value = "555-44-6666"};
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
