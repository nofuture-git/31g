using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Encryption
{
    [TestClass]
    public class TestCreateSelfSignedCert
    {
        [TestMethod]
        public void TestCreateAndSave()
        {
            var myCert = NoFuture.Encryption.NfX509.CreateSelfSignedCert("MyTestCert01", new DateTime(2047, 1, 1),
                "Test1234");
            var pfx = myCert.Export(X509ContentType.Pkcs12, "Test1234");

            System.IO.File.WriteAllBytes(@"C:\Projects\31g\trunk\temp\MyTestCert01.pfx", pfx);

        }
    }
}
