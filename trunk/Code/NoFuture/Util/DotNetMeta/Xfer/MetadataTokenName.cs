using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta.Auxx;
using NoFuture.Util.DotNetMeta.Grp;

namespace NoFuture.Util.DotNetMeta.Xfer
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
        private int? _fullMaxDepth;

        [NonSerialized] private MetadataTokenName[] _selectAll;
        [NonSerialized] private MetadataTokenName[] _selectDistinct;

        public MetadataTokenName[] SelectDistinct()
        {
            if (_selectDistinct != null)
                return _selectDistinct;

            var comparer = new MetadataTokenNameComparer();
            var innerItems = new List<MetadataTokenName> {this};
            if (Items == null || !Items.Any())
                return innerItems.ToArray();
            foreach (var item in Items)
            {
                innerItems.AddRange(item.SelectDistinct());
            }

            _selectDistinct = innerItems.Distinct(comparer).ToArray();
            return _selectDistinct;
        }

        public void ApplyFullName(AsmIndicies asmIndicies)
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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
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
            if (_fullMaxDepth != null)
                return _fullMaxDepth.Value;
            var c = 1;
            if (Items == null || !Items.Any())
                return c;
            foreach (var i in Items)
                c += i.GetFullDepthCount();

            _fullMaxDepth = c;
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
                return string.Empty;
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
    }
}