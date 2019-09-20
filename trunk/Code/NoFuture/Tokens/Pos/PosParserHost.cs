using System;
using System.IO;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tokens.Pos
{
    /// <summary>
    /// A handle type to create the remote and use
    /// NoFuture.Tokens.Pos.Host.Program process.
    /// </summary>
    public class PosParserHost : InvokeConsoleBase
    {
        private readonly InvokePosParserCmd _myCmd;
        public static int DefaultPort = NfConfig.NfDefaultPorts.PartOfSpeechPaserHost;

        /// <summary>
        /// The ctor will create this instance and launch the remote process.
        /// </summary>
        /// <remarks>
        /// Requires the <see cref="NfConfig.CustomTools.TokensPosHost"/> to be assigned to 
        /// some location on the drive.
        /// </remarks>
        public PosParserHost()
        {
            if (String.IsNullOrWhiteSpace(NfConfig.CustomTools.TokensPosHost) || !File.Exists(NfConfig.CustomTools.TokensPosHost))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Tokens.Pos.Host.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.TokensPosHost.");

            MyProcess = StartRemoteProcess(NfConfig.CustomTools.TokensPosHost, null);

            _myCmd = new InvokePosParserCmd { ProcessId = MyProcessId, SocketPort = DefaultPort };
        }

        /// <summary>
        /// Helper method to get <see cref="someText"/> 
        /// as parsed <see cref="ITagset"/>. 
        /// Allows the calling assembly to skip having to manage
        /// the socket and invoke the subsequent Parse methods.
        /// </summary>
        /// <param name="someText"></param>
        /// <returns></returns>
        public TagsetBase[][] GetTaggedText(string someText)
        {
            return _myCmd.Receive(someText);
        }
    }
}
