using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CERTENROLLLib;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util;
using NoFuture.Util.Core;
using X509KeyUsageFlags = CERTENROLLLib.X509KeyUsageFlags;

namespace NoFuture.Encryption
{
    public class NfX509
    {
        public const string MS_CRYPTO_PROV_NAME = "Microsoft Enhanced RSA and AES Cryptographic Provider";
        public const string NF_CRYPTO_EXT = ".nfk"; //nofuture kruptos

        /// <summary>
        /// https://tools.ietf.org/html/rfc4519
        /// </summary>
        public struct DistinguishedName
        {
            public string OrgName;
            public string OrgUnit;
            public string CommonName;
            public string City;
            public string State;

            public DistinguishedName(string cn)
            {
                CommonName = String.IsNullOrWhiteSpace(cn) ? "NoFuture" : cn;
                OrgName = "NoFuture";
                OrgUnit = null;
                City = "Spook City";
                State = "NV";
            }

            /// <summary>
            /// https://www.ietf.org/rfc/rfc4514.txt
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                var dn = new List<string> {"CN=" + CommonName, "C=US"};

                if (!String.IsNullOrWhiteSpace(OrgName))
                    dn.Add("O=" + OrgName);
                if (!String.IsNullOrWhiteSpace(OrgUnit))
                    dn.Add("OU=" + OrgUnit);
                if (!String.IsNullOrWhiteSpace(City))
                    dn.Add("L=" + City);
                if (!String.IsNullOrWhiteSpace(State))
                    dn.Add("S=" + State);

                return String.Join(";", dn);
            }
        }

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
        public static X509Certificate2 CreateSelfSignedCert(DistinguishedName subject, DateTime notAfter, String pwd)
        {
            var cn = new CX500DistinguishedName();
            cn.Encode(subject.ToString(), X500NameFlags.XCN_CERT_NAME_STR_SEMICOLON_FLAG);

            var privateKey = new CX509PrivateKey
            {
                ContainerNamePrefix = "nf-",
                ProviderName = MS_CRYPTO_PROV_NAME,
                MachineContext = false,
                Length = 2048,
                KeySpec = X509KeySpec.XCN_AT_KEYEXCHANGE,
                ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_EXPORT_FLAG
            };
            privateKey.Create();

            var hashobj = new CObjectId();
            hashobj.InitializeFromAlgorithmName(ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID,
                ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY, AlgorithmFlags.AlgorithmFlagsNone,
                RSAPKCS1SHA512SigDesc.SHA_512);

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
            cert.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextUser, privateKey, "");
            cert.Subject = cn;
            cert.Issuer = cn;
            cert.NotBefore = DateTime.Now;
            cert.NotAfter = notAfter;
            cert.X509Extensions.Add((CX509Extension)keyUsage);
            cert.X509Extensions.Add((CX509Extension)eku);
            cert.HashAlgorithm = hashobj;
            cert.Encode();

            var enroll = new CX509Enrollment();
            enroll.InitializeFromRequest(cert);
            enroll.CertificateFriendlyName = subject.CommonName;
            
            var csr = enroll.CreateRequest();

            enroll.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedRoot, csr,
                EncodingType.XCN_CRYPT_STRING_BASE64, pwd);

            var b64Encode = enroll.CreatePFX(pwd, PFXExportOptions.PFXExportEEOnly);

            var managedX509Cert = new X509Certificate2(Convert.FromBase64String(b64Encode), pwd,
                X509KeyStorageFlags.Exportable);

            return managedX509Cert;
        }

        /// <summary>
        /// Export the cert to a Base64 string
        /// </summary>
        /// <param name="cert"></param>
        /// <returns></returns>
        public static string ExportToPem(X509Certificate2 cert)
        {
            var bldr = new StringBuilder();
            bldr.AppendLine("-----BEGIN CERTIFICATE-----");
            bldr.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert),
                Base64FormattingOptions.InsertLineBreaks));
            bldr.AppendLine("-----END CERTIFICATE-----");

            return bldr.ToString();
        }

        /// <summary>
        /// Encrypts a file at <see cref="path"/> using the cert at <see cref="certCerPath"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="certCerPath"></param>
        /// <returns></returns>
        public static void EncryptFile(string path, string certCerPath)
        {
            //test inputs
            TestEnDeCryptInputs(path, certCerPath, String.Empty);

            var encFile = Path.Combine(Path.GetDirectoryName(path) ?? Environment.CurrentDirectory,
                Path.GetFileName(path) + NF_CRYPTO_EXT);

            //import the cert
            var cert = new X509Certificate2(certCerPath);
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
                    var lenK = BitConverter.GetBytes(enKey.Length);
                    var lIv = BitConverter.GetBytes(aes.IV.Length);

                    using (var outFs = new FileStream(encFile, FileMode.Create))
                    {
                        outFs.Write(lenK, 0, 4);
                        outFs.Write(lIv, 0, 4);
                        outFs.Write(enKey, 0, enKey.Length);
                        outFs.Write(aes.IV,0, aes.IV.Length);

                        using (var cryptStream = new CryptoStream(outFs, trns, CryptoStreamMode.Write))
                        {
                            var blockSz = aes.BlockSize/8;
                            var data = new byte[blockSz];
                            using (var inFs = new FileStream(path, FileMode.Open))
                            {
                                var count = 0;
                                do
                                {
                                    count = inFs.Read(data, 0, blockSz);
                                    cryptStream.Write(data, 0, count);
                                } while (count > 0);
                                inFs.Close();
                            }
                            cryptStream.FlushFinalBlock();
                            cryptStream.Close();
                        }
                        outFs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts a file which was encrypted by its sister method <see cref="EncryptFile"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="certCerPath"></param>
        /// <param name="certPassword"></param>
        public static void DecryptFile(string path, string certCerPath, string certPassword)
        {
            //test inputs
            TestEnDeCryptInputs(path, certCerPath, certPassword);

            //import the cert
            var cert = new X509Certificate2();
            cert.Import(certCerPath, certPassword, X509KeyStorageFlags.Exportable);
            var privKey = cert.PrivateKey as RSACryptoServiceProvider;

            if(privKey == null)
                throw new ItsDeadJim("The private could not be resolved.");

            var plainTextFile = Path.Combine(Path.GetDirectoryName(path) ?? Environment.CurrentDirectory,
                Path.GetFileNameWithoutExtension(path) ?? NfString.GetNfRandomName());

            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;

                var bufferK = new byte[4];
                var bufferIv = new byte[4];

                using (var inFs = new FileStream(path, FileMode.Open))
                {
                    inFs.Seek(0, SeekOrigin.Begin);
                    inFs.Read(bufferK, 0, 3);
                    inFs.Seek(4, SeekOrigin.Begin);
                    inFs.Read(bufferIv, 0, 3);

                    var lenK = BitConverter.ToInt32(bufferK, 0);
                    var lenIv = BitConverter.ToInt32(bufferIv, 0);

                    var startC = lenK + lenIv + 8;
                    var enKey = new byte[lenK];
                    var iv = new byte[lenIv];

                    inFs.Seek(8, SeekOrigin.Begin);
                    inFs.Read(enKey, 0, lenK);
                    inFs.Seek(8 + lenK, SeekOrigin.Begin);
                    inFs.Read(iv, 0, lenIv);

                    var decKey = privKey.Decrypt(enKey, false);
                    using (var trans = aes.CreateDecryptor(decKey, iv))
                    {
                        using (var outFs = new FileStream(plainTextFile, FileMode.Create))
                        {
                            var blockSz = aes.BlockSize/8;
                            var data = new byte[blockSz];
                            inFs.Seek(startC, SeekOrigin.Begin);
                            using (var outCrypto = new CryptoStream(outFs, trans, CryptoStreamMode.Write))
                            {
                                var count = 0;
                                do
                                {
                                    count = inFs.Read(data, 0, blockSz);
                                    outCrypto.Write(data, 0, count);
                                } while (count > 0);

                                outCrypto.FlushFinalBlock();
                                outCrypto.Close();
                            }
                            outFs.Close();
                        }
                        inFs.Close();
                    }// end decryptor
                }// end fs in
            } //end aes managed
        }

        private static bool TestEnDeCryptInputs(string path, string certCerPath, string certPassword)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (String.IsNullOrWhiteSpace(certCerPath))
                throw new ArgumentNullException(nameof(certCerPath));
            if (certPassword == null)
                throw new ArgumentNullException(nameof(certPassword));

            if (!File.Exists(path))
                throw new ItsDeadJim($"There is no file at '{path}'");
            if (!File.Exists(certCerPath))
                throw new ItsDeadJim($"There is no certificate at '{certCerPath}'");
            return true;
        }
    }
}
