using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;

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

            var testResult = testSubject.GetYearsOfServiceInDates();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(6, testResult.Count);

            testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2013, 5, 16), new DateTime(2017,8,1));
            testResult = testSubject.GetYearsOfServiceInDates();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(4, testResult.Count);

        }

        [TestMethod]
        public void TestGetPayItemsForRange()
        {
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null);
            testSubject.Occupation = StandardOccupationalClassification.GetById("39-3011");

            var testResult = testSubject.GetPayItemsForRange(55000D.ToPecuniam(), testSubject.Inception,
                testSubject.Inception.AddYears(1));

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            var testPondus = testResult.FirstOrDefault(p => p.My.Name == "Wages");
            Assert.IsNotNull(testPondus);
            Assert.AreNotEqual(Pecuniam.Zero, testPondus.ExpectedValue);

            testSubject.Occupation = StandardOccupationalClassification.GetById("41-2031");
            testResult = testSubject.GetPayItemsForRange(55000D.ToPecuniam(), testSubject.Inception,
                testSubject.Inception.AddYears(1));

            testPondus = testResult.FirstOrDefault(p => p.My.Name == "Commissions");
            Assert.IsNotNull(testPondus);
            Assert.AreNotEqual(Pecuniam.Zero, testPondus.ExpectedValue);

            testSubject.Occupation = StandardOccupationalClassification.GetById("29-2021");
            //handles not date range
            testResult = testSubject.GetPayItemsForRange(55000D.ToPecuniam(), DateTime.MinValue);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

        }

        [TestMethod]
        public void TestGetDeductionItemsForRange()
        {
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null);
            var testResult = testSubject.GetDeductionItemsForRange(55000D.ToPecuniam(), testSubject.Inception,
                testSubject.Inception.AddYears(1));

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            var testPondus = testResult.FirstOrDefault(p => p.My.Name == "Federal tax");
            Assert.IsNotNull(testPondus);
            Assert.AreNotEqual(Pecuniam.Zero, testPondus.ExpectedValue);

            //handles no date range
            testResult = testSubject.GetDeductionItemsForRange(55000D.ToPecuniam(), DateTime.MinValue);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

        }

        [TestMethod]
        public void TestResolveIncomeAndDeductions()
        {
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null);
            testSubject.ResolvePayAndDeductions();

            var testResults = testSubject.AllItems;
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

        }

        [TestMethod]
        public void TestGetIncomeName2RandomRates()
        {
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null);
            testSubject.Occupation = StandardOccupationalClassification.GetById("41-2031");
            var testResult = testSubject.GetIncomeName2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var sumOfRates = 0D;
            foreach (var item in testResult)
            {
                sumOfRates += item.Value;
            }

            Assert.IsTrue(Math.Abs(1D - sumOfRates) < 0.001);
            Assert.IsTrue(Math.Abs(1D - sumOfRates) > -0.001);
        }

        [TestMethod]
        public void TestGetDeductionNames2RandomRates()
        {
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null);
            testSubject.Occupation = StandardOccupationalClassification.GetById("41-2031");
            var testResult = testSubject.GetDeductionNames2RandomRates(55000D.ToPecuniam(), null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var sumOfRates = 0D;
            foreach (var item in testResult)
            {
                sumOfRates += item.Value;
                System.Diagnostics.Debug.WriteLine(string.Join(" - ", item.Key, item.Value));
            }
            System.Diagnostics.Debug.WriteLine(sumOfRates);
            Assert.AreNotEqual(1D, sumOfRates);
        }
    }
}
