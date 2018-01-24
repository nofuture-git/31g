using NUnit.Framework;

namespace NoFuture.Rand.Tests
{
    [TestFixture]
    public class IdentityTests
    {
        [Test]
        public void TestValueLastFour()
        {
            var testSubject = new TestIdentifier {Value = "abcOP99"};
            var testResult = testSubject.ValueLastFour();
            Assert.AreEqual("XXXOP99", testResult);
        }

        [Test]
        public void TestEquals()
        {
            var testSubject = new TestIdentifier {Value = "8955662"};
            var testInput = new TestIdentifier { Value = "8955662" };
            Assert.IsTrue(testSubject.Equals(testInput));
        }

        [Test]
        public void TestToString()
        {
            var testSubject = new TestIdentifier { Value = "my value" };
            Assert.AreEqual("my value", testSubject.ToString());
        }
    }

    public class TestIdentifier : Core.Identifier
    {
        public override string Abbrev => "test";
    }
}
