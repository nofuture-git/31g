using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.Pneuma;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanIncomeTests
    {
        [TestMethod]
        public void TestGetEmploymentRanges()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetEmploymentRanges(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            Assert.IsNotNull(testResult[0]?.Item1);

            var testInput = new Personality();
            testInput.Openness.Value = new Dimension(0.99, 0.10);

            testResult = testSubject.GetEmploymentRanges(testInput);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Count > 1);
            Assert.IsNotNull(testResult[0]?.Item2);

            foreach (var tuple in testResult)
            {
                System.Diagnostics.Debug.WriteLine(tuple);
            }

        }

        [TestMethod]
        public void TestResolveEmployment()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.ResolveEmployment(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testInput = new NoFuture.Rand.Domus.Pneuma.Personality();
            testInput.Openness.Value = new Dimension(0.99, 0.10);

            testResult = testSubject.ResolveEmployment(testInput);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Count > 1);
            foreach (var emply in testResult)
            {
                System.Diagnostics.Debug.WriteLine(emply);
            }
        }

        [TestMethod]
        public void TestGetMinDate()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetMinDate();
            Assert.IsTrue((DateTime.Today - testResult).Days > 365);

            //given only employment - gets its min date
            var emply = new NorthAmericanEmployment(DateTime.Today.AddYears(-3), null);
            testSubject.AddEmployment(emply);
            testResult = testSubject.GetMinDate();
            Assert.AreEqual(DateTime.Today.AddYears(-3), testResult);

            //given other more recent items - employment still the oldest
            var expense = new Pondus("Expense") {Inception = DateTime.Today.AddYears(-1)};
            var otIncome = new Pondus("Other Income") { Inception = DateTime.Today.AddYears(-2) };

            testSubject.AddOtherIncome(otIncome);
            testSubject.AddExpense(expense);

            testResult = testSubject.GetMinDate();
            Assert.AreEqual(DateTime.Today.AddYears(-3), testResult);

        }

        [TestMethod]
        public void TestGetOtherIncomeName2RandomeRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetOtherIncomeName2RandomeRates(0);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.AreEqual(0D, testResult.Values.Sum());

            testResult = testSubject.GetOtherIncomeName2RandomeRates(0.25);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            Assert.AreEqual(0.25, Math.Round(testResult.Values.Sum(),2));

            foreach(var item in testResult)
                System.Diagnostics.Debug.WriteLine(string.Join(" - ", item.Key, item.Value.ToString() ));
        }

        [TestMethod]
        public void TestGetOtherIncomeItemsForRange()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetOtherIncomeItemsForRange(5660.0D.ToPecuniam(), DateTime.Today.AddYears(-1));

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine($"{t.Value} {t.Name} {t.Interval} {t.GetName(KindsOfNames.Group)}");

            var sumResult = Pondus.GetAnnualSum(testResult);
            Assert.IsTrue(sumResult.ToDouble() >= 5659.99 && sumResult.ToDouble() <= 5660.01);
        }
    }
}
