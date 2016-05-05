using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeCmds
{
    public class InvokeGetAsmIndicies : InvokeCmdBase, IInvokeCmd<AsmIndicies>
    {
        public int SocketPort { get; set; }
        public int ProcessId { get; set; }
        public AsmIndicies Receive(object anything)
        {
            if(anything == null)
                throw new ArgumentNullException("anything");

            if (String.IsNullOrWhiteSpace(anything.ToString()) || !File.Exists(anything.ToString()))
                throw new ItsDeadJim("This isn't a valid assembly path");

            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee(String.Format("The process by id [{0}] has exited", ProcessId));

            if (!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var bufferIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(anything));
            var bufferOut = Net.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    String.Format("The remote process by id [{0}] did not return anything on port [{1}]",
                        ProcessId, SocketPort));

            return JsonConvert.DeserializeObject<AsmIndicies>(ConvertJsonFromBuffer(bufferOut),
                JsonSerializerSettings);
        }

        public AsmIndicies LoadFromDisk(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                return null;
            if (!File.Exists(filePath))
                return null;
            var buffer = File.ReadAllBytes(filePath);
            return JsonConvert.DeserializeObject<AsmIndicies>(ConvertJsonFromBuffer(buffer), JsonSerializerSettings);
        }

    }
}
