using System.IO;
using NoFuture.Exceptions;
using NoFuture.Util.NfConsole;
using NoFuture.Tools;
using NoFuture.Util.NfType.InvokeCmds;

namespace NoFuture.Util.NfType
{
    public class NfTypeNameProcess : InvokeConsoleBase
    {
        public const string GET_NF_TYPE_NAME_CMD_SWITCH = "";
        public const int DF_NF_TYPENAME_PORT = NfDefaultPorts.NF_TYPE_NAME_PORT;

        private readonly InvokeGetNfTypeName _invokeCmd;


        public NfTypeNameProcess(int? port)
        {
            if (string.IsNullOrWhiteSpace(CustomTools.InvokeNfTypeName) || !File.Exists(CustomTools.InvokeNfTypeName))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Tokens.InvokeNfTypeName.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.InvokeNfTypeName.");

            var cmdPort = port.GetValueOrDefault(DF_NF_TYPENAME_PORT);
            var args = ConsoleCmd.ConstructCmdLineArgs(GET_NF_TYPE_NAME_CMD_SWITCH, cmdPort.ToString());

            MyProcess = StartRemoteProcess(CustomTools.InvokeNfTypeName, args);

            _invokeCmd = new InvokeGetNfTypeName
            {
                ProcessId = MyProcess.Id,
                SocketPort = cmdPort
            };
        }

        public NfTypeName GetNfTypeName(string ilTypeName)
        {
            return _invokeCmd.Receive(ilTypeName);
        }
    }
}
