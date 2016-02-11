using System;
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
        public void TestGetModelYearYyyy()
        {
            //TODO
        }
    }
}
