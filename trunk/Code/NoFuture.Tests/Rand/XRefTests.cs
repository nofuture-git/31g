using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Data;
using NoFuture.Rand.Gov.Fed;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class XRefTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
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
        [TestMethod]
        public void TestAddXrefValues()
        {
            var testXrefId = new Tuple<Type, string>(typeof(NoFuture.Rand.Com.Bank), "BANK OF AMER NA");
            var testValues = new RoutingTransitNumber {Value = "000015421"};
            var testResult = NoFuture.Rand.Data.Types.XRefGroup.AddXrefValues(testXrefId,testValues, "RoutingNumber");
            Assert.IsTrue(testResult);

            testXrefId = new Tuple<Type, string>(typeof(NoFuture.Rand.Com.Bank), "A new bank entry");
            testValues = new RoutingTransitNumber { Value = "787454541" };
            testResult = NoFuture.Rand.Data.Types.XRefGroup.AddXrefValues(testXrefId, testValues, "RoutingNumber");
            Assert.IsTrue(testResult);

            NoFuture.Util.NfPath.SaveXml(TreeData.XRefXml,
                @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Rand\XRefTestRslt.xml");

        }
    }
}
