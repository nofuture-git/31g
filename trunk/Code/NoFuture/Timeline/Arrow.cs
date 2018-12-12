using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Util;
using NoFuture.Util.Core;
using NfString = NoFuture.Util.Core.NfString;

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
        public Block FromBlock => _fromBlock;

        public Block ToBlock => _toBlock;
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
        public virtual string Id => _id;
        public virtual int RightMaxLen { get; set; }
        public virtual int LeftMinLen { get; set; }

        #endregion

        #region methods

        public TextCanvas ComposeArrow(Rule ruler, TextCanvas tc)
        {
            if (ruler == null)
                throw new NoRuleSetException();
            var arrowIdx = ruler.CalcEntryIndex(this);

            Width = tc.Width;

            //find the line on the text canvas
            var ti = tc.Items.FirstOrDefault(t => t.Index == arrowIdx.Item1);
            if (ti == null)
                return tc;

            //scope in on the ranges for the located line
            TextRange fromTr;
            TryGetTextRange(ti.Ranges, FromBlock.Id, out fromTr);
            TextRange toTr;
            TryGetTextRange(ti.Ranges, ToBlock.Id, out toTr);

            if (fromTr == null || toTr == null)
                return tc;

            System.Diagnostics.Debug.WriteLine(
                $"({(!string.IsNullOrWhiteSpace(tc.Id) ? "Outer Block" : "InnerBlock")}) {fromTr.Id} -> {toTr.Id}");

            var arrowText = new StringBuilder();
            var arrowDirection = DetermineArrowDirection(fromTr.Id, toTr.Id, ti.Ranges);
            switch (arrowDirection)
            {
                case PrintLocation.Left:
                    arrowText = GetLeftArrowStrBldr(fromTr, toTr, ti);
                    break;
                case PrintLocation.Right:
                    arrowText = GetRightArrowStrBldr(fromTr, toTr, ti);
                    break;
            }

            tc.Items.First(t => t.Index == arrowIdx.Item1).Text =
                NfString.MergeString(arrowText.ToString(),
                    new string(tc.Items.First(t => t.Index == arrowIdx.Item1).Text.ToArray())).ToCharArray().ToList();
            
            return tc;
        }

        internal static int GetRightMaxLength(TextRange rightTr, List<TextRange> trs)
        {
            if (rightTr == null || trs == null || !trs.Any())
                return 0;
            var sum = 0;
            foreach (var r in trs)
            {
                var matchOnInner = r.InnerRanges.Any(x => x.Id == rightTr.Id);
                sum += (matchOnInner ? GetRightMaxLength(rightTr, r.InnerRanges) : r.Length) + 1;
                if (r.Id == rightTr.Id || matchOnInner)
                    break;
            }
            return sum;
        }

        internal static int GetLeftMinLength(TextRange leftTr, List<TextRange> trs)
        {
            if (leftTr == null || trs == null || !trs.Any())
                return 0;
            var sum = 0;
            foreach (var r in trs)
            {
                var matchOnInner = r.InnerRanges.Any(x => x.Id == leftTr.Id);
                if (r.Id == leftTr.Id)
                    break;
                if (matchOnInner)
                {
                    sum += GetLeftMinLength(leftTr, r.InnerRanges);
                    break;
                }
                sum += r.Length + 1;
            }
            return sum;
        }

        protected internal StringBuilder GetLeftArrowStrBldr(TextRange fromTr, TextRange toTr, TextItem ti)
        {
            var leftTr = fromTr;
            var rightTr = toTr;

            RightMaxLen = GetRightMaxLength(rightTr, ti.Ranges);
            LeftMinLen = GetLeftMinLength(leftTr, ti.Ranges);

            System.Diagnostics.Debug.WriteLine($"\t Left Min {LeftMinLen}, Right Max {RightMaxLen}");
            var aPriorLen = string.IsNullOrWhiteSpace(ExcludedRangeId)
                ? GetLeftAPriorLen(leftTr.Id, ti.Ranges)
                : GetLeftAPriorLen(leftTr.Id, ti.Ranges, ExcludedRangeId);

            var betwixtLen = GetRangeLenBetwixt(leftTr.Id, rightTr.Id, ti.Ranges);
            return ComposeFromLeftToRightArrow(betwixtLen, aPriorLen, leftTr.Length, Text, rightTr.Length);
        }

        protected internal StringBuilder GetRightArrowStrBldr(TextRange fromTr, TextRange toTr, TextItem ti)
        {
            var leftTr = toTr;
            var rightTr = fromTr;

            RightMaxLen = GetRightMaxLength(rightTr, ti.Ranges);
            LeftMinLen = GetLeftMinLength(leftTr, ti.Ranges);

            System.Diagnostics.Debug.WriteLine($"\t Left Min {LeftMinLen}, Right Max {RightMaxLen}");
            var aPriorLen = string.IsNullOrWhiteSpace(ExcludedRangeId)
                ? GetLeftAPriorLen(leftTr.Id, ti.Ranges)
                : GetLeftAPriorLen(leftTr.Id, ti.Ranges, ExcludedRangeId);
            var betwixtLen = GetRangeLenBetwixt(leftTr.Id, rightTr.Id, ti.Ranges);
            return ComposeFromRightToLeftArrow(betwixtLen, aPriorLen, leftTr.Length, Text, rightTr.Length);
        }

        internal static bool TryGetTextRange(List<TextRange> ranges, string blockId, out TextRange otr)
        {
            otr = ranges.FirstOrDefault(tr => tr.Id == blockId);
            if (otr != null)
                return true;
            foreach (var sti in ranges.Where(s => s.InnerRanges.Any()))
            {
                var iti = sti.InnerRanges.FirstOrDefault(tr => tr.Id == blockId);
                if (iti == null)
                    continue;
                otr = new TextRange
                {
                    Id = iti.Id,
                    Length = iti.Length,
                    StartIndex = sti.StartIndex + iti.StartIndex
                };
            }
            return otr != null;
        }

        protected internal StringBuilder ComposeFromLeftToRightArrow(int rangeBetwixtLen, int leftAPriorLen,
            int leftBlockLen, string arrowText, int rightBlockLen = 0)
        {

            var strBuilder = new StringBuilder();
            strBuilder.Append(new string((char) 0x20, (leftAPriorLen - 1 <= 0) ? 0 : leftAPriorLen - 1));

            var arrowTail = ArrowTail;
            strBuilder.Append(new string((char) 0x20, (leftBlockLen - arrowTail.Length)));
            strBuilder.Append(arrowTail);

            var head = FromLeftToRightArrowHead;
            //if the rbl was passed in and the text plus head won't fit in it then
            if (rightBlockLen > 0 && arrowText.Length + head.Length > rightBlockLen)
            {
                rangeBetwixtLen = rangeBetwixtLen - rightBlockLen > 0 ? rangeBetwixtLen - rightBlockLen : 0;
            }

            strBuilder.Append(new string(ArrowShaft, rangeBetwixtLen));
            strBuilder.Append(arrowText);

            if (strBuilder.Length + head.Length < Width)
                strBuilder.Append(head);
            else
            {
                strBuilder.Append(head.ToCharArray().Last());
            }

            return strBuilder;
        }

        protected internal StringBuilder ComposeFromRightToLeftArrow(int rangeBetwixtLen, int leftAPriorLen,
            int leftBlockLen, string arrowText, int rightBlockLen = 0)
        {

            var strBuilder = new StringBuilder();
            strBuilder.Append(new string((char) 0x20, leftAPriorLen + 1));

            var head = FromRightToLeftArrowHead;

            //determine number of spaces from start idx to start of arrow text
            var padLen = leftBlockLen - (arrowText.Length + head.Length);
            if (padLen > 0)
                strBuilder.Append(new string((char) 0x20, padLen));
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
            if (strBuilder.Length + ArrowTail.Length < Width && strBuilder.Length + ArrowTail.Length < RightMaxLen)
                strBuilder.Append(ArrowTail);
            else
            {
                strBuilder.Append(ArrowTail.ToCharArray().Last());
            }

            return strBuilder;
        }

        internal static int GetLeftAPriorLen(string trLeftId, List<TextRange> ranges, string excludedRangeId = "")
        {
            var aPriorLen = 0;
            if (ranges == null || !ranges.Any())
                return 0;
            foreach (var range in ranges)
            {
                if (range.InnerRanges.Any(y => y.Id == trLeftId))
                {
                    aPriorLen += GetLeftAPriorLen(trLeftId, range.InnerRanges, excludedRangeId);
                    break;
                }
                if (range.Id == trLeftId)
                    break;
                if (range.Id == excludedRangeId)
                    continue;
                aPriorLen += range.Length + 2;//ranges do not encompass the joist between blocks
            }

            return aPriorLen;
        }

        internal static int GetRangeLenBetwixt(string trLeftId, string trRightId, List<TextRange> ranges)
        {
            var rangeLen = 0;
            if (ranges == null || !ranges.Any())
                return 0;
            var inRange = false;
            foreach (var range in ranges)
            {
                //continue left-to-right until the To's Id is in scope
                if (range.Id != trLeftId && range.InnerRanges.All(x => x.Id != trLeftId) && !inRange)
                {
                    continue;
                }

                //enter into range
                if (range.Id == trLeftId || range.InnerRanges.Any(x => x.Id == trLeftId))
                {
                    //add the ranges from right-to-left for inner ranges
                    var innerRanges = range.InnerRanges;
                    innerRanges.Reverse();
                    foreach (var innerRange in innerRanges)
                    {
                        if (innerRange.Id == trLeftId)
                            break;
                        rangeLen += innerRange.Length + 2;
                    }
                    inRange = true;
                    continue;
                }

                if (range.Id == trRightId || range.InnerRanges.Any(x => x.Id == trRightId))
                {

                    foreach (var innerRange in range.InnerRanges)
                    {
                        if (innerRange.Id == trRightId)
                            break;
                        rangeLen += innerRange.Length + 2;
                    }
                    break;
                }

                //HACK - testing for Id being a Guid
                var isGuidId = System.Text.RegularExpressions.Regex.IsMatch(range.Id,
                    @"[a-f0-9]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}");

                if (isGuidId)
                    continue;

                //ranges do not encompass the joist between blocks
                rangeLen += range.Length + 2;
            }
            return rangeLen;
        }

        internal static PrintLocation DetermineArrowDirection(string fromBlockId, string toBlockId, List<TextRange> ranges)
        {
            TextRange fromTr;
            TryGetTextRange(ranges, fromBlockId, out fromTr);
            TextRange toTr;
            TryGetTextRange(ranges, toBlockId, out toTr);

            if (fromTr == null || toTr == null)
                return PrintLocation.Center;

            return fromTr.StartIndex > toTr.StartIndex ? PrintLocation.Right : PrintLocation.Left;
        }

        #endregion
    }
}