using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
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
                System.Diagnostics.Debug.WriteLine($"{t.ExpectedValue} {t.Id.Name} {t.Id.Interval} {t.Id.GetName(KindsOfNames.Group)}");

            var sumResult = Pondus.GetExpectedAnnualSum(testResult);
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

        [TestMethod]
        public void TestGetHomeExpenseNames2RandomRates_WithAssign()
        {
            var testSubject = new NorthAmericanIncome(null,
                new NorthAmericanIncome.IncomeOptions
                {
                    ChildrenAges = new[] { 2, 4 },
                    IsRenting = false,
                    HasVehicle = true,
                    IsVehiclePaidOff = false
                });
            var testInput = new WealthBase.RatesDictionaryArgs()
            {
                SumOfRates = 0.33D,
                DerivativeSlope = -0.2D,
                DirectAssignNames2Rates = new Dictionary<string, double> {{"Mortgage", 0.1377D}}
            };
            var testResult = testSubject.GetHomeExpenseNames2RandomRates(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Mortgage"));
            Assert.IsTrue(testResult["Mortgage"] == 0.1377D);

            var sum = testResult.Select(kv => kv.Value).Sum();
            System.Diagnostics.Debug.WriteLine(sum);
            var diffInSum = Math.Round(Math.Abs(0.33D - sum),3);
            Assert.IsTrue(diffInSum <= 0.001);
            Assert.IsTrue(diffInSum >= -0.001);
        }


        [TestMethod]
        public void TestGetExpenseItemsForRange()
        {
            var testSubject = new NorthAmericanIncome(null,
                new NorthAmericanIncome.IncomeOptions
                {
                    ChildrenAges = new[] {2, 4},
                    IsRenting = false,
                    HasVehicle = true,
                    IsVehiclePaidOff = false
                });
            var testResult = testSubject.GetExpenseItemsForRange(55000.ToPecuniam(), DateTime.MinValue);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            var d = Pondus.GetSum(testResult);
            System.Diagnostics.Debug.WriteLine(d);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine($"{Math.Round(t.Value.ToDouble()/12D)} {t.Id.GetName(KindsOfNames.Group)} {t.Id.Name}");
        }


        [TestMethod]
        public void TestGetExpenseItemsForRange_WithDirectAssign()
        {
            var testSubject = new NorthAmericanIncome(null, new NorthAmericanIncome.IncomeOptions
            {
                ChildrenAges = new[] { 2, 4 },
                IsRenting = false,
                HasVehicle = true,
                IsVehiclePaidOff = false
            });
            var directAssigns = new Dictionary<IVoca, Pecuniam>();

            directAssigns.Add(new Mereo("Mortgage", "Home"), 13476.0D.ToPecuniam());
            directAssigns.Add(new Mereo("Loan Payments", "Transportation"), 3750.0D.ToPecuniam());
            directAssigns.Add(new Mereo("Credit Card", "Debts"), 2728.44D.ToPecuniam());

            var testResult = testSubject.GetExpenseItemsForRange(97860.0D.ToPecuniam(), DateTime.Today.AddYears(-1), null, Interval.Annually, directAssigns);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            var testPondus =
                testResult.FirstOrDefault(p => p.Id.Name == "Mortgage" && p.Id.GetName(KindsOfNames.Group) == "Home");
            Assert.IsNotNull(testPondus);
            System.Diagnostics.Debug.WriteLine(testPondus);
            var testAmt = testPondus.ExpectedValue;
            Assert.IsNotNull(testAmt);
            Assert.AreEqual(13476.0D, testAmt.ToDouble());

            testPondus = testResult.FirstOrDefault(p =>
                p.Id.Name == "Loan Payments" && p.Id.GetName(KindsOfNames.Group) == "Transportation");
            Assert.IsNotNull(testPondus);
            System.Diagnostics.Debug.WriteLine(testPondus);
            testAmt = testPondus.ExpectedValue;
            Assert.IsNotNull(testAmt);
            Assert.AreEqual(3750.0D, testAmt.ToDouble());

            testPondus = testResult.FirstOrDefault(p =>
                p.Id.Name == "Credit Card" && p.Id.GetName(KindsOfNames.Group) == "Debts");
            Assert.IsNotNull(testPondus);
            System.Diagnostics.Debug.WriteLine(testPondus);
            testAmt = testPondus.ExpectedValue;
            Assert.IsNotNull(testAmt);
            Assert.AreEqual(2728.44D, testAmt.ToDouble());
        }

        [TestMethod]
        public void TestGetIncomeYearsInDates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetIncomeYearsInDates(testSubject.GetYearNeg(-3));

            Assert.IsNotNull(testResult);
            Assert.AreEqual(4, testResult.Count);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine(t);
        }

        [TestMethod]
        public void TestResolveIncomeAndExpenses()
        {
            var testSubject = new NorthAmericanIncome(null);
            testSubject.ResolveExpectedIncomeAndExpenses();

            var emplyTr = testSubject.Employment;
            Assert.IsNotNull(emplyTr);
            Assert.AreNotEqual(0, emplyTr.Count);

            var otTr = testSubject.ExpectedOtherIncome;
            Assert.IsNotNull(otTr);
            Assert.AreNotEqual(0, otTr.Count);

            var expenseTr = testSubject.ExpectedExpenses;
            Assert.IsNotNull(expenseTr);
            Assert.AreNotEqual(0, expenseTr.Count);

            var printMe = new List<Pondus>();

            printMe.AddRange(otTr.Take(3).ToList());
            printMe.AddRange(expenseTr.Take(3).ToList());
            foreach(var p in printMe)
                System.Diagnostics.Debug.WriteLine(p);

        }

        [TestMethod]
        public void TestReassignRates()
        {
            var testSubject = new NorthAmericanIncome(null);

            var testInput00 = new Dictionary<string, double>
            {
                {"Item1", 0.25},
                {"Item2", 0.25},
                {"Item3", 0.25},
                {"Item4", 0.25},
            };

            var testInput01 = new List<Tuple<string, double>>
            {
                new Tuple<string, double>("Item1", 0.33D)
            };

            var testResult = testSubject.ReassignRates(testInput00, testInput01);

            //test is gave something back
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //test that the reassignment worked
            Assert.IsTrue(testResult.ContainsKey("Item1"));
            Assert.AreEqual(0.33D, testResult["Item1"]);

            //test that the sum total is preserved
            var sum = testResult.Select(kv => kv.Value).Sum();
            Assert.IsTrue(sum >= 0.99D && sum <= 1.01D);

        }
    }
}
