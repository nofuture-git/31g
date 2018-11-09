using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.CoreTests
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

        [Test]
        public void TestDeriveFromValue()
        {
            var someId = "A-00151244-$fe9006b";
            var testResult = new AccountId(someId);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(someId, testResult.Value);
            Assert.IsTrue(testResult.Validate());

            var anotherValue = testResult.GetRandom();
            System.Console.WriteLine(anotherValue);
            Assert.IsTrue(testResult.Validate(anotherValue));

        }
    }

    public class TestIdentifier : Core.Identifier
    {
        public override string Abbrev => "test";
    }
}
