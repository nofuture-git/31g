using System;
using NUnit.Framework;

namespace NoFuture.Encryption.Tests
{
    [TestFixture]
    public class HashTests
    {
        [Test]
        public void TestSign()
        {
            var testInput = "plain text to sign";
            var salt = "ABCDEF1234";

            var testResult = NoFuture.Encryption.Sjcl.Hash.Sign(testInput, salt);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(testResult));
        }

        [Test]
        public void TestSignCalc()
        {
            var testInput = "plain text to sign";
            var salt = "ABCDEF1234";

            var testResult = NoFuture.Encryption.Sjcl.Hash.Sign(testInput, salt);
            var testCompare = NoFuture.Encryption.Sjcl.Hash.Sign(testInput, salt);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testCompare);

            Assert.AreEqual(testCompare,testResult);
        }
    }
}
