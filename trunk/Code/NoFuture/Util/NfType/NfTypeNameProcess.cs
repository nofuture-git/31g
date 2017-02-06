using System.IO;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Util.NfConsole;
using NoFuture.Tools;
using NoFuture.Util.NfType.InvokeCmds;

namespace NoFuture.Util.NfType
{
    public class NfTypeNameProcess : InvokeConsoleBase
    {
        public const string GET_NF_TYPE_NAME_CMD_SWITCH = "getNfTypeName";
        public const int DF_NF_TYPENAME_PORT = NfDefaultPorts.NF_TYPE_NAME_PORT;

        private readonly InvokeGetNfTypeName _invokeCmd;


        public NfTypeNameProcess(int? port)
        {
            if (string.IsNullOrWhiteSpace(CustomTools.InvokeNfTypeName) || !File.Exists(CustomTools.InvokeNfTypeName))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Tokens.InvokeNfTypeName.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.InvokeNfTypeName.");

            var cmdPort = port.GetValueOrDefault(DF_NF_TYPENAME_PORT);
            var args = ConsoleCmd.ConstructCmdLineArgs(GET_NF_TYPE_NAME_CMD_SWITCH, cmdPort.ToString());

            MyProcess =
                System.Diagnostics.Process.GetProcessesByName("NoFuture.Tokens.InvokeNfTypeName").FirstOrDefault() ??
                StartRemoteProcess(CustomTools.InvokeNfTypeName, args);

            _invokeCmd = new InvokeGetNfTypeName
            {
                ProcessId = MyProcess.Id,
                SocketPort = cmdPort
            };
        }

        public NfTypeNameParseItem GetNfTypeName(string ilTypeName)
        {
            return _invokeCmd.Receive(ilTypeName);
        }
    }
}
