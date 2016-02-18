using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class DataTypesTests
    {
        [TestMethod]
        public void TestRandomSic()
        {
            var testResult = NoFuture.Rand.Data.Types.StandardIndustryClassification.RandomSic();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Value);
        }
    }
}
