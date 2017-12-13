using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanExpenseTests
    {
        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_NoOptions()
        {
            //what happens if you just invoke it with no options whatsoever?
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
            var otherItems = testResult.Where(x => !(new[] { "Student", "Other Consumer" }.Contains(x.Key)))
                .Select(t => t.Value).Sum();
            Assert.AreEqual(0.1667D, otherItems);

            foreach (var u in testResult)
                System.Diagnostics.Debug.WriteLine($"{u.Key} -> {u.Value}");
        }

        [TestMethod]
        public void TestGetDebtExpenseNames2RandRates_JustSumTotal()
        {
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
            var testSubject = new NorthAmericanExpenses(null);
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
    }
}
