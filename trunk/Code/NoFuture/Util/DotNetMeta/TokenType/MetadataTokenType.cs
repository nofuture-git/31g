using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    /// <summary>
    /// The specific resolved name of a single metadata token which refers to a type.
    /// </summary>
    [Serializable]
    public class MetadataTokenType
    {
        [NonSerialized] private MetadataTokenType[] _interfaceTypes;
        [NonSerialized] private readonly MetadataTokenTypeComparer _comparer = new MetadataTokenTypeComparer();
        [NonSerialized] private Tuple<bool, MetadataTokenType> _firstImplementor;
        [NonSerialized] private int? _fullDepth;
        [NonSerialized] private MetadataTokenType[] _singleImplementors;

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

        /// <summary>
        /// Clears any in-memory cache copies from full tree recursions.
        /// </summary>
        public void ClearAllCacheData()
        {
            _interfaceTypes = null;
            _firstImplementor = null;
            _fullDepth = null;
            _singleImplementors = null;
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
                CountOfImplentors(ai, ref cnt);
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
        /// <returns></returns>
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
            if (_firstImplementor?.Item1 ?? false)
                return _firstImplementor.Item2;

            foreach (var nm in Items)
            {
                var vnm = nm.FirstInterfaceImplementor(interfaceType);
                if (vnm != null)
                {
                    _firstImplementor = new Tuple<bool, MetadataTokenType>(true, vnm);
                    return vnm;
                }
            }
            //we searched, we found nothing, don't bother again
            _firstImplementor = new Tuple<bool, MetadataTokenType>(true, null);
            return null;
        }

        /// <summary>
        /// Gets count of token types at all depths whose name exactly matches <see cref="typeName"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="countOf"></param>
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

        /// <summary>
        /// Gets a count, at all depths, of any types which directly implement <see cref="typeName"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="countOf"></param>
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

            return new MetadataTokenType { Items = listOut.ToArray() };
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
            Func<MetadataTokenType, int> hashCode = x => x.GetNameHashCode();

            var d = rightList.Items.Distinct(_comparer).ToDictionary(hashCode);
            var e = leftList.Items.Distinct(_comparer).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new MetadataTokenType { Items = d.Values.ToArray() };
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

            return new MetadataTokenType { Items = listOut.ToArray() };
        }
    }
}
