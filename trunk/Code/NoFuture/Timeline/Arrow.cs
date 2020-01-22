using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public bool MoveArrowHeadToCenterOfBlock { get; set; }

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
            var isOuterBlock = !string.IsNullOrWhiteSpace(tc.Id);

            System.Console.WriteLine(
                $"({(isOuterBlock ? "Outer Block" : "InnerBlock")}) {fromTr.Id} -> {toTr.Id}");

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

        internal int GetRightMaxLength(TextRange rightTr, List<TextRange> trs)
        {
            if (rightTr == null || trs == null || !trs.Any())
                return 0;
            var fromInner = FromBlock.IsInnerBlock;
            var toInner = ToBlock.IsInnerBlock;

            var sum = 0;
            foreach (var r in trs)
            {
                if (Guid.TryParse(r.Id, out _))
                    continue;
                sum +=  r.Length + 1;
                if (r.Id == rightTr.Id)
                    break;
            }
            if (fromInner && toInner)
                sum -= Width;
            return sum;
        }

        internal int GetLeftMinLength(TextRange leftTr, List<TextRange> trs)
        {
            if (leftTr == null || trs == null || !trs.Any())
                return 0;
            var fromInner = FromBlock.IsInnerBlock;
            var toInner = ToBlock.IsInnerBlock;

            var sum = 0;
            foreach (var r in trs)
            {
                if(Guid.TryParse(r.Id, out _))
                    continue;

                if (r.Id == leftTr.Id)
                    break;
                
                if (toInner && !fromInner && r.InnerRanges.Any(ri => ri.Id == leftTr.Id))
                {
                    return GetLeftMinLength(leftTr, r.InnerRanges);
                }

                sum += r.Length + 1;
            }

            if (fromInner && toInner)
            {
                sum -= Width;
                sum = sum - 2 > 0 ? sum - 2 : sum;
            }

            return sum;
        }

        protected internal StringBuilder GetLeftArrowStrBldr(TextRange fromTr, TextRange toTr, TextItem ti)
        {
            var leftTr = fromTr;
            var rightTr = toTr;
            var fromInner = FromBlock.IsInnerBlock;
            var toInner = ToBlock.IsInnerBlock;

            RightMaxLen = GetRightMaxLength(rightTr, ti.Ranges);
            LeftMinLen = GetLeftMinLength(leftTr, ti.Ranges);

            System.Console.WriteLine($"\t (LEFT) FromBlock.IsInnerBlock {fromInner}, ToBlock.IsInnerBlock {toInner}," +
                                     $"  Width {Width}, Left Min {LeftMinLen}, Right Max {RightMaxLen}");
            return ComposeFromLeftToRightArrow(leftTr.Length, Text, rightTr.Length);
        }

        protected internal StringBuilder GetRightArrowStrBldr(TextRange fromTr, TextRange toTr, TextItem ti)
        {
            var leftTr = toTr;
            var rightTr = fromTr;
            var fromInner = FromBlock.IsInnerBlock;
            var toInner = ToBlock.IsInnerBlock;

            RightMaxLen = GetRightMaxLength(rightTr, ti.Ranges);
            LeftMinLen = GetLeftMinLength(leftTr, ti.Ranges);

            System.Console.WriteLine($"\t (RIGHT) FromBlock.IsInnerBlock {fromInner}, ToBlock.IsInnerBlock {toInner}," +
                                     $"  Width {Width}, Left Min {LeftMinLen}, Right Max {RightMaxLen}");
            return ComposeFromRightToLeftArrow(leftTr.Length, Text, rightTr.Length);
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

        protected internal StringBuilder ComposeFromLeftToRightArrow(int leftBlockLen, string arrowText, int rightBlockLen)
        {
            var strBuilder = new StringBuilder();
            var leftMaxLength = LeftMinLen + leftBlockLen;
            var rightMinLength = RightMaxLen - rightBlockLen;

            var leftPoint = leftMaxLength - 2;
            var rightPoint = rightMinLength + 2;

            if (MoveArrowHeadToCenterOfBlock)
            {
                rightPoint = RightMaxLen - (int)(leftBlockLen / 2D);
            }

            Console.WriteLine($"\t\t{nameof(leftPoint)}: {leftPoint}");
            Console.WriteLine($"\t\t{nameof(rightPoint)}: {rightPoint}");

            strBuilder.Append((char) 0x20, leftPoint);
            strBuilder.Append(ArrowTail);
            var span = rightPoint - FromLeftToRightArrowHead.Length - (arrowText?.Length ?? 0) - strBuilder.Length;
            if(span > 0)
                strBuilder.Append(ArrowShaft, span);
            strBuilder.Append(arrowText);
            if (strBuilder.Length + FromLeftToRightArrowHead.Length >= RightMaxLen - 1)
            {
                strBuilder.Append(FromLeftToRightArrowHead.ToCharArray().Last());
            }
            else
            {
                strBuilder.Append(FromLeftToRightArrowHead);
            }
            

            return strBuilder;
        }

        protected internal StringBuilder ComposeFromRightToLeftArrow(int leftBlockLen, string arrowText, int rightBlockLen)
        {
            var strBuilder = new StringBuilder();
            var leftMaxLength = LeftMinLen + leftBlockLen;
            var rightMinLength = RightMaxLen - rightBlockLen;

            var leftPoint = leftMaxLength - 2;
            var rightPoint = rightMinLength + 2;

            if (MoveArrowHeadToCenterOfBlock)
            {
                leftPoint = LeftMinLen + (int) (leftBlockLen / 2D);
            }

            Console.WriteLine($"\t\t{nameof(leftPoint)}: {leftPoint}");
            Console.WriteLine($"\t\t{nameof(rightPoint)}: {rightPoint}");

            strBuilder.Append((char)0x20, leftPoint);
            strBuilder.Append(FromRightToLeftArrowHead);
            strBuilder.Append(arrowText);
            var span = rightPoint - ArrowTail.Length - strBuilder.Length;
            if (span > 0)
                strBuilder.Append(ArrowShaft, span);
            if (strBuilder.Length + ArrowTail.Length >= RightMaxLen - 1)
            {
                strBuilder.Append(ArrowTail.ToCharArray().Last());
            }
            else
            {
                strBuilder.Append(ArrowTail);
            }

            return strBuilder;
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