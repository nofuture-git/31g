using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanEmploymentTests
    {
        [TestMethod]
        public void TestGetYearsOfServiceInDates()
        {
            //still employed
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011,10,5),null);

            var testResult = testSubject.GetYearsOfServiceInDates(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(6, testResult.Count);

            testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2013, 5, 16), new DateTime(2017,8,1));
            testResult = testSubject.GetYearsOfServiceInDates(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(4, testResult.Count);

        }

        [TestMethod]
        public void TestGetPayName2RandRates()
        {
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null);
            testSubject.Occupation = StandardOccupationalClassification.GetById("41-2031");
            var testResult = testSubject.GetPayName2RandRates(testSubject.MyOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));
        }

        [TestMethod]
        public void TestGetGroupNames()
        {
            var testNames = WealthBase.GetGroupNames(WealthBase.DomusOpesDivisions.Employment);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("employment");
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
            var testNames = WealthBase.GetItemNames(WealthBase.DomusOpesDivisions.Employment);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("employment");
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
            var testSubject = new NorthAmericanEmployment(null, null);

            testSubject.ResolveItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            var testResultDeductions = testSubject.Deductions as NorthAmericanDeductions;
            Assert.IsNotNull(testResultDeductions);
            Assert.IsNotNull(testResultDeductions.MyItems);
            Assert.AreNotEqual(0, testResultDeductions.MyItems.Count);

            var testNetIncome = testSubject.TotalAnnualNetPay;

            var testGrossPay = testSubject.TotalAnnualPay;
            var testTotalDeductions = testSubject.Deductions.TotalAnnualDeductions;

            Assert.IsTrue(testGrossPay > testNetIncome);

            System.Diagnostics.Debug.WriteLine(testGrossPay);
            System.Diagnostics.Debug.WriteLine(testTotalDeductions);
            System.Diagnostics.Debug.WriteLine(testNetIncome);
        }
    }
}
