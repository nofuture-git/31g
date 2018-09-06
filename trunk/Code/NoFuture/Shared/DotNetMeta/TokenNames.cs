using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NoFuture.Shared.DotNetMeta
{
    /// <summary>
    /// Bundler type for <see cref="MetadataTokenName"/>
    /// </summary>
    [Serializable]
    public class TokenNames
    {
        public string Msg;
        public MetadataTokenStatus St;
        public MetadataTokenName[] Names;

        /// <summary>
        /// Given the <see cref="asmIndicies"/> each <see cref="MetadataTokenName.Name"/>
        /// is reassigned to the assembly name matched on the <see cref="MetadataTokenName.OwnAsmIdx"/>.
        /// </summary>
        /// <param name="asmIndicies"></param>
        public void ApplyFullName(AsmIndicies asmIndicies)
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
                if (!t.IsPartialName())
                    continue;
                var asm = asmIndicies.Asms.FirstOrDefault(x => x.IndexId == t.OwnAsmIdx);
                if (asm == null)
                    continue;
                var asmName = new AssemblyName(asm.AssemblyName);
                t.Name = asmName.Name + t.Name;
            }
        }

        public TokenIds Convert2TokenIds()
        {
            if(Names == null || !Names.Any())
                return new TokenIds();

            //use the existing logic for flattening based on token Ids, not token names
            var distTokens = Names.Select(d => d.Convert2MetadataTokenId()).ToArray();
            return new TokenIds {St = MetadataTokenStatus.Ok, Tokens = distTokens};
        }

        /// <summary>
        /// Final analysis to merge the token Ids to thier names in a hierarchy.
        /// </summary>
        /// <param name="tokenIds"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal TokenNames GetFullCallStackTree(MetadataTokenId[] tokenIds)
        {
            if (St == MetadataTokenStatus.Error || Names == null || Names.Length <= 0)
                return null;

            var nameMapping = new List<MetadataTokenName>();
            foreach (var tokenId in tokenIds)
                nameMapping.Add(GetNameMapping(tokenId));
            return new TokenNames { Names = nameMapping.ToArray() };
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal MetadataTokenName GetNameMapping(MetadataTokenId tokenId)
        {
            if (tokenId == null || Names == null)
                return null;
            var nm = new MetadataTokenName {Id = tokenId.Id, RslvAsmIdx = tokenId.RslvAsmIdx};
            var frmNm = Names.FirstOrDefault(x => x.Id == tokenId.Id && x.RslvAsmIdx == tokenId.RslvAsmIdx);
            if (frmNm == null)
                return nm;

            nm.OwnAsmIdx = frmNm.OwnAsmIdx;
            nm.Label = frmNm.Label;
            nm.Name = frmNm.Name;

            if (tokenId.Items == null || tokenId.Items.Length <= 0)
                return nm;

            var hs = new List<MetadataTokenName>();
            foreach (var tToken in tokenId.Items)
            {
                var nmTokens = GetNameMapping(tToken);
                if (nmTokens == null)
                    continue;
                hs.Add(nmTokens);
            }

            nm.Items = hs.ToArray();

            return nm;
        }
    }
}