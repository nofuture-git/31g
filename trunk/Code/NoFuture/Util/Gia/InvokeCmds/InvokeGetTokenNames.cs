using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta;
using NoFuture.Util.DotNetMeta.Grp;
using NoFuture.Util.DotNetMeta.Xfer;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeCmds
{
    public class InvokeGetTokenNames : InvokeGetCmdBase<TokenNames>
    {
        public override TokenNames Receive(object anything)
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

            return JsonConvert.DeserializeObject<TokenNames>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }
    }
}
