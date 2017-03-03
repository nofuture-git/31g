using System.IO;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;
using NoFuture.Util.NfType.InvokeCmds;

namespace NoFuture.Util.NfType
{
    public class NfTypeNameProcess : InvokeConsoleBase
    {
        public const string GET_NF_TYPE_NAME_CMD_SWITCH = "getNfTypeName";
        public static int DF_NF_TYPENAME_PORT = NfConfig.NfDefaultPorts.NfTypeNamePort;

        private readonly InvokeGetNfTypeName _invokeCmd;


        public NfTypeNameProcess(int? port)
        {
            //is there one already running?
            MyProcess =
                System.Diagnostics.Process.GetProcessesByName("NoFuture.Tokens.InvokeNfTypeName").FirstOrDefault();
            if (MyProcess != null)
            {
                _invokeCmd = new InvokeGetNfTypeName
                {
                    ProcessId = MyProcess.Id,
                    SocketPort = port.GetValueOrDefault(DF_NF_TYPENAME_PORT)
                };
                return;
            }

            if (string.IsNullOrWhiteSpace(NfConfig.CustomTools.InvokeNfTypeName) || !File.Exists(NfConfig.CustomTools.InvokeNfTypeName))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Tokens.InvokeNfTypeName.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.InvokeNfTypeName.");

            var cmdPort = port.GetValueOrDefault(DF_NF_TYPENAME_PORT);
            var args = ConsoleCmd.ConstructCmdLineArgs(GET_NF_TYPE_NAME_CMD_SWITCH, cmdPort.ToString());

            MyProcess = StartRemoteProcess(NfConfig.CustomTools.InvokeNfTypeName, args);

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
