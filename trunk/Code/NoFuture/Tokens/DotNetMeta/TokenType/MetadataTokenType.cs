using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NoFuture.Shared.Core;
using NoFuture.Tokens.DotNetMeta.TokenName;
using NoFuture.Util.Core;

namespace NoFuture.Tokens.DotNetMeta.TokenType
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
        [NonSerialized] private int _idx = 0;
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
        /// The metadata token ids of all interfaces this type implements
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
        public bool IsInterfaceType()
        {
            return IsIntfc > 0;
        }

        /// <summary>
        /// Helper method to avoid having to check the int value of <see cref="IsAbsct"/>
        /// </summary>
        public bool IsAbstractType()
        {
            return IsAbsct > 0;
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
        /// Gets the full namespace qual. type name without any generic parameters
        /// </summary>
        /// <returns></returns>
        public string GetTypeName()
        {
            return AssemblyAnalysis.GetTypeName(Name);
        }

        /// <summary>
        /// Gets the distinct, flattened, list of types, from __this__ given instance, of abstract and interface types
        /// which are inherited. 
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType GetBaseTypes()
        {
            if (Items == null || !Items.Any())
                return null;

            Func<MetadataTokenType, bool> selector = (v) => v.IsAbstractType() || v.IsInterfaceType();

            var basemtt = new List<MetadataTokenType>();
            Func<MetadataTokenType, MetadataTokenType> addIt = (v) =>
            {
                if (v == null || string.Equals(v.Name, NfSettings.DefaultTypeSeparator.ToString()))
                    return null;
                basemtt.Add(v.GetShallowCopy());
                return v;
            };

            //to avoid including this instance 
            var mtt = new MetadataTokenType
            {
                Items = Items,
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };

            mtt.IterateTree(selector, addIt);

            return new MetadataTokenType
            {
                Items = basemtt.Distinct(_comparer).Cast<MetadataTokenType>().ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Gets all token types with <see cref="IsInterfaceType"/> at all depths
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType[] GetAllInterfaceTypes()
        {
            if (_interfaceTypes != null)
                return _interfaceTypes;
            
            Func<MetadataTokenType, bool> selector = (v) => v.IsInterfaceType();
            var interfaceTypes = new List<MetadataTokenType>();
            Func<MetadataTokenType, MetadataTokenType> addIt = (v) =>
            {
                if (v == null)
                    return null;
                interfaceTypes.Add(v);
                return v;
            };

            IterateTree(selector, addIt);

            _interfaceTypes = interfaceTypes.Distinct(_comparer).Cast<MetadataTokenType>().ToArray();
            return _interfaceTypes;
        }

        /// <summary>
        /// Gets all token interface types, at all depths, which have only
        /// one concrete implementation
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType[] GetAllInterfacesWithSingleImplementor(Action<ProgressMessage> reportProgress = null)
        {
            var sInfcs = new List<MetadataTokenType>();
            if (Items == null || !Items.Any())
                return null;
            if (_singleImplementors != null)
                return _singleImplementors;

            var allInfcs = GetAllInterfaceTypes();
            if (allInfcs == null || !allInfcs.Any())
                return null;
            var totalLen = allInfcs.Length;
            for (var i = 0; i < totalLen; i++)
            {
                var ai = allInfcs[i];
                if (ai == null)
                    continue;
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{ai?.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Getting all interfaces with only one implementation"
                });

                var cnt = 0;
                GetCountOfImplementors(ai, ref cnt);
                if (cnt == 1)
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
        /// Finds the first token type which implements and interface named <see cref="interfaceTypeName"/>
        /// which is not itself an interface
        /// </summary>
        /// <param name="interfaceTypeName"></param>
        /// <param name="immediateOnly">
        /// Optional, indicates to only match on a type which directly implements the given interface
        /// </param>
        /// <returns></returns>
        public MetadataTokenType GetFirstInterfaceImplementor(string interfaceTypeName, bool immediateOnly = false)
        {
            var interfaceType = Items.FirstOrDefault(t =>
                string.Equals(t.Name, interfaceTypeName, StringComparison.OrdinalIgnoreCase));
            if (interfaceType == null)
                return null;
            return GetFirstInterfaceImplementor(interfaceType, immediateOnly);
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
        public MetadataTokenType[] GetAmbiguousTypes(Action<ProgressMessage> reportProgress = null)
        {
            if (!IsRoot())
                return null;

            if (Items == null || !Items.Any())
                return null;

            var impls = new List<MetadataTokenType>();
            var allInterfaces = GetAllInterfaceTypes();
            var totalLen = allInterfaces.Length;

            for (var i = 0; i < totalLen; i++)
            {
                var ifc = allInterfaces[i];
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{ifc?.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Getting all ambiguous types"
                });
                var temp = GetImplementorsOf(ifc);
                if (temp.Count() > 1)
                    impls.AddRange(temp.Items);
            }

            return impls.Distinct(_comparer).Cast<MetadataTokenType>().ToArray().ToArray();
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
        public void GetCountOfImplementors(string typeName, ref int countOf)
        {
            //find the interface by this name
            var allInfcs = GetAllInterfaceTypes();
            if (allInfcs == null || !allInfcs.Any())
                return;
            var mtTypeName = allInfcs.FirstOrDefault(t => string.Equals(t.Name, typeName));
            if (mtTypeName == null || !mtTypeName.IsInterfaceType())
                return;
            GetCountOfImplementors(mtTypeName, ref countOf);

        }

        /// <summary>
        /// Gets a count, at all depths, of any types which directly implement <see cref="typeName"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="countOf"></param>
        public void GetCountOfImplementors(MetadataTokenType typeName, ref int countOf)
        {
            if (typeName == null)
                return;

            var impls = GetImplementorsOf(typeName);

            countOf = impls.Count();
        }

        /// <summary>
        /// Gets a list of the token types which implement <see cref="typeName"/>
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public MetadataTokenType GetImplementorsOf(MetadataTokenType typeName)
        {
            var impls = new List<MetadataTokenType>();
            if (typeName == null)
            {
                return new MetadataTokenType
                {
                    Items = impls.ToArray(),
                    Name = NfSettings.DefaultTypeSeparator.ToString()
                };
            }

            Func<MetadataTokenType, bool> selector = (v) =>
                v?.Items != null && v.Items.Any(vi => _comparer.Equals(vi, typeName));
            Func<MetadataTokenType, MetadataTokenType> addIt = (v) =>
            {
                if (v == null)
                    return null;
                impls.Add(v);
                
                return v;
            };

            IterateTree(selector, addIt);
            return new MetadataTokenType
            {
                Items = impls.Distinct(_comparer).Cast<MetadataTokenType>().Where(t => !t.Equals(typeName)).ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Gets a flat, distinct, shallow root-level token types as a Set.
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType SelectDistinct()
        {
            var allItems = new List<MetadataTokenType>();
            Func<MetadataTokenType, bool> selector = (v) => true;
            Func<MetadataTokenType, MetadataTokenType> addIt = (v) =>
            {
                if (v == null)
                    return null;
                allItems.Add(v.GetShallowCopy());
                return v;
            };

            IterateTree(selector, addIt);
            return new MetadataTokenType
            {
                Items = allItems.Distinct(_comparer).Cast<MetadataTokenType>().ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Gets an copy of this instance less its <see cref="Items"/>
        /// </summary>
        /// <returns></returns>
        public MetadataTokenType GetShallowCopy()
        {
            var mdtCopy = new MetadataTokenType
            {
                Name = Name,
                Id = Id,
                OwnAsmIdx = OwnAsmIdx,
                IsAbsct = IsAbsct,
                IsIntfc = IsIntfc,
            };

            if(AbstractMemberNames != null && AbstractMemberNames.Any())
            {
                var amnames = new List<MetadataTokenName>();
                foreach (var amn in AbstractMemberNames)
                {
                    if (amn == null)
                        continue;
                    amnames.Add(amn.GetShallowCopy());
                }

                mdtCopy.AbstractMemberNames = amnames.ToArray();
            }

            return mdtCopy;
        }

        /// <summary>
        /// Performs a seek and replace operation recursively where the caller must decide what they are looking 
        /// for and what to do when they find it.
        /// </summary>
        /// <param name="searchFunc"> A search function </param>
        /// <param name="doSomething">
        /// A reassignment function which is executed whenever the <see cref="searchFunc"/> returns true
        /// </param>
        /// <remarks> Actual method is iterative and not recursive. </remarks>
        public void IterateTree(Func<MetadataTokenType, bool> searchFunc, Func<MetadataTokenType, MetadataTokenType> doSomething)
        {
            var callStack = new Stack<MetadataTokenType>();
            //start at top
            var ivItem = this;
            while (ivItem != null)
            {
                var nextItem = ivItem.NextItem();
                if (nextItem != null)
                {
                    //detect if some parent item is also a child item
                    if (ReferenceEquals(nextItem, ivItem) || callStack.Any(v => object.ReferenceEquals(v, nextItem)))
                    {
                        nextItem = null;
                    }
                    else
                    {
                        callStack.Push(ivItem);
                    }
                }

                ivItem = nextItem;

                if (ivItem == null)
                {
                    if (callStack.Count <= 0)
                        break;
                    ivItem = callStack.Pop();
                    continue;
                }
                //search for whatever by such-and-such
                if (!searchFunc(ivItem))
                    continue;

                //having a match then do such-and-such
                var parent = callStack.Peek();
                var ivItemIdx = (parent?.GetCurrentIdx() ?? 0) - 1;
                if (parent == null || ivItemIdx < 0 || ivItemIdx >= parent.Count())
                {
                    ivItem = doSomething(ivItem);
                    continue;
                }
                parent.Items[ivItemIdx] = doSomething(ivItem);
            }
        }

        /// <summary>
        /// Gets the current counter index from the cumulative calls to <see cref="NextItem"/> and <see cref="PrevItem"/>
        /// </summary>
        /// <returns></returns>
        public virtual int GetCurrentIdx()
        {
            return _idx;
        }

        /// <summary>
        /// An iterative method to get the next item from <see cref="Items"/>
        /// </summary>
        /// <returns></returns>
        public virtual MetadataTokenType NextItem()
        {
            if (_idx < Count())
            {
                var v = Items[_idx];
                _idx += 1;
                return v;
            }
            _idx = 0;
            return null;
        }

        /// <summary>
        /// An iterative method to get the previous item from <see cref="Items"/>
        /// </summary>
        /// <returns></returns>
        public virtual MetadataTokenType PrevItem()
        {
            _idx -= 1;
            if (_idx >= 0)
            {
                var v = Items[_idx];
                return v;
            }
            _idx = 0;
            return null;
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
