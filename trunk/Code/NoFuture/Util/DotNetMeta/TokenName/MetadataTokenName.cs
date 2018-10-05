﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    /// <summary>
    /// The resolved name of a single metadata token
    /// </summary>
    [Serializable]
    public class MetadataTokenName
    {
        /// <summary>
        /// The original metadata token id
        /// </summary>
        public int Id;

        public string Name;

        /// <summary>
        /// The index of the <see cref="Assembly"/> which contains 
        /// the type's definition.
        /// </summary>
        public int OwnAsmIdx;

        /// <summary>
        /// The index of the <see cref="Assembly"/>  with 
        /// whose <see cref="Assembly.ManifestModule"/>
        /// the <see cref="Id"/> was resolved.
        /// </summary>
        public int RslvAsmIdx;

        /// <summary>
        /// A grouping label.
        /// </summary>
        public string Label;

        /// <summary>
        /// For the calling assembly to construct the full mapping.
        /// </summary>
        [NonSerialized]
        public MetadataTokenName[] Items;

        [NonSerialized]
        public bool IsByRef;

        [NonSerialized]
        private readonly MetadataTokenNameComparer _comparer = new MetadataTokenNameComparer();

        public MetadataTokenName[] SelectDistinct()
        {
            var innerItems = new List<MetadataTokenName> {this};
            if (Items == null || !Items.Any())
                return innerItems.ToArray();
            foreach (var item in Items)
            {
                innerItems.AddRange(item.SelectDistinct());
            }

            return innerItems.Distinct(_comparer).ToArray();
        }

        public void ApplyFullName(AsmIndexResponse asmIndicies)
        {
            if (!IsPartialName())
                return;
            var asm = asmIndicies.Asms.FirstOrDefault(x => x.IndexId == OwnAsmIdx);
            if (asm == null)
                return;
            var asmName = new AssemblyName(asm.AssemblyName);
            Name = asmName.Name + Name;
            if (Items == null || !Items.Any())
                return;
            //recurse down the tree
            foreach (var myItems in Items)
            {
                myItems.ApplyFullName(asmIndicies);
            }
        }

        public MetadataTokenId Convert2MetadataTokenId()
        {
            return new MetadataTokenId
            {
                Id = Id,
                Items = Items?.Select(i => i.Convert2MetadataTokenId()).ToArray(),
                RslvAsmIdx = RslvAsmIdx
            };
        }

        /// <summary>
        /// Determines, by naming pattern, if the current name was derived from <see cref="MethodInfo"/>
        /// </summary>
        /// <returns></returns>
        public bool IsMethodName()
        {
            return !String.IsNullOrWhiteSpace(Name) 
                   && Name.Contains(Constants.TYPE_METHOD_NAME_SPLIT_ON) 
                   && Name.Contains("(") 
                   && Name.Contains(")");
        }

        public bool IsTypeName()
        {
            return !string.IsNullOrWhiteSpace(Name)
                   && !string.IsNullOrWhiteSpace(Label)
                   && string.Equals("RuntimeType", Label);
        }

        public bool IsAnyByRef()
        {
            if (IsByRef)
                return true;
            if (Items == null || !Items.Any())
                return false;
            foreach (var i in Items)
            {
                if (i.IsByRef)
                    return true;
                if (i.IsAnyByRef())
                    return true;
            }

            return false;
        }

        public int GetFullDepthCount()
        {
            var c = 1;
            if (Items == null || !Items.Any())
                return c;
            foreach (var i in Items)
                c += i.GetFullDepthCount();

            return c;
        }

        public string GetMemberName()
        {
            const string SPLT = Constants.TYPE_METHOD_NAME_SPLIT_ON;
            return IsMethodName() ? Name.Substring(Name.IndexOf(SPLT) + SPLT.Length) : string.Empty;
        }

        public string GetTypeName()
        {
            const string SPLT = Constants.TYPE_METHOD_NAME_SPLIT_ON;
            if (string.IsNullOrWhiteSpace(Name))
                return string.Empty;
            var idxOut = Name.IndexOf(SPLT);
            if (idxOut <= 0)
                return Name;
            return Name.Substring(0, Name.IndexOf(SPLT));
        }

        public string GetNamespaceName()
        {
            return NfReflect.GetNamespaceWithoutTypeName(GetTypeName()) ?? string.Empty;
        }

        /// <summary>
        /// Asserts that the part of the token name which
        /// matches the assembly name is omitted.
        /// </summary>
        /// <returns></returns>
        public bool IsPartialName()
        {
            return !String.IsNullOrWhiteSpace(Name) &&
                   Name.StartsWith(NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Finds the first matching token name which is <see cref="IsByRef"/> false.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public MetadataTokenName FirstByVal(MetadataTokenName tokenName)
        {
            if (tokenName == null)
                return null;
            if (_comparer.Equals(tokenName, this) && !IsByRef)
                return this;
            if (Items == null || !Items.Any())
                return null;

            foreach (var nm in Items)
            {
                var matched = nm.FirstByVal(tokenName);
                if (matched != null)
                    return matched;
            }

            return null;
        }

        /// <summary>
        /// Reassigns any member in <see cref="Items"/> to the <see cref="tokenName"/>
        /// if they having matching names.
        /// </summary>
        /// <param name="tokenName"></param>
        public void ReassignAnyItemsByName(MetadataTokenName tokenName)
        {
            if (tokenName == null)
                return;

            if (Items == null || !Items.Any())
                return;

            for (var i = 0; i < Items.Length; i++)
            {
                if (_comparer.Equals(Items[i], tokenName))
                {
                    Items[i] = tokenName;
                    continue;
                }
                Items[i].ReassignAnyItemsByName(tokenName);
            }
        }

        public void GetAllByRefNames(List<MetadataTokenName> tokenNames)
        {
            tokenNames = tokenNames ?? new List<MetadataTokenName>();
            if(IsByRef && tokenNames.All(tn => !_comparer.Equals(tn, this)))
                tokenNames.Add(this);
            if (Items == null || !Items.Any())
                return;
            foreach (var nm in Items)
            {
                nm.GetAllByRefNames(tokenNames);
            }
        }

        public override bool Equals(object obj)
        {
            var mtnObj = obj as MetadataTokenName;
            if (mtnObj == null)
                return false;

            return GetHashCode() == mtnObj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + GetNameHashCode() + OwnAsmIdx.GetHashCode();
        }

        /// <summary>
        /// Gets the hashcode of just the <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public int GetNameHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Gets the names which are exclusive to <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <param name="rightListTopLvlOnly"></param>
        /// <returns></returns>
        public MetadataTokenName GetRightSetDiff(MetadataTokenName otherNames, bool rightListTopLvlOnly = false)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Items == null || rightList.Items.Length <= 0)
                return this;
            if (leftList.Items == null || leftList.Items.Length <= 0)
                return rightList;

            var setOp = rightList.Items.Select(hashCode).Except(leftList.Items.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                var k = rightList.Items.FirstOrDefault(x => hashCode(x) == j);
                if (k == null || rightListTopLvlOnly && k.OwnAsmIdx != 0)
                    continue;
                listOut.Add(k);
            }

            return new MetadataTokenName { Items = listOut.ToArray() };
        }

        /// <summary>
        /// Joins the distinct names of this instance to the names of <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public MetadataTokenName GetUnion(MetadataTokenName otherNames)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            var d = rightList.Items.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);
            var e = leftList.Items.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new MetadataTokenName { Items = d.Values.ToArray() };
        }

        /// <summary>
        /// Gets the names which are shared between this instance and <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public MetadataTokenName GetIntersect(MetadataTokenName otherNames)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Items == null || rightList.Items.Length <= 0)
                return this;
            if (leftList.Items == null || leftList.Items.Length <= 0)
                return rightList;

            var setOp = rightList.Items.Select(hashCode).Intersect(leftList.Items.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                //should be in either list
                var k = leftList.Items.FirstOrDefault(x => hashCode(x) == j);
                if (k == null)
                    continue;
                listOut.Add(k);
            }

            return new MetadataTokenName { Items = listOut.ToArray() };
        }

        public string[] GetUniqueTypeNames()
        {
            return Items.Select(n => n.GetTypeName()).Distinct().ToArray();
        }

        public MetadataTokenName SelectByTypeNames(params string[] typenames)
        {
            return SelectByFunc(n => n.GetTypeName(), (s, f) => s.Any(f.EndsWith), typenames);
        }

        public MetadataTokenName SelectByNamespaceNames(params string[] namespaceNames)
        {
            return SelectByFunc(n => n.GetNamespaceName(), (s, f) => s.Any(f.StartsWith), namespaceNames);
        }

        public MetadataTokenName RemoveByTypeNames(params string[] typenames)
        {
            return SelectByFunc(n => n.GetTypeName(), (s, f) => s.All(v => !f.EndsWith(v)), typenames);
        }

        public MetadataTokenName RemoveByNamespaceNames(params string[] namespaceNames)
        {
            return SelectByFunc(n => n.GetNamespaceName(), (s, f) => s.All(v => !f.StartsWith(v)), namespaceNames);
        }

        protected internal MetadataTokenName SelectByFunc(Func<MetadataTokenName, string> getNameFunc,
            Func<string[], string, bool> selector, params string[] searchNames)
        {
            var names = new List<MetadataTokenName>();
            if (selector(searchNames, getNameFunc(this)))
                names.Add(this);

            if (Items == null || !Items.Any())
                return new MetadataTokenName { Items = names.ToArray() };
            foreach (var name in Items)
            {
                var nameMatch = name.SelectByFunc(getNameFunc, selector, searchNames);
                if (nameMatch.Items.Any())
                    names.AddRange(nameMatch.Items);
            }
            return new MetadataTokenName { Items = names.Distinct(_comparer).ToArray() };
        }

        public void ReassignAllByRefs()
        {
            //find all the byRefs throughout
            var byRefs = new List<MetadataTokenName>();
            foreach (var nm in Items)
                nm.GetAllByRefNames(byRefs);

            if (!byRefs.Any())
                return;

            //for each byref, find it byVal counterpart
            var byVals = new List<MetadataTokenName>();
            foreach (var byRef in byRefs)
            {
                MetadataTokenName byVal = null;
                foreach (var nm in Items)
                {
                    byVal = nm.FirstByVal(byRef);
                    if (byVal == null)
                        continue;
                    byVals.Add(byVal);
                    break;
                }
            }

            if (!byVals.Any())
            {
                Console.WriteLine($"There are {byRefs.Count} ByRef names but no ByVal counterparts.");
                return;
            }

            //reassign each byRef's over to byVal
            foreach (var byVal in byVals)
            {
                foreach (var nm in Items)
                {
                    nm.ReassignAnyItemsByName(byVal);
                }
            }
        }

        /// <summary>
        /// Removes items from <see cref="Items"/> which do not have 
        /// both &apos;get_[...]&apos; and &apos;set_[...]&apos; method for the 
        /// same property (by name)
        /// </summary>
        /// <returns></returns>
        public void RemovePropertiesWithoutBothGetAndSet()
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Items)
            {
                if (!NfReflect.IsClrMethodForProperty(name.GetMemberName(), out var propName))
                {
                    names.Add(name);
                    continue;
                }

                var countOfProp = Items.Count(n => n.GetMemberName().Contains($"get_{propName}(")
                                                   || n.GetMemberName().Contains($"set_{propName}("));
                if (countOfProp < 2)
                    continue;
                names.Add(name);
            }

            Items = names.ToArray();
        }

        public void RemoveClrGeneratedNames()
        {
            var names = new List<MetadataTokenName>();
            foreach (var name in Items)
            {
                if (NfReflect.IsClrGeneratedType(name.Name))
                    continue;
                names.Add(name);
            }

            Items = names.ToArray();
        }
    }
}