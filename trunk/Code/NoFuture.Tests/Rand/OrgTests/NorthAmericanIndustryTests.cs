﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Tests.OrgTests
{
    [TestClass]
    public class NorthAmericanIndustryTests
    {
        [TestMethod]
        public void TestSuperSectors()
        {
            var testResult = NorthAmericanIndustryClassification.AllSectors;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
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
    }
}
