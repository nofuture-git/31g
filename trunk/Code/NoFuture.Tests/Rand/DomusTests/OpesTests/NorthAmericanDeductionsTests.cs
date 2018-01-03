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
                new OpesOptions() { Inception = DateTime.Today.AddYears(-1) };
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
                new OpesOptions { Inception = DateTime.Today.AddYears(-1) };
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
                new OpesOptions { Inception = DateTime.Today.AddYears(-1) };
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
                new OpesOptions { Inception = DateTime.Today.AddYears(-1) };

            var testResult = testSubject.GetJudgmentDeductionName2RandomRates(testOptions);
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

        [TestMethod]
        public void TestDeductionsRatioToIncome()
        {
            var testInput = new NorthAmericanEmployment(DateTime.Today.AddYears(-1), null);
            var annualIncome = 75000.ToPecuniam();

            var options = new OpesOptions {SumTotal = annualIncome};

            testInput.ResolveItems(options);

            //check input is good
            Assert.IsNotNull(testInput.MyItems);
            Assert.AreNotEqual(0, testInput.MyItems.Count);

            var diff = Math.Abs(testInput.TotalAnnualPay.ToDouble() - annualIncome.ToDouble());
            System.Diagnostics.Debug.WriteLine(diff);
            Assert.IsTrue(Math.Round(diff) == 0.0D);

            var testSubject = new NorthAmericanDeductions(testInput);

            testSubject.ResolveItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            var totalDeductions = testSubject.TotalAnnualDeductions;
            Assert.AreNotEqual(0.0D.ToPecuniam(), totalDeductions);
            //expect deductions as a negative number
            Assert.IsTrue(totalDeductions.ToDouble() < 0.0D);
            System.Diagnostics.Debug.WriteLine(totalDeductions);

            var ratio = Math.Abs(Math.Round(((annualIncome + totalDeductions) / annualIncome).ToDouble(), 2));
            Assert.IsTrue(ratio < 1.0);
            Assert.IsTrue(ratio > 0.5);
            System.Diagnostics.Debug.WriteLine(totalDeductions);
        }

        [TestMethod]
        public void TestDeductionsAllPresent()
        {
            var testInput = new NorthAmericanEmployment(DateTime.Today.AddYears(-1), null);
            var annualIncome = 75000.ToPecuniam();

            var options = new OpesOptions { SumTotal = annualIncome };

            testInput.ResolveItems(options);

            //check input is good
            Assert.IsNotNull(testInput.MyItems);
            Assert.AreNotEqual(0, testInput.MyItems.Count);

            var diff = Math.Abs(testInput.TotalAnnualPay.ToDouble() - annualIncome.ToDouble());
            System.Diagnostics.Debug.WriteLine(diff);
            Assert.IsTrue(Math.Round(diff) == 0.0D);

            var testSubject = new NorthAmericanDeductions(testInput);
            System.Diagnostics.Debug.WriteLine(testSubject.MyOptions.SumTotal);
            testSubject.ResolveItems(null);

            var testResults = testSubject.GetDeductionsAt(DateTime.Today.AddDays(-182));
            var allDeductionsItesm = WealthBaseTests.GetExpectedNamesFromXml("deduction");
            Assert.IsNotNull(testResults);
            Assert.IsNotNull(allDeductionsItesm);

            Assert.AreEqual(allDeductionsItesm.Count, testResults.Length);
            System.Diagnostics.Debug.WriteLine(Pondus.GetExpectedAnnualSum(testResults));
        }

        [TestMethod]
        public void TestGetGroupNames2Portions()
        {
            var testInput = new NorthAmericanEmployment(null, null);
            var testSubject = new NorthAmericanDeductions(testInput);

            var testResults = testSubject.GetGroupNames2Portions(null);
            
            Assert.IsTrue(testResults.Any(t => t.Item1 == WealthBase.DeductionGroupNames.INSURANCE));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == WealthBase.DeductionGroupNames.INSURANCE).Item2);

            Assert.IsTrue(testResults.Any(t => t.Item1 == WealthBase.DeductionGroupNames.EMPLOYMENT));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == WealthBase.DeductionGroupNames.EMPLOYMENT).Item2);

            Assert.IsTrue(testResults.Any(t => t.Item1 == WealthBase.DeductionGroupNames.GOVERNMENT));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == WealthBase.DeductionGroupNames.GOVERNMENT).Item2);

            Assert.IsTrue(testResults.Any(t => t.Item1 == WealthBase.DeductionGroupNames.JUDGMENTS));
            Assert.AreEqual(0, testResults.First(t => t.Item1 == WealthBase.DeductionGroupNames.JUDGMENTS).Item2);

            var testOptions = new OpesOptions();
            testOptions.GivenDirectly.Add(new Mereo(WealthBase.DeductionGroupNames.JUDGMENTS) {ExpectedValue =  1000.ToPecuniam()});

            testResults = testSubject.GetGroupNames2Portions(testOptions);
            Assert.IsTrue(testResults.Any(t => t.Item1 == WealthBase.DeductionGroupNames.JUDGMENTS));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == WealthBase.DeductionGroupNames.JUDGMENTS).Item2);
        }

        [TestMethod]
        public void TestResolveItemsWithJudgements()
        {
            var testInput = new NorthAmericanEmployment(DateTime.Today.AddYears(-1), null);
            var testSubject = new NorthAmericanDeductions(testInput);

            var testOptions =
                new OpesOptions() {Inception = testInput.MyOptions.Inception, IsPayingChildSupport = true};
            testSubject.ResolveItems(testOptions);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            var testResultItem = testSubject.MyItems.FirstOrDefault(x => x.My.Name == "Child Support");
            Assert.IsNotNull(testResultItem);
            Assert.AreNotEqual(0.ToPecuniam(), testResultItem.My.ExpectedValue);
            System.Diagnostics.Debug.WriteLine(testResultItem.My.ExpectedValue);
        }
    }
}
