﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Rand.Exo.Tests
{
    [TestClass]
    public class BeaTests
    {
        [TestMethod]
        public void TestBeaParameterToString()
        {
            var testSubject = NoFuture.Rand.Exo.UsGov.Bea.Parameters.RegionalData.GeoFips.Values.FirstOrDefault(gf => gf.Val == "STATE");
            Assert.IsNotNull(testSubject);
           
            System.Diagnostics.Debug.WriteLine(testSubject.ToString());
        }
    }
}
