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

        /// <summary>
        /// The metadata token id of the type this type directly extends
        /// </summary>
        public int ExtendsId;

        /// <summary>
        /// The metadata token ids of all interfaces this type impelments
        /// </summary>
        public int[] ImplementsIds;
    }
}
