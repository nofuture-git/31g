using System.Text;

namespace NoFuture.Host.Encryption.Sjcl
{
    public class BkToCipherTextCommand : ICommand
    {
        private readonly string _bulkCipherKey;
        public BkToCipherTextCommand(string bulkCipherKey)
        {
            _bulkCipherKey = bulkCipherKey;
        }
        public byte[] Execute(byte[] arg)
        {
            var plainText = Encoding.UTF8.GetString(arg);
            var cipherText = NoFuture.Encryption.Sjcl.BulkCipherKey.ToEncryptedText(plainText, _bulkCipherKey);
            return Encoding.UTF8.GetBytes(cipherText.ToString());
        }
    }
}
