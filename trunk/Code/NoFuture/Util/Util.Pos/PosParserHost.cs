﻿using System;
using System.IO;
using NoFuture.Exceptions;
using NoFuture.Tools;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Pos
{
    /// <summary>
    /// A handle type to create the remote and use
    /// NoFuture.Util.Pos.Host.Program process.
    /// </summary>
    public class PosParserHost : InvokeConsoleBase
    {
        private readonly InvokePosParserCmd _myCmd;
        public const int DF_START_PORT = NfDefaultPorts.PART_OF_SPEECH_PARSER_HOST;

        /// <summary>
        /// The ctor will create this instance and launch the remote process.
        /// </summary>
        /// <remarks>
        /// Requires the <see cref="CustomTools.UtilPosHost"/> to be assigned to 
        /// some location on the drive.
        /// </remarks>
        public PosParserHost()
        {
            if (String.IsNullOrWhiteSpace(CustomTools.UtilPosHost) || !File.Exists(CustomTools.UtilPosHost))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Util.Pos.Host.exe, assign " +
                                     "the global variable at NoFuture.Tools.CustomTools.UtilPosHost.");

            MyProcess = StartRemoteProcess(CustomTools.UtilPosHost, null);

            _myCmd = new InvokePosParserCmd { ProcessId = MyProcessId, SocketPort = DF_START_PORT };
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