using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Tests.Encryption
{
    [TestClass]
    public class Nfx509Tests
    {
        public const string TEST_PWD = "Test1234";
        public string TEST_CERT_PFX_PATH = TestAssembly.UnitTestsRoot + @"\Encryption\MyTestCert01.pfx";
        public string TEST_CERT_CER_PATH = TestAssembly.UnitTestsRoot + @"\Encryption\MyTestCert01.cer";
        public string TEST_PLAINTEXT = TestAssembly.UnitTestsRoot + @"\Encryption\TestPlainText.txt";

        [TestInitialize]
        public void Init()
        {
            NoFuture.Util.FxPointers.AddSHA512ToCryptoConfig();
        }

        [TestMethod]
        public void TestCreateAndSave()
        {
            var dn = new NoFuture.Encryption.NfX509.DistinguishedName("MyTestCert01");
            var myCert = NoFuture.Encryption.NfX509.CreateSelfSignedCert(dn, new DateTime(2047, 1, 1),
                TEST_PWD);
            var pfx = myCert.Export(X509ContentType.Pkcs12, TEST_PWD);

            File.WriteAllBytes(TEST_CERT_PFX_PATH, pfx);
        }

        [TestMethod]
        public void TestGetCerOfTestPfx()
        {
            var myCert = new X509Certificate2(TEST_CERT_PFX_PATH, TEST_PWD);
            var myCerCert = NoFuture.Encryption.NfX509.ExportToPem(myCert);
            File.WriteAllText(
                Path.Combine(Path.GetDirectoryName(TEST_CERT_PFX_PATH),
                    Path.GetFileNameWithoutExtension(TEST_CERT_PFX_PATH) + ".cer"), myCerCert);
        }

        [TestMethod]
        public void TestEncryptFile()
        {
            
            //a file to encrypts
            var someText = Etc.LoremIpsumEightParagraphs;
            File.WriteAllText(TEST_PLAINTEXT, someText);
            Thread.Sleep(1000);

            Assert.IsTrue(File.Exists(TEST_PLAINTEXT));

            NoFuture.Encryption.NfX509.EncryptFile(TEST_PLAINTEXT, TEST_CERT_CER_PATH);

            Assert.IsTrue(File.Exists(TEST_PLAINTEXT + Constants.NF_CRYPTO_EXT));

        }

        [TestMethod]
        public void TestDecryptFile()
        {
            Assert.IsTrue(File.Exists(TEST_PLAINTEXT + Constants.NF_CRYPTO_EXT));

            if (File.Exists(TEST_PLAINTEXT))
            {
                File.Delete(TEST_PLAINTEXT);
            }
            Thread.Sleep(1000);

            NoFuture.Encryption.NfX509.DecryptFile(TEST_PLAINTEXT + Constants.NF_CRYPTO_EXT, TEST_CERT_PFX_PATH, TEST_PWD);

            Assert.IsTrue(File.Exists(TEST_PLAINTEXT));

            var content = File.ReadAllText(TEST_PLAINTEXT);

            Assert.AreEqual(Etc.LoremIpsumEightParagraphs, content);
        }
    }
}
