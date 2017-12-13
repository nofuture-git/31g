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
using NoFuture.Shared.Core;

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
                System.Diagnostics.Debug.WriteLine($"{t.My.ExpectedValue} {t.My.Name} {t.My.Interval} {t.My.GetName(KindsOfNames.Group)}");

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
            testSubject = new NorthAmericanIncome(null, new OpesOptions{IsRenting = true});
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

            testSubject = new NorthAmericanIncome(null, new OpesOptions{ IsRenting = true, NumberOfVehicles = 1});
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

            testSubject = new NorthAmericanIncome(null, new OpesOptions{ IsRenting = true, NumberOfVehicles = 1 });
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

            testSubject = new NorthAmericanIncome(null, new OpesOptions { IsRenting = true });
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
            var options = new OpesOptions();
            options.ChildrenAges.Add(2);
            options.ChildrenAges.Add(4);
            testSubject = new NorthAmericanIncome(null, options);
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
        public void TestGetDebtExpenseNames2RandRates_NoOptions()
        {
            //what happens if you just invoke it with no options whatsoever?
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetDebtExpenseNames2RandomRates((OpesOptions) null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //then its truely random
            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_SingleGivenDirectly()
        {
            //what happens if its just a single item and no SumTotal?
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 9000.ToPecuniam() });
            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //given no subtotal the assign amount is considered "all of it"
            var singleItem = testResult.FirstOrDefault(x => x.Key == "Student");
            Assert.IsNotNull(singleItem);
            Assert.AreEqual(1D, singleItem.Value);
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_TwoGivenDirectly()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //so if we add another then their assigned rates will be their portion of the whole?
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 9000.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Other Consumer", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000.ToPecuniam() });
            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //yes, since no SumTotal was given these are the only two to divide the whole on
            var firstItem = testResult.FirstOrDefault(x => x.Key == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.9D, firstItem.Value);

            var secondItem = testResult.FirstOrDefault(x => x.Key == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.1D, secondItem.Value);
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_TwoGivenDirectlyAndSumWhichEquals()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //so now what happens if we do give a SumTotal which happens to exactly equal the GivenDirectly's sum?
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 9000.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Other Consumer", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000.ToPecuniam() });
            testOptions.SumTotal = 10000.ToPecuniam();

            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //nothing changes, assigning the sumtotal as the actual sum doesn't change anything
            var firstItem = testResult.FirstOrDefault(x => x.Key == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.9D, firstItem.Value);

            var secondItem = testResult.FirstOrDefault(x => x.Key == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.1D, secondItem.Value);
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_TwoGivenDirectlyAndSumWhichLt()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //so what happens if the sumtotal is actually less than the sum of the GivenDirectly's sum?
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 9000.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Other Consumer", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000.ToPecuniam() });
            testOptions.SumTotal = 9000.ToPecuniam(); //1000 less

            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //the assigned sumtotal is ignored and replaced with the GivenDirectly's sum to make everything fit.
            var firstItem = testResult.FirstOrDefault(x => x.Key == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.9D, firstItem.Value);

            var secondItem = testResult.FirstOrDefault(x => x.Key == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.1D, secondItem.Value);

        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_TwoGivenDirectlyAndSumWhichGt()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //what about when the sumtotal is greater than the GivenDirectly's sum?
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 9000.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Other Consumer", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000.ToPecuniam() });
            
            testOptions.SumTotal = 12000.ToPecuniam(); //2000 more

            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //the given directly rates will equal their values over the sumtotal 
            var firstItem = testResult.FirstOrDefault(x => x.Key == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.75D, firstItem.Value);

            var secondItem = testResult.FirstOrDefault(x => x.Key == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.0833D, secondItem.Value);

            //and the remainder will be randomly allocated to one of the other items 
            var otherItems = testResult.Where(x => !(new[] {"Student", "Other Consumer"}.Contains(x.Key)))
                .Select(t => t.Value).Sum();
            Assert.AreEqual(0.1667D, otherItems);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_JustSumTotal()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //so what happens if I give a sumtotal and no GivenDirectly's - does it matter?
            testOptions.StartDate = DateTime.Today;

            testOptions.SumTotal = 10000.ToPecuniam();

            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //correct, it doesn't matter, since we are dealing in ratio's 
            // sumtotal gives us a denominator and GivenDirectly give us a numerator
            // without both there is nothing to do...
            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_UnmatchedNames()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //so will it blow up if GivenDirectly's names are not found?
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("NotFound", "Somewhere") { ExpectedValue = 9000.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("404", "Somewhere") { ExpectedValue = 1000.ToPecuniam() });

            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //no, it just ignores them
            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");

        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_GivenDirectlyValueOfZero()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //so how will it handle a case where GivenDirectly's are assigned zero
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = Pecuniam.Zero });
            testOptions.SumTotal = 12000.ToPecuniam(); 

            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //you'll get a randomized list less the one assigned directly to zero - it gets zero
            var testItem = testResult.FirstOrDefault(t => t.Key == "Student");
            Assert.IsNotNull(testItem);
            Assert.AreEqual(0.0D, testItem.Value);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        [ExpectedException(typeof(RahRowRagee))]
        public void TestGetDebtExpenseNames2RandRates_EverythingZeroOut()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testOptions = new OpesOptions();

            //how will it handle the case where I accidently zero'ed out everything?
            testOptions.StartDate = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Credit Card", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = Pecuniam.Zero });
            testOptions.GivenDirectly.Add(new Mereo("Health Care", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = Pecuniam.Zero });
            testOptions.GivenDirectly.Add(new Mereo("Other Consumer", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = Pecuniam.Zero });
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = Pecuniam.Zero });
            testOptions.GivenDirectly.Add(new Mereo("Tax", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = Pecuniam.Zero });
            testOptions.GivenDirectly.Add(new Mereo("Other", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = Pecuniam.Zero });

            //this is actually exceptional and so an exception is thrown
            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);
        }

        [TestMethod]
        public void TestGetHomeExpenseNames2RandomRates_WithAssign()
        {
            var options = new OpesOptions()
            {
                IsRenting = false,
                NumberOfVehicles = 1,
                IsVehiclePaidOff = false
            };
            options.ChildrenAges.Add(2);
            options.ChildrenAges.Add(4);
            var testSubject = new NorthAmericanIncome(null,options);
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
            var options = new OpesOptions()
            {
                IsRenting = false,
                NumberOfVehicles = 1,
                IsVehiclePaidOff = false
            };
            options.ChildrenAges.Add(2);
            options.ChildrenAges.Add(4);
            var testSubject = new NorthAmericanIncome(null, options);
            var testResult = testSubject.GetExpenseItemsForRange(55000.ToPecuniam(), DateTime.MinValue);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            var d = Pondus.GetExpectedSum(testResult);
            System.Diagnostics.Debug.WriteLine(d);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine($"{Math.Round(t.Value.ToDouble()/12D)} {t.My.GetName(KindsOfNames.Group)} {t.My.Name}");
        }


        [TestMethod]
        public void TestGetExpenseItemsForRange_WithDirectAssign()
        {
            var options = new OpesOptions()
            {
                IsRenting = false,
                NumberOfVehicles = 1,
                IsVehiclePaidOff = false
            };
            options.ChildrenAges.Add(2);
            options.ChildrenAges.Add(4);
            var testSubject = new NorthAmericanIncome(null, options);

            testSubject.MyOptions.GivenDirectly.Add(new Mereo("Mortgage", "Home") {ExpectedValue = 13476.0D.ToPecuniam() });
            testSubject.MyOptions.GivenDirectly.Add(new Mereo("Loan Payments", "Transportation") {ExpectedValue = 3750.0D.ToPecuniam()});
            testSubject.MyOptions.GivenDirectly.Add(new Mereo("Credit Card", "Debts") {ExpectedValue = 2728.44D.ToPecuniam()});

            var testResult = testSubject.GetExpenseItemsForRange(97860.0D.ToPecuniam(), DateTime.Today.AddYears(-1), null, Interval.Annually);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            var testPondus =
                testResult.FirstOrDefault(p => p.My.Name == "Mortgage" && p.My.GetName(KindsOfNames.Group) == "Home");
            Assert.IsNotNull(testPondus);
            System.Diagnostics.Debug.WriteLine(testPondus);
            var testAmt = testPondus.My.ExpectedValue;
            Assert.IsNotNull(testAmt);
            Assert.AreEqual(13476.0D, testAmt.ToDouble());

            testPondus = testResult.FirstOrDefault(p =>
                p.My.Name == "Loan Payments" && p.My.GetName(KindsOfNames.Group) == "Transportation");
            Assert.IsNotNull(testPondus);
            System.Diagnostics.Debug.WriteLine(testPondus);
            testAmt = testPondus.My.ExpectedValue;
            Assert.IsNotNull(testAmt);
            Assert.AreEqual(3750.0D, testAmt.ToDouble());

            testPondus = testResult.FirstOrDefault(p =>
                p.My.Name == "Credit Card" && p.My.GetName(KindsOfNames.Group) == "Debts");
            Assert.IsNotNull(testPondus);
            System.Diagnostics.Debug.WriteLine(testPondus);
            testAmt = testPondus.My.ExpectedValue;
            Assert.IsNotNull(testAmt);
            Assert.AreEqual(2728.44D, testAmt.ToDouble());
        }

        [TestMethod]
        public void TestGetIncomeYearsInDates()
        {
            var testSubject = new NorthAmericanIncome(null);
            var testResult = testSubject.GetYearsInDates(testSubject.GetYearNeg(-3));

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

            var testResult = WealthBase.ReassignRates(testInput00, testInput01);

            //test is gave something back
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //test that the reassignment worked
            Assert.IsTrue(testResult.ContainsKey("Item1"));
            Assert.AreEqual(0.33D, testResult["Item1"]);

            //test that the sum total is preserved
            var sum = testResult.Select(kv => kv.Value).Sum();
            Assert.IsTrue(sum >= 0.99D && sum <= 1.01D);


            //test it will zero out as requested
            var testInput02 = new List<Tuple<string, double>>
            {
                new Tuple<string, double>("Item1", 0.0D),
                new Tuple<string, double>("Item3", 0.0D)
            };

            testResult = WealthBase.ReassignRates(testInput00, testInput02);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey("Item1"));
            Assert.AreEqual(0.0D, testResult["Item1"]);

            Assert.IsTrue(testResult.ContainsKey("Item3"));
            Assert.AreEqual(0.0D, testResult["Item3"]);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();

            System.Diagnostics.Debug.WriteLine(testResultSum);
        }

        [TestMethod]
        public void TestExpectedOtherIncome()
        {
            var testSubject = new NorthAmericanIncome(null);
            testSubject.ResolveExpectedIncomeAndExpenses();

            var testResults = testSubject.ExpectedOtherIncome;
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

            var sumOfTestResults = Pondus.GetExpectedSum(testResults);

            Assert.AreNotEqual(Pecuniam.Zero, sumOfTestResults);

            var expectations = WealthBaseTests.GetExpectedNamesFromXml("income");

            //expect every item in income which is not the 'employment' group to be present
            foreach (var expect in expectations.Where(e => e.Item1 != "Employment"))
            {
                System.Diagnostics.Debug.WriteLine(expect);
                Assert.IsTrue(testResults.Any(x =>
                    x.My.Name == expect.Item2 && x.My.GetName(KindsOfNames.Group) == expect.Item1));
            }
        }

        [TestMethod]
        public void TestExpectedExpenses()
        {
            var testSubject = new NorthAmericanIncome(null);
            testSubject.ResolveExpectedIncomeAndExpenses();

            var testResults = testSubject.ExpectedExpenses;
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

            var sumOfTestResults = Pondus.GetExpectedSum(testResults);

            Assert.AreNotEqual(Pecuniam.Zero, sumOfTestResults);

            var expectations = WealthBaseTests.GetExpectedNamesFromXml("expense");

            //expect every item in income which is not the 'employment' group to be present
            foreach (var expect in expectations)
            {
                System.Diagnostics.Debug.WriteLine(expect);
                Assert.IsTrue(testResults.Any(x =>
                    x.My.Name == expect.Item2 && x.My.GetName(KindsOfNames.Group) == expect.Item1));
            }
        }
    }
}
