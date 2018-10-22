using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta.TokenName;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    /// <summary>
    /// The specific resolved name of a single metadata token which refers to a type.
    /// </summary>
    [Serializable]
    public class MetadataTokenType : INfToken
    {
        #region fields
        [NonSerialized] private MetadataTokenType[] _interfaceTypes;
        [NonSerialized] private readonly MetadataTokenTypeComparer _comparer = new MetadataTokenTypeComparer();
        [NonSerialized] private int? _fullDepth;
        [NonSerialized] private MetadataTokenType[] _singleImplementors;
        #endregion

        #region properties
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
        /// The member names defined by this type
        /// </summary>
        public MetadataTokenName[] AbstractMemberNames { get; set; }

        /// <summary>
        /// Indicates if the original type IsInterface was true when -gt one
        /// </summary>
        public int IsIntfc { get; set; }

        /// <summary>
        /// Indicates if the original type IsAbstract was true when -gt one
        /// </summary>
        public int IsAbsct { get; set; }

        #endregion

        /// <summary>
        /// Convenience method to get Items count
        /// </summary>
        public int Count()
        {
            return Items?.Length ?? 0;
        }

        /// <summary>
        /// Indicates if this instance is a list or tree data structure
        /// </summary>
        /// <returns></returns>
        public bool IsFlattened()
        {
            if (Items == null || !Items.Any())
                return false;

            foreach (var i in Items)
            {
                if ((i?.Count() ?? 0) > 0)
                    return false;
            }

            return true;
        }

        protected internal bool IsRoot()
        {
            return string.Equals(Name, NfSettings.DefaultTypeSeparator.ToString());
        }

        /// <summary>
        /// Helper method to avoid having to check the int value of <see cref="IsIntfc"/>
        /// </summary>
        /// <returns></returns>
        public bool IsInterfaceType()
        {
            return IsIntfc > 0;
        }

        /// <summary>
        /// Clears any in-memory cache copies from full tree recursions.
        /// </summary>
        public void ClearAllCacheData()
        {
            _interfaceTypes = null;
            _fullDepth = null;
            _singleImplementors = null;
        }

        /// <summary>
        /// Gets the first token type in <see cref="Items"/> which is not an interface.
        /// Since its .NET, there should only be one.
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType GetBaseType()
        {
            if (Items == null || !Items.Any())
                return null;
            return Items.FirstOrDefault(i => i.IsInterfaceType());
        }

        /// <summary>
        /// Gets all token types with <see cref="IsInterfaceType"/> at all depths
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all token interface types, at all depths, which have only
        /// one concrete impelemntation
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType[] GetAllInterfacesWithSingleImplementor()
        {
            var sInfcs = new List<MetadataTokenType>();
            if (Items == null || !Items.Any())
                return null;
            if (_singleImplementors != null)
                return _singleImplementors;

            var allInfcs = GetAllInterfaceTypes();
            foreach (var ai in allInfcs)
            {
                var cnt = 0;
                GetCountOfImplentors(ai, ref cnt);
                if(cnt == 1)
                    sInfcs.Add(ai);
            }

            _singleImplementors = sInfcs.ToArray();
            return _singleImplementors;
        }

        /// <summary>
        /// Gets the interface types which this token type directly extends
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType[] GetImmediateInterfaceTypes()
        {
            return Items.Where(i => i.IsInterfaceType()).ToArray();
        }

        /// <summary>
        /// Finds the first token type which implements <see cref="interfaceType"/> and is 
        /// not itself an interface
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="immediateOnly">
        /// Optional, indicates to only match on a type which directly implements the given interface
        /// </param>
        /// <returns></returns>
        public MetadataTokenType GetFirstInterfaceImplementor(MetadataTokenType interfaceType, bool immediateOnly = false)
        {
            if (interfaceType == null)
                return null;
            if (IsInterfaceType())
                return null;
            if (!IsRoot())
            {
                var isImplementor = immediateOnly
                    ? GetImmediateInterfaceTypes().Any(vi => _comparer.Equals(interfaceType, vi))
                    : GetAllInterfaceTypes().Any(vi => _comparer.Equals(interfaceType, vi));
                if (isImplementor)
                    return this;
            }

            if (Items == null || !Items.Any())
                return null;

            foreach (var nm in Items)
            {
                var vnm = nm.GetFirstInterfaceImplementor(interfaceType, immediateOnly);
                if (vnm != null)
                {
                    return vnm;
                }
            }
            return null;
        }

        /// <summary>
        /// Get concrete types which share an implementation with others
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType[] GetAmbiguousTypes()
        {
            if (!IsRoot())
                return null;

            if (Items == null || !Items.Any())
                return null;

            var impls = new List<MetadataTokenType>();
            foreach (var i in GetAllInterfaceTypes())
            {
                var count = 0;
                GetCountOfImplentors(i, ref count);
                if(count > 1)
                    impls.Add(i);
            }

            return impls.ToArray();
        }

        /// <summary>
        /// Get the count of all nodes throughout at all depths
        /// </summary>
        /// <returns></returns>
        public int GetFullDepthCount()
        {
            var c = 1;
            if (Items == null || !Items.Any())
                return c;
            if (_fullDepth != null)
                return _fullDepth.Value;
            foreach (var i in Items)
                c += i.GetFullDepthCount();

            _fullDepth = c;
            return c;
        }

        /// <summary>
        /// Gets the names which are exclusive to <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <param name="rightListTopLvlOnly"></param>
        /// <returns></returns>
        public MetadataTokenType GetRightSetDiff(MetadataTokenType otherNames, bool rightListTopLvlOnly = false)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenType, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Items == null || rightList.Items.Length <= 0)
                return this;
            if (leftList.Items == null || leftList.Items.Length <= 0)
                return rightList;

            var setOp = rightList.Items.Select(hashCode).Except(leftList.Items.Select(hashCode));

            var listOut = new List<MetadataTokenType>();
            foreach (var j in setOp)
            {
                var k = rightList.Items.FirstOrDefault(x => hashCode(x) == j);
                if (k == null || rightListTopLvlOnly && k.OwnAsmIdx != 0)
                    continue;
                listOut.Add(k);
            }

            return new MetadataTokenType
            {
                Items = listOut.ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Joins the distinct names of this instance to the names of <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public MetadataTokenType GetUnion(MetadataTokenType otherNames)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<INfToken, int> hashCode = x => x.GetNameHashCode();

            var d = rightList.Items.Distinct(_comparer).ToDictionary(hashCode);
            var e = leftList.Items.Distinct(_comparer).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new MetadataTokenType
            {
                Items = d.Values.Cast<MetadataTokenType>().ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Gets the names which are shared between this instance and <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public MetadataTokenType GetIntersect(MetadataTokenType otherNames)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenType, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Items == null || rightList.Items.Length <= 0)
                return this;
            if (leftList.Items == null || leftList.Items.Length <= 0)
                return rightList;

            var setOp = rightList.Items.Select(hashCode).Intersect(leftList.Items.Select(hashCode));

            var listOut = new List<MetadataTokenType>();
            foreach (var j in setOp)
            {
                //should be in either list
                var k = leftList.Items.FirstOrDefault(x => hashCode(x) == j);
                if (k == null)
                    continue;
                listOut.Add(k);
            }

            return new MetadataTokenType
            {
                Items = listOut.ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Gets count of token types at all depths whose name exactly matches <see cref="typeName"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="countOf"></param>
        public void GetCountOfImplentors(string typeName, ref int countOf)
        {
            //find the interface by this name
            var allInfcs = GetAllInterfaceTypes();
            if (allInfcs == null || !allInfcs.Any())
                return;
            var mtTypeName = allInfcs.FirstOrDefault(t => string.Equals(t.Name, typeName));
            if (mtTypeName == null || !mtTypeName.IsInterfaceType())
                return;
            GetCountOfImplentors(mtTypeName, ref countOf);

        }

        /// <summary>
        /// Gets a count, at all depths, of any types which directly implement <see cref="typeName"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="countOf"></param>
        public void GetCountOfImplentors(MetadataTokenType typeName, ref int countOf)
        {
            if (typeName == null)
                return;
            var tnInfcs = GetImmediateInterfaceTypes();
            if (tnInfcs == null || !tnInfcs.Any())
                return;
            countOf += tnInfcs.Count(v => _comparer.Equals(typeName, v));

            if (Items == null || !Items.Any())
                return;

            foreach (var nm in Items)
                nm.GetCountOfImplentors(nm, ref countOf);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 1;
        }

        /// <summary>
        /// Gets the hashcode of just the <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public int GetNameHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var tn = obj as MetadataTokenType;
            if (tn == null)
                return false;

            return string.Equals(tn.Name, Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
