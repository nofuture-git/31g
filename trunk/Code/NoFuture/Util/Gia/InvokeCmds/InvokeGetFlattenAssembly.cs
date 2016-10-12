using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Exceptions;

namespace NoFuture.Util.Gia.InvokeCmds
{
    public class InvokeGetFlattenAssembly : InvokeGetCmdBase<FlattenAssembly>
    {
        public override FlattenAssembly Receive(object anything)
        {
            if(anything == null)
                throw new ArgumentNullException(nameof(anything));

            if (String.IsNullOrWhiteSpace(anything.ToString()) || !File.Exists(anything.ToString()))
                throw new ItsDeadJim("This isn't a valid assembly path " + anything);

            if (!IsMyProcessRunning(ProcessId))
                throw new ItsDeadJim("The NoFuture.Util.Gia.InvokeFlatten.exe is either dead or was never started.");

            if(!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var bufferOut = Net.SendToLocalhostSocket(Encoding.UTF8.GetBytes(anything.ToString()), SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");

            return JsonConvert.DeserializeObject<FlattenAssembly>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }
    }
}
