using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class IdentityTests
    {

        [TestMethod]
        public void TestSuperSectors()
        {
            var testResult = NorthAmericanIndustryClassification.AllSectors;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult.Length);
            foreach (var ss in testResult)
            {
                Assert.IsInstanceOfType(ss, typeof(NaicsSuperSector));
                System.Diagnostics.Debug.WriteLine($"{ss.Value} {ss.Description}");
                foreach (var s in ss.Divisions)
                {
                    System.Diagnostics.Debug.WriteLine(s.Description);
                }
            }
        }

        [TestMethod]
        public void TestXref()
        {
            var testResults = NoFuture.Rand.Data.Types.XRefGroup.AllXrefGroups;
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
            var testInput = NoFuture.Rand.Data.TreeData.CommercialBankData;
            Assert.AreNotEqual(0, testInput.Length);

            var testTarget = testInput.FirstOrDefault(x => x.Name == "JPMORGAN CHASE BK NA");

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
            testTarget = testInput.FirstOrDefault(x => x.Name == "STATE STREET B&TC");

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
    }
}
