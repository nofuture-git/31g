using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Sec
{
    /// <summary>
    /// &quot;
    /// The federal securities laws require public companies to disclose 
    /// information on an ongoing basis. For example, domestic companies 
    /// must submit annual reports on Form 10-K, quarterly reports on 
    /// Form 10-Q, and current reports on Form 8-K for a number of 
    /// specified events and must comply with a variety of other 
    /// disclosure requirements.
    /// &quot;
    /// src [https://www.sec.gov/fast-answers/answers-form10khtm.html]
    /// </summary>
    [Serializable]
    public abstract class SecForm : Identifier
    {
        /// <summary>
        /// Inner type to order the <see cref="SecForm.GetTextBlocks()"/> by the length of
        /// Item2
        /// </summary>
        internal class TextBlockComparer : IComparer<Tuple<string, string>>
        {
            private const int Y_IS_GT_X = -1;
            private const int X_IS_GT_Y = 1;

            public int Compare(Tuple<string, string> x, Tuple<string, string> y)
            {
                if (x == null && y == null)
                    return 0;
                if (x?.Item2 == null)
                    return Y_IS_GT_X;
                if (y?.Item2 == null)
                    return X_IS_GT_Y;

                var xItem2Len = x.Item2.Length;
                var yItem2Len = y.Item2.Length;

                if (xItem2Len == yItem2Len)
                    return 0;

                return xItem2Len > yItem2Len ? X_IS_GT_Y : Y_IS_GT_X;
            }
        }

        #region constants
        public const string NOTIFICATION_OF_INABILITY_TO_TIMELY_FILE = "NT";
        private const StringComparison STR_OPT = StringComparison.OrdinalIgnoreCase;
        #endregion

        #region fields
        protected string secFormNumber;
        private readonly List<Tuple<string, string>> _textBlocks = new List<Tuple<string, string>>();
        private readonly IComparer<Tuple<string, string>> _textBlockComparer = new TextBlockComparer();
        #endregion

        #region properties
        public CentralIndexKey CIK { get; set; }
        public override string Abbrev => secFormNumber;
        public override string Value { get; set; }
        public DateTime FilingDate { get; set; }

        /// <summary>
        /// This html will contain, when available from the SEC, the uri to the XBRL xml
        /// </summary>
        public Uri HtmlFormLink { get; set; }
        
        /// <summary>
        /// Reports prefixed with <see cref="NOTIFICATION_OF_INABILITY_TO_TIMELY_FILE"/> will 
        /// have this set to true.
        /// </summary>
        public bool IsLate { get; set; }
        public string AccessionNumber { get => Value; set => Value = value; }
        public Uri XmlLink { get; set; }


        #endregion

        protected SecForm(string secFormNumber)
        {
            this.secFormNumber = secFormNumber;
        }

        #region methods

        /// <summary>
        /// Represents any kind of text found in an SEC report which is tagged with 
        /// some kind of label so that it has a sense of context.
        /// </summary>
        public List<Tuple<string, string>> GetTextBlocks()
        {
            _textBlocks.Sort(_textBlockComparer);
            return _textBlocks;
        }

        /// <summary>
        /// Adds a single text block tuple to the underlying data
        /// </summary>
        /// <param name="textBlock"></param>
        public void AddTextBlock(Tuple<string, string> textBlock)
        {
            if (textBlock == null)
                return;
            if (_textBlocks.Any(tb =>
                string.Equals(tb.Item1, textBlock.Item1, STR_OPT) && string.Equals(tb.Item2, textBlock.Item2, STR_OPT)))
                return;
            _textBlocks.Add(textBlock);
        }

        /// <summary>
        /// Adds a range of text block tuples to the underlying data
        /// </summary>
        /// <param name="textBlocks"></param>
        public void AddRangeTextBlocks(IEnumerable<Tuple<string, string>> textBlocks)
        {
            if (textBlocks == null || !textBlocks.Any())
                return;
            foreach (var tb in textBlocks)
            {
                AddTextBlock(tb);
            }
        }

        /// <summary>
        /// Helper method to ctor an instance of <see cref="SecForm"/>
        /// using it common abbreviation
        /// </summary>
        /// <param name="reportAbbrev"></param>
        /// <returns></returns>
        public static SecForm SecFormFactory(string reportAbbrev)
        {
            if (string.IsNullOrWhiteSpace(reportAbbrev))
                return null;
            var secFormTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.BaseType == typeof(SecForm)).ToList();

            Type secFormType = null;
            foreach (var mt in secFormTypes)
            {
                var fis = mt.GetFields().Where(x => x.IsLiteral);
                foreach (var fi in fis)
                {
                    var fiVal = fi.GetValue(null) as string;
                    if (fiVal == null)
                        continue;
                    if (!fiVal.Equals(reportAbbrev, StringComparison.OrdinalIgnoreCase))
                        continue;
                    secFormType = mt;
                    break;
                }
            }

            if (secFormType == null)
                return null;
            return Activator.CreateInstance(secFormType) as SecForm;
        }
        #endregion
    }
}