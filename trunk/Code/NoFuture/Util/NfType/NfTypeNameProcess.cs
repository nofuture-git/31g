using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoFuture.Antlr.DotNetIlTypeName;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.NfConsole;
using NoFuture.Util.NfType.InvokeCmds;

namespace NoFuture.Util.NfType
{
    /// <summary>
    /// This is to both launch and have a comm-link to a remote process on the localhost
    /// which can receive raw IL typenames and return their parsed results.
    /// 
    /// Its original intent was to isolate the ANTLR dependencies from the NoFuture.Util;
    /// however, some time later ANTLRv4 was made to stand on its own without the enormous 
    /// amount of dependencies on the IKVM.OpenJDK assemblies (28 assemblies at 51MB).
    /// 
    /// Nevertheless, this still works as intended even though the need of that intent is lost.
    /// </summary>
    public class NfTypeNameProcess : InvokeConsoleBase
    {
        public const string GET_NF_TYPE_NAME_CMD_SWITCH = "getNfTypeName";
        public static int DefaultPort = NfConfig.NfDefaultPorts.NfTypeNamePort;
        private static Dictionary<string, NfTypeNameParseItem> _cacheFromProc = new Dictionary<string, NfTypeNameParseItem>();

        private readonly InvokeGetNfTypeName _invokeCmd;

        /// <summary>
        /// The ctor both instantiates this type and launches the remote process which does 
        /// the actual parsing.  
        /// </summary>
        /// <param name="port"></param>
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

        /// <summary>
        /// The capital method of this type which hides all the open-socket communications
        /// from the caller making it appear as if this method is the actualy performer of 
        /// the parse when it is, in fact, the remote process its in communication with.
        /// </summary>
        /// <param name="ilTypeName"></param>
        /// <returns></returns>
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
