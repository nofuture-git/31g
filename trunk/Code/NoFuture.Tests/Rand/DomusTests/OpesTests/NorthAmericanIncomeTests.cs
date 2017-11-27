using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var testInput = new NoFuture.Rand.Domus.Pneuma.Personality();
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
            var expense = new Pondus("Expense") {FromDate = DateTime.Today.AddYears(-1)};
            var otIncome = new Pondus("Other Income") { FromDate = DateTime.Today.AddYears(-2) };

            testSubject.AddOtherIncome(otIncome);
            testSubject.AddExpense(expense);

            testResult = testSubject.GetMinDate();
            Assert.AreEqual(DateTime.Today.AddYears(-3), testResult);

        }
    }
}
