using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

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
                   Name.StartsWith(Constants.DEFAULT_TYPE_SEPARATOR.ToString(CultureInfo.InvariantCulture));
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
