using System;
using System.IO;
using Newtonsoft.Json;

namespace NoFuture.Util.NfConsole
{
    public abstract class InvokeGetCmdBase<T> : InvokeCmdBase, IInvokeCmd<T>
    {
        public int SocketPort { get; set; }
        public int ProcessId { get; set; }
        public abstract T Receive(object anything);

        public T LoadFromDisk(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                return default(T);
            if (!File.Exists(filePath))
                return default(T);
            var buffer = File.ReadAllBytes(filePath);
            return JsonConvert.DeserializeObject<T>(ConvertJsonFromBuffer(buffer), JsonSerializerSettings);
        }
    }
}
