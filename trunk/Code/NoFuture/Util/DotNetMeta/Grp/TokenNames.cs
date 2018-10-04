using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta.Auxx;
using NoFuture.Util.DotNetMeta.Xfer;

namespace NoFuture.Util.DotNetMeta.Grp
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

        public static TokenNames ReadFromFile(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return new TokenNames();
            var jsonContent = File.ReadAllText(fullFileName);
            return JsonConvert.DeserializeObject<TokenNames>(jsonContent);
        }

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

        public string[] GetUniqueTypeNames()
        {
            return Names.Select(n => n.GetTypeName()).Distinct().ToArray();
        }

        public TokenNames SelectByTypeNames(params string[] typenames)
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Names)
            {
                var tname = name.GetTypeName();
                if (typenames.Contains(tname))
                    names.Add(name);
            }

            return new TokenNames { Names = names.ToArray() };
        }

        public TokenNames SelectByNamespaceNames(params string[] namespaceNames)
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Names)
            {
                var tname = name.GetNamespaceName();
                if (namespaceNames.Contains(tname))
                    names.Add(name);
            }

            return new TokenNames { Names = names.ToArray() };
        }

        public TokenNames RemoveByTypeNames(params string[] typenames)
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

        public TokenNames RemoveByNamespaceNames(params string[] namespaceNames)
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Names)
            {
                var tname = name.GetNamespaceName();
                if (namespaceNames.Contains(tname))
                    continue;
                names.Add(name);
            }

            return new TokenNames { Names = names.ToArray() };
        }

        public void ReassignAllByRefs()
        {
            //find all the byRefs throughout
            var byRefs = new List<MetadataTokenName>();
            foreach(var nm in Names)
                nm.GetAllByRefNames(byRefs);

            if (!byRefs.Any())
                return;

            //for each byref, find it byVal counterpart
            var byVals = new List<MetadataTokenName>();
            foreach (var byRef in byRefs)
            {
                MetadataTokenName byVal = null;
                foreach (var nm in Names)
                {
                    byVal = nm.FirstByVal(byRef);
                    if (byVal == null)
                        continue;
                    byVals.Add(byVal);
                    break;
                }
            }

            if (!byVals.Any())
            {
                Console.WriteLine($"There are {byRefs.Count} ByRef names but no ByVal counterparts.");
                return;
            }

            //reassign each byRef's over to byVal
            foreach (var byVal in byVals)
            {
                foreach (var nm in Names)
                {
                    nm.ReassignAnyItemsByName(byVal);
                }
            }
        }

        /// <summary>
        /// Removes items from <see cref="Names"/> which do not have 
        /// both &apos;get_[...]&apos; and &apos;set_[...]&apos; method for the 
        /// same property (by name)
        /// </summary>
        /// <returns></returns>
        public void RemovePropertiesWithoutBothGetAndSet()
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Names)
            {
                if (!NfReflect.IsClrMethodForProperty(name.GetMemberName(), out var propName))
                {
                    names.Add(name);
                    continue;
                }

                var countOfProp = Names.Count(n => n.GetMemberName().Contains($"get_{propName}(")
                                                   || n.GetMemberName().Contains($"set_{propName}("));
                if (countOfProp < 2)
                    continue;
                names.Add(name);
            }

            Names = names.ToArray();
        }

        public void RemoveClrGeneratedNames()
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Names)
            {
                if(NfReflect.IsClrGeneratedType(name.Name))
                    continue;
                names.Add(name);
            }

            Names = names.ToArray();
        }

        /// <summary>
        /// Final analysis to merge the token Ids to thier names in a hierarchy.
        /// </summary>
        /// <param name="tokenIds"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal TokenNames GetNames(TokenIds tokenIds)
        {
            if (St == MetadataTokenStatus.Error || Names == null || Names.Length <= 0)
                return null;

            var nameMapping = new List<MetadataTokenName>();
            foreach (var tokenId in tokenIds.Tokens)
                nameMapping.Add(GetNames(tokenId));
            return new TokenNames { Names = nameMapping.ToArray() };
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal MetadataTokenName GetNames(MetadataTokenId tokenId)
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