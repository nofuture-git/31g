using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NoFuture.Shared.Core;

namespace NoFuture.Shared.DotNetMeta
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