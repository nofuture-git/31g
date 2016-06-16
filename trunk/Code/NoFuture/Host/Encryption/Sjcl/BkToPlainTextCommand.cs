using System;
using System.Text;
using NoFuture.Shared;

namespace NoFuture.Host.Encryption.Sjcl
{
    public class BkToPlainTextCommand : Util.NfConsole.ICmd
    {
        private readonly string _bulkCipherKey;
        public BkToPlainTextCommand(string bulkCipherKey)
        {
            _bulkCipherKey = bulkCipherKey;
        }
        public byte[] Execute(byte[] arg)
        {
            var cipherTextString = Encoding.UTF8.GetString(arg);
            CipherText cipherText;
            if(!CipherText.TryParse(cipherTextString, out cipherText))
                throw new ArgumentException(string.Format("the cipher text could not be parsed \n '{0}'",cipherTextString));

            var plainText = NoFuture.Encryption.Sjcl.BulkCipherKey.ToPlainText(cipherText, _bulkCipherKey);
            return Encoding.UTF8.GetBytes(plainText);
        }
    }
}
