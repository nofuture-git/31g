using System;
using System.IO;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class TokenTypeResponse
    {
        public string Msg;
        public MetadataTokenStatus St;
        public MetadataTokenType[] Types;

        public static TokenTypeResponse ReadFromFile(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return new TokenTypeResponse();
            var jsonContent = File.ReadAllText(fullFileName);
            return JsonConvert.DeserializeObject<TokenTypeResponse>(jsonContent);
        }

        public MetadataTokenType GetTypesAsSingle()
        {
            return new MetadataTokenType {Items = Types};
        }
    }
}
