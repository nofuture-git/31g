using System;
using System.IO;
using NoFuture.Exceptions;
using NoFuture.Tools;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Pos
{
    public class PosParserHost : InvokeConsoleBase
    {
        private readonly InvokePosParserCmd _myCmd;
        public const int DF_START_PORT = 5063;
        public PosParserHost()
        {
            if (String.IsNullOrWhiteSpace(CustomTools.UtilPosHost) || !File.Exists(CustomTools.UtilPosHost))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Util.Pos.Host.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.UtilPosHost.");

            MyProcess = StartRemoteProcess(CustomTools.UtilPosHost, null);

            _myCmd = new InvokePosParserCmd { ProcessId = MyProcessId, SocketPort = DF_START_PORT };
        }

        public TagsetBase[][] GetTaggedText(string someText)
        {
            return _myCmd.Receive(someText);
        }
    }
}
