using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace NoFuture.Shared
{
    [Serializable]
    public class MetadataToken
    {
        public int Id;
        public string Nm;

        public string Ap;

        public string Aqn;

        public string Lbl;

        public string Msg;

        public MetadataTokenStatus St; 
        
        public MetadataToken[] Items;

        public override string ToString()
        {
            var jsonSerializer = new DataContractJsonSerializer(this.GetType());
            using (var ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, this);
                var rdr = new StreamReader(ms);
                ms.Position = 0;
                return rdr.ReadToEnd();
            }
        }

        public static bool TryParse(string metadataToken, out MetadataToken parseResult)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(metadataToken);
                var jsonSerializer = new DataContractJsonSerializer(typeof (MetadataToken));
                using (var ms = new MemoryStream(data))
                {
                    parseResult = (MetadataToken) jsonSerializer.ReadObject(ms);
                    return true;
                }
            }
            catch
            {
                parseResult = null;
                return false;
            }
        }
    }

    [Serializable]
    public class MetadataTokenId
    {
        public int Id;
        public MetadataTokenId[] Items;
    }

    [Serializable]
    public class TokenIds
    {
        public string Msg;
        public MetadataTokenId[] Tokens;
        public MetadataTokenStatus St;
    }

    [Serializable]
    public class MetadataTokenName
    {
        public int Id;
        public string Name;
        public int AsmIndexId;
        public string Label;
    }

    [Serializable]
    public class TokenNames
    {
        public string Msg;
        public MetadataTokenStatus St;
        public MetadataTokenName[] Names;
    }

    [Serializable]
    public class MetadataTokenAsm
    {
        public string AssemblyName;
        public int IndexId;
    }

    [Serializable]
    public class AsmIndicies
    {
        public string Msg;
        public MetadataTokenAsm[] Asms;
        public MetadataTokenStatus St;
    }

    [Serializable]
    public enum MetadataTokenStatus
    {
        Ok,
        Error
    }
}
