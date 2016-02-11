using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class IdentityTests
    {
        [TestInitialize]
        public void Init()
        {
            NoFuture.BinDirectories.Root = @"C:\Projects\31g\trunk\Code\NoFuture\Rand";
        }
        [TestMethod]
        public void TestSuperSectors()
        {
            var testResult = NorthAmericanIndustryClassification.AllSectors;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult.Length);
            foreach (var ss in testResult)
            {
                Assert.IsInstanceOfType(ss, typeof(NaicsSuperSector));

                System.Diagnostics.Debug.WriteLine(ss.Description);
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
                Assert.IsFalse(string.IsNullOrWhiteSpace(t.NodeName));
                Assert.IsNotNull(t.XRefData);
                Assert.AreNotEqual(0, t.XRefData.Length);

                foreach (var dc in t.XRefData)
                {
                    Assert.AreNotEqual(0, dc.ReferenceDictionary.Count);
                    Assert.IsNotNull(dc.XrefIds);
                    Assert.AreNotEqual(0, dc.XrefIds.Length);

                    foreach (var dataFileXred in dc.XrefIds)
                    {
                        Assert.IsFalse(string.IsNullOrWhiteSpace(dataFileXred.XmlLocalName));
                        Assert.IsFalse(string.IsNullOrWhiteSpace(dataFileXred.Value));
                    }
                }
            }
        }
    }
}
