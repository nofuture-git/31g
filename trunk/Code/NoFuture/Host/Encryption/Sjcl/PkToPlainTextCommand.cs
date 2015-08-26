using System;
using System.Text;
using NoFuture.Shared;

namespace NoFuture.Host.Encryption.Sjcl
{
    public class PkToPlainTextCommand : ICommand
    {
        private readonly string _pkPrivateKey;
        public PkToPlainTextCommand(string privateKey)
        {
            _pkPrivateKey = privateKey;
        }
        public byte[] Execute(byte[] arg)
        {
            var cipherTextString = Encoding.UTF8.GetString(arg);
            CipherText cipherText;
            if(!CipherText.TryParse(cipherTextString, out cipherText))
                throw new ArgumentException(string.Format("the cipher text could not be parsed \n '{0}'", cipherTextString));

            var plainText = NoFuture.Encryption.Sjcl.PublicKey.ToPlainText(cipherText, _pkPrivateKey);
            return Encoding.UTF8.GetBytes(plainText);
        }
    }
}
