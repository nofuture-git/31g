using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class AmericanExpenseTests
    {
        #region FAQ

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_NoOptions()
        {
            //what happens if you just invoke it with no options whatsoever?
            var testSubject = new AmericanExpenses(null);
            var testResult = testSubject.GetDebtExpenseNames2RandomRates((OpesOptions)null);

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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();
            testOptions.Inception = DateTime.Today;
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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //so if we add another then their assigned rates will be their portion of the whole?
            testOptions.Inception = DateTime.Today;
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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //so now what happens if we do give a SumTotal which happens to exactly equal the GivenDirectly's sum?
            testOptions.Inception = DateTime.Today;
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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //so what happens if the sumtotal is actually less than the sum of the GivenDirectly's sum?
            testOptions.Inception = DateTime.Today;
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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //what about when the sumtotal is greater than the GivenDirectly's sum?
            testOptions.Inception = DateTime.Today;
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
            var otherItems = testResult.Where(x => !(new[] { "Student", "Other Consumer" }.Contains(x.Key)))
                .Select(t => t.Value).Sum();
            Assert.IsTrue(0.1667D == otherItems || 0.1666D == otherItems);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_JustSumTotal()
        {
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //so what happens if I give a sumtotal and no GivenDirectly's - does it matter?
            testOptions.Inception = DateTime.Today;

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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //so will it blow up if GivenDirectly's names are not found?
            testOptions.Inception = DateTime.Today;
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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //so how will it handle a case where GivenDirectly's are assigned zero
            testOptions.Inception = DateTime.Today;
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
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //how will it handle the case where I accidently zero'ed out everything?
            testOptions.Inception = DateTime.Today;
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
        public void TestGetDebtExpenseNames2RandRates_SumTotalIsZero()
        {
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //so what is going to happen if the only thing I give is a SumTotal of zero?
            testOptions.Inception = DateTime.Today;
            testOptions.SumTotal = Pecuniam.Zero;

            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            //nothing - its just the random ratios since the SumTotal is zero by default
            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_GivenDirectlyOverlapZeroOuts()
        {
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //how do the PossiableZero outs play with explict values on GivenDirectly?
            testOptions.Inception = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Credit Card", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000D.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Health Care", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000D.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Other Consumer", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000D.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Student", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000D.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Tax", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000D.ToPecuniam() });
            testOptions.GivenDirectly.Add(new Mereo("Other", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000D.ToPecuniam() });

            testOptions.PossibleZeroOuts.AddRange(new []{ "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });

            //the PossiableZeroOuts are only considered when they are not present in the GivenDirectly 
            // so the results are the same as if PossiableZeroOuts had nothing in it at all
            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_SumTotalExceedsAndZeroOuts()
        {
            var testSubject = new AmericanExpenses(null);
            var testOptions = new OpesOptions();

            //what if the SumTotal exceeds the GivenDirectly's sum but all the other options are present in the PossiablyZeroOut's?
            // and it just so happens that they all, in fact do, get selected to be zero'ed out
            testOptions.Inception = DateTime.Today;
            testOptions.GivenDirectly.Add(new Mereo("Credit Card", WealthBase.ExpenseGroupNames.DEBT) { ExpectedValue = 1000D.ToPecuniam() });
            testOptions.DiceRoll = (i, dice) => true;
            testOptions.PossibleZeroOuts.AddRange(new[] { "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            testOptions.SumTotal = 2000D.ToPecuniam(); //1000 above

            //it leaves one to receive the excess - in effect forcing the dice role to be false for at least one item in this case
            var testResult = testSubject.GetDebtExpenseNames2RandomRates(testOptions);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        #endregion

        [TestMethod]
        public void TestGetHomeExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses(null);

            var testResult = testSubject.GetHomeExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            System.Diagnostics.Debug.WriteLine("Owns");
            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);

            testResult = testSubject.GetHomeExpenseNames2RandomRates(new OpesOptions(){IsRenting = true});

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            System.Diagnostics.Debug.WriteLine("Rents");
            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetUtilityExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses(null);

            var testResult = testSubject.GetUtilityExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetTransportationExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses(null);

            var testResult = testSubject.GetTransportationExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));
            System.Diagnostics.Debug.WriteLine("No Car");
            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);

            Assert.IsTrue(testResult.ContainsKey("Public Transportation"));
            Assert.AreEqual(1D, Math.Round(testResult["Public Transportation"]));

            testResult = testSubject.GetTransportationExpenseNames2RandomRates(new OpesOptions{NumberOfVehicles = 1});

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsTrue(testResult.ContainsKey("Public Transportation"));
            Assert.AreEqual(0D, testResult["Public Transportation"]);

            System.Diagnostics.Debug.WriteLine("Has Car");
            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetInsuranceExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses(null);

            var testResult = testSubject.GetInsuranceExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsTrue(testResult.ContainsKey("Renters"));
            Assert.AreEqual(0D, Math.Round(testResult["Renters"]));

            System.Diagnostics.Debug.WriteLine("Owns");
            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);

            testResult = testSubject.GetInsuranceExpenseNames2RandomRates(new OpesOptions{IsRenting = true});

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsTrue(testResult.ContainsKey("Renters"));
            Assert.IsTrue(testResult["Renters"] > 0.0D);

            System.Diagnostics.Debug.WriteLine("Rents");
            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetPersonalExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses(null);

            var testResult = testSubject.GetPersonalExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }


        [TestMethod]
        public void TestGetChildrenExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses(null);

            var testResult = testSubject.GetChildrenExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetHealthExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses(null);

            var testResult = testSubject.GetHealthExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }


        [TestMethod]
        public void TestGetGroupNames()
        {
            var testNames = WealthBase.GetGroupNames(WealthBase.DomusOpesDivisions.Expense);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("expense");
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
            var testNames = WealthBase.GetItemNames(WealthBase.DomusOpesDivisions.Expense);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("expense");
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
            var testOptions = new OpesOptions {SumTotal = 10000.0D.ToPecuniam()};
            var testSubject = new AmericanExpenses(testOptions);

            testSubject.ResolveItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach (var item in testSubject.MyItems)
                System.Diagnostics.Debug.WriteLine(item);
        }
    }
}
