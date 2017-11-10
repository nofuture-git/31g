﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Exo.NfText;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Util.Core;

namespace NoFuture.Tests.Rand
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
            var testInput = FedLrgBnk.CommercialBankData;
            Assert.AreNotEqual(0, testInput.Length);

            var testTarget = testInput.FirstOrDefault(x => x.GetName(KindsOfNames.Abbrev) == "JPMORGAN CHASE BK NA");

            Assert.IsNotNull(testTarget);

            //verify the properties have no value prior to test
            Assert.IsNull(testTarget.CIK);
            Assert.IsNull(testTarget.TickerSymbols);

            testTarget.LoadXrefXmlData();

            Assert.IsNotNull(testTarget.CIK);
            Assert.AreNotEqual(0, testTarget.TickerSymbols);

            System.Diagnostics.Debug.WriteLine(testTarget.CIK.ToString());
            System.Diagnostics.Debug.WriteLine(testTarget.TickerSymbols[0].Symbol);
            System.Diagnostics.Debug.WriteLine(testTarget.TickerSymbols[0].Exchange);
            System.Diagnostics.Debug.WriteLine(testTarget.SIC.ToString());

            //test nothing found - no problems and no change
            testTarget = testInput.FirstOrDefault(x => x.GetName(KindsOfNames.Abbrev) == "STATE STREET B&TC");

            Assert.IsNotNull(testTarget);
            Assert.IsNull(testTarget.CIK);
            Assert.IsNull(testTarget.TickerSymbols);

            testTarget.LoadXrefXmlData();

            Assert.IsNotNull(testTarget.CIK);
            Assert.IsNotNull(testTarget.TickerSymbols);

            System.Diagnostics.Debug.WriteLine(testTarget.CIK.ToString());
            System.Diagnostics.Debug.WriteLine(testTarget.TickerSymbols[0].Symbol);
            System.Diagnostics.Debug.WriteLine(testTarget.TickerSymbols[0].Exchange);
            System.Diagnostics.Debug.WriteLine(testTarget.SIC.ToString());

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

            NfPath.SaveXml(TreeData.XRefXml,
                TestAssembly.UnitTestsRoot + @"\Rand\XRefTestRslt.xml");

        }
    }
}
