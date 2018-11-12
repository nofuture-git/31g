using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NUnit.Framework;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Tests.OrgTests
{
    [TestFixture]
    public class NorthAmericanIndustryTests
    {
        [Test]
        public void TestSuperSectors()
        {
            var testResult = NorthAmericanIndustryClassification.AllSectors;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach (var ss in testResult)
            {
                Assert.IsInstanceOf<NaicsSuperSector>(ss);
                System.Diagnostics.Debug.WriteLine($"{ss.Value} {ss.Description}");
                foreach (var s in ss.GetDivisions())
                {
                    System.Diagnostics.Debug.WriteLine(s.Description);
                }
            }
        }

        [Test]
        public void TestRandomMarket()
        {
            var testResult = NaicsMarket.RandomNaicsMarket();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

        [Test]
        public void TestRandomSuperSector()
        {
            var testResult = NaicsSuperSector.RandomNaicsSuperSector();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

        [Test]
        public void TestRandomPrimarySector()
        {
            var testResult = NaicsPrimarySector.RandomNaicsPrimarySector();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

        [Test]
        public void TestRandomSector()
        {
            var testResult = NaicsSector.RandomNaicsSector();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Description);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = new IObviate[]
            {
                NaicsSector.RandomNaicsSector(), NaicsPrimarySector.RandomNaicsPrimarySector(),
                NaicsSuperSector.RandomNaicsSuperSector(), NaicsMarket.RandomNaicsMarket()
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
