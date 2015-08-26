namespace NoFuture.Hbm.Command.Receivers
{
    public interface IReceiverStatus
    {
        System.Exception Error { get; set; }
        string CommandResponseCode { get; set; }
    }
}
