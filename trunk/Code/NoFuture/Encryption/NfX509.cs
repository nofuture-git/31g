using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CERTENROLLLib;
using NoFuture.Exceptions;
using X509KeyUsageFlags = CERTENROLLLib.X509KeyUsageFlags;

namespace NoFuture.Encryption
{
    public class NfX509
    {
        /// <summary>
        /// Creates a self-signed cert.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="notAfter"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://blogs.msdn.microsoft.com/alejacma/2008/09/05/how-to-create-a-certificate-request-with-certenroll-and-net-c/
        /// http://stackoverflow.com/questions/13806299/how-to-create-a-self-signed-certificate-using-c
        /// https://technet.microsoft.com/es-es/aa379410
        /// </remarks>
        public static X509Certificate2 CreateSelfSignedCert(string subject, DateTime notAfter, String pwd)
        {
            var dn = new CX500DistinguishedName();
            dn.Encode("CN=" + subject);

            var privateKey = new CX509PrivateKey
            {
                ProviderName = "Microsoft Base Cryptographic Provider v1.0",
                MachineContext = true,
                Length = 2048,
                KeySpec = X509KeySpec.XCN_AT_SIGNATURE,
                KeyUsage = X509PrivateKeyUsageFlags.XCN_NCRYPT_ALLOW_ALL_USAGES,
                ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG
            };
            privateKey.Create();

            var hashobj = new CObjectId();
            hashobj.InitializeFromAlgorithmName(ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID,
                ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY, AlgorithmFlags.AlgorithmFlagsNone, "SHA512");

            var keyUsage = new CX509ExtensionKeyUsage();

            keyUsage.InitializeEncode(X509KeyUsageFlags.XCN_CERT_DIGITAL_SIGNATURE_KEY_USAGE |
                                      X509KeyUsageFlags.XCN_CERT_DATA_ENCIPHERMENT_KEY_USAGE);

            //add extended key usage 
            var serverAuth = new CObjectId();
            serverAuth.InitializeFromValue("1.3.6.1.5.5.7.3.1");

            var fileCrypt = new CObjectId();
            fileCrypt.InitializeFromValue("1.3.6.1.4.1.311.10.3.4");

            var docSign = new CObjectId();
            docSign.InitializeFromValue("1.3.6.1.4.1.311.10.3.12");

            var oidList = new CObjectIds { serverAuth, fileCrypt, docSign };
            var eku = new CX509ExtensionEnhancedKeyUsage();
            eku.InitializeEncode(oidList);

            var cert = new CX509CertificateRequestCertificate();
            cert.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextMachine, privateKey, "");
            cert.Subject = dn;
            cert.Issuer = dn;
            cert.NotBefore = DateTime.Now;
            cert.NotAfter = notAfter;
            cert.X509Extensions.Add((CX509Extension)keyUsage);
            cert.X509Extensions.Add((CX509Extension)eku);
            cert.HashAlgorithm = hashobj;
            cert.Encode();

            var enroll = new CX509Enrollment();
            enroll.InitializeFromRequest(cert);
            enroll.CertificateFriendlyName = subject;
            
            var csr = enroll.CreateRequest();

            enroll.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedCertificate, csr,
                EncodingType.XCN_CRYPT_STRING_BASE64, pwd);

            var b64Encode = enroll.CreatePFX(pwd, PFXExportOptions.PFXExportChainWithRoot);

            var managedX509Cert = new X509Certificate2(Convert.FromBase64String(b64Encode), pwd, X509KeyStorageFlags.Exportable);

            return managedX509Cert;
        }

        /// <summary>
        /// Export the cert to a Base64 string
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string ExportToCer(X509Certificate2 cert, String pwd)
        {
            var bldr = new StringBuilder();
            bldr.AppendLine("-----BEGIN CERTIFICATE-----");
            bldr.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert, pwd),
                Base64FormattingOptions.InsertLineBreaks));
            bldr.AppendLine("-----END CERTIFICATE-----");

            return bldr.ToString();
        }

        /// <summary>
        /// Encrypts a file at <see cref="path"/> using the base64 encoded cert at <see cref="certCerPath"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="certCerPath"></param>
        /// <param name="certPassword"></param>
        /// <returns></returns>
        public static string EncryptFile(string path, string certCerPath, string certPassword)
        {
            //test inputs
            TestEnDeCryptInputs(path, certCerPath, certPassword);

            //import the cert
            var cert = new X509Certificate2();
            cert.Import(certCerPath, certPassword, X509KeyStorageFlags.Exportable);
            var pubKey = cert.PublicKey.Key;

            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;

                using (var trns = aes.CreateEncryptor())
                {
                    var keyFm = new RSAPKCS1KeyExchangeFormatter(pubKey);

                    var enKey = keyFm.CreateKeyExchange(aes.Key, aes.GetType());
                    

                    
                }


            }

            throw new NotImplementedException();
        }

        public static string DecryptFile(string path, string certCerPath, string certPassword)
        {
            //test inputs
            TestEnDeCryptInputs(path, certCerPath, certPassword);

            //import the cert
            var cert = new X509Certificate2();
            cert.Import(certCerPath, certPassword, X509KeyStorageFlags.Exportable);
            var privKey = cert.PrivateKey;


            throw new NotImplementedException();
        }

        private static void TestEnDeCryptInputs(string path, string certCerPath, string certPassword)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(path);
            if (string.IsNullOrWhiteSpace(certCerPath))
                throw new ArgumentNullException(certCerPath);
            if (string.IsNullOrWhiteSpace(certPassword))
                throw new ArgumentNullException(certPassword);

            if (!File.Exists(path))
                throw new ItsDeadJim(string.Format("There is no file at {0}", path));
            if (!File.Exists(certCerPath) || string.Equals(Path.GetExtension(certCerPath), ".cer", StringComparison.OrdinalIgnoreCase))
                throw new ItsDeadJim(string.Format("There doesn't appear to be a base64 encoded certificate at '{0}'", certCerPath));            
        }
    }
}
