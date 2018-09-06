using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Shared.DotNetMeta;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeCmds
{
    public class InvokeGetTokenPageRank : InvokeGetCmdBase<TokenPageRanks>
    {
        public override TokenPageRanks Receive(object anything)
        {
            if(anything == null)
                throw new ArgumentNullException(nameof(anything));
            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var pageRank = anything as TokenIds;
            if (pageRank == null)
                throw new InvalidCastException("Was expecting the 'anything' arg to be castable " +
                                               "to " + typeof(TokenIds).Name);

            var bufferIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pageRank));
            var bufferOut = Net.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");

            return JsonConvert.DeserializeObject<TokenPageRanks>(ConvertJsonFromBuffer(bufferOut),
                JsonSerializerSettings);
        }
    }
}
