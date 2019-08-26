using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace NoFuture.Util.DotNetMeta.TokenId
{
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
        public int RslvAsmIdx { get; set; }
        public int Id { get; set; }
        public MetadataTokenId[] Items { get; set; }
        public int IsByRef { get; set; }

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

        public override string ToString()
        {
            var copy = new MetadataTokenId {Id = Id, IsByRef = IsByRef, RslvAsmIdx = RslvAsmIdx};
            return JsonConvert.SerializeObject(copy);
        }

        /// <summary>
        /// Returns a list of distinct <see cref="MetadataTokenId"/> having only their
        /// <see cref="RslvAsmIdx"/> and <see cref="Id"/> assigned.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="perserveDirectChildItems">Retains the the direct children Items</param>
        /// <returns></returns>
        public static MetadataTokenId[] SelectDistinct(MetadataTokenId token, bool perserveDirectChildItems = false)
        {
            var list = new HashSet<MetadataTokenId>();

            if (token == null)
                return list.ToArray();

            SelectDistinct(token, list, perserveDirectChildItems);
            return list.ToArray();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void SelectDistinct(MetadataTokenId token, HashSet<MetadataTokenId> list,
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
                SelectDistinct(jToken, list, perserveDirectChildItems);
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


        /// <summary>
        /// Helper method to get all distinct token ids from the current instance.
        /// </summary>
        /// <returns></returns>
        public MetadataTokenId[] SelectDistinct(bool perserveDirectChildItems = false)
        {
            var tokenHashset = new HashSet<MetadataTokenId>();
            if (Items == null || Items.Length <= 0)
            {
                tokenHashset.Add(this);
                return tokenHashset.ToArray();
            }
            foreach (var t in Items)
            {
                SelectDistinct(t, tokenHashset, perserveDirectChildItems);
            }

            return tokenHashset.ToArray();
        }

        /// <summary>
        /// Gets an adjancency matrix coupled with index-to-id dictionary.
        /// </summary>
        /// <returns></returns>
        public Tuple<Dictionary<int, MetadataTokenId>, int[,]> GetAdjancencyMatrix(bool rmIsolatedNodes = false)
        {
            return GetAdjancencyMatrix(SelectDistinct(true).ToList(), rmIsolatedNodes);
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
}