using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Tests.OrgTests
{
    [TestClass]
    public class StandardIndustryClassificationTests
    {
        [TestMethod]
        public void TestRandomSic()
        {
            var testResult = StandardIndustryClassification.RandomSic();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Value);
        }
    }
}
