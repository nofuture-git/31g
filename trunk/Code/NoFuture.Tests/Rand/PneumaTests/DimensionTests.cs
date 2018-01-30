using NoFuture.Rand.Pneuma;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.PneumaTests
{
    [TestFixture]
    public class DimensionTests
    {
        [Test]
        public void TestEquals()
        {
            var test00 = new Dimension(0.31D,0.125D);
            var test01 = new Dimension(0.31D, 0.125D);

            Assert.IsTrue(test00.Equals(test01));
        }
    }
}
