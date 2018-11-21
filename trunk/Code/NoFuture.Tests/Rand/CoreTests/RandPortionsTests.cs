using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Opes;
using NoFuture.Shared.Core;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.CoreTests
{
    [TestFixture]
    public class RandPortionsTests
    {
        [Test]
        public void TestGetNames2Portions_NoOptions()
        {
            var testSubject = new RandPortions();
            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);
            //then its truely random
            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }

        [Test]
        public void TestGetNames2Portions_SingleGivenDirectly()
        {
            var testSubject = new RandPortions();
            testSubject.AddGivenDirectly("Student", "Debts", 9000);
            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);
            var singleItem = testResult.FirstOrDefault(x => x.Item1 == "Student");
            Assert.IsNotNull(singleItem);
            Assert.AreEqual(1D, singleItem.Item2);
        }

        [Test]
        public void TestGetNames2Portions_TwoGivenDirectly()
        {
            var testSubject = new RandPortions();
            testSubject.AddGivenDirectly("Student", "Debts", 9000);
            testSubject.AddGivenDirectly("Other Consumer", "Debts", 1000);
            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);

            //yes, since no SumTotal was given these are the only two to divide the whole on
            var firstItem = testResult.FirstOrDefault(x => x.Item1 == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.9D, firstItem.Item2);

            var secondItem = testResult.FirstOrDefault(x => x.Item1 == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.1D, secondItem.Item2);

        }

        [Test]
        public void TestGetNames2Portions_TwoGivenDirectlyAndSumWhichEquals()
        {
            var testSubject = new RandPortions();

            //so now what happens if we do give a SumTotal which happens to exactly equal the GivenDirectly's sum?
            testSubject.AddGivenDirectly("Student", "Debts", 9000);
            testSubject.AddGivenDirectly("Other Consumer", "Debts", 1000);

            testSubject.SumTotal = 10000D;

            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);

            //nothing changes, assigning the sumtotal as the actual sum doesn't change anything
            var firstItem = testResult.FirstOrDefault(x => x.Item1 == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.9D, firstItem.Item2);

            var secondItem = testResult.FirstOrDefault(x => x.Item1 == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.1D, secondItem.Item2);
        }

        [Test]
        public void TestGetNames2Portions_TwoGivenDirectlyAndSumWhichLt()
        {
            var testSubject = new RandPortions();

            //so what happens if the sumtotal is actually less than the sum of the GivenDirectly's sum?
            testSubject.AddGivenDirectly("Student", "Debts", 9000);
            testSubject.AddGivenDirectly("Other Consumer", "Debts", 1000);

            testSubject.SumTotal = 9000D; //1000 less

            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);

            //the assigned sumtotal is ignored and replaced with the GivenDirectly's sum to make everything fit.
            var firstItem = testResult.FirstOrDefault(x => x.Item1 == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.9D, firstItem.Item2);

            var secondItem = testResult.FirstOrDefault(x => x.Item1 == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.1D, secondItem.Item2);

        }

        [Test]
        public void TestGetNames2Portions_TwoGivenDirectlyAndSumWhichGt()
        {
            var testSubject = new RandPortions();

            //what about when the sumtotal is greater than the GivenDirectly's sum?
            testSubject.AddGivenDirectly("Student", "Debts", 9000);
            testSubject.AddGivenDirectly("Other Consumer", "Debts", 1000);

            testSubject.SumTotal = 12000D; //2000 more

            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);

            //the given directly rates will equal their values over the sumtotal 
            var firstItem = testResult.FirstOrDefault(x => x.Item1 == "Student");
            Assert.IsNotNull(firstItem);
            Assert.AreEqual(0.75D, firstItem.Item2);

            var secondItem = testResult.FirstOrDefault(x => x.Item1 == "Other Consumer");
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(0.0833D, secondItem.Item2);

            //and the remainder will be randomly allocated to one of the other items 
            var otherItems = testResult.Where(x => !(new[] { "Student", "Other Consumer" }.Contains(x.Item1)))
                .Select(t => t.Item2).Sum();
            Assert.IsTrue(otherItems < 0.17D && otherItems > 0.15D);

            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }

        [Test]
        public void TestGetNames2Portions_JustSumTotal()
        {
            var testSubject = new RandPortions();

            //so what happens if I give a sumtotal and no GivenDirectly's - does it matter?
            testSubject.SumTotal = 10000;

            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);

            //correct, it doesn't matter, since we are dealing in ratio's 
            // sumtotal gives us a denominator and GivenDirectly give us a numerator
            // without both there is nothing to do...
            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }

        [Test]
        public void TestGetNames2Portions_UnmatchedNames()
        {
            var testSubject = new RandPortions();

            //so will it blow up if GivenDirectly's names are not found?
            testSubject.AddGivenDirectly("NotFound", "Somewhere", 9000);
            testSubject.AddGivenDirectly("404", "Somewhere", 1000);

            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);

            //no, it just ignores them
            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }

        [Test]
        public void TestGetNames2Portions_GivenDirectlyValueOfZero()
        {
            var testSubject = new RandPortions();

            //so how will it handle a case where GivenDirectly's are assigned zero
            testSubject.AddGivenDirectly("Student", "Debts");
            testSubject.SumTotal = 12000D;

            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //you'll get a randomized list less the one assigned directly to zero - it gets zero
            var testItem = testResult.FirstOrDefault(t => t.Item1 == "Student");
            Assert.IsNotNull(testItem);
            Assert.AreEqual(0.0D, testItem.Item2);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.001);
            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }

        [Test]
        public void TestGetNames2Portions_EverythingZeroOut()
        {
            var testSubject = new RandPortions();

            //how will it handle the case where I accidently zero'ed out everything?
            testSubject.AddGivenDirectly("Credit Card", "Debts");
            testSubject.AddGivenDirectly("Health Care", "Debts");
            testSubject.AddGivenDirectly("Other Consumer", "Debts");
            testSubject.AddGivenDirectly("Student", "Debts");
            testSubject.AddGivenDirectly("Tax", "Debts");
            testSubject.AddGivenDirectly("Other", "Debts");

            //this is actually exceptional and so an exception is thrown
            Assert.Throws<WatDaFookIzDis>(() => testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" }));
        }

        [Test]
        public void TestGetNames2Portions_SumTotalIsZero()
        {
            var testSubject = new RandPortions();

            //so what is going to happen if the only thing I give is a SumTotal of zero?
            testSubject.SumTotal = 0D;

            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);
            //nothing - its just the random ratios since the SumTotal is zero by default
            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }

        [Test]
        public void TestGetNames2Portions_GivenDirectlyOverlapZeroOuts()
        {
            var testSubject = new RandPortions();

            //how do the PossiableZero outs play with explict values on GivenDirectly?
            testSubject.AddGivenDirectly("Credit Card", "Debts", 1000D);
            testSubject.AddGivenDirectly("Health Care", "Debts", 1000D);
            testSubject.AddGivenDirectly("Other Consumer", "Debts", 1000D);
            testSubject.AddGivenDirectly("Student", "Debts", 1000D);
            testSubject.AddGivenDirectly("Tax", "Debts", 1000D);
            testSubject.AddGivenDirectly("Other", "Debts", 1000D);

            testSubject.PossibleZeroOuts.AddRange(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });

            //the PossiableZeroOuts are only considered when they are not present in the GivenDirectly 
            // so the results are the same as if PossiableZeroOuts had nothing in it at all
            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);
            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }

        [Test]
        public void TestGetNames2Portions_SumTotalExceedsAndZeroOuts()
        {
            var testSubject = new RandPortions();

            //what if the SumTotal exceeds the GivenDirectly's sum but all the other options are present in the PossiablyZeroOut's?
            // and it just so happens that they all, in fact do, get selected to be zero'ed out
            testSubject.AddGivenDirectly("Credit Card", "Debts", 1000D);
            testSubject.DiceRoll = (i, dice) => true;
            testSubject.PossibleZeroOuts.AddRange(new[] { "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            testSubject.SumTotal = 2000D; //1000 above

            //it leaves one to receive the excess - in effect forcing the dice role to be false for at least one item in this case
            var testResult =
                testSubject.GetNames2Portions(new[] { "Credit Card", "Health Care", "Other Consumer", "Student", "Tax", "Other" });
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultSum = testResult.Sum(x => x.Item2);
            Console.WriteLine($"Sum of portions {testResultSum}");
            Assert.IsTrue(Math.Abs(1D - testResultSum) < 0.0001);

            foreach (var u in testResult)
                Console.WriteLine($"{u.Item1} -> {u.Item2}");
        }
    }
}
