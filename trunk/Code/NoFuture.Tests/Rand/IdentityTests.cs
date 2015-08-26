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
                System.Diagnostics.Debug.WriteLine(ss.PercentOfTotalMarketAssests);
                foreach (var s in ss.Divisions)
                {
                    System.Diagnostics.Debug.WriteLine(s.Description);
                }
            }
        }
    }
}
