using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeCmds
{
    public class InvokeGetFlattenAssembly : InvokeCmdBase, IInvokeCmd<FlattenAssembly>
    {
        public int SocketPort { get; set; }
        public int ProcessId { get; set; }
        public FlattenAssembly Receive(object anything)
        {
            if(anything == null)
                throw new ArgumentNullException("anything");

            if (String.IsNullOrWhiteSpace(anything.ToString()) || !File.Exists(anything.ToString()))
                throw new ItsDeadJim("This isn't a valid assembly path " + anything);

            if (!IsMyProcessRunning(ProcessId))
                throw new ItsDeadJim("The NoFuture.Util.Gia.InvokeFlatten.exe is either dead or was never started.");

            if(!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var bufferOut = Net.SendToLocalhostSocket(Encoding.UTF8.GetBytes(anything.ToString()), SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    String.Format("The remote process by id [{0}] did not return anything on port [{1}]",
                        ProcessId, SocketPort));

            return JsonConvert.DeserializeObject<FlattenAssembly>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }

        public FlattenAssembly LoadFromDisk(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                return null;
            if (!File.Exists(filePath))
                return null;
            var buffer = File.ReadAllBytes(filePath);
            return JsonConvert.DeserializeObject<FlattenAssembly>(ConvertJsonFromBuffer(buffer), JsonSerializerSettings);
        }
    }
}
