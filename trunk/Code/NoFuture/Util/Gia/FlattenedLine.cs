using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Gia
{
    /// <summary>
    /// Represents a single fully expanded and flattened property of <see cref="FlattenedType"/>
    /// </summary>
    public class FlattenedLine
    {
        public FlattenedLine(List<FlattenedItem> items)
        {
            Items = items;
        }
        public string Separator { get; set; }
        public bool UseTypeNames { get; set; }
        public string ValueType { get; set; }
        public List<FlattenedItem> Items { get; private set; }

        public bool Equals(FlattenedLine line)
        {
            if (line == null)
                return false;
            if (!string.Equals(line.ValueType, ValueType, StringComparison.OrdinalIgnoreCase))
                return false;
            if (Items.Count != line.Items.Count)
                return false;
            for (var i = 0; i < Items.Count; i++)
            {
                if (!Items[i].Equals(line.Items[i]))
                    return false;
            }

            return true;
        }

        public bool Contains(string targetWord)
        {
            return UseTypeNames
                ? Items.Any(x => string.Format("{0}", x.FlType).Contains(targetWord))
                : Items.Any(x => x.FlName.Contains(targetWord));
        }

        public FlattenedItem FirstOrDefaultOnWord(string targetWord)
        {
            return UseTypeNames
                ? Items.FirstOrDefault(x => string.Format("{0}", x.FlType).Contains(targetWord))
                : Items.FirstOrDefault(x => x.FlName.Contains(targetWord));
        }

        public bool LastItemContains(string targetWord)
        {
            return UseTypeNames
                ? string.Format("{0}", Items.Last().FlType).Contains(targetWord)
                : Items.Last().FlName.Contains(targetWord);
        }

        public FlattenedItem FirstOnLeft(FlattenedItem searchItem)
        {
            if (Items == null)
                return null;
            var searchIndex = Items.IndexOf(searchItem);
            return searchIndex <= 0 ? null : Items[searchIndex - 1];
        }

        public FlattenedItem FirstOnRight(FlattenedItem searchItem)
        {
            if (Items == null)
                return null;
            var searchIndex = Items.IndexOf(searchItem);
            if (searchIndex < 0)
                return null;
            return searchIndex == Items.Count - 1 ? null : Items[searchIndex + 1];
        }

        public string ToFlattenedString(string separator, bool usetypeNames)
        {
            var lnBldr = new StringBuilder();
            lnBldr.Append(ValueType);
            lnBldr.Append(" ");

            lnBldr.Append(usetypeNames
                ? string.Join(separator, Items.Select(x => x.SimpleTypeName))
                : string.Join(separator, Items.Select(x => x.FlName)));

            return lnBldr.ToString();
        }
    }


}