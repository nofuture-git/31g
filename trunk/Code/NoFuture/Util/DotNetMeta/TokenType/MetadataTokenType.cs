using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The metadata token ids of all interfaces this type impelments
        /// </summary>
        public MetadataTokenType[] Items;

        /// <summary>
        /// Indicates if the original type IsInterface was true when -gt one
        /// </summary>
        public int IsIntfc;

        /// <summary>
        /// Indicates if the original type IsAbstract was true when -gt one
        /// </summary>
        public int IsAbsct;

        [NonSerialized] private MetadataTokenType[] _interfaceTypes;

        public MetadataTokenType GetBaseType()
        {
            if (Items == null || !Items.Any())
                return null;
            return Items.FirstOrDefault(i => i.IsIntfc < 1);
        }

        public MetadataTokenType[] GetInterfaceTypes()
        {
            if (_interfaceTypes != null)
                return _interfaceTypes;

            var infcs = new List<MetadataTokenType>();
            if (IsIntfc > 0)
                infcs.Add(this);
            if (Items == null || !Items.Any())
                return infcs.ToArray();
            foreach (var infc in Items)
            {
                infcs.AddRange(infc.GetInterfaceTypes());
            }

            _interfaceTypes = infcs.Distinct().ToArray();
            return _interfaceTypes;
        }

        public MetadataTokenType FirstInterfaceImplementor(MetadataTokenType interfaceType)
        {
            if (interfaceType == null)
                return null;
            if (IsIntfc > 0)
                return null;
            if (!string.IsNullOrWhiteSpace(Name) && GetInterfaceTypes().Any(vi => vi.Equals(interfaceType)))
                return this;

            if (Items == null || !Items.Any())
                return null;

            foreach (var nm in Items)
            {
                var vnm = nm.FirstInterfaceImplementor(interfaceType);
                if (vnm != null)
                    return vnm;
            }

            return null;
        }

        public void CountOfImplentors(MetadataTokenType typeName, ref int countOf)
        {
            if (typeName == null)
                return;

            if (Equals(typeName))
                countOf += 1;

            if (Items == null || !Items.Any())
                return;

            foreach(var nm in Items)
                nm.CountOfImplentors(typeName, ref countOf);
        }

        public override bool Equals(object obj)
        {
            var tn = obj as MetadataTokenType;
            if (tn == null)
                return false;

            return string.Equals(tn.Name, Name);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
