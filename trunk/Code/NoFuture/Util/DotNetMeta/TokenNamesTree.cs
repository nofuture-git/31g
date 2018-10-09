using System;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.DotNetMeta.TokenType;

namespace NoFuture.Util.DotNetMeta
{
    /// <summary>
    /// A container type to hold the various parts together
    /// </summary>
    [Serializable]
    public class TokenNamesTree
    {
        /// <summary>
        /// The flat list of token names from ctor-time
        /// </summary>
        public MetadataTokenName FlatTokenNames { get; set; }

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

            TokenIds = treeIds.GetAsRoot();
            TokenTypes = tTypes.GetTypesAsSingle();

            FlatTokenNames = flatNames.GetNamesAsSingle();
            FlatTokenNames.ApplyFullName(AssemblyIndices);

            TokenNameRoot = MetadataTokenName.BuildMetadataTokenName(FlatTokenNames, TokenIds, AssemblyIndices, TokenTypes);
        }
    }
}