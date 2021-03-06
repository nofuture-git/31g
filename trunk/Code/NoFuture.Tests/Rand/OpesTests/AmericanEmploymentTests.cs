﻿using System;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Opes;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Org;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.OpesTests
{
    [TestFixture]
    public class AmericanEmploymentTests
    {
        [Test]
        public void TestCtor()
        {
            var testSubject = new AmericanEmployment();
            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreEqual(0, testSubject.MyItems.Count);
            Assert.AreEqual(Pecuniam.Zero, testSubject.Total);

            testSubject.AddItem("Salary", "Pay", 55000D.ToPecuniam());
            var testResultSum = testSubject.Total;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(55000D.ToPecuniam(), testResultSum);

            testSubject.AddItem("Tips", "Pay", 8000D.ToPecuniam());
            testResultSum = testSubject.Total;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(63000D.ToPecuniam(), testResultSum);

        }

        [Test]
        public void TestGetYearsOfServiceInDates()
        {
            //still employed
            var testSubject = new AmericanEmployment();

            var testResult = testSubject.GetYearsOfServiceInDates(new AmericanDomusOpesOptions{Inception = new DateTime(2011, 10, 5) });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            testSubject = new AmericanEmployment();
            testResult = testSubject.GetYearsOfServiceInDates(new AmericanDomusOpesOptions(){Inception = new DateTime(2013, 5, 16), Terminus = new DateTime(2017, 8, 1) });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(5, testResult.Count);

        }

        [Test]
        public void TestGetPayName2RandRates()
        {
            var testSubject = new AmericanEmployment();
            testSubject.Occupation = StandardOccupationalClassification.GetById("41-2031");
            var testResult = testSubject.GetPayName2RandRates(new AmericanDomusOpesOptions(){Inception = new DateTime(2011, 10, 5) });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));
        }

        [Test]
        public void TestGetGroupNames()
        {
            var testSubject = new AmericanEmployment();
            var testNames = testSubject.GetGroupNames();
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("employment");
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
            var testSubject = new AmericanEmployment();
            var testNames = testSubject.GetItemNames();
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("employment");
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
            var testSubject = new AmericanEmployment();

            testSubject.RandomizeAllItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            var testResultDeductions = testSubject.Deductions as AmericanDeductions;
            Assert.IsNotNull(testResultDeductions);
            Assert.IsNotNull(testResultDeductions.MyItems);
            Assert.AreNotEqual(0, testResultDeductions.MyItems.Count);

            var testNetIncome = testSubject.TotalAnnualNetPay;

            var testGrossPay = testSubject.Total;
            var testTotalDeductions = testSubject.Deductions.Total;

            Assert.IsTrue(testGrossPay > testNetIncome);

            System.Diagnostics.Debug.WriteLine(testGrossPay);
            System.Diagnostics.Debug.WriteLine(testTotalDeductions);
            System.Diagnostics.Debug.WriteLine(testNetIncome);
        }

        [Test]
        public void TestRandomEmployment()
        {
            var testSubject =
                AmericanEmployment.RandomEmployment(new AmericanDomusOpesOptions
                {
                    Inception = new DateTime(DateTime.Today.Year, 1, 1)
                });
            Assert.IsNotNull(testSubject);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);
        }

        [Test]
        public void TestAddItems()
        {
            var testSubject = new AmericanEmployment();

            var testResult = testSubject.Total;
            Assert.AreEqual(Pecuniam.Zero, testResult);
            testResult = testSubject.TotalAnnualNetPay;
            Assert.AreEqual(Pecuniam.Zero, testResult);

            var v = 55000M.ToPecuniam();

            var occ = new SocDetailedOccupation {Value = "Accountant"};
            testSubject.Occupation = occ;
            testSubject.AddItem("Salary",null, 55000.0);
            testResult = testSubject.Total;
            Assert.AreEqual(v, testResult);
            testResult = testSubject.TotalAnnualNetPay;
            Assert.AreEqual(v, testResult);

            var tax = new AmericanDeductions(testSubject);
            testSubject.Deductions = tax;
            testResult = testSubject.Total;
            Assert.AreEqual(v, testResult);
            testResult = testSubject.TotalAnnualNetPay;
            Assert.AreEqual(v, testResult);

            var fedTax = 55000.0D * AmericanEquations.FederalIncomeTaxRate.SolveForY(55000.0);

            tax.AddItem("Federal",null, fedTax);
            testResult = testSubject.Total;
            Assert.AreEqual(v, testResult);
            testResult = testSubject.TotalAnnualNetPay;
            Assert.AreEqual((55000.0D - fedTax).ToPecuniam(), testResult);

            testSubject.AddItem("Commission", null, 5000.0D);
            testResult = testSubject.Total;
            Assert.AreEqual(60000.ToPecuniam(), testResult);
            testResult = testSubject.TotalAnnualNetPay;
            Assert.AreEqual(49841.5.ToPecuniam(), testResult);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = AmericanEmployment.RandomEmployment();
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");

        }
    }
}
