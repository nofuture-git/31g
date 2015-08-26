using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;

namespace NoFuture.Tests.Encryption
{
    [TestClass]
    public class BulkKeyTests
    {
        public const string BK = "ABCDE1234";

        [TestMethod]
        public void TestToCipherText()
        {
            var testInput = "plain text";
            var testResult = NoFuture.Encryption.Sjcl.BulkCipherKey.ToEncryptedText(testInput, BK);
            Assert.IsInstanceOfType(testResult, typeof(CipherText));
        }
        [TestMethod]
        public void TestTogglePlainText()
        {
            var testInput = "plain text";
            var cipherText = NoFuture.Encryption.Sjcl.BulkCipherKey.ToEncryptedText(testInput, BK);
            Assert.IsInstanceOfType(cipherText, typeof(CipherText));
            var testResult = NoFuture.Encryption.Sjcl.BulkCipherKey.ToPlainText(cipherText, BK);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(String.Empty, testResult);
            Assert.AreEqual(testInput, testResult);
        }
    }
}
