using System;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Opes;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.OpesTests
{
    [TestFixture]
    public class AmericanExpenseTests
    {
        [Test]
        public void TestCtor()
        {
            var testSubject = new AmericanExpenses();
            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreEqual(0, testSubject.MyItems.Count);

            testSubject.AddItem("Gas", "Utility", 65D.ToPecuniam());
            var testResultSum = testSubject.Total;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(65D.ToPecuniam().GetNeg(), testResultSum);

            testSubject.AddItem("Groceries", "Personal", 600D.ToPecuniam());
            testResultSum = testSubject.Total;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(665D.ToPecuniam().GetNeg(), testResultSum);

        }

        [Test]
        public void TestGetHomeExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses();

            var testResult = testSubject.GetHomeExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Console.WriteLine("Owns");
            foreach (var rate in testResult)
                Console.WriteLine(rate);

            testResult = testSubject.GetHomeExpenseNames2RandomRates(new DomusOpesOptions(){IsRenting = true});

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Console.WriteLine("Rents");
            foreach (var rate in testResult)
                Console.WriteLine(rate);
        }

        [Test]
        public void TestGetUtilityExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses();

            var testResult = testSubject.GetUtilityExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                Console.WriteLine(rate);
        }

        [Test]
        public void TestGetTransportationExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses();

            var testResult = testSubject.GetTransportationExpenseNames2RandomRates(new DomusOpesOptions(){NumberOfVehicles = 0});

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));
            Console.WriteLine("No Car");
            foreach (var rate in testResult)
                Console.WriteLine(rate);

            Assert.IsTrue(testResult.ContainsKey("Public Transportation"));
            Assert.AreEqual(1D, Math.Round(testResult["Public Transportation"]));

            testResult = testSubject.GetTransportationExpenseNames2RandomRates(new DomusOpesOptions{NumberOfVehicles = 1});

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsTrue(testResult.ContainsKey("Public Transportation"));
            Assert.AreEqual(0D, testResult["Public Transportation"]);

            Console.WriteLine("Has Car");
            foreach (var rate in testResult)
                Console.WriteLine(rate);
        }

        [Test]
        public void TestGetInsuranceExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses();

            var testResult = testSubject.GetInsuranceExpenseNames2RandomRates(new DomusOpesOptions { IsRenting = false });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsTrue(testResult.ContainsKey("Renters"));
            Assert.AreEqual(0D, Math.Round(testResult["Renters"]));

            Console.WriteLine("Owns");
            foreach (var rate in testResult)
                Console.WriteLine(rate);

            testResult = testSubject.GetInsuranceExpenseNames2RandomRates(new DomusOpesOptions{IsRenting = true});

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            Assert.IsTrue(testResult.ContainsKey("Renters"));
            Assert.IsTrue(testResult["Renters"] > 0.0D);

            Console.WriteLine("Rents");
            foreach (var rate in testResult)
                Console.WriteLine(rate);
        }

        [Test]
        public void TestGetPersonalExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses();

            var testResult = testSubject.GetPersonalExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                Console.WriteLine(rate);
        }


        [Test]
        public void TestGetChildrenExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses();

            var testResult = testSubject.GetChildrenExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                Console.WriteLine(rate);
        }

        [Test]
        public void TestGetHealthExpenseNames2RandomRates()
        {
            var testSubject = new AmericanExpenses();

            var testResult = testSubject.GetHealthExpenseNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                Console.WriteLine(rate);
        }


        [Test]
        public void TestGetGroupNames()
        {
            var testSubject = new AmericanExpenses();
            var testNames = testSubject.GetGroupNames();
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("expense");
            var expectations = allNames.Select(n => n.Item1).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn, StringComparison.OrdinalIgnoreCase)));
                Console.WriteLine(tn);
            }
        }

        [Test]
        public void TestGetItemNames()
        {
            var testSubject = new AmericanExpenses();
            var testNames = testSubject.GetItemNames();
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("expense");
            var expectations = allNames.Select(n => n.Item2).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn.Name, StringComparison.OrdinalIgnoreCase)));
                Console.WriteLine(tn);
            }
        }

        [Test]
        public void TestResolveItems()
        {
            var testOptions = new DomusOpesOptions {SumTotal = 10000.0D };
            var testSubject = new AmericanExpenses();

            testSubject.RandomizeAllItems(testOptions);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach (var item in testSubject.MyItems)
                Console.WriteLine(item);
        }

        [Test]
        public void TestRandomExpenses()
        {
            var testSubject = AmericanExpenses.RandomExpenses(new DomusOpesOptions { Inception = new DateTime(DateTime.Today.Year, 1, 1) });
            Assert.IsNotNull(testSubject);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = AmericanExpenses.RandomExpenses();
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
