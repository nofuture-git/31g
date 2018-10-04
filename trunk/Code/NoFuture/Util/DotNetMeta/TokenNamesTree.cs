using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.Auxx;
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
        private readonly MetadataTokenNameComparer _comparer = new MetadataTokenNameComparer();

        /// <summary>
        /// The flat list of token names from ctor-time
        /// </summary>
        public MetadataTokenName FlatTokenNames { get; }

        /// <summary>
        /// The token ids in hierarchy from ctor-time
        /// </summary>
        public MetadataTokenId TokenIds { get; }

        /// <summary>
        /// The assembly name -to- index 
        /// </summary>
        public AsmIndicies AssemblyIndices { get; }

        /// <summary>
        /// The resultant token names in call-stack tree form
        /// </summary>
        public MetadataTokenName TokenNameRoot { get; }

        /// <summary>
        /// Ctor of final assembly analysis 
        /// </summary>
        /// <param name="flatNames">The result names in a flat set</param>
        /// <param name="treeIds">The token ids in hierarchy form</param>
        /// <param name="asms">The assembly-to-index</param>
        public TokenNamesTree(TokenNames flatNames, TokenIds treeIds, AsmIndicies asms)
        {
            var tokenNameRspn = flatNames ?? throw new ArgumentNullException(nameof(flatNames));
            var tokenIdRspn = treeIds ?? throw new ArgumentNullException(nameof(treeIds));
            AssemblyIndices = asms ?? throw new ArgumentNullException(nameof(treeIds));
            tokenNameRspn.ApplyFullName(AssemblyIndices);

            TokenIds = tokenIdRspn.GetAsRoot();
            FlatTokenNames = tokenNameRspn.GetNamesAsSingle();
            TokenNameRoot = tokenNameRspn.BindTree2Names(tokenIdRspn);
            TokenNameRoot.ApplyFullName(AssemblyIndices);
            TokenNameRoot.ReassignAllByRefs();
        }

        /// <summary>
        /// Selectively gets the flatten call stack for the given type-method pair
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public MetadataTokenName SelectDistinct(string typeName, string methodName)
        {
            var df = new MetadataTokenName { Items = new MetadataTokenName[] { } };
            if (string.IsNullOrWhiteSpace(typeName))
                return df;
            methodName = methodName ?? "";
            if (methodName.Contains("("))
                methodName = methodName.Substring(0, methodName.IndexOf('('));

            //want to avoid an interface type 
            var tokenName = TokenNameRoot.SelectByTypeNames(typeName);
            var typeNameTree = tokenName.Items.FirstOrDefault(t => t.IsTypeName());

            if(typeNameTree == null)
                return tokenName;
            
            var names = new List<MetadataTokenName>();
            if (string.IsNullOrWhiteSpace(methodName) || !typeNameTree.Items.Any())
            {
                names.AddRange(typeNameTree.SelectDistinct());
                df.Items = names.Distinct(_comparer).ToArray();
                return df;
            }

            //match on overloads
            var targetMethods = typeNameTree.Items.Where(item =>
                item.Name.Contains($"{typeName}{Constants.TYPE_METHOD_NAME_SPLIT_ON}{methodName}("));
            foreach (var t in targetMethods)
            {
                names.AddRange(t.SelectDistinct());
            }

            df.Items = names.Distinct(_comparer).ToArray();
            return df;
            
        }
    }
}