using System;
using System.Text;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Pos
{
    public class InvokePosParserCmd : InvokeCmdBase, IInvokeCmd<TagsetBase[][]>
    {
        public int SocketPort { get; set; }
        public int ProcessId { get; set; }
        public TagsetBase[][] Receive(object anything)
        {
            if (anything == null)
                throw new ArgumentNullException(nameof(anything));
            if (!IsMyProcessRunning(ProcessId))
                throw new RahRowRagee($"The process by id [{ProcessId}] has exited");

            if (!NfNet.IsValidPortNumber(SocketPort))
                throw new ItsDeadJim("The assigned socket port is not valids " + SocketPort);

            var bufferOut = NfNet.SendToLocalhostSocket(Encoding.UTF8.GetBytes(anything.ToString()), SocketPort);
            
            var taggedText = Encoding.UTF8.GetString(bufferOut);

            TagsetBase[][] tagsOut;
            if (!PtTagset.TryParse(taggedText, out tagsOut))
                throw new RahRowRagee("was unable to parse the tagged string sent back from the" +
                                      $"remote process [{ProcessId}] on port [{SocketPort}]");

            return tagsOut;
        }

        public TagsetBase[][] LoadFromDisk(string filePath)
        {
            return PtTagset.ParseFile(filePath);
        }
    }
}
