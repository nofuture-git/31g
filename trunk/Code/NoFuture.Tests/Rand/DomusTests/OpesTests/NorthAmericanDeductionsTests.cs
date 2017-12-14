using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanDeductionsTests
    {
        [TestMethod]
        public void TestGetInsuranceDeductionName2RandRates()
        {
            var testInput = new NorthAmericanEmployment(null, null);
            var testSubject = new NorthAmericanDeductions(testInput);

            var testOptions =
                new OpesOptions() { StartDate = DateTime.Today.AddYears(-1) };
            var testResult = testSubject.GetInsuranceDeductionName2RandRates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsNotNull(testOptions.SumTotal);
            Assert.AreNotEqual(Pecuniam.Zero, testOptions.SumTotal);

            System.Diagnostics.Debug.WriteLine(testOptions.SumTotal);

        }

        [TestMethod]
        public void TestGetGovernmentDeductionName2Rates()
        {

            var testInput = new NorthAmericanEmployment(null, null);
            var testSubject = new NorthAmericanDeductions(testInput);

            var testOptions =
                new OpesOptions { StartDate = DateTime.Today.AddYears(-1) };
            var testResult = testSubject.GetGovernmentDeductionName2Rates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsNotNull(testOptions.SumTotal);
            Assert.AreNotEqual(Pecuniam.Zero, testOptions.SumTotal);

            System.Diagnostics.Debug.WriteLine(testOptions.SumTotal);
        }

        [TestMethod]
        public void TestGetEmploymentDeductionName2Rates()
        {
            var testInput = new NorthAmericanEmployment(null, null);
            var testSubject = new NorthAmericanDeductions(testInput);

            var testOptions =
                new OpesOptions { StartDate = DateTime.Today.AddYears(-1) };
            var testResult = testSubject.GetEmploymentDeductionName2Rates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsNotNull(testOptions.SumTotal);
            Assert.AreNotEqual(Pecuniam.Zero, testOptions.SumTotal);

            System.Diagnostics.Debug.WriteLine(testOptions.SumTotal);
        }

        [TestMethod]
        public void TestGetJudgementDeductionName2RandomRates()
        {
            var testInput = new NorthAmericanEmployment(null, null);
            var testSubject = new NorthAmericanDeductions(testInput);

            var testOptions =
                new OpesOptions { StartDate = DateTime.Today.AddYears(-1) };

            var testResult = testSubject.GetJudgementDeductionName2RandomRates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));
        }

        [TestMethod]
        public void TestGetGroupNames()
        {
            var testNames = WealthBase.GetGroupNames(WealthBase.DomusOpesDivisions.Deduction);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("deduction");
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
            var testNames = WealthBase.GetItemNames(WealthBase.DomusOpesDivisions.Deduction);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("deduction");
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
            var testInput = new NorthAmericanEmployment(null, null);
            var testSubject = new NorthAmericanDeductions(testInput);

            testSubject.ResolveItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach (var p in testSubject.MyItems)
                System.Diagnostics.Debug.WriteLine(p);
        }
    }
}
