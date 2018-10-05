using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.Auxx;
using NoFuture.Util.DotNetMeta.Xfer;

namespace NoFuture.Util.DotNetMeta.Grp
{
    /// <summary>
    /// Bundler type for <see cref="MetadataTokenName"/>
    /// </summary>
    [Serializable]
    public class TokenNameResponse
    {
        public string Msg;
        public MetadataTokenStatus St;
        public MetadataTokenName[] Names;

        public static TokenNameResponse ReadFromFile(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return new TokenNameResponse();
            var jsonContent = File.ReadAllText(fullFileName);
            return JsonConvert.DeserializeObject<TokenNameResponse>(jsonContent);
        }

        public MetadataTokenName GetNamesAsSingle()
        {
            return new MetadataTokenName { Items = Names };
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