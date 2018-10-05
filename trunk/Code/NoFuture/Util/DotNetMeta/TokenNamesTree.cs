using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.DotNetMeta.TokenType;

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
        public AsmIndexResponse AssemblyIndices { get; }

        /// <summary>
        /// The types in which all the various names and ids are scoped.
        /// </summary>
        public MetadataTokenType TokenTypes { get; }

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
        /// <param name="tTypes">The full list of all types in scope of the analysis</param>
        public TokenNamesTree(TokenNameResponse flatNames, TokenIdResponse treeIds, AsmIndexResponse asms, TokenTypeResponse tTypes)
        {
            var tokenNameRspn = flatNames ?? throw new ArgumentNullException(nameof(flatNames));
            var tokenIdRspn = treeIds ?? throw new ArgumentNullException(nameof(treeIds));
            var tokenTypes = tTypes ?? throw new ArgumentNullException(nameof(tTypes));
            AssemblyIndices = asms ?? throw new ArgumentNullException(nameof(treeIds));
            TokenTypes = tokenTypes.GetTypesAsSingle();

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