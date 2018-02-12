using System.Linq;
using NUnit.Framework;

namespace NoFuture.Rand.Exo.Tests
{
    [TestFixture]
    public class BeaTests
    {
        [Test]
        public void TestBeaParameterToString()
        {
            var testSubject = NoFuture.Rand.Exo.UsGov.Bea.Parameters.RegionalData.GeoFips.Values.FirstOrDefault(gf => gf.Val == "STATE");
            Assert.IsNotNull(testSubject);
           
            System.Console.WriteLine(testSubject.ToString());
        }
    }
}
