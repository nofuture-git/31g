using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    public class InvokeGetTokenNames : InvokeGetCmdBase<TokenNameResponse>
    {
        public override TokenNameResponse Receive(object anything)
        {
            if(anything == null)
                throw new ArgumentNullException(nameof(anything));

            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var metadataTokenIds = anything as MetadataTokenId[];
            if(metadataTokenIds == null)
                throw new InvalidCastException("Was expecting the 'anything' arg to be castable " +
                                               "to an array of " + typeof(MetadataTokenId).FullName);

            var bufferIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metadataTokenIds));

            var bufferOut = Net.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");

            return JsonConvert.DeserializeObject<TokenNameResponse>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }
    }
}
