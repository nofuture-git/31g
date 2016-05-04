using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class ProcessProgress : CmdBase<ProgressMessage>
    {
        private readonly TaskFactory _taskFactory = new TaskFactory();
        private readonly int _outboundPort;
        public ProcessProgress(int outboundPort)
        {
            _outboundPort = outboundPort;
        }

        public void ReportIn(ProgressMessage arg)
        {
            if (!Net.IsValidPortNumber(((IaaProgram)MyProgram).ProcessProgressCmdPort))
                return;

            var buffer = EncodedResponse(arg);
            _taskFactory.StartNew(() => Execute(buffer));
        }

        public override byte[] Execute(byte[] arg)
        {
            using (var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                server.Connect(new IPEndPoint(IPAddress.Loopback, _outboundPort));
                server.Send(arg);
                server.Close();
            }
            return new[] {(byte) '\0'};
        }
    }
}
