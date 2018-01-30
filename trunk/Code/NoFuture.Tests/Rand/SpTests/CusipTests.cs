using System;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class CusipTests
    {
        [Test]
        public void TestLetter2Num()
        {
            var testResult = Cusip.Letter2Num['A'];
            Assert.AreEqual(10, testResult);

            testResult = Cusip.Letter2Num['B'];
            Assert.AreEqual(11, testResult);

            testResult = Cusip.Letter2Num['M'];
            Assert.AreEqual(22, testResult);

            testResult = Cusip.Letter2Num['Z'];
            Assert.AreEqual(35, testResult);
        }

        [Test]
        public void TestNum2Num()
        {
            var testResult = Cusip.Num2Num['0'];
            Assert.AreEqual(0, testResult);

            testResult = Cusip.Num2Num['1'];
            Assert.AreEqual(1, testResult);

            testResult = Cusip.Num2Num['4'];
            Assert.AreEqual(4, testResult);

            testResult = Cusip.Num2Num['9'];
            Assert.AreEqual(9, testResult);
        }

        [Test]
        public void TestCusipChkDigit()
        {
            Assert.AreEqual(0, Cusip.CusipChkDigit("03783310"));
            Assert.AreEqual(2, Cusip.CusipChkDigit("17275R10"));
            Assert.AreEqual(9, Cusip.CusipChkDigit("38259P50"));
            Assert.AreEqual(4, Cusip.CusipChkDigit("59491810"));
            Assert.AreEqual(5, Cusip.CusipChkDigit("68389X10"));
        }

        [Test]
        public void TestGetRandom()
        {
            var testSubject = new Cusip();
            var testResult = testSubject.GetRandom();
            Assert.AreEqual(9, testResult.Length);
            Assert.IsTrue(testSubject.Validate(testResult));
        }

        [Test]
        public void TestGetValue()
        {
            var testSubject = new Cusip();
            var testValue = testSubject.Value;
            Console.WriteLine(testValue);

            Assert.IsNotNull(testSubject.Issuer);
            Assert.AreEqual(5,testSubject.Issuer.Length);
            Assert.IsNotNull(testSubject.Issue);
            Assert.AreEqual(2, testSubject.Issue.Length);
            
            Assert.IsTrue(testSubject.Validate(testValue));

            testSubject = new Cusip { Value = "U4OR21282" };
            Assert.AreEqual("28", testSubject.Issue);
        }

        [Test]
        public void TestIssueGroup()
        {
            var testSubject = new Cusip {Value = "U4OR21282"};
            Assert.AreEqual(SecurityIssueGroup.Equity, testSubject.IssueGroup);

            testSubject = new Cusip { Value = "U4OR212A2" };
            Assert.AreEqual(SecurityIssueGroup.FixedIncome, testSubject.IssueGroup);
            Assert.IsTrue(testSubject.Validate("U4OR21282"));
        }

    }
}
