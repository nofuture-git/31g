using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class BeaTests
    {
        [TestMethod]
        public void TestBeaParameterToString()
        {
            var testSubject = NoFuture.Rand.Gov.Bea.Parameters.RegionalData.GeoFips.Values.FirstOrDefault(gf => gf.Val == "STATE");
            Assert.IsNotNull(testSubject);
           
            System.Diagnostics.Debug.WriteLine(testSubject.ToString());
        }
    }
}
