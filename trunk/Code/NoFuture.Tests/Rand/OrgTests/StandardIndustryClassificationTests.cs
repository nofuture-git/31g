using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
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

        [Test]
        public void TestToData()
        {
            var testSubject = new IObviate[]
            {
                StandardIndustryClassification.RandomStandardIndustryClassification()
            };

            foreach (var ts in testSubject)
            {
                Console.WriteLine();
                var testResult = ts.ToData(KindsOfTextCase.Kabab);
                Assert.IsNotNull(testResult);
                Assert.AreNotEqual(0, testResult.Count);
                foreach (var k in testResult.Keys)
                    Console.WriteLine($"{k}: {testResult[k]}");
            }
        }
    }
}
