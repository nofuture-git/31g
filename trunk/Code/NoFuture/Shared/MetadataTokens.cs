﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Shared.Core;
using NoFuture.Shared.Core.Cfg;

namespace NoFuture.Shared
{
    #region token ids
    /// <summary>
    /// Defines the identity of a single metadata token.
    /// </summary>
    [Serializable]
    public class MetadataTokenId
    {
        /// <summary>
        /// The index of the <see cref="Assembly"/>  with 
        /// whose <see cref="Assembly.ManifestModule"/>
        /// should be used to resolve the <see cref="Id"/>.
        /// </summary>
        public int RslvAsmIdx;
        public int Id;
        public MetadataTokenId[] Items;

        public override bool Equals(object obj)
        {
            var objMdti = obj as MetadataTokenId;
            if (objMdti == null)
                return false;
            return objMdti.Id == Id && objMdti.RslvAsmIdx == RslvAsmIdx;
        }

        public override int GetHashCode()
        {
            return RslvAsmIdx.GetHashCode() + Id.GetHashCode();
        }

        /// <summary>
        /// Prints to token ids with <see cref="MetadataTokenId.RslvAsmIdx"/> prefixed to the front.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Print(MetadataTokenId token)
        {
            var depth = 0;
            return Print(token, ref depth);
        }

        /// <summary>
        /// Returns a list of distinct <see cref="MetadataTokenId"/> having only thier
        /// <see cref="RslvAsmIdx"/> and <see cref="Id"/> assigned.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="perserveDirectChildItems">Retains the the direct children Items</param>
        /// <returns></returns>
        public static MetadataTokenId[] FlattenToDistinct(MetadataTokenId token, bool perserveDirectChildItems = false)
        {
            var list = new HashSet<MetadataTokenId>();

            if (token == null)
                return list.ToArray();

            FlattenToDistinct(token, list, perserveDirectChildItems);
            return list.ToArray();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void FlattenToDistinct(MetadataTokenId token, HashSet<MetadataTokenId> list,
            bool perserveDirectChildItems = false)
        {
            if (token == null)
                return;
            var iToken = list.FirstOrDefault(x => x.Id == token.Id && x.RslvAsmIdx == token.RslvAsmIdx);
            var preExisting = iToken != null;
            iToken = iToken ?? new MetadataTokenId {Id = token.Id, RslvAsmIdx = token.RslvAsmIdx};
            if(!preExisting)
                list.Add(iToken);

            if (token.Items == null)
                return;

            if (perserveDirectChildItems)
            {
                var i2j = new HashSet<MetadataTokenId>();
                if (iToken.Items != null)
                {
                    foreach (var existingItem in iToken.Items)
                    {
                        if (existingItem.Equals(iToken))
                            continue;
                        i2j.Add(existingItem);
                    }
                }
                    
                foreach (var ijToken in token.Items)
                {
                    if (ijToken.Equals(iToken))
                        continue;
                    i2j.Add(new MetadataTokenId {Id = ijToken.Id, RslvAsmIdx = ijToken.RslvAsmIdx});
                }
                if (i2j.Any())
                    iToken.Items = i2j.ToArray();
            }

            foreach (var jToken in token.Items)
            {
                FlattenToDistinct(jToken, list, perserveDirectChildItems);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static string Print(MetadataTokenId token, ref int currentDepth)
        {
            if (token == null)
                return string.Empty;
            var strBldr = new StringBuilder();
            var tabs = currentDepth <= 0 ? string.Empty : new string('\t', currentDepth);
            strBldr.AppendFormat("{0}{1}.{2}\n", tabs, token.RslvAsmIdx, token.Id.ToString("X4"));
            if (token.Items == null || token.Items.Length <= 0)
                return strBldr.ToString();

            foreach (var tToken in token.Items)
            {
                currentDepth += 1;
                strBldr.Append(Print(tToken, ref currentDepth));
                currentDepth -= 1;
            }
            return strBldr.ToString();
        }
    }

    /// <summary>
    /// Bundler type for <see cref="MetadataTokenId"/>
    /// </summary>
    [Serializable]
    public class TokenIds
    {
        public string Msg;
        public MetadataTokenId[] Tokens;
        public MetadataTokenStatus St;

        /// <summary>
        /// Helper method to get all distinct token ids from the current instance.
        /// </summary>
        /// <returns></returns>
        public MetadataTokenId[] FlattenToDistinct(bool perserveDirectChildItems = false)
        {
            if (St == MetadataTokenStatus.Error || Tokens == null ||  Tokens.Length <= 0)
                return null;
            var tokenHashset = new HashSet<MetadataTokenId>();
            foreach (var t in Tokens)
            {
                MetadataTokenId.FlattenToDistinct(t, tokenHashset, perserveDirectChildItems);
            }
            return tokenHashset.ToArray();
        }

        /// <summary>
        /// Gets an adjancency matrix coupled with index-to-id dictionary.
        /// </summary>
        /// <returns></returns>
        public Tuple<Dictionary<int, MetadataTokenId>,  int[,]> GetAdjancencyMatrix(bool rmIsolatedNodes = false)
        {
            return GetAdjancencyMatrix(FlattenToDistinct(true).ToList(), rmIsolatedNodes);
        }

        internal static Tuple<Dictionary<int, MetadataTokenId>, int[,]> GetAdjancencyMatrix(List<MetadataTokenId> uqTokens,
            bool rmIsolatedNodes)
        {
            var adjMatrix = new int[uqTokens.Count, uqTokens.Count];
            var idxMapping = new Dictionary<int, MetadataTokenId>();
            for (var i = 0; i < adjMatrix.GetLongLength(0); i++)
            {
                var rowToken = uqTokens[i];
                idxMapping.Add(i, rowToken);
                for (var j = 0; j < adjMatrix.GetLongLength(1); j++)
                {
                    if (i == j)
                    {
                        adjMatrix[i, j] = 0;
                        continue;
                    }

                    var colToken = uqTokens[j];
                    var hasOutLink = rowToken?.Items?.Any(x => x.Equals(colToken));
                    adjMatrix[i, j] = hasOutLink.GetValueOrDefault(false) ? 1 : 0;
                }
            }

            if (!rmIsolatedNodes || adjMatrix.GetLongLength(0) <= 0)
                return new Tuple<Dictionary<int, MetadataTokenId>, int[,]>(idxMapping, adjMatrix);

            //expected a directed graph so when idx's entire row and column is all 0 should it be removed
            for (var i = 0; i < adjMatrix.GetLongLength(0); i++)
            {
                var sum = 0;
                for (var j = 0; j < adjMatrix.GetLongLength(1); j++)
                {
                    sum += adjMatrix[i, j] + adjMatrix[j, i];
                }
                if (sum <= 0)
                {
                    
                    var isoToken = uqTokens.FirstOrDefault(x => x.Equals(idxMapping[i]));
                    if (isoToken != null)
                    {
                        uqTokens.Remove(isoToken);
                    }
                }
            }
            return GetAdjancencyMatrix(uqTokens, false);
        }
    }
    #endregion

    #region token names
    /// <summary>
    /// The resolved name of a single metadata token
    /// </summary>
    [Serializable]
    public class MetadataTokenName
    {
        /// <summary>
        /// The original metadata token id
        /// </summary>
        public int Id;

        public string Name;

        /// <summary>
        /// The index of the <see cref="Assembly"/> which contains 
        /// the type's definition.
        /// </summary>
        public int OwnAsmIdx;

        /// <summary>
        /// The index of the <see cref="Assembly"/>  with 
        /// whose <see cref="Assembly.ManifestModule"/>
        /// the <see cref="Id"/> was resolved.
        /// </summary>
        public int RslvAsmIdx;

        /// <summary>
        /// A grouping label.
        /// </summary>
        public string Label;

        /// <summary>
        /// For the calling assembly to construct the full mapping.
        /// </summary>
        [NonSerialized]
        public MetadataTokenName[] Items;

        /// <summary>
        /// Determines, by naming pattern, if the current name was derived from <see cref="MethodInfo"/>
        /// </summary>
        /// <returns></returns>
        public bool IsMethodName()
        {
            return !String.IsNullOrWhiteSpace(Name) 
                && Name.Contains(Constants.TYPE_METHOD_NAME_SPLIT_ON) 
                && Name.Contains("(") 
                && Name.Contains(")");
        }

        /// <summary>
        /// Asserts that the part of the token name which
        /// matches the assembly name is omitted.
        /// </summary>
        /// <returns></returns>
        public bool IsPartialName()
        {
            return !String.IsNullOrWhiteSpace(Name) &&
                   Name.StartsWith(Constants.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture));
        }

        public override bool Equals(object obj)
        {
            var mtnObj = obj as MetadataTokenName;
            if (mtnObj == null)
                return false;

            return GetHashCode() == mtnObj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + GetNameHashCode() + OwnAsmIdx.GetHashCode();
        }

        /// <summary>
        /// Gets the hashcode of just the <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public int GetNameHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }

    [Serializable]
    public class MetadataTokenNameComparer : IEqualityComparer<MetadataTokenName>
    {
        public bool Equals(MetadataTokenName x, MetadataTokenName y)
        {
            if (x == null || y == null)
                return false;
            return x.GetNameHashCode() == y.GetNameHashCode();
        }

        public int GetHashCode(MetadataTokenName obj)
        {
            return obj.GetNameHashCode();
        }
    }

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
        /// Final analysis to merge the token Ids to thier names in a hierarchy.
        /// </summary>
        /// <param name="tokenIds"></param>
        /// <returns></returns>
        public MetadataTokenName[] ApplyMapping(MetadataTokenId[] tokenIds)
        {
            if (St == MetadataTokenStatus.Error || Names == null || Names.Length <= 0)
                return null;
            
            var nameMapping = new List<MetadataTokenName>();
            foreach(var tokenId in tokenIds)
                nameMapping.Add(GetNameMapping(tokenId));
            return nameMapping.ToArray();
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
                if (!t.IsPartialName())
                    continue;
                var asm = asmIndicies.Asms.FirstOrDefault(x => x.IndexId == t.OwnAsmIdx);
                if (asm == null)
                    continue;
                var asmName = new AssemblyName(asm.AssemblyName);
                t.Name = asmName.Name + t.Name;
            }
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
    #endregion

    #region token assemblies
    /// <summary>
    /// A dictionary for assembly names.
    /// </summary>
    [Serializable]
    public class MetadataTokenAsm
    {
        public string AssemblyName;
        public int IndexId;
        public override bool Equals(object obj)
        {
            var mta = obj as MetadataTokenAsm;
            return mta != null && string.Equals(mta.AssemblyName, AssemblyName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return AssemblyName?.GetHashCode() ?? 1;
        }
    }

    /// <summary>
    /// Bundler type for <see cref="MetadataTokenAsm"/>
    /// </summary>
    [Serializable]
    public class AsmIndicies
    {
        public string Msg;
        public MetadataTokenAsm[] Asms;
        public MetadataTokenStatus St;

        public Assembly GetAssemblyByIndex(int idx)
        {
            var owningAsmName = Asms.FirstOrDefault(x => x.IndexId == idx);
            if (owningAsmName == null)
                return null;
            var owningAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(
                        x =>
                            string.Equals(x.GetName().FullName, owningAsmName.AssemblyName,
                                StringComparison.OrdinalIgnoreCase));
            return owningAsm;
        }
    }
    #endregion

    #region token page rank

    [Serializable]
    public class TokenPageRanks
    {
        public string Msg;
        public MetadataTokenStatus St;
        public Tuple<int, int, double>[] Ranks;
    }
    #endregion

    #region assembly dependecy adjacency

    [Serializable]
    public class RankedMetadataTokenAsm : MetadataTokenAsm
    {
        public double PageRank;
        public string DllFullName;
        public bool HasPdb;
    }

    [Serializable]
    public class AsmAdjancyGraph
    {
        public string Msg;
        public MetadataTokenStatus St;
        public RankedMetadataTokenAsm[] Asms;
        [NonSerialized]
        public int[,] Graph;

        public SortedSet<RankedMetadataTokenAsm> GetRankedAsms()
        {
            var ss = new SortedSet<RankedMetadataTokenAsm>(new RankedMetadataTokenAsmComparer());
            if (Asms == null || !Asms.Any())
                return ss;
            foreach (var a in Asms)
                ss.Add(a);

            return ss;
        }
    }

    [Serializable]
    public class RankedMetadataTokenAsmComparer : IEqualityComparer<RankedMetadataTokenAsm>,
        IComparer<RankedMetadataTokenAsm>
    {
        public int Compare(RankedMetadataTokenAsm x, RankedMetadataTokenAsm y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            if (x.PageRank == y.PageRank)
            {
                if (x.HasPdb && !y.HasPdb)
                    return -1;
                if (!x.HasPdb && y.HasPdb)
                    return 1;
            }
            return x.PageRank > y.PageRank ? -1 : 1;
        }

        public bool Equals(RankedMetadataTokenAsm x, RankedMetadataTokenAsm y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(RankedMetadataTokenAsm obj)
        {
            return obj?.PageRank.GetHashCode() ?? 1;
        }
    }

    #endregion

    #region supports tokens
    /// <summary>
    /// A criteria type to send across the wire to a listening socket.
    /// </summary>
    [Serializable]
    public class GetTokenIdsCriteria
    {
        private string _ranlB64;//persist this as a base64 since regex can be difficult to encode\decode
        public string AsmName;

        /// <summary>
        /// A regex pattern on which <see cref="MetadataTokenId"/> should be resolved. 
        /// The default is to resolve only the top-level assembly.
        /// </summary>
        public string ResolveAllNamedLike
        {
            get { return Encoding.UTF8.GetString(Convert.FromBase64String(_ranlB64)); }
            set
            {
                var t = value;
                if (string.IsNullOrWhiteSpace(t))
                    _ranlB64 = null;
                _ranlB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            }
        }
    }

    /// <summary>
    /// Factory methods to reduce the amount of code needed in PowerShell
    /// </summary>
    public static class TokenFactory
    {
        public static Tuple<AsmIndicies, TokenNames> CoupleIdx(this AsmIndicies indicies, TokenNames names)
        {
            return new Tuple<AsmIndicies, TokenNames>(indicies,names);
        }

        /// <summary>
        /// For the given assembly, gets the named being made from itself to itself.
        /// </summary>
        /// <param name="asmIdx"></param>
        /// <param name="tokenMap"></param>
        /// <param name="tokenNames"></param>
        /// <returns></returns>
        public static TokenNames InternallyCalled(AsmIndicies asmIdx, TokenIds tokenMap, TokenNames tokenNames)
        {
            if (tokenNames?.Names == null || tokenNames.Names.Length <= 0)
                return new TokenNames {Names = new List<MetadataTokenName>().ToArray()};
            if (tokenMap?.Tokens == null || tokenMap.Tokens.Length <= 0)
                return new TokenNames {Names = new List<MetadataTokenName>().ToArray()};
            var typeTokens = tokenMap.Tokens;

            if (typeTokens.All(x => x.Items == null || x.Items.Length <= 0))
                return new TokenNames {Names = new List<MetadataTokenName>().ToArray()};


            var memberTokens = typeTokens.SelectMany(x => x.Items).ToList();

            //these are all the token Ids one could call on this assembly
            var memberTokenIds = memberTokens.Select(x => x.Id).ToList();

            //get all the tokenIds, defined in this assembly and being called from this assembly
            var callsInMembers =
                memberTokens.SelectMany(x => x.Items)
                    .Select(x => x.Id)
                    .Where(x => memberTokenIds.Any(y => x == y))
                    .ToList();


            tokenNames.ApplyFullName(asmIdx);

            var namesArray = tokenNames.Names.Where(x => callsInMembers.Any(y => y == x.Id)).ToArray();
            return new TokenNames {Names = namesArray};
        }

        /// <summary>
        /// Set operation for comparision of two <see cref="TokenNames"/> with
        /// thier respective <see cref="NoFuture.Shared.AsmIndicies"/>
        /// </summary>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <param name="rightListTopLvlOnly">
        /// Set to true to have <see cref="rightListTopLvlOnly"/> 
        /// only those in <see cref="rightList"/> whose <see cref="NoFuture.Util"/> is '0'.
        /// </param>
        /// <returns></returns>
        public static TokenNames RightSetDiff(Tuple<AsmIndicies, TokenNames> leftList,
            Tuple<AsmIndicies, TokenNames> rightList, bool rightListTopLvlOnly = false)
        {
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (leftList == null || rightList == null)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            if (rightList.Item2?.Names == null || rightList.Item2.Names.Length <= 0)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            if (leftList.Item2?.Names == null || leftList.Item2.Names.Length <= 0)
                return new TokenNames { Names = rightList.Item2.Names.ToArray() }; 

            //expand to full names
            leftList.Item2.ApplyFullName(leftList.Item1);
            rightList.Item2.ApplyFullName(rightList.Item1);

            var setOp = rightList.Item2.Names.Select(hashCode).Except(leftList.Item2.Names.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                var k = rightList.Item2.Names.FirstOrDefault(x => hashCode(x) == j);
                if (k == null || (rightListTopLvlOnly && k.OwnAsmIdx != 0) || !k.IsMethodName())
                    continue;
                listOut.Add(k);
            }

            return new TokenNames {Names = listOut.ToArray()};
        }

        /// <summary>
        /// Set operation for the joining of two <see cref="TokenNames"/> with
        /// thier respective <see cref="NoFuture.Shared.AsmIndicies"/>
        /// </summary>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <returns></returns>
        public static TokenNames Union(Tuple<AsmIndicies, TokenNames> leftList,
            Tuple<AsmIndicies, TokenNames> rightList)
        {
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (leftList == null || rightList == null)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            if (rightList.Item2 == null)
                return new TokenNames {Names = leftList.Item2.Names};
            if (leftList.Item2 == null)
                return new TokenNames { Names = rightList.Item2.Names};

            //expand to full names
            leftList.Item2.ApplyFullName(leftList.Item1);
            rightList.Item2.ApplyFullName(rightList.Item1);

            var d = rightList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);
            var e = leftList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new TokenNames { Names = d.Values.Where(x => x.IsMethodName()).ToArray()};
        }
    }

    /// <summary>
    /// Bundler type's status
    /// </summary>
    [Serializable]
    public enum MetadataTokenStatus
    {
        Ok,
        Error
    }

    [Serializable]
    public enum MetadataTokenSerialize
    {
        Binary,
        Json
    }
    #endregion
}
