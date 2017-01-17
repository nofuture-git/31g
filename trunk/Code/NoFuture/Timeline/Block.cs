using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    [Serializable]
    public class Block : IDrawing, IRuleEntry
    {
        #region fields
        protected internal List<Entry> _entries;
        protected internal List<Block> _innerBlocks;
        protected internal List<Arrow> _arrows;
        protected internal int _width;
        protected internal double _startValue;
        protected internal bool _startValueExplicitlySet;
        protected internal double _endValue;
        protected internal bool _endValueExplicitlySet;
        protected internal Rule _myRuler;
        #endregion

        #region properties

        public virtual Rule Ruler
        {
            get
            {
                return _myRuler;
            }
            set
            {
                _myRuler = value;
                foreach (var entry in _entries)
                    entry.Ruler = _myRuler;
                foreach (var iBlk in _innerBlocks)
                    iBlk.Ruler = _myRuler;
            }
        }

        public virtual string Title { get; set; }
        public virtual char EmptyChar { get; set; }
        #endregion

        #region ctors
        public Block()
        {
            _entries = new List<Entry>();
            _innerBlocks = new List<Block>();
            _width = Config.Numerics.DefaultWidth;
            _arrows = new List<Arrow>();
        }
        #endregion

        #region IRuleEntry methods
        public virtual double StartValue {
            get
            {
                if (!_startValueExplicitlySet && Ruler != null)
                {
                    _startValue = Ruler.StartValue;
                }
                return _startValue;
            }
            set
            {
                _startValueExplicitlySet = true;
                _startValue = value;
            } 
        }
        public virtual double EndValue {
            get
            {
                if (!_endValueExplicitlySet && Ruler != null)
                {
                    _endValue = Ruler.EndValue;
                }
                return _endValue;
            }
            set
            {
                _endValueExplicitlySet = true;
                _endValue = value;
            }
        }
        public virtual int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                _entries.ForEach(e => e.Width = _width);
            }
        }
        public virtual string Id => Title;

        #endregion

        #region Object overrides
        public override string ToString()
        {
            if (Ruler == null)
                throw new NoRuleSetException();

            var strBuilder = new StringBuilder();

            if (EmptyChar == '\0')
                EmptyChar = (char)0x20;

            var entryIndex = Ruler.CalcEntryIndex(this);
            strBuilder.Append(DrawHeader());
            for (var i = entryIndex.Item1 + 1; i <= entryIndex.Item2 - 1; i++)
            {
                strBuilder.AppendLine(new string(EmptyChar, Width));
            }
            strBuilder.Append(DrawBar());
            return strBuilder.ToStringUnixNewline();

        }
        #endregion

        #region public methods
        public virtual void AddEntry(Entry entry)
        {
            entry.Ruler = Ruler;
            entry.Width = Width;
            _entries.Add(entry);

            var reqW = GetRequiredWidth();
            if (Width < reqW)
                Width = reqW;

        }
        public virtual void AddEntry(double startValue, string text)
        {
            AddEntry(new SimpleFactEntry { StartValue = startValue, Text = text });
        }
        public virtual void AddEntry(double startValue, string text, PrintLocation location)
        {
            AddEntry(new SimpleFactEntry { StartValue = startValue,Text = text, Location = location});
        }
        public virtual void AddEntry(double startValue, double endValue, string text)
        {
            AddEntry(new PeriodEntry { StartValue = startValue, EndValue = endValue, Text = text });
        }
        public virtual void AddEntry(double startValue, double endValue, string text, PrintLocation location)
        {
            AddEntry(new PeriodEntry { StartValue = startValue, EndValue = endValue, Text = text, Location = location });
        }//end AddEntry
        public virtual void AddInnerBlock(Block innerBlock)
        {
            if (Ruler == null)
                throw new NoRuleSetException();
            if(innerBlock.StartValue == Ruler.StartValue)
                throw new InnerBlockRequiresStartValue(innerBlock);

            //for the first inner block added take the larger width (thiers or ours)
            if (_innerBlocks.Count == 0)
            {
                var gtWidth = Width > innerBlock.Width ? Width : innerBlock.Width;

                Width = gtWidth;
                innerBlock.Width = gtWidth;
            }
            else//thereafter set the width as a sum 
            {
                Width += innerBlock.Width+1;
            }

            //back the inner block up by one line for printing the title
            var innerBlockIdx = Ruler.CalcEntryIndex(innerBlock);
            var valForNextLine = Convert.ToInt32(Math.Floor(Ruler.GetIndexRule()[(innerBlockIdx.Item1 - 1)]));
            innerBlock.StartValue = valForNextLine;
            _innerBlocks.Add(innerBlock);

        }//end AddInnerBlock

        public virtual void AddArrow(Arrow arrow)
        {
            arrow.ExcludedRangeId = this.Title;
            _arrows.Add(arrow);
        }//end AddArrow
        public virtual TextCanvas PrintEntriesOnCanvas()
        {
            if(Ruler == null)
                throw new NoRuleSetException();

            var myCanvas = this.ToTextCanvas(Ruler);

            TextCanvas myInnerBlockCanvas = null;

            if (_innerBlocks.Count > 0)
            {
                myInnerBlockCanvas = _innerBlocks[0].PrintEntriesOnCanvas();
                for (var i = 1; i < _innerBlocks.Count; i++)
                {
                    myInnerBlockCanvas = myInnerBlockCanvas.Concat(_innerBlocks[i].PrintEntriesOnCanvas(), Ruler);
                }
            }

            foreach (var entry in _entries)
                myCanvas = myCanvas.Merge(entry.ToTextCanvas(Ruler), Ruler);

            if (myInnerBlockCanvas != null)
            {
                myCanvas = myCanvas.Merge(myInnerBlockCanvas, Ruler);
            }
            foreach (var arrow in _arrows)
                myCanvas = arrow.ComposeArrow(Ruler, myCanvas);

            myCanvas.Id = Id;
            return myCanvas;
        }//end PrintEntriesOnCanvas
        #endregion

        #region hidden
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal virtual string DrawHeader()
        {
            var label = Util.Etc.PrintInCenter(Width, Title);
            var bar = DrawBar();
            var strBuilder = new StringBuilder();
            for (var i = 0; i < Width; i++)
            {
                var headerChar = label[i] == (char)0x20 ? bar[i] : label[i];
                strBuilder.Append(headerChar);
            }
            strBuilder.AppendLine();
            return strBuilder.ToStringUnixNewline();
        }//end DrawHeader
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal virtual string DrawBar()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(Config.GraphChars.BarRailIntersect);
            for (var i = 1; i < Width - 1; i++)
            {
                strBuilder.Append(Config.GraphChars.Rail);
            }
            strBuilder.Append(Config.GraphChars.BarRailIntersect);
            strBuilder.AppendLine();

            return strBuilder.ToStringUnixNewline();
        }//end DrawBar
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal virtual int GetRequiredWidth()
        {
            if (_entries.Count <= 0)
                return Width;

            var maxWidthReq = Width;

            //get a distinct list of entries start values
            var distinctStartVals = new List<double>();
            foreach (var entry in _entries.Where(entry => !distinctStartVals.Contains(entry.StartValue)))
                distinctStartVals.Add(entry.StartValue);

            //find contended start values
            foreach (var dsv in distinctStartVals.Where(dsv => _entries.Count(e => e.StartValue == dsv) > 1))
            {
                var reqW = 0;
                var contendedEntries = _entries.Where(e => e.StartValue == dsv).ToList();
                contendedEntries.ForEach(ce => reqW += ce.Text.Length);
                if (maxWidthReq < reqW + Config.Numerics.MinPaddingBetweenContentions)
                    maxWidthReq = reqW + Config.Numerics.MinPaddingBetweenContentions;
            }

            var maxInTextLen = _entries.Max(x => x.Text.Length);
            if (maxInTextLen > maxWidthReq)
                maxWidthReq = maxInTextLen;

            return maxWidthReq;
        }//end GetRequiredWidth
        #endregion
    }
}