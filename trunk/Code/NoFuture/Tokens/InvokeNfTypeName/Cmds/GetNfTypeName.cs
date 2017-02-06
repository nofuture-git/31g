using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Util;
using NoFuture.Util.NfConsole;
using NoFuture.Util.NfType;

namespace NoFuture.Tokens.InvokeNfTypeName.Cmds
{
    public class GetNfTypeName : CmdBase<NfTypeName>, ICmd
    {
        public GetNfTypeName(Program myProgram):base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            throw new NotImplementedException();
        }
    }
}
