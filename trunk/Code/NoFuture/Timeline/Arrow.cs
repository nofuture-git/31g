using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Util;

namespace NoFuture.Timeline
{
    [Serializable]
    public class Arrow : IRuleEntry 
    {
        #region fields
        protected Block _fromBlock;
        protected Block _toBlock;
        protected double _singleValue;
        private readonly string _id;
        private string _fromLeftToRightArrowHead = Config.GraphStrings.ArrowHeadLeft;
        private string _arrowTail = Config.GraphStrings.ArrowTailShaft;
        private string _fromRightToLeftArrowHead = Config.GraphStrings.ArrowHeadRight;
        private char _arrowShaft = Config.GraphChars.DirectShaft;

        #endregion

        #region ctors

        internal Arrow()
        {
            Text = string.Empty;
        }
        public Arrow(Block fromBlock, Block toBlock)
        {
            _fromBlock = fromBlock;
            _toBlock = toBlock;
            _id = $"From={fromBlock.Id},To={toBlock.Id}";
            Text = string.Empty;
        }
        #endregion

        #region properties
        public string Text { get; set; }
        public Block FromBlock
        {
            get { return _fromBlock; }
            
        }
        public Block ToBlock
        {
            get { return _toBlock; }
        }
        public string ExcludedRangeId { get; set; }
        public string FromRightToLeftArrowHead
        {
            get { return _fromRightToLeftArrowHead; }
            set { _fromRightToLeftArrowHead = value; }
        }
        public string FromLeftToRightArrowHead
        {
            get { return _fromLeftToRightArrowHead; }
            set { _fromLeftToRightArrowHead = value; }
        }
        public string ArrowTail
        {
            get { return _arrowTail; }
            set { _arrowTail = value; }
        }
        public char ArrowShaft
        {
            get { return _arrowShaft; }
            set { _arrowShaft = value; }
        }
        #endregion

        #region IRuleEntry methods
        public virtual double StartValue
        {
            get { return _singleValue; }
            set { _singleValue = value; }
        }
        public virtual double EndValue
        {
            get { return _singleValue; }
            set { _singleValue = value; }
        }
        public virtual int Width { get; set; }
        public virtual string Id { get { return _id; } }
        #endregion

        #region methods
        public TextCanvas ComposeArrow(Rule ruler, TextCanvas tc)
        {
            if (ruler == null)
                throw new NoRuleSetException();
            var arrowIdx = ruler.CalcEntryIndex(this);

            //find the line on the text canvas
            var ti = tc.Items.FirstOrDefault(t => t.Index == arrowIdx.Item1);
            if (ti == null)
                return tc;

            //scope in on the ranges for the located line
            var fromTr = ti.Ranges.FirstOrDefault(tr => tr.Id == FromBlock.Id);
            var toTr = ti.Ranges.FirstOrDefault(tr => tr.Id == ToBlock.Id);

            if (fromTr == null || toTr == null)
                return tc;
            var arrowText = new StringBuilder();
            var arrowDirection = DetermineArrowDirection(fromTr.Id, toTr.Id, ti.Ranges);
            if (arrowDirection == PrintLocation.Left)
            {
                var leftTr = fromTr;
                var rightTr = toTr;
                var aPriorLen = String.IsNullOrWhiteSpace(ExcludedRangeId)
                    ? GetLeftAPriorLen(leftTr.Id, ti.Ranges)
                    : GetLeftAPriorLen(leftTr.Id, ti.Ranges, ExcludedRangeId);

                var betwixtLen = GetRangeLenBetwixt(leftTr.Id, rightTr.Id, ti.Ranges);
                arrowText = ComposeToRightArrow(betwixtLen, aPriorLen, leftTr.Length, Text, rightTr.Length);
            }
            else if (arrowDirection == PrintLocation.Right)
            {
                var leftTr = toTr;
                var rightTr = fromTr;
                var aPriorLen = String.IsNullOrWhiteSpace(ExcludedRangeId)
                    ? GetLeftAPriorLen(leftTr.Id, ti.Ranges)
                    : GetLeftAPriorLen(leftTr.Id, ti.Ranges, ExcludedRangeId);
                var betwixtLen = GetRangeLenBetwixt(leftTr.Id, rightTr.Id, ti.Ranges);
                arrowText = ComposeToLeftArrow(betwixtLen, aPriorLen, leftTr.Length, Text);
            }

            tc.Items.First(t => t.Index == arrowIdx.Item1).Text =
                Etc.MergeString(arrowText.ToString(),
                    new string(tc.Items.First(t => t.Index == arrowIdx.Item1).Text.ToArray())).ToCharArray().ToList();
            
            return tc;
        }//end ComposeArrow
        #endregion

        #region hidden
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal StringBuilder ComposeToRightArrow(int rangeBetwixtLen, int leftAPriorLen, int leftBlockLen, string arrowText, int rightBlockLen = 0)
        {
            System.Diagnostics.Debug.WriteLine(
                string.Format(
                    "{0} is ComposeToRightArrow, rangeBetwixtLen={1}, leftAPriorLen = {2}, leftBlockLen = {3}, rightBlockLen = {4}, arrowTailLength = {5}, arrowHeadLength = {6}",
                    arrowText, rangeBetwixtLen, leftAPriorLen, leftBlockLen, rightBlockLen,ArrowTail.Length, FromLeftToRightArrowHead.Length));

            var strBuilder = new StringBuilder();
            strBuilder.Append(new string((char)0x20, (leftAPriorLen - 1 <= 0) ? 0 : leftAPriorLen -1));

            var arrowTail = ArrowTail;
            strBuilder.Append(new string((char)0x20, (leftBlockLen - arrowTail.Length)));
            strBuilder.Append(arrowTail);

            var head = FromLeftToRightArrowHead;
            //if the rbl was passed in and the text plus head won't fit in it then
            if (rightBlockLen > 0 && arrowText.Length + head.Length > rightBlockLen)
            {
                rangeBetwixtLen = rangeBetwixtLen - rightBlockLen > 0 ? rangeBetwixtLen - rightBlockLen : 0;
            }

            strBuilder.Append(new string(ArrowShaft, rangeBetwixtLen));
            strBuilder.Append(arrowText);
            strBuilder.Append(head);

            return strBuilder;
        }//end ComposeToRightArrow
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal StringBuilder ComposeToLeftArrow(int rangeBetwixtLen, int leftAPriorLen, int leftBlockLen, string arrowText)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} is ComposeToLeftArrow", arrowText));

            var strBuilder = new StringBuilder();
            strBuilder.Append(new string((char)0x20, leftAPriorLen + 1));

            var head = FromRightToLeftArrowHead;

            //determine number of spaces from start idx to start of arrow text
            var padLen = leftBlockLen - (arrowText.Length + head.Length);
            if (padLen > 0)
                strBuilder.Append(new string((char)0x20, padLen));
            strBuilder.Append(head);
            strBuilder.Append(arrowText);

            if (Math.Abs(leftBlockLen - head.Length) < arrowText.Length)
            {
                var reduxRange = rangeBetwixtLen -
                                 ((Math.Abs(leftBlockLen - head.Length)) - (arrowText.Length - 1));
                strBuilder.Append(new string(ArrowShaft, reduxRange));
            }
            else
            {
                strBuilder.Append(new string(ArrowShaft, rangeBetwixtLen));    
            }

            strBuilder.Append(ArrowTail);

            return strBuilder;
        }//end ComposeToLeftArrow
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal static int GetLeftAPriorLen(string trLeftId, List<TextRange> ranges, string excludedRangeId = "")
        {
            var aPriorLen = 0;
            foreach (var range in ranges)
            {
                
                if (range.Id == trLeftId)
                    break;
                if (range.Id == excludedRangeId)
                    continue;
                aPriorLen += range.Length + 2;//ranges do not encompass the joist between blocks
            }

            return aPriorLen;
        }//end GetLeftAPriorLen
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal static int GetRangeLenBetwixt(string trLeftId, string trRightId, List<TextRange> ranges)
        {
            var rangeLen = 0;
            var inRange = false;
            foreach (var range in ranges)
            {
                //continue left-to-right until the To's Id is in scope
                if (range.Id != trLeftId && !inRange)
                    continue;

                if (range.Id == trLeftId)
                {
                    inRange = true;
                    continue;
                }
                if (range.Id == trRightId)
                    break;

                //HACK - testing for Id being a Guid
                var isGuidId = System.Text.RegularExpressions.Regex.IsMatch(range.Id,
                    @"[a-f0-9]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}");
                if (!isGuidId)
                    rangeLen += range.Length + 2;//ranges do not encompass the joist between blocks

            }
            return rangeLen;
        }//end GetRangeLenBetwixt
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal static PrintLocation DetermineArrowDirection(string fromBlockId, string toBlockId, List<TextRange> ranges)
        {
            var fromTr = ranges.FirstOrDefault(tr => tr.Id == fromBlockId);
            var toTr = ranges.FirstOrDefault(tr => tr.Id == toBlockId);

            if (fromTr == null || toTr == null)
                return PrintLocation.Center;

            return fromTr.StartIndex > toTr.StartIndex ? PrintLocation.Right : PrintLocation.Left;
        }//end DetermineArrowDirection
        #endregion
    }
}