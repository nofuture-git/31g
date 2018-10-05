using System;
using System.Reflection;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class MetadataTokenType
    {
        /// <summary>
        /// The original metadata token id
        /// </summary>
        public int Id;

        /// <summary>
        /// The index of the <see cref="Assembly"/> which contains 
        /// the type's definition.
        /// </summary>
        public int OwnAsmIdx;

        public string Name;

        /// <summary>
        /// The metadata token id of the type this type directly extends
        /// </summary>
        public MetadataTokenType Extds;

        /// <summary>
        /// The metadata token ids of all interfaces this type impelments
        /// </summary>
        public MetadataTokenType[] Implmts;

        /// <summary>
        /// Indicates if the original type IsInterface was true when -gt one
        /// </summary>
        public int IsIntfc;

        /// <summary>
        /// Indicates if the original type IsAbstract was true when -gt one
        /// </summary>
        public int IsAbsct;
    }
}
