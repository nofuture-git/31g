using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.Opes.US;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Org;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestFixture]
    public class AmericanIncomeTests
    {
        [Test]
        public void TestCtor()
        {
            var testSubject = new AmericanIncome();
            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreEqual(0, testSubject.MyItems.Count);

            testSubject.AddItem("Lottery","Subito", 10000D.ToPecuniam());
            var testResultSum = testSubject.TotalAnnualExpectedIncome;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(10000D.ToPecuniam(), testResultSum);

            testSubject.AddItem("something else", "Subitio", 900D.ToPecuniam());
            testResultSum = testSubject.TotalAnnualExpectedIncome;
            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(10900D.ToPecuniam(), testResultSum);
        }

        [Test]
        public void TestGetEmploymentRanges()
        {
            var testSubject = new AmericanIncome();
            var testResult = testSubject.GetEmploymentRanges(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            Assert.IsNotNull(testResult[0]?.Item1);

        }

        [Test]
        public void TestResolveEmployment()
        {
            var testSubject = new AmericanIncome();
            var testResult = testSubject.GetRandomEmployment(null, null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
        }

        [Test]
        public void TestGetRandomIncomeAmount()
        {
            //no args test
            var testSubject = new AmericanIncome();
            var testResult = testSubject.GetRandomExpectedIncomeAmount(null);

            Assert.IsNotNull(testResult);
            Assert.IsFalse(testResult == Pecuniam.Zero);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [Test]
        public void TestGetIncomeYearsInDates()
        {
            var testSubject = new AmericanIncome();
            var testResult = testSubject.GetYearsInDates(testSubject.GetYearNeg(-3));

            Assert.IsNotNull(testResult);
            Assert.AreEqual(4, testResult.Count);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine(t);
        }

        [Test]
        public void TestGetJudgementIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome();
            
            var testResult = testSubject.GetJudgmentIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach(var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [Test]
        public void TestGetSubitoIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome();

            var testResult = testSubject.GetSubitoIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [Test]
        public void TestGetRealPropertyIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome();

            var testResult = testSubject.GetRealPropertyIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [Test]
        public void TestGetSecuritiesIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome();

            var testResult = testSubject.GetSecuritiesIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [Test]
        public void TestGetInstitutionalIncomeNames2RandomRates()
        {
            var testSubject = new AmericanIncome();

            var testResult = testSubject.GetInstitutionalIncomeNames2RandomRates(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [Test]
        public void TestGetGroupNames()
        {
            var testNames = WealthBase.GetGroupNames(WealthBase.DomusOpesDivisions.Income);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("income");
            var expectations = allNames.Select(n => n.Item1).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn, StringComparison.OrdinalIgnoreCase)));
                System.Diagnostics.Debug.WriteLine(tn);
            }
        }

        [Test]
        public void TestGetItemNames()
        {
            var testNames = WealthBase.GetItemNames(WealthBase.DomusOpesDivisions.Income);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("income");
            var expectations = allNames.Select(n => n.Item2).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn.Name, StringComparison.OrdinalIgnoreCase)));
                System.Diagnostics.Debug.WriteLine(tn);
            }
        }

        [Test]
        public void TestResolveItems()
        {
            var testSubject = new AmericanIncome();

            testSubject.RandomizeAllItems(null);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach(var item in testSubject.MyItems)
                System.Diagnostics.Debug.WriteLine(item);
        }
        [Test]
        public void TestRandomIncome()
        {
            var testSubject = AmericanIncome.RandomIncome(new OpesOptions { Inception = new DateTime(DateTime.Today.Year, 1, 1) });
            Assert.IsNotNull(testSubject);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);
        }

        [Test]
        public void TestAddItems()
        {
            var testSubject = new AmericanIncome();

            var testResult = testSubject.TotalAnnualExpectedGrossEmploymentIncome;
            Assert.AreEqual(Pecuniam.Zero, testResult);
            testResult = testSubject.TotalAnnualExpectedNetEmploymentIncome;
            Assert.AreEqual(Pecuniam.Zero, testResult);
            testResult = testSubject.TotalAnnualExpectedIncome;
            Assert.AreEqual(Pecuniam.Zero, testResult);

            testSubject.AddItem("stocks", "securities", 9000.0D.ToPecuniam());

            testResult = testSubject.TotalAnnualExpectedGrossEmploymentIncome;
            Assert.AreEqual(Pecuniam.Zero, testResult);
            testResult = testSubject.TotalAnnualExpectedNetEmploymentIncome;
            Assert.AreEqual(Pecuniam.Zero, testResult);
            testResult = testSubject.TotalAnnualExpectedIncome;
            Assert.AreEqual(9000.0D.ToPecuniam(), testResult);

            testSubject.AddItem("savings", "Banks", 600.0D.ToPecuniam());

            testResult = testSubject.TotalAnnualExpectedGrossEmploymentIncome;
            Assert.AreEqual(Pecuniam.Zero, testResult);
            testResult = testSubject.TotalAnnualExpectedNetEmploymentIncome;
            Assert.AreEqual(Pecuniam.Zero, testResult);
            testResult = testSubject.TotalAnnualExpectedIncome;
            Assert.AreEqual(9600.0D.ToPecuniam(), testResult);

            var testEmployment = new AmericanEmployment();
            var occ = new SocDetailedOccupation { Value = "Accountant" };
            testEmployment.Occupation = occ;
            testEmployment.AddItem("Salary", 55000.0);

            testSubject.AddEmployment(testEmployment);
            testResult = testSubject.TotalAnnualExpectedGrossEmploymentIncome;
            Assert.AreEqual(55000.0.ToPecuniam(), testResult);
            testResult = testSubject.TotalAnnualExpectedNetEmploymentIncome;
            Assert.AreEqual(55000.0.ToPecuniam(), testResult);
            testResult = testSubject.TotalAnnualExpectedIncome;
            Assert.AreEqual((55000.0 + 9600.0D).ToPecuniam(), testResult);

            var tax = new AmericanDeductions(testEmployment);
            testEmployment.Deductions = tax;

            var fedTax = 55000.0D * AmericanEquations.FederalIncomeTaxRate.SolveForY(55000.0);
            tax.AddItem("Federal", fedTax);

            testResult = testSubject.TotalAnnualExpectedGrossEmploymentIncome;
            Assert.AreEqual(55000.0.ToPecuniam(), testResult);
            testResult = testSubject.TotalAnnualExpectedNetEmploymentIncome;
            Assert.AreEqual((55000.0 - fedTax).ToPecuniam(), testResult);
            testResult = testSubject.TotalAnnualExpectedIncome;
            Assert.AreEqual((55000.0 + 9600.0D - fedTax).ToPecuniam(), testResult);

        }
    }
}
