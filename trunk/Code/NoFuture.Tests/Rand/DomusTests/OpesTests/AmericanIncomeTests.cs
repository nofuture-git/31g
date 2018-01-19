using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.Opes.US;
using NoFuture.Rand.Domus.Pneuma;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class AmericanIncomeTests
    {
        [TestMethod]
        public void TestGetEmploymentRanges()
        {
            var testSubject = new AmericanIncome(null);
            var testResult = testSubject.GetEmploymentRanges(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            Assert.IsNotNull(testResult[0]?.Item1);

        }

        [TestMethod]
        public void TestResolveEmployment()
        {
            var testSubject = new AmericanIncome(null);
            var testResult = testSubject.GetRandomEmployment(null, null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [TestMethod]
        public void TestGetRandomIncomeAmount()
        {
            //no args test
            var testSubject = new AmericanIncome(null);
            var testResult = testSubject.GetRandomExpectedIncomeAmount(null);

            Assert.IsNotNull(testResult);

            testResult = testSubject.GetRandomExpectedIncomeAmount(null, 69);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(testResult == Pecuniam.Zero);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetIncomeYearsInDates()
        {
            var testSubject = new AmericanIncome(null);
            var testResult = testSubject.GetYearsInDates(testSubject.GetYearNeg(-3));

            Assert.IsNotNull(testResult);
            Assert.AreEqual(4, testResult.Count);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine(t);
        }

        [TestMethod]
        public void TestGetJudgementIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome(null);
            
            var testResult = testSubject.GetJudgmentIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach(var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetSubitoIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome(null);

            var testResult = testSubject.GetSubitoIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetRealPropertyIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome(null);

            var testResult = testSubject.GetRealPropertyIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetSecuritiesIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome(null);

            var testResult = testSubject.GetSecuritiesIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetInstitutionalIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome(null);

            var testResult = testSubject.GetInstitutionalIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetGroupNames()
        {
            var testNames = WealthBase.GetGroupNames(WealthBase.DomusOpesDivisions.Income);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("income");
            var expectations = allNames.Select(n => n.Item1).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn, StringComparison.OrdinalIgnoreCase)));
                System.Diagnostics.Debug.WriteLine(tn);
            }
        }

        [TestMethod]
        public void TestGetItemNames()
        {
            var testNames = WealthBase.GetItemNames(WealthBase.DomusOpesDivisions.Income);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("income");
            var expectations = allNames.Select(n => n.Item2).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn.Name, StringComparison.OrdinalIgnoreCase)));
                System.Diagnostics.Debug.WriteLine(tn);
            }
        }

        [TestMethod]
        public void TestResolveItems()
        {
            var testSubject = new AmericanIncome(null);

            testSubject.ResolveItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach(var item in testSubject.MyItems)
                System.Diagnostics.Debug.WriteLine(item);
        }
        [TestMethod]
        public void TestRandomIncome()
        {
            var testSubject = AmericanIncome.RandomIncome(new OpesOptions { Inception = new DateTime(DateTime.Today.Year, 1, 1) });
            Assert.IsNotNull(testSubject);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);
        }
    }
}
