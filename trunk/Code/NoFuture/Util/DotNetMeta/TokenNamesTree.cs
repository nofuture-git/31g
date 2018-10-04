using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.Grp;
using NoFuture.Util.DotNetMeta.Xfer;

namespace NoFuture.Util.DotNetMeta
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

        /// <summary>
        /// Ctor of final assembly analysis 
        /// </summary>
        /// <param name="flatNames">The result names in a flat set</param>
        /// <param name="treeIds">The token ids in hierarchy form</param>
        /// <param name="asms">The assembly-to-index</param>
        public TokenNamesTree(TokenNames flatNames, TokenIds treeIds, AsmIndicies asms)
        {
            FlatTokenNames = flatNames ?? throw new ArgumentNullException(nameof(flatNames));
            TokenIds = treeIds ?? throw new ArgumentNullException(nameof(treeIds));
            AssemblyIndices = asms ?? throw new ArgumentNullException(nameof(treeIds));
            FlatTokenNames.ApplyFullName(AssemblyIndices);
            TreeTokenNames = FlatTokenNames.GetFullCallStackTree(TokenIds);
            TreeTokenNames.ApplyFullName(AssemblyIndices);
        }

        /// <summary>
        /// Selectively gets the flatten call stack for the given type-method pair
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public TokenNames SelectDistinct(string typeName, string methodName)
        {
            var df = new TokenNames { Names = new MetadataTokenName[] { } };
            if (string.IsNullOrWhiteSpace(typeName) || string.IsNullOrWhiteSpace(methodName))
                return df;
            if (methodName.Contains("("))
                methodName = methodName.Substring(0, methodName.IndexOf('('));

            //want to avoid an interface type 
            var typeNameTree = TreeTokenNames.Names.FirstOrDefault(i =>
                i.IsTypeName() && i.Name.EndsWith(typeName) &&
                i.Items.Any(ii => ii.Items != null && ii.Items.Length > 0));

            if(typeNameTree?.Items == null || !typeNameTree.Items.Any())
                return df;

            //match on overloads
            var targetMethods = typeNameTree.Items.Where(item =>
                item.Name.Contains($"{typeName}{Constants.TYPE_METHOD_NAME_SPLIT_ON}{methodName}("));
            throw new NotImplementedException();
            
        }

        public TokenNames GetInternallyCalledTokenNames()
        {
            if (FlatTokenNames?.Names == null || FlatTokenNames.Names.Length <= 0)
                return new TokenNames { Names = new MetadataTokenName[]{} };
            if (TokenIds?.Tokens == null || TokenIds.Tokens.Length <= 0)
                return new TokenNames { Names = new MetadataTokenName[] { } };
            var typeTokens = TokenIds.Tokens;

            if (typeTokens.All(x => x.Items == null || x.Items.Length <= 0))
                return new TokenNames { Names = new MetadataTokenName[] { } };

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
    }
}