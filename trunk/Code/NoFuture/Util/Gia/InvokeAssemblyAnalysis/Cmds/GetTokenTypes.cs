using System;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenType;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenTypes : CmdBase<TokenTypeResponse>, ICmd
    {
        public GetTokenTypes(Program myProgram) : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole($"{nameof(GetTokenIds)} invoked");
            MyProgram.ProgressMessageState = null;
            try
            {
                //TODO get all the types data 
            }
            catch (Exception ex)
            {
                Console.Write('\n');
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(
                    new TokenTypeResponse
                    {
                        Msg = ex.Message,
                        St = MetadataTokenStatus.Error
                    });
            }

            throw new NotImplementedException();
        }
    }
}
