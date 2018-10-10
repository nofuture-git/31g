using System;
using System.IO;
using System.Linq;
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

        public static void SaveToFile(string filePath, MetadataTokenId rootTokenName)
        {
            if (rootTokenName?.Items == null || !rootTokenName.Items.Any())
                return;
            var json = JsonConvert.SerializeObject(rootTokenName, Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(filePath, json);
        }

        public MetadataTokenId GetAsRoot()
        {
            return new MetadataTokenId {Items = Tokens};
        }
    }
}