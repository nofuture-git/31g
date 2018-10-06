using System;
using System.IO;
using Newtonsoft.Json;

namespace NoFuture.Util.DotNetMeta.TokenId
{
    /// <summary>
    /// Bundler type for <see cref="MetadataTokenId"/>
    /// </summary>
    [Serializable]
    public class TokenIdResponse
    {
        public string Msg { get; set; }
        public MetadataTokenId[] Tokens { get; set; }
        public MetadataTokenStatus St { get; set; }

        public static TokenIdResponse ReadFromFile(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return new TokenIdResponse();
            var jsonContent = File.ReadAllText(fullFileName);
            return JsonConvert.DeserializeObject<TokenIdResponse>(jsonContent);
        }

        public MetadataTokenId GetAsRoot()
        {
            return new MetadataTokenId {Items = Tokens};
        }
    }
}