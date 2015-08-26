using System;

namespace NoFuture.Hbm.Command.Receivers
{
    [Serializable]
    public class ReceiverStatus : IReceiverStatus
    {
        public Exception Error { get; set; }
        public string CommandResponseCode { get; set; }
    }
}
