using System;
using NUnit.Framework;
using NoFuture.Shared;

namespace NoFuture.Tests.Encryption
{
    [TestFixture]
    public class BulkKeyTests
    {
        public const string BK = "ABCDE1234";

        [Test]
        public void TestToCipherText()
        {
            var testInput = "plain text";
            var testResult = NoFuture.Encryption.Sjcl.BulkCipherKey.ToEncryptedText(testInput, BK);
            Assert.IsInstanceOf(typeof(CipherText),testResult);
        }
        [Test]
        public void TestTogglePlainText()
        {
            var testInput = "plain text";
            var cipherText = NoFuture.Encryption.Sjcl.BulkCipherKey.ToEncryptedText(testInput, BK);
            Assert.IsInstanceOf(typeof(CipherText), cipherText);
            var testResult = NoFuture.Encryption.Sjcl.BulkCipherKey.ToPlainText(cipherText, BK);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(String.Empty, testResult);
            Assert.AreEqual(testInput, testResult);
        }
    }
}
