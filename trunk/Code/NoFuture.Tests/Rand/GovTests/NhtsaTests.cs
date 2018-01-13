using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Gov.US.Nhtsa;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestClass]
    public class NhtsaTests
    {
        [TestMethod]
        public void TestVinNoValues()
        {
            var testSubject = new Vin();
            Assert.AreEqual("000000A0200000000", testSubject.ToString());
        }

        [TestMethod]
        public void TestGetChkDigit()
        {
            var testSubject = new Vin
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
            for (var i = 0; i < 45; i++)
            {
                var testResult = Vin.GetRandomVin();
                Assert.IsNotNull(testResult);
                var testResultYear = testResult.GetModelYearYyyy();
                Assert.IsNotNull(testResultYear);
                Assert.IsTrue(testResultYear.Value <= DateTime.Today.Year);

            }

            for (var i = 0; i < 45; i++)
            {
                var byYearTestResult = Vin.GetRandomVin(true, 2014);
                var testResultYear = byYearTestResult.GetModelYearYyyy();
                Assert.IsNotNull(testResultYear);
                Assert.IsTrue(testResultYear.Value <= 2014);
                System.Diagnostics.Debug.WriteLine(string.Join(" ", byYearTestResult.Value, byYearTestResult.Description, byYearTestResult.GetModelYearYyyy()));
            }
        }

        [TestMethod]
        public void TestGetRandoManufacturerId()
        {
            var testResult = Vin.GetRandomManufacturerId();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(string.Join(" ", testResult.Item1, testResult.Item2));
            Assert.IsNotNull(testResult.Item1);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Item2));
        }
    }
}
