using System;
using System.Security.Cryptography;

namespace NoFuture.Shared
{
    public class RSAPKCS1SHA512SigDesc : SignatureDescription
    {
        public const string XML_NS_DIGEST = "http://www.w3.org/2001/04/xmlenc#sha512";
        public const string XML_NS_SIG = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

        public const string SHA_512 = "SHA512";

        public RSAPKCS1SHA512SigDesc()
        {
            FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName;
            DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName;

            KeyAlgorithm = typeof(RSACryptoServiceProvider).FullName;
            DigestAlgorithm = typeof(SHA512Managed).FullName;
        }

        public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            var d = new RSAPKCS1SignatureDeformatter(key);
            d.SetHashAlgorithm(SHA_512);
            return d;
        }

        public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            var f = new RSAPKCS1SignatureFormatter(key);
            f.SetHashAlgorithm(SHA_512);
            return f;
        }
    }
}
