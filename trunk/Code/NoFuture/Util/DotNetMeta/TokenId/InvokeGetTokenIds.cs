using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.TokenId
{
    public class InvokeGetTokenIds : InvokeGetCmdBase<TokenIdResponse>
    {
        public AsmIndexResponse AsmIndices { get; set; }

        public string RecurseAnyAsmNamedLike { get; set; }
        public override TokenIdResponse Receive(object anything)
        {
            int asmIdx;
            if(anything == null)
                throw new ArgumentNullException( nameof(anything));

            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!NfNet.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valid " + SocketPort);

            if(!int.TryParse(anything.ToString(), out asmIdx))
                throw new ArgumentException("Was expected an Assembly index Id");

            if(AsmIndices == null)
                throw new RahRowRagee("In a chain of responsibility - this type requires " +
                                      "an instance of AsmIndices.");

            if (AsmIndices.Asms.All(x => x.IndexId != asmIdx))
                throw new RahRowRagee($"No matching index found for {asmIdx}");

            var asmName = AsmIndices.Asms.First(x => x.IndexId == asmIdx).AssemblyName;

            var rqst = new TokenIdRequest { AsmName = asmName, ResolveAllNamedLike = RecurseAnyAsmNamedLike };
            var json = JsonConvert.SerializeObject(rqst);
            var bufferIn = Encoding.UTF8.GetBytes(json);

            var bufferOut = NfNet.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");
            json = ConvertJsonFromBuffer(bufferOut);
            return JsonConvert.DeserializeObject<TokenIdResponse>(json, JsonSerializerSettings);
        }
    }
}
