using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class CusipTests
    {
        [TestMethod]
        public void TestLetter2Num()
        {
            var testResult = NoFuture.Rand.Data.Sp.Cusip.Letter2Num['A'];
            Assert.AreEqual(1, testResult);

            testResult = NoFuture.Rand.Data.Sp.Cusip.Letter2Num['B'];
            Assert.AreEqual(2, testResult);

            testResult = NoFuture.Rand.Data.Sp.Cusip.Letter2Num['M'];
            Assert.AreEqual(13, testResult);

            testResult = NoFuture.Rand.Data.Sp.Cusip.Letter2Num['Z'];
            Assert.AreEqual(26, testResult);
        }

        [TestMethod]
        public void TestNum2Num()
        {
            var testResult = NoFuture.Rand.Data.Sp.Cusip.Num2Num['0'];
            Assert.AreEqual(0, testResult);

            testResult = NoFuture.Rand.Data.Sp.Cusip.Num2Num['1'];
            Assert.AreEqual(1, testResult);

            testResult = NoFuture.Rand.Data.Sp.Cusip.Num2Num['4'];
            Assert.AreEqual(4, testResult);

            testResult = NoFuture.Rand.Data.Sp.Cusip.Num2Num['9'];
            Assert.AreEqual(9, testResult);
        }

        [TestMethod]
        public void TestCusipChkDigit()
        {
            Assert.AreEqual(0, NoFuture.Rand.Data.Sp.Cusip.CusipChkDigit("03783310"));
            Assert.AreEqual(2, NoFuture.Rand.Data.Sp.Cusip.CusipChkDigit("17275R10"));
            Assert.AreEqual(8, NoFuture.Rand.Data.Sp.Cusip.CusipChkDigit("38259P50"));
            Assert.AreEqual(4, NoFuture.Rand.Data.Sp.Cusip.CusipChkDigit("59491810"));
            Assert.AreEqual(5, NoFuture.Rand.Data.Sp.Cusip.CusipChkDigit("68389X10"));
        }

        [TestMethod]
        public void TestGetRandom()
        {
            var testSubject = new NoFuture.Rand.Data.Sp.Cusip();
            var testResult = testSubject.GetRandom();
            Assert.AreEqual(9, testResult.Length);
            Assert.IsTrue(testSubject.Validate(testResult));
        }
    }
}
