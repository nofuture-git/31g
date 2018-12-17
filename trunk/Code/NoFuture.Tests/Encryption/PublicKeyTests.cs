using System;
using NoFuture.Shared;
using NUnit.Framework;

namespace NoFuture.Encryption.Tests
{
    [TestFixture]
    public class PublicKeyTests
    {
        public const string CIPHER_KEY = "5cK+?XLr_Gc0Rt@0bYW$7JMpTmj";

        [Test]
        public void TestToCipherText()
        {
            var testInput = "plain text";
            var testResult = NoFuture.Encryption.Sjcl.BulkCipherKey.ToEncryptedText(testInput, CIPHER_KEY);
            Assert.IsInstanceOf(typeof(CipherText),testResult);
        }
        [Test]
        public void TestTogglePlainText()
        {
            var testInput = "plain text";
            var cipherText = NoFuture.Encryption.Sjcl.BulkCipherKey.ToEncryptedText(testInput, CIPHER_KEY);
            Assert.IsInstanceOf(typeof(CipherText),cipherText);
            Console.WriteLine(cipherText.ToString());
            var testResult = NoFuture.Encryption.Sjcl.BulkCipherKey.ToPlainText(cipherText, CIPHER_KEY);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(String.Empty,testResult);
            Assert.AreEqual(testInput,testResult);
        }
        [Test]
        public void TestToPlainTextWithEscCharInData()
        {
            var pk = "5cK+?XLr_Gc0Rt@0bYW$7JMpTmj";
            var test_input = new NoFuture.Shared.CipherText
            {
                adata = string.Empty,
                cipher = "aes",
                ct = "praRbuuVATkwyxDMjZhiACQaLupW3Jsde78=",
                iter = 1000,
                iv = "DF4D2nAa6WZT6qYMojq2YQ==",
                ks = 128,
                mode = "ccm",
                salt = @"M9egkIg2Y\/Q=",
                ts = 64,
                v = 1
            };
            Console.WriteLine(test_input.ToString());
            var testResult = NoFuture.Encryption.Sjcl.BulkCipherKey.ToPlainText(test_input, pk);
            Assert.IsNotNull(testResult);
        }
    }
}
