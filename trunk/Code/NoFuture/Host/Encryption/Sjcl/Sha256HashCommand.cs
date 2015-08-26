using System.Text;

namespace NoFuture.Host.Encryption.Sjcl
{
    public class Sha256HashCommand : ICommand
    {
        private readonly string _salt;
        public Sha256HashCommand(string salt)
        {
            _salt = salt;
        }
        public byte[] Execute(byte[] arg)
        {
            var candidate = Encoding.UTF8.GetString(arg);
            var hash = NoFuture.Encryption.Sjcl.Hash.Sign(candidate, _salt);
            return Encoding.UTF8.GetBytes(hash);
        }
    }
}
