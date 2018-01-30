using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Opes;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestFixture]
    public class AmericanDeductionsTests
    {
        [Test]
        public void TestCtor()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreEqual(0, testSubject.MyItems.Count);
            Assert.AreEqual(Pecuniam.Zero, testSubject.TotalAnnualDeductions);

            testSubject.AddItem("FICA", "Government", 1200D.ToPecuniam());
            var testResultSum = testSubject.TotalAnnualDeductions;
            Assert.IsNotNull( testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(1200D.ToPecuniam().Neg, testResultSum);

            testSubject.AddItem("State Tax", "Government", 600D.ToPecuniam());
            testResultSum = testSubject.TotalAnnualDeductions;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(1800D.ToPecuniam().Neg, testResultSum);
        }

        [Test]
        public void TestGetInsuranceDeductionName2RandRates()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

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

        [Test]
        public void TestGetGovernmentDeductionName2Rates()
        {

            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

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

        [Test]
        public void TestGetEmploymentDeductionName2Rates()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

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

        [Test]
        public void TestGetJudgementDeductionName2RandomRates()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

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

        [Test]
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

        [Test]
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

        [Test]
        public void TestResolveItems()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

            testSubject.RandomizeAllItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach (var p in testSubject.MyItems)
                System.Diagnostics.Debug.WriteLine(p);
        }

        [Test]
        public void TestDeductionsRatioToIncome()
        {
            var testInput = new AmericanEmployment();
            var annualIncome = 75000.ToPecuniam();

            var options = new OpesOptions {SumTotal = annualIncome, Inception = DateTime.Today.AddYears(-1)};

            testInput.RandomizeAllItems(options);

            //check input is good
            Assert.IsNotNull(testInput.MyItems);
            Assert.AreNotEqual(0, testInput.MyItems.Count);

            var diff = Math.Abs(testInput.TotalAnnualPay.ToDouble() - annualIncome.ToDouble());
            System.Diagnostics.Debug.WriteLine(diff);
            Assert.IsTrue(Math.Round(diff) == 0.0D);

            var testSubject = new AmericanDeductions(testInput);

            testSubject.RandomizeAllItems(null);

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

        [Test]
        public void TestDeductionsAllPresent()
        {
            var testInput = new AmericanEmployment();
            var annualIncome = 75000.ToPecuniam();

            var options = new OpesOptions { SumTotal = annualIncome, Inception = DateTime.Today.AddYears(-1) };

            testInput.RandomizeAllItems(options);

            //check input is good
            Assert.IsNotNull(testInput.MyItems);
            Assert.AreNotEqual(0, testInput.MyItems.Count);

            var diff = Math.Abs(testInput.TotalAnnualPay.ToDouble() - annualIncome.ToDouble());
            System.Diagnostics.Debug.WriteLine(diff);
            Assert.IsTrue(Math.Round(diff) == 0.0D);

            var testSubject = new AmericanDeductions(testInput);
            testSubject.RandomizeAllItems(null);

            var testResults = testSubject.GetDeductionsAt(DateTime.Today.AddDays(-182));
            var allDeductionsItesm = WealthBaseTests.GetExpectedNamesFromXml("deduction");
            Assert.IsNotNull(testResults);
            Assert.IsNotNull(allDeductionsItesm);

            Assert.AreEqual(allDeductionsItesm.Count, testResults.Length);
            System.Diagnostics.Debug.WriteLine(Pondus.GetExpectedAnnualSum(testResults));
        }

        [Test]
        public void TestGetGroupNames2Portions()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

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
            testOptions.GivenDirectly.Add(new Mereo(WealthBase.DeductionGroupNames.JUDGMENTS) {Value =  1000.ToPecuniam()});

            testResults = testSubject.GetGroupNames2Portions(testOptions);
            Assert.IsTrue(testResults.Any(t => t.Item1 == WealthBase.DeductionGroupNames.JUDGMENTS));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == WealthBase.DeductionGroupNames.JUDGMENTS).Item2);
        }

        [Test]
        public void TestResolveItemsWithJudgements()
        {
            var testInput = new AmericanEmployment();
            var options = new OpesOptions { SumTotal = 75000D.ToPecuniam(), Inception = DateTime.Today.AddYears(-1) };

            testInput.RandomizeAllItems(options);

            var testSubject = new AmericanDeductions(testInput);

            options.IsPayingChildSupport = true;
            testSubject.RandomizeAllItems(options);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            var testResultItem = testSubject.MyItems.FirstOrDefault(x => x.Expectation.Name == "Child Support");
            Assert.IsNotNull(testResultItem);
            Assert.AreNotEqual(0.ToPecuniam(), testResultItem.Expectation.Value);
            System.Diagnostics.Debug.WriteLine(testResultItem.Expectation.Value);
        }
    }
}
