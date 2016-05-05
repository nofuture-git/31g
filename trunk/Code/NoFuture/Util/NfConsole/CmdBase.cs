using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace NoFuture.Util.NfConsole
{
    public abstract class CmdBase<T>
    {
        protected byte[] EmptyBytes = {(byte) '\0'};

        protected readonly Program MyProgram;

        protected CmdBase(Program p)
        {
            MyProgram = p;
        }

        protected CmdBase()
        {
            
        } 

        public byte[] EncodedResponse(T rspn)
        {
            try
            {
                var objOut = JsonConvert.SerializeObject(rspn, Formatting.None,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

                var jsonChars = objOut.ToCharArray();
                var encoder = Encoding.UTF8.GetEncoder();
                var bufferOut = new byte[encoder.GetByteCount(jsonChars, 0, jsonChars.Length, false)];
                encoder.GetBytes(jsonChars, 0, jsonChars.Length, bufferOut, 0, false);

                WriteOutputToDisk(bufferOut);

                return bufferOut;
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return EmptyBytes;
            }
        }
        public abstract byte[] Execute(byte[] arg);

        public void WriteOutputToDisk(byte[] bytes)
        {
            var tn = this.GetType().FullName;
            var dir = MyProgram == null ? TempDirectories.AppData : MyProgram.LogDirectory;
            File.WriteAllBytes(Path.Combine(dir, tn + ".json"), bytes);
        }
    }
}
