using System;
using System.IO;
using NUnit.Framework;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestFixture]
    public class UsStreetPoTests
    {

        [Test]
        public void TryParseTests()
        {
            var testInput = "102 MAIN ST APT. 101";

            UsStreetPo testResultOut = null;
            var testResult = UsStreetPo.TryParse(testInput, out testResultOut);

            Assert.IsTrue(testResult);
            Assert.AreEqual("102", testResultOut.PostBox);
            Assert.AreEqual("MAIN", testResultOut.StreetName);
            Assert.AreEqual("ST", testResultOut.StreetKind);
            Assert.AreEqual("APT. 101", testResultOut.SecondaryUnit);

            testInput = "1356 EXECUTIVE DR STE 202";
            testResult = UsStreetPo.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("1356", testResultOut.PostBox);
            Assert.AreEqual("EXECUTIVE", testResultOut.StreetName);
            Assert.AreEqual("DR", testResultOut.StreetKind);
            Assert.AreEqual("STE 202", testResultOut.SecondaryUnit);

            testInput = "7227 N. 16th St. #235";
            testResult = UsStreetPo.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("7227", testResultOut.PostBox);
            Assert.AreEqual("16th", testResultOut.StreetName);
            Assert.AreEqual("N",testResultOut.GetData().ThoroughfareDirectional);
            Assert.AreEqual("St.", testResultOut.StreetKind);
            Assert.AreEqual("235", testResultOut.SecondaryUnit);

            testInput = "250 GLEN ST";
            testResult = UsStreetPo.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("250", testResultOut.PostBox);
            Assert.AreEqual("GLEN", testResultOut.StreetName);
            Assert.AreEqual("ST", testResultOut.StreetKind);

            testInput = "40 Commerce Street";
            testResult = UsStreetPo.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("40", testResultOut.PostBox);
            Assert.AreEqual("Commerce", testResultOut.StreetName);
            Assert.AreEqual("Street", testResultOut.StreetKind);

            testInput = "9848 Upper 173rd Ct W";
            testResult = UsStreetPo.TryParse(testInput, out testResultOut);
            Assert.IsFalse(testResult);

            var oddAddrs = new[]
            {
                "115 112th ave NE Apt 103",
                "117-D PARK CHARLES BLVD. S.",
                "1503 Anna Ruby Lane NW",
                "2625 Piedmont rd. STE 56-407",
                "2834 S 2475 E",
                "30930 HIGHWAY 431 LOT 13",
                "3428 S. King Dr.",
                "40 CEDAR POINTE LOOP",
                "4004 S 1500 E",
                "4211 AVENUE R",
                "44W244 Plato Rd",
                "5311 Wong Dr  #208",
                "E9805 190TH AVE",
                "HCR 70 BOX 126",
                "N75W15375 COLONY RD",
                "P.O. BOX 1049",
                "P.O. Box 521653",
                "PMB 189",
                
            };
            foreach (var addr in oddAddrs)
            {
                testResultOut = null;
                testResult = UsStreetPo.TryParse(addr, out testResultOut);
                Assert.IsTrue(testResult);
                Console.WriteLine($"Original: '{addr}'");
                Console.WriteLine($"PostBox: '{testResultOut.PostBox}'");
                Console.WriteLine($"StreetName: '{testResultOut.StreetName}'" );
                Console.WriteLine($"StreetKind: '{testResultOut.StreetKind}'");
                Console.WriteLine($"SecondaryUnit: '{testResultOut.SecondaryUnit}'");
                Console.WriteLine($"CountyTownship: '{testResultOut.CountyTownship}'");
            }

            /*
         "115 112th ave NE Apt 103",
         "117-D PARK CHARLES BLVD. S.",
         "1503 Anna Ruby Lane NW",
         "2625 Piedmont rd. STE 56-407",
         "2834 S 2475 E",
         "30930 HIGHWAY 431 LOT 13",
         "3428 S. King Dr.",
         "40 CEDAR POINTE LOOP",
         "4004 S 1500 E",
         "4211 AVENUE R",
         "44W244 Plato Rd",
         "5311 Wong Dr  #208",
         "E9805 190TH AVE",
         "HCR 70 BOX 126",
         "N75W15375 COLONY RD",
         "P.O. BOX 1049",
         "P.O. Box 521653",
         "PMB 189",
          */

        }

        [Test]
        public void TestToString()
        {
            var testSubject = NoFuture.Rand.Geo.StreetPo.RandomAmericanStreet();
            var testResult = testSubject.ToString();

            Assert.IsFalse(testResult.Contains("  "));

        }

        [Test]
        public void TestUsPostalStreetKindFullNames()
        {
            var testResult = UsStreetPo.UsPostalStreetKindFullNames;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [Test]
        public void TestUsPostalStreetKindAbbrev()
        {
            var testResult = UsStreetPo.UsPostalStreetKindAbbrev;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [Test]
        public void TestUsPostalSecondaryUnits()
        {
            var testResult = UsStreetPo.UsPostalSecondaryUnits;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [Test]
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
                    ThoroughfareNumber = addressNumber,
                    PostalCode = postalCode,
                    Locality = city,
                    ThoroughfareName = streetName,
                    RegionAbbrev = stateAbbrv,
                    ThoroughfareType = streetType
                });

            Assert.IsFalse(testSubject.Equals(null));
            Assert.IsFalse(testSubject.Equals(new UsStreetPo(new AddressData())));

            var testAreEqual = new UsStreetPo(new AddressData()
            {
                ThoroughfareNumber = addressNumber,
                PostalCode = postalCode,
                Locality = city,
                ThoroughfareName = streetName,
                RegionAbbrev = stateAbbrv,
                ThoroughfareType = streetType
            });

            Assert.IsTrue(testSubject.Equals(testAreEqual));

            testAreEqual.GetData().ThoroughfareNumber = testAreEqual.GetData().ThoroughfareNumber.ToUpper();

            Assert.IsTrue(testSubject.Equals(testAreEqual));

        }
    }
}
