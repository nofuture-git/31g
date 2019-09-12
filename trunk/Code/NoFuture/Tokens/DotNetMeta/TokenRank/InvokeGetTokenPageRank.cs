using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.TokenRank
{
    public class InvokeGetTokenPageRank : InvokeGetCmdBase<TokenPageRankResponse>
    {
        public override TokenPageRankResponse Receive(object anything)
        {
            if(anything == null)
                throw new ArgumentNullException(nameof(anything));
            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!NfNet.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var pageRank = anything as TokenIdResponse;
            if (pageRank == null)
                throw new InvalidCastException("Was expecting the 'anything' arg to be castable " +
                                               "to " + typeof(TokenIdResponse).Name);
            var json = JsonConvert.SerializeObject(pageRank);
            var bufferIn = Encoding.UTF8.GetBytes(json);
            var bufferOut = NfNet.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");
            json = ConvertJsonFromBuffer(bufferOut);
            return JsonConvert.DeserializeObject<TokenPageRankResponse>(json, JsonSerializerSettings);
        }
    }
}
