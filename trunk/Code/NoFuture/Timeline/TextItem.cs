using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Timeline
{
    [Serializable]
    public class TextItem
    {
        #region properties
        public int Index { get; set; }
        public double HashMarkValue { get; set; }
        public List<char> Text { get; set; } = new List<char>();
        public List<TextRange> Ranges { get; set; } = new List<TextRange>();

        #endregion

        #region methods
        public bool IsEmptyText()
        {
            if (Text == null)
                return true;
            return Text.Count == 0;
        }//end IsEmptyText
        public TextItem Copy()
        {
            var ti = new TextItem { HashMarkValue = HashMarkValue, Index = Index };
            var txtChars = new char[Text.Count];
            Text.CopyTo(txtChars);
            ti.Text = txtChars.ToList();
            foreach (var tr in Ranges)
            {
                ti.Ranges.Add(new TextRange {Id = tr.Id, Length = tr.Length, StartIndex = tr.StartIndex});
            }
            return ti;
        }//end Copy
        #endregion
    }
}