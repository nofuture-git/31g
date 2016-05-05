using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.NfConsole
{
    public interface IInvokeCmd<out T>
    {
        int SocketPort { get; set; }
        int ProcessId { get; set; }
        T Receive(object anything);
        T LoadFromDisk(string filePath);
    }
}
