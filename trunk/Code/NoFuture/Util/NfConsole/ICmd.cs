namespace NoFuture.Util.NfConsole
{
    public interface ICmd
    {
        byte[] Execute(byte[] arg);
    }
}
