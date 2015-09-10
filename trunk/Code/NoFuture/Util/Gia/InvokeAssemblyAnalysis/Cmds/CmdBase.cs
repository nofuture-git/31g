﻿using System;
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
                var objOut = JsonConvert.SerializeObject(rspn, Formatting.None,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

                var jsonChars = objOut.ToCharArray();
                var encoder = Encoding.UTF8.GetEncoder();
                var bufferOut = new byte[encoder.GetByteCount(jsonChars, 0, jsonChars.Length, false)];
                encoder.GetBytes(jsonChars, 0, jsonChars.Length, bufferOut, 0, false);

                return bufferOut;
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
