﻿using System;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Opes;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.OpesTests
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
            Assert.AreEqual(Pecuniam.Zero, testSubject.Total);

            testSubject.AddItem("FICA", "Government", 1200D.ToPecuniam());
            var testResultSum = testSubject.Total;
            Assert.IsNotNull( testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(1200M.ToPecuniam().GetNeg(), testResultSum);

            testSubject.AddItem("State Tax", "Government", 600D.ToPecuniam());
            testResultSum = testSubject.Total;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(1800M.ToPecuniam().GetNeg(), testResultSum);
        }

        [Test]
        public void TestGetInsuranceDeductionName2RandRates()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

            var testOptions =
                new AmericanDomusOpesOptions() { Inception = DateTime.Today.AddYears(-1) };
            var testResult = testSubject.GetInsuranceDeductionName2RandRates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var tr in testResult)
                Console.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsNotNull(testOptions.SumTotal);
            Assert.AreNotEqual(0D, testOptions.SumTotal);

            Console.WriteLine(testOptions.SumTotal);

        }

        [Test]
        public void TestGetGovernmentDeductionName2Rates()
        {

            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

            var testOptions =
                new AmericanDomusOpesOptions { Inception = DateTime.Today.AddYears(-1) };
            var testResult = testSubject.GetGovernmentDeductionName2Rates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var tr in testResult)
                Console.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsNotNull(testOptions.SumTotal);
            Assert.AreNotEqual(0D, testOptions.SumTotal);

            Console.WriteLine(testOptions.SumTotal);
        }

        [Test]
        public void TestGetEmploymentDeductionName2Rates()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

            var testOptions =
                new AmericanDomusOpesOptions { Inception = DateTime.Today.AddYears(-1) };
            var testResult = testSubject.GetEmploymentDeductionName2Rates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var tr in testResult)
                Console.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsNotNull(testOptions.SumTotal);
            Assert.AreNotEqual(0D, testOptions.SumTotal);

            Console.WriteLine(testOptions.SumTotal);
        }

        [Test]
        public void TestGetJudgementDeductionName2RandomRates()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

            var testOptions =
                new AmericanDomusOpesOptions { Inception = DateTime.Today.AddYears(-1) };

            var testResult = testSubject.GetJudgmentDeductionName2RandomRates(testOptions);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var tr in testResult)
                Console.WriteLine(tr);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));
        }

        [Test]
        public void TestGetGroupNames()
        {
            var testSubject = new AmericanDeductions();
            var testNames = testSubject.GetGroupNames(testSubject.DivisionName);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("deduction");
            var expectations = allNames.Select(n => n.Item1).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn, StringComparison.OrdinalIgnoreCase)));
                Console.WriteLine(tn);
            }
        }

        [Test]
        public void TestGetItemNames()
        {
            var testSubject = new AmericanDeductions();
            var testNames = testSubject.GetItemNames(testSubject.DivisionName);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("deduction");
            var expectations = allNames.Select(n => n.Item2).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn.Name, StringComparison.OrdinalIgnoreCase)));
                Console.WriteLine(tn);
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
                Console.WriteLine(p);
        }

        [Test]
        public void TestDeductionsRatioToIncome()
        {
            var testInput = new AmericanEmployment();
            var annualIncome = 75000.ToPecuniam();

            var options = new AmericanDomusOpesOptions
            {
                Inception = DateTime.Today.AddYears(-1),
                SumTotal = annualIncome.ToDouble()
            };

            testInput.RandomizeAllItems(options);

            //check input is good
            Assert.IsNotNull(testInput.MyItems);
            Assert.AreNotEqual(0, testInput.MyItems.Count);

            var diff = Math.Abs(testInput.Total.ToDouble() - annualIncome.ToDouble());
            Console.WriteLine(diff);
            Assert.IsTrue(Math.Round(diff) == 0.0D);

            var testSubject = new AmericanDeductions(testInput);

            testSubject.RandomizeAllItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            var totalDeductions = testSubject.Total;
            Assert.AreNotEqual(0.0D.ToPecuniam(), totalDeductions);
            //expect deductions as a negative number
            Assert.IsTrue(totalDeductions.ToDouble() < 0.0D);
            Console.WriteLine(totalDeductions);

            var ratio = Math.Abs(Math.Round(((annualIncome + totalDeductions) / annualIncome).ToDouble(), 2));
            Assert.IsTrue(ratio < 1.0);
            Assert.IsTrue(ratio > 0.5);
            Console.WriteLine(totalDeductions);
        }

        [Test]
        public void TestDeductionsAllPresent()
        {
            var testInput = new AmericanEmployment();

            var options = new AmericanDomusOpesOptions
            {
                Inception = DateTime.Today.AddYears(-1),
                SumTotal = 75000D
            };

            testInput.RandomizeAllItems(options);

            //check input is good
            Assert.IsNotNull(testInput.MyItems);
            Assert.AreNotEqual(0, testInput.MyItems.Count);

            var diff = Math.Abs(testInput.Total.ToDouble() - 75000);
            Console.WriteLine(diff);
            Assert.IsTrue(Math.Round(diff) == 0.0D);

            var testSubject = new AmericanDeductions(testInput);
            testSubject.RandomizeAllItems(null);

            var testResults = testSubject.GetAt(DateTime.Today.AddDays(-182));
            var allDeductionsItesm = WealthBaseTests.GetExpectedNamesFromXml("deduction");
            Assert.IsNotNull(testResults);
            Assert.IsNotNull(allDeductionsItesm);

            Assert.AreEqual(allDeductionsItesm.Count, testResults.Length);
        }

        [Test]
        public void TestGetGroupNames2Portions()
        {
            var testInput = new AmericanEmployment();
            var testSubject = new AmericanDeductions(testInput);

            var testResults = testSubject.GetGroupNames2Portions(null);
            
            Assert.IsTrue(testResults.Any(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.INSURANCE));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.INSURANCE).Item2);

            Assert.IsTrue(testResults.Any(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.EMPLOYMENT));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.EMPLOYMENT).Item2);

            Assert.IsTrue(testResults.Any(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.GOVERNMENT));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.GOVERNMENT).Item2);

            Assert.IsTrue(testResults.Any(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.JUDGMENTS));
            Assert.AreEqual(0, testResults.First(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.JUDGMENTS).Item2);

            var testOptions = new AmericanDomusOpesOptions();
            testOptions.AddGivenDirectly(AmericanDomusOpesBase.DeductionGroupNames.JUDGMENTS, 1000);

            testResults = testSubject.GetGroupNames2Portions(testOptions);
            Assert.IsTrue(testResults.Any(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.JUDGMENTS));
            Assert.AreNotEqual(0, testResults.First(t => t.Item1 == AmericanDomusOpesBase.DeductionGroupNames.JUDGMENTS).Item2);
        }

        [Test]
        public void TestResolveItemsWithJudgements()
        {
            var testInput = new AmericanEmployment();
            var options = new AmericanDomusOpesOptions
            {
                Inception = DateTime.Today.AddYears(-1),
                SumTotal = 75000D
            };

            testInput.RandomizeAllItems(options);

            var testSubject = new AmericanDeductions(testInput);

            options.IsPayingChildSupport = true;
            testSubject.RandomizeAllItems(options);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            var testResultItem = testSubject.MyItems.FirstOrDefault(x => x.Name == "Child Support");
            Assert.IsNotNull(testResultItem);
            Assert.AreNotEqual(0.ToPecuniam(), testResultItem.Value);
            Console.WriteLine(testResultItem.Value);
        }

        [Test]
        public void TestToData()
        {
            var testSubjectContext = AmericanEmployment.RandomEmployment();
            var testSubject = new AmericanDeductions(testSubjectContext);
            testSubject.RandomizeAllItems(null);

            Assert.IsNotNull(testSubject);
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
