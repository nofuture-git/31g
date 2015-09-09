using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public abstract class CmdBase<T> : ICmd
    {
        protected byte[] EmptyBytes = {(byte) '\0'};

        public byte[] EncodedResponse(T rspn)
        {
            try
            {
                return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rspn));
            }
            catch (Exception ex)
            {
                Program.PrintToConsole(ex);
                return EmptyBytes;
            }
        }
        public abstract byte[] Execute(byte[] arg);
    }
}
