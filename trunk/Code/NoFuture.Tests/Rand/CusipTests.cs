using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using Dbg = System.Diagnostics.Debug;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class CusipTests
    {
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestCusipChkDigit()
        {
            Assert.AreEqual(0, Cusip.CusipChkDigit("03783310"));
            Assert.AreEqual(7, Cusip.CusipChkDigit("17275R10"));
            Assert.AreEqual(3, Cusip.CusipChkDigit("38259P50"));
            Assert.AreEqual(4, Cusip.CusipChkDigit("59491810"));
            Assert.AreEqual(0, Cusip.CusipChkDigit("68389X10"));
        }

        [TestMethod]
        public void TestGetRandom()
        {
            var testSubject = new Cusip();
            var testResult = testSubject.GetRandom();
            Assert.AreEqual(9, testResult.Length);
            Assert.IsTrue(testSubject.Validate(testResult));
        }

        [TestMethod]
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

        [TestMethod]
        public void TestIssueGroup()
        {
            var testSubject = new Cusip {Value = "U4OR21282"};
            Assert.AreEqual(SecurityIssueGroup.Equity, testSubject.IssueGroup);

            testSubject = new Cusip { Value = "U4OR212A2" };
            Assert.AreEqual(SecurityIssueGroup.FixedIncome, testSubject.IssueGroup);
            Assert.IsTrue(testSubject.Validate("U4OR21282"));
        }

        [TestMethod]
        public void TestGetSearchCompanyName()
        {
            var testResult = Cusip.GetSearchCompanyName("HNI Corporation");
            Assert.AreEqual("HNI CORP", testResult);
            testResult = Cusip.GetSearchCompanyName("ZION OIL & GAS INC.");
            Assert.AreEqual("ZION OIL & GAS INC", testResult);
            testResult = Cusip.GetSearchCompanyName("The Saint Louis Glass Company");
            Assert.AreEqual("ST LOUIS GLASS CO", testResult);
            testResult = Cusip.GetSearchCompanyName("Twenty-Nine Palms, California");
            Assert.AreEqual("TWENTY NINE PALMS CALIF", testResult);
            testResult = Cusip.GetSearchCompanyName("B/G Foods Company");
            Assert.AreEqual("B G FOODS CO", testResult);
            testResult = Cusip.GetSearchCompanyName("A. & C. Company Mortgage");
            Assert.AreEqual("A & C CO MTG", testResult);

            testResult = Cusip.GetSearchCompanyName("M&T Bank Corporation");
            Assert.AreEqual("M&T BK CORP", testResult);

            testResult = Cusip.GetSearchCompanyName("Parsons and Company Incorporated");
            Assert.AreEqual("PARSONS & CO INC",testResult);
        }

        [TestMethod]
        public void TestGetNameFull()
        {
            var testResult = Cusip.GetNameFull("BRAND BKG CO");
            Assert.AreEqual("Brand Banking Company", testResult);

        }
    }
}
