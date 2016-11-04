namespace NoFuture.Util.NfConsole
{
    /// <summary>
    /// The calling assembly side's implementation 
    /// to invoke a <see cref="ICmd"/> on the <see cref="SocketConsole"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInvokeCmd<out T>
    {
        int SocketPort { get; set; }
        int ProcessId { get; set; }
        T Receive(object anything);
        T LoadFromDisk(string filePath);
    }
}
