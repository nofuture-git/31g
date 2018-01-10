using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Gov.Fed;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class XRefTests
    {

        [TestMethod]
        public void TestXref()
        {
            var testResults = XRefGroup.AllXrefGroups;
            Assert.IsNotNull(testResults);

            Assert.AreNotEqual(0, testResults.Length);

            foreach (var t in testResults)
            {
                Assert.IsNotNull(t.XRefData);

                foreach (var dc in t.XRefData)
                {
                    Assert.AreNotEqual(0, dc.ReferenceDictionary.Count);
                    Assert.IsNotNull(dc.XrefIds);
                    Assert.AreNotEqual(0, dc.XrefIds.Length);

                    foreach (var dataFileXred in dc.XrefIds)
                    {
                        Assert.IsFalse(string.IsNullOrWhiteSpace(dataFileXred.LocalName));
                        Assert.IsFalse(string.IsNullOrWhiteSpace(dataFileXred.Value));
                    }
                }
            }
        }

        [TestMethod]
        public void TestXrefOnTypes()
        {

            var testTarget = new Bank();
            testTarget.UpsertName(KindsOfNames.Legal, "JPMorgan Chase Bank, N.A.");
            testTarget.UpsertName(KindsOfNames.Abbrev, "JPMORGAN CHASE BK NA");
            testTarget.Rssd = new ResearchStatisticsSupervisionDiscount {Value = "852218" };


            Assert.IsNotNull(testTarget);

            //verify the properties have no value prior to test
            Assert.IsNull(testTarget.CIK);
            Assert.IsTrue(!testTarget.TickerSymbols.Any());

            testTarget.LoadXrefXmlData();

            Assert.IsNotNull(testTarget.CIK);
            Assert.AreNotEqual(0, testTarget.TickerSymbols);

            System.Diagnostics.Debug.WriteLine(testTarget.CIK.ToString());
            System.Diagnostics.Debug.WriteLine(testTarget.TickerSymbols[0].Symbol);
            System.Diagnostics.Debug.WriteLine(testTarget.TickerSymbols[0].Exchange);
            System.Diagnostics.Debug.WriteLine(testTarget.SIC.ToString());

            //test nothing found - no problems and no change
            testTarget = new Bank();
            testTarget.UpsertName(KindsOfNames.Legal, "Pacific Western Bank");
            testTarget.UpsertName(KindsOfNames.Abbrev, "PACIFIC WESTERN BK");
            testTarget.Rssd = new ResearchStatisticsSupervisionDiscount { Value = "494261" };

            Assert.IsNotNull(testTarget);
            Assert.IsNull(testTarget.CIK);
            Assert.IsTrue(!testTarget.TickerSymbols.Any());

            testTarget.LoadXrefXmlData();

            Assert.IsNull(testTarget.CIK);
            Assert.IsTrue(!testTarget.TickerSymbols.Any());
        }
        [TestMethod]
        public void TestAddXrefValues()
        {
            var testXrefId = new Tuple<Type, string>(typeof(NoFuture.Rand.Com.Bank), "BANK OF AMER NA");
            var testValues = new RoutingTransitNumber {Value = "000015421"};
            var testResult = XRefGroup.AddXrefValues(testXrefId,testValues, "RoutingNumber");
            Assert.IsTrue(testResult);

            testXrefId = new Tuple<Type, string>(typeof(NoFuture.Rand.Com.Bank), "A new bank entry");
            testValues = new RoutingTransitNumber { Value = "787454541" };
            testResult = XRefGroup.AddXrefValues(testXrefId, testValues, "RoutingNumber");
            Assert.IsTrue(testResult);

        }
    }
}
