using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Antlr.DotNetIlTypeName;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.NfType.InvokeCmds
{
    public class InvokeGetNfTypeName : InvokeGetCmdBase<NfTypeNameParseItem>
    {
        public override NfTypeNameParseItem Receive(object anything)
        {
            if (anything == null)
                throw new ArgumentNullException(nameof(anything));

            if (string.IsNullOrWhiteSpace(anything.ToString()))
                throw new ItsDeadJim("This isn't a valid assembly path " + anything);

            if (!IsMyProcessRunning(ProcessId))
                throw new ItsDeadJim("The NoFuture.Tokens.InvokeNfTypeName.exe is either dead or was never started.");

            if (!NfNet.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var bufferOut = NfNet.SendToLocalhostSocket(Encoding.UTF8.GetBytes(anything.ToString()), SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");

            return JsonConvert.DeserializeObject<NfTypeNameParseItem>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }
    }
}
