using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.TokenAsm;
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

        internal void RemoveClrAndEmptyNames()
        {
            var sNames = GetNamesAsSingle();
            sNames.RemoveClrGeneratedNames();
            sNames.RemoveEmptyNames();
            Names = sNames.Items;
        }

        /// <summary>
        /// Given the <see cref="asmIndicies"/> each <see cref="MetadataTokenName.Name"/>
        /// is reassigned to the assembly name matched on the <see cref="MetadataTokenName.OwnAsmIdx"/>.
        /// </summary>
        /// <param name="asmIndicies"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void ApplyFullName(AsmIndexResponse asmIndicies)
        {
            if (Names == null || Names.Length <= 0)
                return;
            if (Names.All(x => !x.IsPartialName()))
                return;
            if (asmIndicies == null)
                return;
            if (St == MetadataTokenStatus.Error)
                return;
            foreach (var t in Names)
            {
                t.ApplyFullName(asmIndicies);
            }
        }

        /// <summary>
        /// Final analysis to merge the token Ids to thier names in a hierarchy.
        /// </summary>
        /// <param name="tokenIdResponse"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal MetadataTokenName BindTree2Names(TokenIdResponse tokenIdResponse)
        {
            if (Names == null || Names.Length <= 0)
                return null;

            var nameMapping = new List<MetadataTokenName>();
            foreach (var tokenId in tokenIdResponse.Tokens)
                nameMapping.Add(GetNames(tokenId));
            return new MetadataTokenName {Items = nameMapping.ToArray() };
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal MetadataTokenName GetNames(MetadataTokenId tokenId)
        {
            if (tokenId == null || Names == null)
                return null;
            var nm = new MetadataTokenName { Id = tokenId.Id, RslvAsmIdx = tokenId.RslvAsmIdx };
            var frmNm = Names.FirstOrDefault(x => x.Id == tokenId.Id && x.RslvAsmIdx == tokenId.RslvAsmIdx);
            if (frmNm == null)
                return nm;

            nm.OwnAsmIdx = frmNm.OwnAsmIdx;
            nm.Label = frmNm.Label;
            nm.Name = frmNm.Name;
            nm.IsByRef = tokenId.IsByRef > 0;
            nm.DeclTypeId = frmNm.DeclTypeId;

            if (tokenId.Items == null || tokenId.Items.Length <= 0)
                return nm;

            var hs = new List<MetadataTokenName>();
            foreach (var tToken in tokenId.Items)
            {
                var nmTokens = GetNames(tToken);
                if (nmTokens == null)
                    continue;
                hs.Add(nmTokens);
            }

            nm.Items = hs.ToArray();

            return nm;
        }
    }
}