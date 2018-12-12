using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class InvokeGetTokenTypes : InvokeGetCmdBase<TokenTypeResponse>
    {
        public string RecurseAnyAsmNamedLike { get; set; }
        public override TokenTypeResponse Receive(object anything)
        {
            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!NfNet.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var rqst = new TokenTypeRequest {ResolveAllNamedLike = RecurseAnyAsmNamedLike};
            var json = JsonConvert.SerializeObject(rqst);
            var bufferIn = Encoding.UTF8.GetBytes(json);

            var bufferOut = NfNet.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");

            json = ConvertJsonFromBuffer(bufferOut);
            return JsonConvert.DeserializeObject<TokenTypeResponse>(json, JsonSerializerSettings);
        }
    }
}
