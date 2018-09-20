using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NoFuture.Util.DotNetMeta
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

        public int Count => Names.Length;
        public bool IsReadOnly => false;

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
                t.ApplyFullName(asmIndicies);
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

        public TokenNames FlattenToDistinct()
        {
            var innerItems = new List<MetadataTokenName>();
            if (Names == null || !Names.Any())
                return new TokenNames {Names = innerItems.ToArray()};

            foreach (var name in Names)
            {
                innerItems.AddRange(name.FlattenToDistinct());
            }

            return new TokenNames { Names = innerItems.Distinct(new MetadataTokenNameComparer()).ToArray()};
        }

        /// <summary>
        /// Gets the names which are exclusive to <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <param name="rightListTopLvlOnly"></param>
        /// <returns></returns>
        public TokenNames GetRightSetDiff(TokenNames otherNames, bool rightListTopLvlOnly = false)
        {
            if (otherNames == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Names == null || rightList.Names.Length <= 0)
                return this;
            if (leftList.Names == null || leftList.Names.Length <= 0)
                return rightList;

            var setOp = rightList.Names.Select(hashCode).Except(leftList.Names.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                var k = rightList.Names.FirstOrDefault(x => hashCode(x) == j);
                if (k == null || rightListTopLvlOnly && k.OwnAsmIdx != 0)
                    continue;
                listOut.Add(k);
            }

            return new TokenNames { Names = listOut.ToArray() };
        }

        /// <summary>
        /// Joins the distinct names of this instance to the names of <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public TokenNames GetUnion(TokenNames otherNames)
        {
            if (otherNames == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            var d = rightList.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);
            var e = leftList.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new TokenNames { Names = d.Values.ToArray() };
        }

        /// <summary>
        /// Gets the names which are shared between this instance and <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public TokenNames GetIntersect(TokenNames otherNames)
        {
            if (otherNames == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Names == null || rightList.Names.Length <= 0)
                return this;
            if (leftList.Names == null || leftList.Names.Length <= 0)
                return rightList;

            var setOp = rightList.Names.Select(hashCode).Intersect(leftList.Names.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                //should be in either list
                var k = leftList.Names.FirstOrDefault(x => hashCode(x) == j);
                if (k == null)
                    continue;
                listOut.Add(k);
            }

            return new TokenNames { Names = listOut.ToArray() };
        }

        public TokenNames FilterOnTypeNames(params string[] typenames)
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Names)
            {
                var tname = name.GetTypeName();
                if (typenames.Contains(tname))
                    continue;
                names.Add(name);
            }

            return new TokenNames {Names = names.ToArray()};
        }

        public TokenNames FilterOnNamespaceNames(params string[] namespaceNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes items from <see cref="Names"/> which do not have 
        /// both &apos;get_[...]&apos; and &apos;set_[...]&apos; method for the 
        /// same property (by name)
        /// </summary>
        /// <returns></returns>
        public TokenNames RemovePropertiesWithoutBothGetAndSet()
        {
            throw new NotImplementedException();
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