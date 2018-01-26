using System;
using NUnit.Framework;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Tests.OrgTests
{
    [TestFixture]
    public class StandardIndustryClassificationTests
    {
        [Test]
        public void TestRandomSic()
        {
            var testResult = StandardIndustryClassification.RandomStandardIndustryClassification();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Value);
        }
    }
}
