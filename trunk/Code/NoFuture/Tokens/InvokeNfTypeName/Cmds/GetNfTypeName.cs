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
            MyProgram.PrintToConsole("GetNfTypeName invoked");

            try
            {
                if(arg == null || arg.Length <= 0)
                    throw new ItsDeadJim("No Type Name given to parse.");

                var f = Etx.ParseIl(Encoding.UTF8.GetString(arg));
                return JsonEncodedResponse(f);

            }
            catch (Exception ex)
            {
                Console.WriteLine('\n');
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new NfTypeNameParseItem() {Error = ex});
            }

        }
    }
}
