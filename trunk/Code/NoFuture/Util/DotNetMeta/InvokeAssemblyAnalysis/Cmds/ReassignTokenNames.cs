using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds
{
    public class ReassignTokenNames : CmdBase<TokenReassignResponse>, ICmd
    {
        public ReassignTokenNames(Program myProgram) : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole();
            MyProgram.PrintToConsole($"{nameof(ReassignTokenNames)} invoked");
            MyProgram.ProgressMessageState = null;
            try
            {
                var json = Encoding.UTF8.GetString(arg);
                var rqst = JsonConvert.DeserializeObject<TokenReassignRequest>(json);
                var subj = rqst.SubjectTokenNames;
                var frgn = rqst.ForeignTokenNames;
                var typs = rqst.ForeignTokenTypes;
                if (subj == null || typs == null)
                {
                    return JsonEncodedResponse(new TokenReassignResponse
                    {
                        Msg = "Both the SubjectTokenNames and ForeignTokenTypes are required.",
                        St = MetadataTokenStatus.Error
                    });
                }

                subj.ReassignAllInterfaceTokens(typs, MyProgram.PrintToConsole, frgn);
                var rspn = new TokenReassignResponse {Names = subj.Items};
                return JsonEncodedResponse(rspn);
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new TokenReassignResponse
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }
    }
}
