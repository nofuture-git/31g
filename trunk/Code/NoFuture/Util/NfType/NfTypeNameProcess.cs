using System.Collections.Generic;
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
        public static int DefaultPort = NfConfig.NfDefaultPorts.NfTypeNamePort;
        private static Dictionary<string, NfTypeNameParseItem> _cacheFromProc = new Dictionary<string, NfTypeNameParseItem>();

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
                    SocketPort = port.GetValueOrDefault(DefaultPort)
                };
                return;
            }

            if (string.IsNullOrWhiteSpace(NfConfig.CustomTools.InvokeNfTypeName) || !File.Exists(NfConfig.CustomTools.InvokeNfTypeName))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Tokens.InvokeNfTypeName.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.InvokeNfTypeName.");

            var cmdPort = port.GetValueOrDefault(DefaultPort);
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
            if (_cacheFromProc.ContainsKey(ilTypeName) && _cacheFromProc[ilTypeName] != null)
                return _cacheFromProc[ilTypeName];

            var nftnpi = _invokeCmd.Receive(ilTypeName);
            
            _cacheFromProc.Add(ilTypeName, nftnpi);
                
            return nftnpi;
        }
    }
}
