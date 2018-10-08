using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
            flatNames = flatNames ?? throw new ArgumentNullException(nameof(flatNames));
            treeIds = treeIds ?? throw new ArgumentNullException(nameof(treeIds));
            tTypes = tTypes ?? throw new ArgumentNullException(nameof(tTypes));

            AssemblyIndices = asms ?? throw new ArgumentNullException(nameof(treeIds));
            TokenTypes = tTypes.GetTypesAsSingle();
            //concat ns\asm name to flat partial names
            flatNames.ApplyFullName(AssemblyIndices);
            //reduce the size 
            flatNames.RemoveClrAndEmptyNames();
            //assign the full call stack tree, by token ids only
            TokenIds = treeIds.GetAsRoot();
            //convert flat names to a root-style node
            FlatTokenNames = flatNames.GetNamesAsSingle();
            //get the full call stack tree as names 
            TokenNameRoot = flatNames.BindTree2Names(treeIds);
            //apply full names to call stack tree of names
            TokenNameRoot.ApplyFullName(AssemblyIndices);
            //replace any byRef terminating nodes with their fully expander counterpart
            TokenNameRoot.ReassignAllByRefs();
            //replace any interface token\names with implementation when its the only one
            var reassignInterfaces = TokenTypes.GetAllInterfacesWithSingleImplementor();
            if (reassignInterfaces == null || !reassignInterfaces.Any())
                return;
            foreach (var ri in reassignInterfaces)
            {
                var cri = TokenTypes.FirstInterfaceImplementor(ri);
                if(cri == null)
                    continue;

                var n2n = TokenNameRoot.GetImplentorDictionary(ri, cri);
                if(n2n == null || !n2n.Any())
                    continue;
                
                TokenNameRoot.ReassignAllInterfaceTokens(n2n);
            }
            //reassign this to the modified version
            FlatTokenNames = new MetadataTokenName {Items = TokenNameRoot.SelectDistinct()};
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