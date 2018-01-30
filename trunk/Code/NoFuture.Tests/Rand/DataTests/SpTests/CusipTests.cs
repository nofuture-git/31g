using NUnit.Framework;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using Dbg = System.Diagnostics.Debug;

namespace NoFuture.Rand.Tests.DataTests.SpTests
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
            Dbg.WriteLine(testValue);

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
            Assert.AreEqual("PARSONS & CO INC",testResult);
        }

        [Test]
        public void TestGetNameFull()
        {
            var testResult = Com.Firm.GetNameFull("BRAND BKG CO");
            Assert.AreEqual("Brand Banking Company", testResult);

        }
    }
}
