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
            var testResult = testSubject.GetRandomEmployment(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testInput = new NoFuture.Rand.Domus.Pneuma.Personality();
            testInput.Openness.Value = new Dimension(0.99, 0.10);

            testResult = testSubject.GetRandomEmployment(testInput);
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
            var testResult = testSubject.GetMinDateAmongExpectations();
            Assert.IsTrue((DateTime.Today - testResult).Days > 365);

            //given only employment - gets its min date
            var emply = new NorthAmericanEmployment(DateTime.Today.AddYears(-3), null);
            testSubject.AddEmployment(emply);
            testResult = testSubject.GetMinDateAmongExpectations();
            Assert.AreEqual(DateTime.Today.AddYears(-3), testResult);

            //given other more recent items - employment still the oldest
            var expense = new Pondus("Expense") {Inception = DateTime.Today.AddYears(-1)};
            var otIncome = new Pondus("Other Income") { Inception = DateTime.Today.AddYears(-2) };

            testSubject.AddExpectedOtherIncome(otIncome);
            testSubject.AddExpectedExpense(expense);

            testResult = testSubject.GetMinDateAmongExpectations();
            Assert.AreEqual(DateTime.Today.AddYears(-3), testResult);

        }

        [TestMethod]
        public void TestGetOtherIncomeName2RandomeRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetOtherIncomeName2RandomRates(0);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.AreEqual(0D, testResult.Values.Sum());

            testResult = testSubject.GetOtherIncomeName2RandomRates(0.25);

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

        [TestMethod]
        public void TestGetRandomIncomeAmount()
        {
            //no args test
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetRandomExpectedIncomeAmount(null);

            Assert.IsNotNull(testResult);

            testResult = testSubject.GetRandomExpectedIncomeAmount(null, 69);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(testResult == Pecuniam.Zero);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetUtilitiesNames2RandomRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetUtilityExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //try it on rental
            testSubject = new NorthAmericanIncome(null, new NorthAmericanIncome.IncomeOptions{IsRenting = true});
            testResult = testSubject.GetUtilityExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [TestMethod]
        public void TestGetPersonalExpenseNames2RandomRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetPersonalExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [TestMethod]
        public void TestGetInsuranceExpenseNames2RandomRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetInsuranceExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Home"));
            Assert.IsTrue(testResult["Home"] > 0D);

            Assert.IsTrue(testResult.ContainsKey("Vehicle"));
            Assert.IsTrue(testResult["Vehicle"] == 0D);

            Assert.IsTrue(testResult.ContainsKey("Renters"));
            Assert.IsTrue(testResult["Renters"] == 0D);

            testSubject = new NorthAmericanIncome(null, new NorthAmericanIncome.IncomeOptions { IsRenting = true, HasVehicle = true});
            testResult = testSubject.GetInsuranceExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Home"));
            Assert.IsTrue(testResult["Home"] == 0D);

            Assert.IsTrue(testResult.ContainsKey("Vehicle"));
            Assert.IsTrue(testResult["Vehicle"] > 0D);

            Assert.IsTrue(testResult.ContainsKey("Renters"));
            Assert.IsTrue(testResult["Renters"] > 0D);


        }

        [TestMethod]
        public void TestGetTransportationExpenseNames2RandomRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetTransportationExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Public Transportation"));
            Assert.IsTrue(testResult["Public Transportation"] > 0D);

            testSubject = new NorthAmericanIncome(null, new NorthAmericanIncome.IncomeOptions { IsRenting = true, HasVehicle = true });
            testResult = testSubject.GetTransportationExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Public Transportation"));
            Assert.IsTrue(testResult["Public Transportation"] == 0D);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");

        }

        [TestMethod]
        public void TestGetHomeExpenseNames2RandomRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetHomeExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Rent"));
            Assert.IsTrue(testResult["Rent"] == 0D);

            testSubject = new NorthAmericanIncome(null, new NorthAmericanIncome.IncomeOptions { IsRenting = true });
            testResult = testSubject.GetHomeExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Rent"));
            Assert.IsTrue(testResult["Rent"] > 0D);
        }

        [TestMethod]
        public void TestGetChildrenExpenseNames2RandomRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetChildrenExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testSubject = new NorthAmericanIncome(null, new NorthAmericanIncome.IncomeOptions { ChildrenAges = new []{2,4}});
            testResult = testSubject.GetChildrenExpenseNames2RandomRates();

            Assert.IsTrue(testResult.ContainsKey("Extracurricular"));
            Assert.IsTrue(testResult["Extracurricular"] == 0D);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandomRates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetDebtExpenseNames2RandomRates();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }
    }
}
