using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    [Serializable]
    public class TextCanvas //this should not implement IRuleEntry
    {
        #region properties
        public Rule Ruler { get; set; }
        public List<TextItem> Items { get; set; }
        public int MinIndex { get; set; }
        public int MaxIndex { get; set; }
        public double StartValue { get; set; }
        public double EndValue { get; set; }
        public int Width { get; set; }
        public string Id { get; set; }
        #endregion

        #region ctors
        public TextCanvas()
        {
            Items = new List<TextItem>();
        }
        #endregion

        #region Object overrides
        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var item in Items)
                str.AppendLine(new string(item.Text.ToArray()));

            return str.ToStringUnixNewline();
        }
        #endregion

        #region methods
        public bool IsEmptyItems()
        {
            if (Items == null)
                return true;
            return Items.Count == 0 || Items.All(i => i.IsEmptyText());
        }//end IsEmptyItems
        public virtual TextItem GetTextOrEmpty(int index, int emptyTextLen = Config.Numerics.DEFAULT_WIDTH)
        {
            if(Ruler == null)
                throw new NoRuleSetException();
            var emptyText = new string(' ', emptyTextLen).ToCharArray().ToList();
            if (IsEmptyItems() || Ruler.GetIndexRule().Count < index)
            {
                return new TextItem() {Index = -1, Text = emptyText};
            }

            var matchedItem = Items.FirstOrDefault(ti => ti.Index == index);
            return matchedItem ??
                   new TextItem() {HashMarkValue = Ruler.GetIndexRule()[index], Index = index, Text = emptyText};
        }//end GetTextOrEmpty
        #endregion
    }
}