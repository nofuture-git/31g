using System;
using System.Text;
using NoFuture.Exceptions;
using NoFuture.Util.NfConsole;
using NoFuture.Util.NfType.InvokeCmds;

namespace NoFuture.Tokens.InvokeNfTypeName.Cmds
{
    public class GetNfTypeName : CmdBase<NfTypeNameParseItem>, ICmd
    {
        public GetNfTypeName(Program myProgram):base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            try
            {
                if(arg == null || arg.Length <= 0)
                    throw new ItsDeadJim("No Type Name given to parse.");
                var nm = Encoding.UTF8.GetString(arg);
                MyProgram.PrintToConsole(nm);
                var f = Etx.ParseIl(nm);
                return JsonEncodedResponse(f);

            }
            catch (Exception ex)
            {
                Console.WriteLine('\n');
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new NfTypeNameParseItem {Error = ex});
            }

        }
    }
}
