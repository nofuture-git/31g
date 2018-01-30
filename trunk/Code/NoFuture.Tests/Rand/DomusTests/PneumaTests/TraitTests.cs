using System;
using NoFuture.Rand.Pneuma;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.DomusTests.PneumaTests
{
    [TestFixture]
    public class TraitTests
    {
        [Test]
        public void TestEquals()
        {
            var test00 = new Openness();
            var test01 = new Openness();

            test01.Value = test00.Value;

            Assert.IsTrue(test00.Equals(test01));
        }
    }
}
