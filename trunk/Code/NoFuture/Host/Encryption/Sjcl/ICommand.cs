namespace NoFuture.Host.Encryption.Sjcl
{
    public interface ICommand
    {
        byte[] Execute(byte[] arg);
    }
}
