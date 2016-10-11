using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeCmds
{
    public class InvokeGetTokenIds : InvokeGetCmdBase<TokenIds>
    {
        private readonly AsmIndicies _asmIndices;

        public InvokeGetTokenIds(AsmIndicies asmIndicies)
        {
            _asmIndices = asmIndicies;
        }

        public string RecurseAnyAsmNamedLike { get; set; }
        public override TokenIds Receive(object anything)
        {
            int asmIdx;
            if(anything == null)
                throw new ArgumentNullException("anything");

            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee(String.Format("The process by id [{0}] has exited", ProcessId));

            if (!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            if(!int.TryParse(anything.ToString(), out asmIdx))
                throw new ArgumentException("Was expected an Assembly index Id");

            if(_asmIndices == null)
                throw new RahRowRagee("In a chain of responsiability - this type requires " +
                                      "an instance of AsmIndices.");

            if (_asmIndices.Asms.All(x => x.IndexId != asmIdx))
                throw new RahRowRagee(String.Format("No matching index found for {0}", asmIdx));

            var asmName = _asmIndices.Asms.First(x => x.IndexId == asmIdx).AssemblyName;

            var crit = new GetTokenIdsCriteria { AsmName = asmName, ResolveAllNamedLike = RecurseAnyAsmNamedLike };

            var bufferIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(crit));

            var bufferOut = Net.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    String.Format("The remote process by id [{0}] did not return anything on port [{1}]",
                        ProcessId, SocketPort));

            return JsonConvert.DeserializeObject<TokenIds>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }
    }
}
