using System.Text;

namespace NoFuture.Host.Encryption.Sjcl
{
    public class PkToCipherTextCommand : ICommand
    {
        private readonly string _pkPublicKey;
        public PkToCipherTextCommand(string publicKey)
        {
            _pkPublicKey = publicKey;
        }
        public byte[] Execute(byte[] arg)
        {
            var plainText = Encoding.UTF8.GetString(arg);
            var cipherText = NoFuture.Encryption.Sjcl.PublicKey.ToEncryptedText(plainText, _pkPublicKey);
            return Encoding.UTF8.GetBytes(cipherText.ToString());
        }
    }
}
