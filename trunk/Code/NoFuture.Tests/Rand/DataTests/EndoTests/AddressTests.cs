﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Endo;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class AddressTests
    {
        [TestMethod]
        public void AmericanTest()
        {
            var testResult = StreetPo.American();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostBox));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StreetName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StreetKind));

            testResult.Data.SecondaryUnitDesignator = "UNIT DESG";
            testResult.Data.SecondaryUnitId = "4451";

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TryParseTests()
        {
            var testInput = "102 MAIN ST APT 101";

            UsStreetPo testResultOut = null;
            var testResult = UsStreetPo.TryParse(testInput, out testResultOut);

            Assert.IsTrue(testResult);
            Assert.AreEqual("102", testResultOut.PostBox);
            Assert.AreEqual("MAIN", testResultOut.StreetName);
            Assert.AreEqual("ST",testResultOut.StreetKind);
            Assert.AreEqual("APT 101", testResultOut.SecondaryUnit);

            testInput = "1356 EXECUTIVE DR STE 202";
            testResult = UsStreetPo.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("1356",testResultOut.PostBox);
            Assert.AreEqual("EXECUTIVE",testResultOut.StreetName);
            Assert.AreEqual("DR", testResultOut.StreetKind);
            Assert.AreEqual("STE 202", testResultOut.SecondaryUnit);

            testInput = "7227 N. 16th St. #235";
            testResult = UsStreetPo.TryParse(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("7227", testResultOut.PostBox);
            Assert.AreEqual("N. 16th", testResultOut.StreetName);
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

        }
    }
}