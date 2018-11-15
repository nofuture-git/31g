using System;
using System.Diagnostics;
using NoFuture.Rand.Geo;
using NUnit.Framework;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestFixture]
    public class UsCityStateZipTests
    {
        [Test]
        public void RandomUsZipWithRespectToPopTest()
        {
            var testResult = UsCityStateZip.RandomAmericanPartialZipCode();
            Assert.IsNotNull(testResult);

            for (var i = 0; i < 100; i++)
            {
                testResult = UsCityStateZip.RandomAmericanPartialZipCode();
                Assert.IsNotNull(testResult);
                Console.WriteLine(testResult);
            }
        }

        [Test]
        public void TestTryParse()
        {
            var testAddresses = new[]
                                {
                                    "RIVERTON WY 82501",
                                    "Los Angeles CA 90029",
                                    "Los Angeles, CA 90029",
                                    "CARROLLTON MS 38917",
                                    "TAYLORS SC 29687",
                                    "CARROLLTON MS 38917",
                                    "ASHLANDCITY TN 37015",
                                    "Riverside CA 92501",
                                    "PLEASANTVIEW TN 37146",
                                    "FRANKLIN TN 37067",
                                    "FRENCH CAMP, MS 39745",
                                    "MURFREESBORO TN 37128",
                                    "Vernal UT 84078",
                                    "Roosevelt UT 84066",
                                    "HILTON HEAD SC 29926",
                                    "SARDIS MS 38666",
                                    "FRANKLIN TN 37069",
                                    "VENTURA CA 93004",
                                    "Mayfield KY 42066",
                                    "Washington DC 20006"
                                };
            foreach (var testAddr in testAddresses)
            {
                var testResult = UsCityStateZip.TryParse(testAddr, out var testResultOut);
                Assert.IsTrue(testResult);
                Assert.AreNotEqual(string.Empty, testResultOut.City);
                Assert.AreNotEqual(string.Empty, testResultOut.StateAbbrev);
                Assert.AreNotEqual(string.Empty, testResultOut.ZipCode);
            }

            var anotherTest = UsCityStateZip.TryParse("EL CAMPO, TX", out var usCityStateZip);
            Assert.IsFalse(anotherTest);
            Assert.AreEqual("El Campo", usCityStateZip.City);
            Assert.AreEqual("TX", usCityStateZip.StateAbbrev);
            Console.WriteLine(usCityStateZip.City);
            Console.WriteLine(usCityStateZip.StateAbbrev);
        }

        [Test]
        public void AmericanRaceRatioByZipCodeTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = UsCityStateZip.RandomAmericanRaceWithRespectToZip(TEST_ZIP);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);

            var testZipPrefix = new string[]
            {
                "945","089","945","330","785","970","974","402",
                "920","799","761","023","750","021","840","782",
                "605","925","549","801","731","503","806","378",
                "770","173","076","440","754","917","460","926",
                "317","921","016","437","299","900","357","114",
                "070","028","953","600","112","913","923","496",
                "731","785","882","601","307","850","631","223",
                "208","275","112","967","356","600","829","761",
                "322","481","463","301","902","377","776",
                "372","073","802","752","360","454","735","119",
                "127","206","104","341","481","331","925","970",
                "435","321","851","600","719","781","336","349",
                "800","802","744","481"
            };

            foreach (var t in testZipPrefix)
            {
                Console.WriteLine(t);
                testResult = UsCityStateZip.RandomAmericanRaceWithRespectToZip(t);
                Assert.IsNotNull(testResult);
                Console.WriteLine(testResult);
            }
        }


        [Test]
        public void AmericanRaceTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = UsCityStateZip.GetAmericanRace(TEST_ZIP);
            Assert.AreNotEqual(string.Empty, testResult);
            Debug.WriteLine(testResult);

            testResult = UsCityStateZip.GetAmericanRace("68415");
            Assert.AreEqual(NorthAmericanRace.White, testResult);
        }


        [Test]
        public void TestUsCityStateZipCtor()
        {
            var addrData = new AddressData { RegionAbbrev = "NV", PostalCode = "89421" };
            var testResult = new UsCityStateZip(addrData);
            Assert.AreNotEqual("New York City", testResult.City);
            Console.WriteLine(testResult.City);

        }
        [Test]
        public void TestUsCityStateZipCtorPickWithMsa()
        {
            var addrData = new AddressData { RegionAbbrev = "FL", PostalCode = "32701" };
            var testResult = new UsCityStateZip(addrData);
            testResult.GetXmlData();
            Assert.AreNotEqual("New York City", testResult.City);
            Assert.IsNotNull(testResult.Msa);
            Console.WriteLine(testResult.City);
        }
        [Test]
        public void TestUsCityStateZipCtorPickSuburb()
        {
            var addrData = new AddressData { RegionAbbrev = "FL", PostalCode = "32101" };
            var testResult = new UsCityStateZip(addrData);
            testResult.GetXmlData();
            Assert.AreNotEqual("New York City", testResult.City);
            Assert.IsNotNull(testResult.Msa);
            Console.WriteLine(testResult.City);
        }

        [Test]
        public void TestUsCityStateZipCtorStateAndCityOnly()
        {
            var addrData = new AddressData { RegionAbbrev = "NC", Locality = "CHARLOTTE" };
            var testResult = new UsCityStateZip(addrData);
            testResult.GetXmlData();
            Assert.AreNotEqual("New York City", testResult.City);
            Assert.IsNotNull(testResult.Msa);
            Console.WriteLine(testResult.City);

            addrData = new AddressData { RegionAbbrev = "CA", Locality = "Westhaven-Moonstone" };
            testResult = new UsCityStateZip(addrData);
            testResult.GetXmlData();
            Assert.IsNotNull(testResult.Msa);
        }

        [Test]
        public void TestGetZipCode()
        {
            var addrData = new AddressData();
            UsCityStateZip.GetZipCode("OKLAHOMA CITY, OK", addrData);
            Assert.IsNull(addrData.PostalCode);
            //Washington DC 20006
            UsCityStateZip.GetZipCode("Washington DC 20006", addrData);
            Assert.AreEqual("20006", addrData.PostalCode);
        }

        [Test]
        public void TestGetState()
        {
            var addrData = new AddressData();
            UsCityStateZip.GetState("OKLAHOMA CITY, OK", addrData);
            Assert.IsNotNull(addrData.RegionAbbrev);
            Assert.AreEqual("OK", addrData.RegionAbbrev);
            UsCityStateZip.GetState("Washington DC 20006", addrData);
            Assert.IsNotNull(addrData.RegionAbbrev);
            Assert.AreEqual("DC", addrData.RegionAbbrev);
        }

        [Test]
        public void TestGetCity()
        {
            var addrData = new AddressData();
            var ln = "EL CAMPO, TX";
            UsCityStateZip.GetState(ln, addrData);
            UsCityStateZip.GetCity(ln, addrData);
            Assert.IsNotNull(addrData.Locality);
            Assert.AreEqual("TX", addrData.RegionAbbrev);
            Assert.AreEqual("El Campo", addrData.Locality);
            Console.WriteLine($"{addrData.Locality} {addrData.RegionAbbrev}");
            ln = "Washington DC 20006";
            UsCityStateZip.GetZipCode(ln, addrData);
            UsCityStateZip.GetState(ln, addrData);
            UsCityStateZip.GetCity(ln, addrData);
            Assert.IsNotNull(addrData.Locality);
            Assert.AreEqual("DC", addrData.RegionAbbrev);
            Assert.AreEqual("Washington", addrData.Locality);
            Console.WriteLine($"{addrData.Locality} {addrData.RegionAbbrev}");
        }

        [Test]
        public void TestFinesseCityName()
        {
            var testOutput = UsCityStateZip.FinesseCityName("CANDLER-MCAFEE");
            Assert.AreEqual("Candler-McAfee", testOutput);
            Console.WriteLine(testOutput);
            testOutput = UsCityStateZip.FinesseCityName("MC LEAN");
            Assert.AreEqual("McLean", testOutput);
            Console.WriteLine(testOutput);
        }
    }
}
