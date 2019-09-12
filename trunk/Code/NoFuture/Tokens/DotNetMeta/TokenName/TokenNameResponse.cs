using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Tokens.DotNetMeta.TokenId;

namespace NoFuture.Tokens.DotNetMeta.TokenName
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
            var rspn =  JsonConvert.DeserializeObject<TokenNameResponse>(jsonContent);

            if (rspn.Names == null || !rspn.Names.Any())
            {
                var mdtn = JsonConvert.DeserializeObject<MetadataTokenName>(jsonContent);
                rspn = new TokenNameResponse {Names = mdtn.Items};
            }
            return rspn;
        }

        public static void SaveToFile(string filePath, MetadataTokenName rootTokenName)
        {
            if (rootTokenName?.Items == null || !rootTokenName.Items.Any())
                return;
            var json = JsonConvert.SerializeObject(rootTokenName, Formatting.None,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            File.WriteAllText(filePath, json);
        }

        public virtual MetadataTokenName GetAsRoot()
        {
            return new MetadataTokenName
            {
                Items = Names,
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

    }
}