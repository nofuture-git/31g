﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestClass]
    public class SocialSecurityNumberTests
    {
        [TestMethod]
        public void TestRandomSsn()
        {
            var testResult = SocialSecurityNumber.RandomSsn();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("", testResult.ToString());
            Assert.IsNotNull(testResult.AreaNumber);
            Assert.IsNotNull(testResult.GroupNumber);
            Assert.IsNotNull(testResult.SerialNumber);
            System.Diagnostics.Debug.Write(testResult);
        }

        [TestMethod]
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

        [TestMethod]
        public void TestValidate()
        {
            var testSubject = new SocialSecurityNumber();
            var testResult = testSubject.Validate("361-09-1010");
            Assert.IsTrue(testResult);

            testResult = testSubject.Validate("361091010");
            System.Diagnostics.Debug.Write(testResult);
        }
    }
}