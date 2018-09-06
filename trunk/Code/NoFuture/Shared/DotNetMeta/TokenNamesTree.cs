using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Shared.DotNetMeta
{
    /// <summary>
    /// A resultant type of a fully recursive assembly analysis 
    /// </summary>
    [Serializable]
    public class TokenNamesTree
    {
        /// <summary>
        /// The flat list of token names from ctor-time
        /// </summary>
        public TokenNames FlatTokenNames { get; }

        /// <summary>
        /// The token ids in hierarchy from ctor-time
        /// </summary>
        public TokenIds TokenIds { get; }

        /// <summary>
        /// The assembly name -to- index 
        /// </summary>
        public AsmIndicies AssemblyIndices { get; }

        /// <summary>
        /// The resultant token names in call-stack tree form
        /// </summary>
        public TokenNames TreeTokenNames { get; }

        public TokenNamesTree(TokenNames flatNames, TokenIds treeIds, AsmIndicies asms)
        {
            FlatTokenNames = flatNames ?? throw new ArgumentNullException(nameof(flatNames));
            TokenIds = treeIds ?? throw new ArgumentNullException(nameof(treeIds));
            AssemblyIndices = asms ?? throw new ArgumentNullException(nameof(treeIds));
            TreeTokenNames = FlatTokenNames.GetFullCallStackTree(TokenIds.Tokens);
        }

        public TokenNames GetInternallyCalledTokenNames()
        {
            if (FlatTokenNames?.Names == null || FlatTokenNames.Names.Length <= 0)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            if (TokenIds?.Tokens == null || TokenIds.Tokens.Length <= 0)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            var typeTokens = TokenIds.Tokens;

            if (typeTokens.All(x => x.Items == null || x.Items.Length <= 0))
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };


            var memberTokens = typeTokens.SelectMany(x => x.Items).ToList();

            //these are all the token Ids one could call on this assembly
            var memberTokenIds = memberTokens.Select(x => x.Id).ToList();

            //get all the tokenIds, defined in this assembly and being called from this assembly
            var callsInMembers =
                memberTokens.SelectMany(x => x.Items)
                    .Select(x => x.Id)
                    .Where(x => memberTokenIds.Any(y => x == y))
                    .ToList();

            FlatTokenNames.ApplyFullName(AssemblyIndices);

            var namesArray = FlatTokenNames.Names.Where(x => callsInMembers.Any(y => y == x.Id)).ToArray();
            return new TokenNames { Names = namesArray };
        }

        public TokenNames GetRightSetDiff(TokenNamesTree tokenNamesTree, bool rightListTopLvlOnly = false)
        {
            if (tokenNamesTree == null)
                return TreeTokenNames;
            var leftList = new Tuple<AsmIndicies, TokenNames>(AssemblyIndices, TreeTokenNames);
            var rightList = new Tuple<AsmIndicies, TokenNames>(tokenNamesTree.AssemblyIndices, tokenNamesTree.TreeTokenNames);
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

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

            return new TokenNames { Names = listOut.ToArray() };
        }

        public TokenNames GetUnion(TokenNamesTree tokenNamesTree)
        {
            if (tokenNamesTree == null)
                return TreeTokenNames;
            var leftList = new Tuple<AsmIndicies, TokenNames>(AssemblyIndices, TreeTokenNames);
            var rightList = new Tuple<AsmIndicies, TokenNames>(tokenNamesTree.AssemblyIndices, tokenNamesTree.TreeTokenNames);
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Item2 == null)
                return new TokenNames { Names = leftList.Item2.Names };
            if (leftList.Item2 == null)
                return new TokenNames { Names = rightList.Item2.Names };

            //expand to full names
            leftList.Item2.ApplyFullName(leftList.Item1);
            rightList.Item2.ApplyFullName(rightList.Item1);

            var d = rightList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);
            var e = leftList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new TokenNames { Names = d.Values.Where(x => x.IsMethodName()).ToArray() };
        }
    }
}