using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    public class InvokeReassignTokenNames : InvokeGetCmdBase<TokenReassignResponse>
    {
        public TokenReassignRequest Request { get; set; }

        public override TokenReassignResponse Receive(object anything)
        {
            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!Net.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var rqst = Request ?? anything as TokenReassignRequest;
            if(rqst == null)
                throw new ItsDeadJim("The request object is not assigned.");
            var bufferIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rqst));

            var bufferOut = Net.SendToLocalhostSocket(bufferIn, SocketPort);

            if (bufferOut == null || bufferOut.Length <= 0)
                throw new ItsDeadJim(
                    $"The remote process by id [{ProcessId}] did not return anything on port [{SocketPort}]");

            return JsonConvert.DeserializeObject<TokenReassignResponse>(ConvertJsonFromBuffer(bufferOut), JsonSerializerSettings);
        }
    }
}
