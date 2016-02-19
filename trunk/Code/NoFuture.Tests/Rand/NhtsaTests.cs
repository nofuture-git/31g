﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Gov.Nhtsa;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class NhtsaTests
    {
        [TestMethod]
        public void TestVinNoValues()
        {
            var testSubject = new NoFuture.Rand.Gov.Nhtsa.Vin();
            Assert.AreEqual("00000000000000000", testSubject.ToString());
        }

        [TestMethod]
        public void TestGetChkDigit()
        {
            var testSubject = new NoFuture.Rand.Gov.Nhtsa.Vin
            {
                Wmi = new WorldManufacturerId {Country = '1', RegionMaker = '1', VehicleType = '1'},
                Vds = new VehicleDescription {Four = '1', Five = '1', Six = '1', Seven = '1', Eight = '1'},
                Vis = new VehicleIdSection {ModelYear = '1', PlantCode = '1', SequentialNumber = "111111"}
            };

            var testREsult = testSubject.GetCheckDigit();
            Assert.AreEqual('1',testREsult);
        }

        [TestMethod]
        public void TestVinSetValue()
        {
            var testVinValue = "3N1CN7AP0GL861987";

            var testSubject = new Vin {Value = testVinValue};

            Assert.AreEqual('3', testSubject.Wmi.Country);
            Assert.AreEqual('N', testSubject.Wmi.RegionMaker);
            Assert.AreEqual('1', testSubject.Wmi.VehicleType);

            Assert.AreEqual('C', testSubject.Vds.Four);
            Assert.AreEqual('N', testSubject.Vds.Five);
            Assert.AreEqual('7', testSubject.Vds.Six);
            Assert.AreEqual('A', testSubject.Vds.Seven);
            Assert.AreEqual('P', testSubject.Vds.Eight);

            Assert.AreEqual('G', testSubject.Vis.ModelYear);
            Assert.AreEqual('L', testSubject.Vis.PlantCode);
            Assert.AreEqual("861987", testSubject.Vis.SequentialNumber);
        }

        [TestMethod]
        public void TestGetModelYearYyyy()
        {
            var testVinValue = "3N1CN7AP0GL861987";

            var testSubject = new Vin { Value = testVinValue };

            var testResult = testSubject.GetModelYearYyyy();
            Assert.AreEqual(2016,testResult);
        }

        [TestMethod]
        public void TestVinToString()
        {
            var testVinValue = "3N1CN7AP0GL861987";
            var testSubject = new Vin {Value = testVinValue};
            Assert.AreEqual(testVinValue, testSubject.ToString());
        }

        [TestMethod]
        public void TestVinEquality()
        {
            var testVinValue = "3N1CN7AP0GL861987";
            var testSubject = new Vin { Value = testVinValue };

            var compareTest = new Vin {Value = testVinValue};

            Assert.IsTrue(testSubject.Equals(compareTest));

        }

        [TestMethod]
        public void TestRandomVin()
        {
            var testResult = Vin.GetRandomVin();
            Assert.IsNotNull(testResult);
            
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}