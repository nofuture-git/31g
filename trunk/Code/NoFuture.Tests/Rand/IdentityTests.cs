using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class IdentityTests
    {
        [TestMethod]
        public void TestValueLastFour()
        {
            var testSubject = new TestIdentifier {Value = "abcOP99"};
            var testResult = testSubject.ValueLastFour();
            Assert.AreEqual("XXXOP99", testResult);
        }

        [TestMethod]
        public void TestEquals()
        {
            var testSubject = new TestIdentifier {Value = "8955662"};
            var testInput = new TestIdentifier { Value = "8955662" };
            Assert.IsTrue(testSubject.Equals(testInput));
        }

        [TestMethod]
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
