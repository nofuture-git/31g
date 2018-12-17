using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using NoFuture.Util.Binary;
using NoFuture.Util.Core;
using NUnit.Framework;

namespace NoFuture.Encryption.Tests
{
    [TestFixture]
    public class Nfx509Tests
    {
        public const string TEST_PWD = "Test1234";

        [SetUp]
        public void Init()
        {
            FxPointers.AddSHA512ToCryptoConfig();
        }

        public void TestCreateAndSave()
        {
            var certPath = PutTestFileOnDisk("MyTestCert01.pfx");
            var dn = new NoFuture.Encryption.NfX509.DistinguishedName("MyTestCert01");
            var myCert = NoFuture.Encryption.NfX509.CreateSelfSignedCert(dn, new DateTime(2047, 1, 1),
                TEST_PWD);
            var pfx = myCert.Export(X509ContentType.Pkcs12, TEST_PWD);

            File.WriteAllBytes(certPath, pfx);
        }

        public void TestGetCerOfTestPfx()
        {
            var certPath = PutTestFileOnDisk("MyTestCert01.pfx");
            var myCert = new X509Certificate2(certPath, TEST_PWD);
            var myCerCert = NoFuture.Encryption.NfX509.ExportToPem(myCert);
            File.WriteAllText(
                Path.Combine(Path.GetDirectoryName(certPath),
                    Path.GetFileNameWithoutExtension(certPath) + ".cer"), myCerCert);
        }

        [Test]
        public void TestEncryptFile()
        {
            
            //a file to encrypts
            var someText = Etc.LoremIpsumEightParagraphs;
            var testFile = PutTestFileOnDisk("TestPlainText.txt");
            File.WriteAllText(testFile, someText);
            Thread.Sleep(1000);

            Assert.IsTrue(File.Exists(testFile));
            var certPath = PutTestFileOnDisk("MyTestCert01.cer");
            NoFuture.Encryption.NfX509.EncryptFile(testFile, certPath);

            Assert.IsTrue(File.Exists(testFile + NfX509.NF_CRYPTO_EXT));

        }

        [Test]
        public void TestDecryptFile()
        {

            var certPath = PutTestFileOnDisk("MyTestCert01.pfx");
            var testFile = PutTestFileOnDisk("TestCipherText.txt.nfk");
            Assert.IsTrue(File.Exists(testFile));

            Thread.Sleep(1000);

            NfX509.DecryptFile(testFile, certPath, TEST_PWD);

            var testResultFile = Path.Combine(GetTestFileDirectory(), Path.GetFileNameWithoutExtension(testFile));
            Console.WriteLine(testResultFile);
            Assert.IsNotNull(testResultFile);
            Assert.IsTrue(File.Exists(testResultFile));

            var content = File.ReadAllText(testResultFile);

            Assert.AreEqual(Etc.LoremIpsumEightParagraphs, content);
        }

        public static string PutTestFileOnDisk(string embeddedFileName)
        {
            var nfAppData = GetTestFileDirectory();
            var fileOnDisk = Path.Combine(nfAppData, embeddedFileName);
            if (File.Exists(fileOnDisk))
                return fileOnDisk;

            //need this to be another object each time and not just another reference
            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{asmName}.{embeddedFileName}");
            if (liSteam == null)
            {
                Assert.Fail($"Cannot find the embedded file {embeddedFileName}");
            }
            if (!Directory.Exists(nfAppData))
            {
                Directory.CreateDirectory(nfAppData);
            }

            var buffer = new byte[liSteam.Length];
            liSteam.Read(buffer, 0, buffer.Length);
            File.WriteAllBytes(fileOnDisk, buffer);
            System.Threading.Thread.Sleep(50);
            return fileOnDisk;
        }

        public static string GetTestFileDirectory()
        {
            var nfAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (String.IsNullOrWhiteSpace(nfAppData) || !Directory.Exists(nfAppData))
                throw new DirectoryNotFoundException("The Environment.GetFolderPath for " +
                                                     "SpecialFolder.ApplicationData returned a bad path.");
            nfAppData = Path.Combine(nfAppData, "NoFuture.Tests");
            return nfAppData;
        }
    }
}
