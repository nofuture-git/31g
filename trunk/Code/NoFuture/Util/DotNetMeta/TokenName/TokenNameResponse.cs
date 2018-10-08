using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    /// <summary>
    /// Bundler type for <see cref="MetadataTokenName"/>
    /// </summary>
    [Serializable]
    public class TokenNameResponse
    {
        public string Msg { get; set; }
        public MetadataTokenStatus St { get; set; }
        public MetadataTokenName[] Names { get; set; }

        public static TokenNameResponse ReadFromFile(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return new TokenNameResponse();
            var jsonContent = File.ReadAllText(fullFileName);
            return JsonConvert.DeserializeObject<TokenNameResponse>(jsonContent);
        }

        public static void SaveToFile(string filePath, MetadataTokenName rootTokenName)
        {
            if (rootTokenName?.Items == null || !rootTokenName.Items.Any())
                return;
            var rspn = new TokenNameResponse {Names = rootTokenName.Items};
            var json = JsonConvert.SerializeObject(rspn, Formatting.None,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            System.IO.File.WriteAllText(filePath, json);
        }

        public MetadataTokenName GetNamesAsSingle()
        {
            return new MetadataTokenName { Items = Names };
        }

    }
}