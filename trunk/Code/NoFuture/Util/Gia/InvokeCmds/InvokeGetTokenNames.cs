using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeCmds
{
    public class InvokeGetTokenNames : InvokeCmdBase, IInvokeCmd<TokenNames>
    {
        public int SocketPort { get; set; }
        public int ProcessId { get; set; }
        public TokenNames Receive(object anything)
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
                    String.Format("The remote process by id [{0}] did not return anything on port [{1}]",
                        ProcessId, SocketPort));

            return JsonConvert.DeserializeObject<TokenNames>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }

        public TokenNames LoadFromDisk(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                return null;
            if (!File.Exists(filePath))
                return null;
            var buffer = File.ReadAllBytes(filePath);
            return JsonConvert.DeserializeObject<TokenNames>(ConvertJsonFromBuffer(buffer), JsonSerializerSettings);
        }
    }
}
