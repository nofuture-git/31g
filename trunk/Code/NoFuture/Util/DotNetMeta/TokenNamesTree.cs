using System;
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
        }

    }
}