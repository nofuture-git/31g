using System;
using System.Text;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Pos.Host.Cmds
{
    public class PosParserCmd : ICmd
    {
        private readonly NfConsole.Program _myProgram;
        public PosParserCmd(NfConsole.Program myProgram)
        {
            _myProgram = myProgram;
        }

        public byte[] Execute(byte[] arg)
        {
            try
            {
                if (arg == null || arg.Length <= 0)
                {
                    _myProgram.PrintToConsole("no data was received");
                    return new[] {(byte) '\0'};
                }

                var text = Encoding.UTF8.GetString(arg);

                _myProgram.PrintToConsole($"data received, {text.Length} chars long");

                var taggedText = text.ToTaggedString();

                _myProgram.PrintToConsole($"Part-Of-Speech tagger complete");

                return Encoding.UTF8.GetBytes(taggedText);
            }
            catch (Exception ex)
            {
                _myProgram.PrintToConsole(ex);
                return new[] { (byte)'\0' };
            }
        }
    }
}
