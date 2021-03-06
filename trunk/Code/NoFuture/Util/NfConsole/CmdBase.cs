﻿using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Core;

namespace NoFuture.Util.NfConsole
{
    /// <summary>
    /// A concrete implementation for command objects on 
    /// the console\exe side.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CmdBase<T>
    {
        #region fields
        protected byte[] EmptyBytes = {(byte) '\0'};

        protected readonly Program MyProgram;
        #endregion

        #region ctor
        protected CmdBase(Program p)
        {
            MyProgram = p;
        }

        protected CmdBase() { }
        #endregion

        #region methods
        public virtual byte[] JsonEncodedResponse(T rspn)
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

        public virtual void WriteOutputToDisk(byte[] bytes, string fileName = null, string fileExt = ".json")
        {
            
            var tn = fileName ?? GetType().Name;
            tn = Core.NfPath.SafeFilename(tn);
            var dir = MyProgram == null ? NfSettings.AppData : MyProgram.LogDirectory;
            File.WriteAllBytes(Path.Combine(dir, tn + fileExt), bytes);
        }
        #endregion
    }
}
