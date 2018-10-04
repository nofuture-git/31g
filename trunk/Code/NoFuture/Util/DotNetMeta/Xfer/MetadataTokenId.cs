using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace NoFuture.Util.DotNetMeta.Xfer
{
    /// <summary>
    /// Defines the identity of a single metadata token.
    /// </summary>
    [Serializable]
    public class MetadataTokenId
    {
        /// <summary>
        /// The index of the <see cref="Assembly"/>  with 
        /// whose <see cref="Assembly.ManifestModule"/>
        /// should be used to resolve the <see cref="Id"/>.
        /// </summary>
        public int RslvAsmIdx;
        public int Id;
        public MetadataTokenId[] Items;
        public int IsByRef;

        public override bool Equals(object obj)
        {
            var objMdti = obj as MetadataTokenId;
            if (objMdti == null)
                return false;
            return objMdti.Id == Id && objMdti.RslvAsmIdx == RslvAsmIdx;
        }

        public override int GetHashCode()
        {
            return RslvAsmIdx.GetHashCode() + Id.GetHashCode();
        }

        /// <summary>
        /// Prints to token ids with <see cref="MetadataTokenId.RslvAsmIdx"/> prefixed to the front.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Print(MetadataTokenId token)
        {
            var depth = 0;
            return Print(token, ref depth);
        }

        public override string ToString()
        {
            var copy = new MetadataTokenId {Id = Id, IsByRef = IsByRef, RslvAsmIdx = RslvAsmIdx};
            return JsonConvert.SerializeObject(copy);
        }


        /// <summary>
        /// Returns a list of distinct <see cref="MetadataTokenId"/> having only thier
        /// <see cref="RslvAsmIdx"/> and <see cref="Id"/> assigned.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="perserveDirectChildItems">Retains the the direct children Items</param>
        /// <returns></returns>
        public static MetadataTokenId[] SelectDistinct(MetadataTokenId token, bool perserveDirectChildItems = false)
        {
            var list = new HashSet<MetadataTokenId>();

            if (token == null)
                return list.ToArray();

            SelectDistinct(token, list, perserveDirectChildItems);
            return list.ToArray();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void SelectDistinct(MetadataTokenId token, HashSet<MetadataTokenId> list,
            bool perserveDirectChildItems = false)
        {
            if (token == null)
                return;
            var iToken = list.FirstOrDefault(x => x.Id == token.Id && x.RslvAsmIdx == token.RslvAsmIdx);
            var preExisting = iToken != null;
            iToken = iToken ?? new MetadataTokenId {Id = token.Id, RslvAsmIdx = token.RslvAsmIdx};
            if(!preExisting)
                list.Add(iToken);

            if (token.Items == null)
                return;

            if (perserveDirectChildItems)
            {
                var i2j = new HashSet<MetadataTokenId>();
                if (iToken.Items != null)
                {
                    foreach (var existingItem in iToken.Items)
                    {
                        if (existingItem.Equals(iToken))
                            continue;
                        i2j.Add(existingItem);
                    }
                }
                    
                foreach (var ijToken in token.Items)
                {
                    if (ijToken.Equals(iToken))
                        continue;
                    i2j.Add(new MetadataTokenId {Id = ijToken.Id, RslvAsmIdx = ijToken.RslvAsmIdx});
                }
                if (i2j.Any())
                    iToken.Items = i2j.ToArray();
            }

            foreach (var jToken in token.Items)
            {
                SelectDistinct(jToken, list, perserveDirectChildItems);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static string Print(MetadataTokenId token, ref int currentDepth)
        {
            if (token == null)
                return string.Empty;
            var strBldr = new StringBuilder();
            var tabs = currentDepth <= 0 ? string.Empty : new string('\t', currentDepth);
            strBldr.AppendFormat("{0}{1}.{2}\n", tabs, token.RslvAsmIdx, token.Id.ToString("X4"));
            if (token.Items == null || token.Items.Length <= 0)
                return strBldr.ToString();

            foreach (var tToken in token.Items)
            {
                currentDepth += 1;
                strBldr.Append(Print(tToken, ref currentDepth));
                currentDepth -= 1;
            }
            return strBldr.ToString();
        }
    }
}