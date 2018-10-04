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
            TreeTokenNames = FlatTokenNames.GetNames(TokenIds);
            TreeTokenNames.ApplyFullName(AssemblyIndices);
            TreeTokenNames.ReassignAllByRefs();
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
            var names = new List<MetadataTokenName>();
            foreach (var t in targetMethods)
            {
                names.AddRange(t.SelectDistinct());
            }

            names = names.Distinct(new MetadataTokenNameComparer()).ToList();
            df.Names = names.ToArray();
            return df;
            
        }
    }
}