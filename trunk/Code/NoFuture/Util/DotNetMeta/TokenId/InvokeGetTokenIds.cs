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
        private readonly AsmIndexResponse _asmIndices;

        public InvokeGetTokenIds() { }

        public InvokeGetTokenIds(AsmIndexResponse asmIndicies)
        {
            _asmIndices = asmIndicies;
        }

        public string RecurseAnyAsmNamedLike { get; set; }
        public override TokenIdResponse Receive(object anything)
        {
            int asmIdx;
            if(anything == null)
                throw new ArgumentNullException( nameof(anything));

            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            if(!int.TryParse(anything.ToString(), out asmIdx))
                throw new ArgumentException("Was expected an Assembly index Id");

            if(_asmIndices == null)
                throw new RahRowRagee("In a chain of responsiability - this type requires " +
                                      "an instance of AsmIndices.");

            if (_asmIndices.Asms.All(x => x.IndexId != asmIdx))
                throw new RahRowRagee($"No matching index found for {asmIdx}");

            var asmName = _asmIndices.Asms.First(x => x.IndexId == asmIdx).AssemblyName;

            var crit = new GetTokenIdsCriteria { AsmName = asmName, ResolveAllNamedLike = RecurseAnyAsmNamedLike };

            var bufferIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(crit));

            var bufferOut = Net.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");

            return JsonConvert.DeserializeObject<TokenIdResponse>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }
    }
}
