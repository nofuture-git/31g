using System;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public abstract class CmdBase<T> : ICmd
    {
        protected byte[] EmptyBytes = {(byte) '\0'};

        public byte[] EncodedResponse(T rspn)
        {
            try
            {
                return Encoding.UTF8.GetBytes(
                                    Newtonsoft.Json.JsonConvert.SerializeObject(rspn));
            }
            catch (Exception ex)
            {
                Program.PrintToConsole(ex);
                return new[] {(byte) '\0'};
            }
        }

        public abstract byte[] Execute(byte[] arg);
    }
}
