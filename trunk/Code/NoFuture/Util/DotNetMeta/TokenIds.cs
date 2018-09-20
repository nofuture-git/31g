using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Util.DotNetMeta
{
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
}