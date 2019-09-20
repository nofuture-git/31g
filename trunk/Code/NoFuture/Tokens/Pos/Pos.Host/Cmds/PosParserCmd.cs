using System;
using System.Text;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tokens.Pos.Host.Cmds
{
    public class PosParserCmd : CmdBase<string>, ICmd
    {
        public PosParserCmd(Util.NfConsole.Program myProgram): base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            try
            {
                if (arg == null || arg.Length <= 0)
                {
                    MyProgram.PrintToConsole("no data was received");
                    return EmptyBytes;
                }

                var text = Encoding.UTF8.GetString(arg);

                MyProgram.PrintToConsole($"data received, {text.Length} chars long");

                var taggedText = text.ToTaggedString();

                MyProgram.PrintToConsole($"Part-Of-Speech tagger complete");

                var bufferout = Encoding.UTF8.GetBytes(taggedText);

                WriteOutputToDisk(bufferout, ".txt");

                return bufferout;
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return EmptyBytes;
            }
        }
    }
}
