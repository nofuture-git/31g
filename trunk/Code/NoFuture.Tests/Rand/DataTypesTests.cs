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
            NoFuture.BinDirectories.Root = @"C:\Projects\31g\trunk\Code\NoFuture\Rand";//this gets set by ps scripts
            var testResult = NoFuture.Rand.Data.Types.StandardIndustryClassification.RandomSic();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Value);
        }
    }
}
