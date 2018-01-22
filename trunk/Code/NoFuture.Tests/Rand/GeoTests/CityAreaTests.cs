using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestClass]
    public class CityAreaTests
    {

        [TestMethod]
        public void AmericanTest()
        {
            var testResult = CityArea.RandomAmericanCity();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.City));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ZipCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StateAbbrev));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostalCodeAddonFour));

            System.Diagnostics.Debug.WriteLine(testResult.City);
            System.Diagnostics.Debug.WriteLine(testResult.StateAbbrev);
            System.Diagnostics.Debug.WriteLine(testResult.ZipCode);
        }

        [TestMethod]
        public void CanadianTest()
        {
            var testResult = CityArea.RandomCanadianCity();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.City));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostalCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Providence));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ProvidenceAbbrv));

            System.Diagnostics.Debug.WriteLine(testResult.City);
            System.Diagnostics.Debug.WriteLine(testResult.ProvidenceAbbrv);
            System.Diagnostics.Debug.WriteLine(testResult.Providence);
            System.Diagnostics.Debug.WriteLine(testResult.PostalCode);
        }

        [TestMethod]
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
                var testResult = UsCityStateZip.TryParse(testAddr, out var testResultOut, true);
                Assert.IsTrue(testResult);
                Assert.AreNotEqual(string.Empty,testResultOut.City);
                Assert.AreNotEqual(string.Empty, testResultOut.StateAbbrev);
                Assert.AreNotEqual(string.Empty, testResultOut.ZipCode);
            }

            var anotherTest = UsCityStateZip.TryParse("EL CAMPO, TX", out var usCityStateZip, false);
            Assert.IsTrue(anotherTest);
            Assert.AreEqual("El Campo", usCityStateZip.City);
            Assert.AreEqual("TX", usCityStateZip.StateAbbrev);
            Console.WriteLine(usCityStateZip.City);
            Console.WriteLine(usCityStateZip.StateAbbrev);
        }

        [TestMethod]
        public void TestEquality()
        {
            var addressNumber = Path.GetRandomFileName();
            var postalCode = Path.GetRandomFileName();
            var city = Path.GetRandomFileName();
            var streetName = Path.GetRandomFileName();
            var stateAbbrv = Path.GetRandomFileName();
            var streetType = Path.GetRandomFileName();
            var testSubject =
                new UsStreetPo(new AddressData()
                {
                    AddressNumber = addressNumber,
                    PostalCode = postalCode,
                    City = city,
                    StreetName = streetName,
                    StateAbbrev = stateAbbrv,
                    StreetType = streetType
                });

            Assert.IsFalse(testSubject.Equals(null));
            Assert.IsFalse(testSubject.Equals(new UsStreetPo(new AddressData())));

            var testAreEqual = new UsStreetPo(new AddressData()
                {
                    AddressNumber = addressNumber,
                    PostalCode = postalCode,
                    City = city,
                    StreetName = streetName,
                    StateAbbrev = stateAbbrv,
                    StreetType = streetType
                });

            Assert.IsTrue(testSubject.Equals(testAreEqual));

            testAreEqual.Data.AddressNumber = testAreEqual.Data.AddressNumber.ToUpper();

            Assert.IsTrue(testSubject.Equals(testAreEqual));

        }

        [TestMethod]
        public void TestUsCityStateZipCtor()
        {
            var addrData = new AddressData {StateAbbrev = "NV", PostalCode = "89421"};
            var testResult = new UsCityStateZip(addrData);
            Assert.AreNotEqual("New York City", testResult.City);
            System.Diagnostics.Debug.WriteLine(testResult.City);

        }
        [TestMethod]
        public void TestUsCityStateZipCtorPickWithMsa()
        {
            var addrData = new AddressData { StateAbbrev = "FL", PostalCode = "32701" };
            var testResult = new UsCityStateZip(addrData);
            Assert.AreNotEqual("New York City", testResult.City);
            Assert.IsNotNull(testResult.Msa);
            System.Diagnostics.Debug.WriteLine(testResult.City);
        }
        [TestMethod]
        public void TestUsCityStateZipCtorPickSuburb()
        {
            var addrData = new AddressData { StateAbbrev = "FL", PostalCode = "32101" };
            var testResult = new UsCityStateZip(addrData);
            Assert.AreNotEqual("New York City", testResult.City);
            Assert.IsNotNull(testResult.Msa);
            System.Diagnostics.Debug.WriteLine(testResult.City);
        }

        [TestMethod]
        public void TestUsCityStateZipCtorStateAndCityOnly()
        {
            var addrData = new AddressData {StateAbbrev = "NC", City = "CHARLOTTE"};
            var testResult = new UsCityStateZip(addrData);
            Assert.AreNotEqual("New York City", testResult.City);
            Assert.IsNotNull(testResult.Msa);
            System.Diagnostics.Debug.WriteLine(testResult.City);

            addrData = new AddressData { StateAbbrev = "CA", City = "Westhaven-Moonstone" };
            testResult = new UsCityStateZip(addrData);
            Assert.IsNotNull(testResult.Msa);
        }

        [TestMethod]
        public void TestGetZipCode()
        {
            var addrData = new AddressData();
            UsCityStateZip.GetZipCode("OKLAHOMA CITY, OK", addrData);
            Assert.IsNull(addrData.PostalCode);
            //Washington DC 20006
            UsCityStateZip.GetZipCode("Washington DC 20006", addrData);
            Assert.AreEqual("20006", addrData.PostalCode);
        }

        [TestMethod]
        public void TestGetState()
        {
            var addrData = new AddressData();
            UsCityStateZip.GetState("OKLAHOMA CITY, OK", addrData);
            Assert.IsNotNull(addrData.StateAbbrev);
            Assert.AreEqual("OK", addrData.StateAbbrev);
            UsCityStateZip.GetState("Washington DC 20006", addrData);
            Assert.IsNotNull(addrData.StateAbbrev);
            Assert.AreEqual("DC", addrData.StateAbbrev);
        }

        [TestMethod]
        public void TestGetCity()
        {
            var addrData = new AddressData();
            var ln = "EL CAMPO, TX";
            UsCityStateZip.GetState(ln, addrData);
            UsCityStateZip.GetCity(ln, addrData);
            Assert.IsNotNull(addrData.City);
            Assert.AreEqual("TX", addrData.StateAbbrev);
            Assert.AreEqual("EL CAMPO", addrData.City);
            System.Diagnostics.Debug.WriteLine($"{addrData.City} {addrData.StateAbbrev}");
            ln = "Washington DC 20006";
            UsCityStateZip.GetZipCode(ln, addrData);
            UsCityStateZip.GetState(ln, addrData);
            UsCityStateZip.GetCity(ln, addrData);
            Assert.IsNotNull(addrData.City);
            Assert.AreEqual("DC",addrData.StateAbbrev);
            Assert.AreEqual("Washington", addrData.City);
            System.Diagnostics.Debug.WriteLine($"{addrData.City} {addrData.StateAbbrev}");
        }

        [TestMethod]
        public void TestFinesseCityName()
        {
            var testOutput = UsCityStateZip.FinesseCityName("CANDLER-MCAFEE");
            Assert.AreEqual("Candler-McAfee", testOutput);
            System.Diagnostics.Debug.WriteLine(testOutput);
            testOutput = UsCityStateZip.FinesseCityName("MC LEAN");
            Assert.AreEqual("McLean", testOutput);
            System.Diagnostics.Debug.WriteLine(testOutput);
        }
    }
}
