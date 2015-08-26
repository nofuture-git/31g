using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;

namespace NoFuture.Tests.Encryption
{
    [TestClass]
    public class PublicKeyTests
    {
        public const string PK_PUB = "5cK+?XLr_Gc0Rt@0bYW$7JMpTmj";
        public const string PK_PRIV = PK_PUB;//sjcl doesn't actually have asymetric support...

        [TestMethod]
        public void TestToCipherText()
        {
            var testInput = "plain text";
            var testResult = NoFuture.Encryption.Sjcl.PublicKey.ToEncryptedText(testInput, PK_PUB);
            Assert.IsInstanceOfType(testResult, typeof(CipherText));
        }
        [TestMethod]
        public void TestTogglePlainText()
        {
            var testInput = "plain text";
            var cipherText = NoFuture.Encryption.Sjcl.PublicKey.ToEncryptedText(testInput, PK_PUB);
            Assert.IsInstanceOfType(cipherText, typeof(CipherText));
            Console.WriteLine(cipherText.ToString());
            var testResult = NoFuture.Encryption.Sjcl.PublicKey.ToPlainText(cipherText, PK_PRIV);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(String.Empty,testResult);
            Assert.AreEqual(testInput,testResult);
        }
        [TestMethod]
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
            var testResult = NoFuture.Encryption.Sjcl.PublicKey.ToPlainText(test_input, pk);
        }
    }
}
