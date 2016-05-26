using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Data.Types;
using System.IO;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class CityAreaTests
    {
        [TestMethod]
        public void AmericanTest()
        {
            var testResult = CityArea.American(null);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.City));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ZipCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostalState));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostalCodeAddonFour));

            System.Diagnostics.Debug.WriteLine(testResult.City);
            System.Diagnostics.Debug.WriteLine(testResult.PostalState);
            System.Diagnostics.Debug.WriteLine(testResult.ZipCode);
        }

        [TestMethod]
        public void CanadianTest()
        {
            var testResult = CityArea.Canadian();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.City));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostalCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Providence));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ProvidenceAbbrv));

            System.Diagnostics.Debug.WriteLine(testResult.City);
            System.Diagnostics.Debug.WriteLine(testResult.ProvidenceAbbrv);
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
                UsCityStateZip testResultOut;
                var testResult = UsCityStateZip.TryParse(testAddr, out testResultOut);
                Assert.IsTrue(testResult);
                Assert.AreNotEqual(string.Empty,testResultOut.City);
                //Console.WriteLine(testResultOut.City);
                Assert.AreNotEqual(string.Empty, testResultOut.PostalState);
                //Console.WriteLine(testResultOut.PostalState);
                Assert.AreNotEqual(string.Empty, testResultOut.ZipCode);
                //Console.WriteLine(testResultOut.ZipCode);
            }

            UsCityStateZip testResultOut2;
            var anotherTest = UsCityStateZip.TryParse("OKLAHOMA CITY, OK", out testResultOut2);
            Assert.IsTrue(anotherTest);

            Console.WriteLine(testResultOut2.City);
            Console.WriteLine(testResultOut2.PostalState);
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
                new UsAddress(new AddressData()
                {
                    AddressNumber = addressNumber,
                    PostalCode = postalCode,
                    City = city,
                    StreetName = streetName,
                    StateAbbrv = stateAbbrv,
                    StreetType = streetType
                });

            Assert.IsFalse(testSubject.Equals(null));
            Assert.IsFalse(testSubject.Equals(new UsAddress(new AddressData())));

            var testAreEqual = new UsAddress(new AddressData()
                {
                    AddressNumber = addressNumber,
                    PostalCode = postalCode,
                    City = city,
                    StreetName = streetName,
                    StateAbbrv = stateAbbrv,
                    StreetType = streetType
                });

            Assert.IsTrue(testSubject.Equals(testAreEqual));

            testAreEqual.Data.AddressNumber = testAreEqual.Data.AddressNumber.ToUpper();

            Assert.IsTrue(testSubject.Equals(testAreEqual));

        }
    }
}
