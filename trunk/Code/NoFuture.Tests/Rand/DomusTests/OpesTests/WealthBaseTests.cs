using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class WealthBaseTests
    {

        [TestMethod]
        public void TestGetFactor()
        {
            var testResult = AmericanFactors.GetFactor(FactorTables.HomeDebt,
                (OccidentalEdu.Bachelor | OccidentalEdu.Grad), NorthAmericanRace.Asian, AmericanRegion.West, 38,
                Gender.Male, MaritialStatus.Single);
            Assert.AreNotEqual(0.0D, testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetFactorBaseValue()
        {
            var testResult = AmericanFactors.GetFactorBaseValue(FactorTables.VehicleDebt);
            Assert.AreEqual(10000.0D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.HomeEquity);
            Assert.AreEqual(81000.0D, testResult);
        }

        [TestMethod]
        public void TestGetPaycheck()
        {
            var amer = new American(UsState.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new AmericanIncome(new OpesOptions { IsRenting = true, Gender = Gender.Female, BirthDate = UsState.GetWorkingAdultBirthDate() });
            System.Diagnostics.Debug.WriteLine(string.Join(" ", amer.Age, amer.MaritialStatus, amer.Education, amer.Race));

            var testResult = testSubject.GetRandomYearlyIncome(null, 1.0.ToPecuniam());
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Amount > 0.0M);
        }

        [TestMethod]
        public void TestGetXmlEduName()
        {
            var testResult = AmericanFactors.GetXmlEduName(OccidentalEdu.Bachelor);
            Assert.AreEqual("Associate",testResult);
            testResult = AmericanFactors.GetXmlEduName(OccidentalEdu.Bachelor | OccidentalEdu.Grad);
            Assert.AreEqual("Bachelor", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }



        [TestMethod]
        public void TestGetIncomeItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetIncomeItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach(var i in testResult)
                System.Diagnostics.Debug.WriteLine($"{i.Name} {i.GetName(KindsOfNames.Group)}");
        }

        [TestMethod]
        public void TestGetDeductionItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetDeductionItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var i in testResult)
                System.Diagnostics.Debug.WriteLine(i);
        }

        [TestMethod]
        public void TestGetExpenseItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetExpenseItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var i in testResult)
                System.Diagnostics.Debug.WriteLine(i);
        }

        [TestMethod]
        public void TestGetAssetItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetAssetItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var i in testResult)
                System.Diagnostics.Debug.WriteLine(i);
        }

        [TestMethod]
        public void TestGetRandomRateFromClassicHook()
        {
            var testSubject = new AmericanIncome(null);
            var testResult = testSubject.GetRandomRateFromClassicHook();

            Assert.IsTrue(testResult >= 0D);

            testResult = testSubject.GetRandomRateFromClassicHook(69);
            Assert.IsTrue(testResult > 0D);
            System.Diagnostics.Debug.WriteLine(testResult);

        }

        [TestMethod]
        public void TestGetYearNeg()
        {
            var testSubject = new AmericanIncome(null);
            var testResult = testSubject.GetYearNeg(-3);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetGroupNames2Portions()
        {
            var testInput = new OpesOptions();
            testInput.GivenDirectly.Add(new Mereo("Real Property"){ExpectedValue = 7800.ToPecuniam()});
            testInput.GivenDirectly.Add(new Mereo("Securities"){ExpectedValue = 1000.ToPecuniam()});
            testInput.SumTotal = 12000.ToPecuniam();
            var testSubject = new AmericanAssets(testInput);
            var testResult = testSubject.GetGroupNames2Portions(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(i => i.Item2).Sum();
            Assert.IsTrue(Math.Round(testResultSum) == 1.0D);

            var testResult00 = testResult.FirstOrDefault(k => k.Item1 == "Real Property");
            Assert.IsNotNull(testResult00);
            var testResult01 = testResult.FirstOrDefault(k => k.Item1 == "Securities");
            Assert.IsNotNull(testResult01);

            foreach (var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);

            Assert.AreEqual(Math.Round(7800.0D/12000, 3), Math.Round(testResult00.Item2,3));
            Assert.AreEqual(Math.Round(1000.0D / 12000, 3), Math.Round(testResult01.Item2, 3));


        }

        [TestMethod]
        public void TestGetItemNames2Portions()
        {
            var testInput = new OpesOptions();
            var grpName = "Institutional";
            testInput.GivenDirectly.Add(new Mereo("Partnerships", grpName) { ExpectedValue = 7800.ToPecuniam() });
            testInput.GivenDirectly.Add(new Mereo("Fellowships", grpName) { ExpectedValue = 1000.ToPecuniam() });
            testInput.GivenDirectly.Add(new Mereo("Annuity", grpName) { ExpectedValue = 1000.ToPecuniam() });
            testInput.SumTotal = 15000.ToPecuniam();

            var testSubject = new AmericanIncome(testInput);

            var testResults =
                testSubject.GetItemNames2Portions(grpName, testInput);

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

            var testResultSum = testResults.Select(kv => kv.Item2).Sum();
            System.Diagnostics.Debug.WriteLine(testResultSum);
            Assert.IsTrue(Math.Round(testResultSum) == 1.0D);

            //expect that when SumTotal is unassigned the ratios align exactly with assigned values
            testInput = new OpesOptions();
            testInput.GivenDirectly.Add(new Mereo("Partnerships", grpName) { ExpectedValue = 7800.ToPecuniam() });
            testInput.GivenDirectly.Add(new Mereo("Fellowships", grpName) { ExpectedValue = 1000.ToPecuniam() });
            testInput.GivenDirectly.Add(new Mereo("Annuity", grpName) { ExpectedValue = 1000.ToPecuniam() });
            testSubject = new AmericanIncome(testInput);
            testResults =
                testSubject.GetItemNames2Portions(grpName, testInput);

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

            testResultSum = testResults.Select(kv => kv.Item2).Sum();
            System.Diagnostics.Debug.WriteLine(testResultSum);

            var trWages = testResults.FirstOrDefault(kv => kv.Item1 == "Partnerships");
            var trOt = testResults.FirstOrDefault(kv => kv.Item1 == "Fellowships");
            var trBonus = testResults.FirstOrDefault(kv => kv.Item1 == "Annuity");

            Assert.IsNotNull(trWages);
            Assert.IsNotNull(trOt);
            Assert.IsNotNull(trBonus);

            Assert.AreEqual(0.796D, Math.Round(trWages.Item2, 3));
            Assert.AreEqual(0.102D, Math.Round(trOt.Item2,3));
            Assert.AreEqual(0.102D, Math.Round(trBonus.Item2, 3));

            foreach (var tr in testResults)
                System.Diagnostics.Debug.WriteLine(tr);
        }

        public static List<Tuple<string, string>> GetExpectedNamesFromXml(string sectionName)
        {
            var grpsAndNames = new List<Tuple<string, string>>();
            var srcXml = WealthBase.UsDomusOpesData;
            var incomeNode = srcXml.SelectSingleNode($"//{sectionName}");

            foreach (var grpNode in incomeNode.ChildNodes)
            {
                var grpElem = grpNode as XmlElement;
                if (grpElem == null)
                    continue;
                var grpName = grpElem.GetAttribute("name");
                foreach (var mereoNode in grpElem.ChildNodes)
                {
                    var mereoElem = mereoNode as XmlElement;
                    if (mereoElem == null)
                        continue;
                    var name = mereoElem.GetAttribute("name");
                    grpsAndNames.Add(new Tuple<string, string>(grpName, name));
                }
            }
            return grpsAndNames;
        }
    }
}
