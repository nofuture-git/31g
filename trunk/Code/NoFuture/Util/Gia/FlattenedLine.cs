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

        public virtual bool Equals(FlattenedLine line)
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

        public virtual bool Contains(string targetWord)
        {
            return UseTypeNames
                ? Items.Any(x => string.Format("{0}", x.TypeFullName).Contains(targetWord))
                : Items.Any(x => x.FlName.Contains(targetWord));
        }

        public virtual FlattenedItem FirstOrDefaultOnWord(string targetWord)
        {
            return UseTypeNames
                ? Items.FirstOrDefault(x => string.Format("{0}", x.TypeFullName).Contains(targetWord))
                : Items.FirstOrDefault(x => x.FlName.Contains(targetWord));
        }

        public virtual bool LastItemContains(string targetWord)
        {
            return UseTypeNames
                ? string.Format("{0}", Items.Last().TypeFullName).Contains(targetWord)
                : Items.Last().FlName.Contains(targetWord);
        }

        public virtual FlattenedItem FirstOnLeft(FlattenedItem searchItem)
        {
            if (Items == null)
                return null;
            var searchIndex = Items.IndexOf(searchItem);
            return searchIndex <= 0 ? null : Items[searchIndex - 1];
        }

        public virtual FlattenedItem FirstOnRight(FlattenedItem searchItem)
        {
            if (Items == null)
                return null;
            var searchIndex = Items.IndexOf(searchItem);
            if (searchIndex < 0)
                return null;
            return searchIndex == Items.Count - 1 ? null : Items[searchIndex + 1];
        }

        public virtual string ToFlattenedString(string separator, bool usetypeNames)
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

    public class NullFlattenedLine : FlattenedLine
    {
        private readonly List<FlattenedItem> _blank = new List<FlattenedItem>
        {
            new FlattenedItem(typeof (object)) {FlName = string.Empty }
        };

        private readonly Exception _error;

        public NullFlattenedLine(Exception ex)
            : base(new List<FlattenedItem>
            {
                new FlattenedItem(typeof (object)) {FlName = string.Empty}
            })
        {
            _error = ex;
        }

        public override bool Equals(FlattenedLine line)
        {
            var nfl = line as NullFlattenedLine;
            return nfl != null;
        }

        public override bool Contains(string targetWord)
        {
            return false;
        }

        public override FlattenedItem FirstOnLeft(FlattenedItem searchItem)
        {
            return _blank.FirstOrDefault();
        }

        public override FlattenedItem FirstOnRight(FlattenedItem searchItem)
        {
            return _blank.FirstOrDefault();
        }

        public override FlattenedItem FirstOrDefaultOnWord(string targetWord)
        {
            return _blank.FirstOrDefault();
        }

        public override bool LastItemContains(string targetWord)
        {
            return false;
        }

        public override string ToFlattenedString(string separator, bool usetypeNames)
        {
            return _error == null ? string.Empty : string.Format("{0}\n{1}", _error.Message, _error.StackTrace);
        }
    }

}