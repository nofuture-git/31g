using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class MetadataTokenType
    {
        [NonSerialized]
        private MetadataTokenType[] _interfaceTypes;

        /// <summary>
        /// The original metadata token id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The index of the <see cref="Assembly"/> which contains 
        /// the type's definition.
        /// </summary>
        public int OwnAsmIdx { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The metadata token ids of all interfaces this type impelments
        /// </summary>
        public MetadataTokenType[] Items { get; set; }

        /// <summary>
        /// Indicates if the original type IsInterface was true when -gt one
        /// </summary>
        public int IsIntfc { get; set; }

        /// <summary>
        /// Indicates if the original type IsAbstract was true when -gt one
        /// </summary>
        public int IsAbsct { get; set; }

        public bool IsInterfaceType()
        {
            return IsIntfc > 0;
        }

        public MetadataTokenType GetBaseType()
        {
            if (Items == null || !Items.Any())
                return null;
            return Items.FirstOrDefault(i => i.IsInterfaceType());
        }

        public MetadataTokenType[] GetAllInterfaceTypes()
        {
            if (_interfaceTypes != null)
                return _interfaceTypes;

            var infcs = new List<MetadataTokenType>();
            if (IsInterfaceType())
                infcs.Add(this);
            if (Items == null || !Items.Any())
                return infcs.ToArray();
            foreach (var infc in Items)
            {
                infcs.AddRange(infc.GetAllInterfaceTypes());
            }

            _interfaceTypes = infcs.Distinct().ToArray();
            return _interfaceTypes;
        }

        public MetadataTokenType[] GetAllInterfacesWithSingleImplementor()
        {
            var sInfcs = new List<MetadataTokenType>();
            if (Items == null || !Items.Any())
                return null;
            var allInfcs = GetAllInterfaceTypes();
            foreach (var ai in allInfcs)
            {
                var cnt = 0;
                CountOfImplentors(ai, ref cnt);
                if(cnt == 1)
                    sInfcs.Add(ai);
            }

            return sInfcs.ToArray();
        }

        public MetadataTokenType[] GetImmediateInterfaceTypes()
        {
            return Items.Where(i => i.IsInterfaceType()).ToArray();
        }

        public MetadataTokenType FirstInterfaceImplementor(MetadataTokenType interfaceType)
        {
            if (interfaceType == null)
                return null;
            if (IsInterfaceType())
                return null;
            if (!string.IsNullOrWhiteSpace(Name) && GetAllInterfaceTypes().Any(vi => vi.Equals(interfaceType)))
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

        public void CountOfImplentors(string typeName, ref int countOf)
        {
            //find the interface by this name
            var allInfcs = GetAllInterfaceTypes();
            if (allInfcs == null || !allInfcs.Any())
                return;
            var mtTypeName = allInfcs.FirstOrDefault(t => string.Equals(t.Name, typeName));
            if (mtTypeName == null || !mtTypeName.IsInterfaceType())
                return;
            CountOfImplentors(mtTypeName, ref countOf);

        }

        public void CountOfImplentors(MetadataTokenType typeName, ref int countOf)
        {
            if (typeName == null)
                return;
            var tnInfcs = GetImmediateInterfaceTypes();
            if (tnInfcs == null || !tnInfcs.Any())
                return;
            countOf += tnInfcs.Count(v => v.Equals(typeName));

            if (Items == null || !Items.Any())
                return;

            foreach(var nm in Items)
                nm.CountOfImplentors(nm, ref countOf);
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

        /// <summary>
        /// Gets the hashcode of just the <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public int GetNameHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}
