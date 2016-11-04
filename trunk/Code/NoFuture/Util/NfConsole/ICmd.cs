namespace NoFuture.Util.NfConsole
{
    /// <summary>
    /// The typical Command pattern object.  
    /// This is intended to be implemented on the console\exe side.  
    /// </summary>
    public interface ICmd
    {
        byte[] Execute(byte[] arg);
    }
}
