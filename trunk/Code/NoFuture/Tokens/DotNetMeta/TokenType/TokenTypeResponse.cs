using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class TokenTypeResponse
    {
        public string Msg { get; set; }
        public MetadataTokenStatus St { get; set; }
        public MetadataTokenType[] Types { get; set; }

        public static TokenTypeResponse ReadFromFile(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return new TokenTypeResponse();
            var jsonContent = File.ReadAllText(fullFileName);
            var rspn = JsonConvert.DeserializeObject<TokenTypeResponse>(jsonContent);
            if (rspn.Types == null || !rspn.Types.Any())
            {
                var mdtts = JsonConvert.DeserializeObject<MetadataTokenType>(jsonContent);
                rspn.Types = mdtts.Items;
            }

            return rspn;
        }

        public static void SaveToFile(string filePath, MetadataTokenType rootTokenName)
        {
            if (rootTokenName?.Items == null || !rootTokenName.Items.Any())
                return;
            var json = JsonConvert.SerializeObject(rootTokenName, Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(filePath, json);
        }

        public MetadataTokenType GetAsRoot()
        {
            return new MetadataTokenType
            {
                Items = Types,
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }
    }
}
