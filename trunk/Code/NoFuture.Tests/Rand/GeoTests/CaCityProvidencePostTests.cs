using System;
using System.Linq;
using System.Text.RegularExpressions;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.CA;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestFixture]
    public class CaCityProvidencePostTests
    {
        [Test]
        public void TestCaPostalProvidenceAbbrev()
        {
            var testResult = CaCityProvidencePost.CaPostalProvidenceAbbrev;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.IsTrue(testResult.All(x => Regex.IsMatch(x, "^[A-Z]{2}$")));
        }

        [Test]
        public void TestTryParse()
        {
            var testAddresses = new[]
            {
                "St. John's NL A1A 4Z4",
                "Calgary AB T2A 4Z4",
                "Edmonton,  AB T2A 4Z4",
                "Vancouver BC V6C 4Z4",
                "Winnipeg MB R3T 4Z4",
                "Fredericton NB E3B 4Z4",
                "Arctic Bay NT XA0 4Z4",
                "Dartmouth NS B2Z 4Z4",
                "Fort Liard NU X0G 4Z4",
                "Ottawa, ON K2A 4Z4",
                "Mississuaga ON L5M 4Z4",
                "Toronto ON M5A 4Z4",
                "Windsor, ON N9A 4Z4",
                "Thunder Bay ON P7A4Z4",
                "Charlottetown PE C1A 4Z4",
                "Quebec City QC G1P 4Z4",
                "Montreal QC H3A 4Z4",
                "Sherbrooke QC J1H 4Z4",
                "Saskatoon SK S7L4Z4",
                "Whitehorse YT Y1A4Z4",
            };
            foreach (var testAddr in testAddresses)
            {
                var testResult = CaCityProvidencePost.TryParse(testAddr, out var testResultOut);
                Assert.IsTrue(testResult);
                Assert.AreNotEqual(string.Empty, testResultOut.City);
                Assert.AreNotEqual(string.Empty, testResultOut.ProvidenceAbbrv);
                Assert.AreNotEqual(string.Empty, testResultOut.PostalCode);
                Console.WriteLine(testResultOut.ToString());
            }
        }

        [Test]
        public void TestGetPostalCode()
        {
            var testInput = new AddressData();
            CaCityProvidencePost.GetPostalCode("Thunder Bay ON P7A4Z4", testInput);
            var testResult = testInput.PostalCode;
            Assert.IsNotNull(testResult);
            Assert.AreEqual("P7A 4Z4", testResult);
            Console.WriteLine(testResult);
        }
    }
}
