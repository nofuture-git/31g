using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util;

namespace NoFuture.Timeline
{
    public interface IDrawing
    {
        Rule Ruler { get; set; }
    }

    public interface IRuleEntry
    {
        int StartValue { get; set; }
        int EndValue { get; set; }
        int Width { get; set; }
        string Id { get; }
    }//end IRuleEntry

    [Serializable]
    public class TextRange
    {
        public string Id { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
    }//end TextRange

    [Serializable]
    public class TextItem
    {
        #region properties
        public int Index { get; set; }
        public double HashMarkValue { get; set; }
        public List<char> Text { get; set; }
        public List<TextRange> Ranges { get; set; }
        #endregion

        #region ctors
        public TextItem()
        {
            Ranges = new List<TextRange>();
            Text = new List<char>();
        }
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
    }//end TextItem

    [Serializable]
    public class TextCanvas //this should not implement IRuleEntry
    {
        #region properties
        public Rule Ruler { get; set; }
        public List<TextItem> Items { get; set; }
        public int MinIndex { get; set; }
        public int MaxIndex { get; set; }
        public int StartValue { get; set; }
        public int EndValue { get; set; }
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
        public virtual TextItem GetTextOrEmpty(int index, int emptyTextLen = Constants.Numerics.DEFAULT_WIDTH)
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
    }//end TextCanvas

    [Serializable]
    public class Plate
    {
        #region fields
        protected internal List<Block> _blocks;
        protected internal List<Arrow> _arrows;
        private int _width;
        private int _rulerWidth;
        #endregion

        #region properties
        public Rule Ruler { get; set; }
        public List<Block> Blocks
        {
            get { return _blocks; }
            
        }
        public string Name { get; set; }
        public List<string> Notes { get; set; }

        public List<Arrow> Arrows
        {
            get { return _arrows; }
        }
        #endregion

        #region ctors
        public Plate()
        {
            _blocks = new List<Block>();
            _arrows = new List<Arrow>();
            Notes = new List<string>();
        }
        #endregion

        #region Object overrides
        public override string ToString()
        {
            var tc = ToTextCanvas();
            var strBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Name))
            {
                strBuilder.AppendLine(DrawBar());
                strBuilder.AppendLine(DrawHeader());
            }
            foreach (var arrow in _arrows)
            {
                tc = arrow.ComposeArrow(Ruler, tc);
            }
            strBuilder.AppendLine(tc.ToString());

            foreach (var note in Notes)
                strBuilder.AppendLine(note);

            return strBuilder.ToStringUnixNewline();
        }
        #endregion

        #region methods
        public virtual void AddBlock(Block block)
        {
            block.Ruler = Ruler;
            _blocks.Add(block);
        }//end AddBlock
        public virtual void AddArrow(Arrow arrow) { _arrows.Add(arrow); }//end AddArrow

        public virtual void AddArrow(Block fromBlock, Block toBlock, int startVal)
        {
            _arrows.Add(new Arrow(fromBlock, toBlock) {StartValue = startVal});
        }
        public TextCanvas ToTextCanvas()
        {
            var tc = Ruler.ToTextCanvas();
            _rulerWidth = tc.Width;
            tc = _blocks.Aggregate(tc, (current, block) => current.Concat(block.PrintEntriesOnCanvas(), Ruler));

            foreach (var ti in tc.Items)
            {
                ti.Text.Add(Constants.GraphChars.Bar);
                if (ti.Text.Count > _width)
                    _width = ti.Text.Count;
            }
            _width = _width - _rulerWidth;
            return tc;
        }//end ToTextCanvas
        public virtual void ToPdf(string filePath)
        {
            var content = ToString();
            var doc = DrawingDimensions.ToPdfDoc(content);

            var fs = new System.IO.FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using (var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs))
            {
                doc.Open();

                var paragraph = new iTextSharp.text.Paragraph(content,
                    iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.COURIER))
                {
                    ExtraParagraphSpace = 0F,
                    FirstLineIndent = 0F,
                    IndentationLeft = 0F,
                    IndentationRight = 0F,
                    PaddingTop = 0F,
                    SpacingBefore = 0F,
                    SpacingAfter = 0F
                };
                doc.Add(paragraph);

                doc.AddTitle(Name);
                doc.AddSubject("Timelines");
                doc.AddCreator(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                doc.AddCreationDate();
                doc.Close();
            }
        }//end ToPdf
        #endregion

        #region hidden
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal string DrawHeader()
        {
            if (_width - 2 <= 0)
                return Name;

            var title = Etc.PrintInCenter((_width - 2), Name);
            var strBuilder = new StringBuilder();
            if (_rulerWidth > 0)
                strBuilder.Append(new string((char) 0x20, _rulerWidth));
            strBuilder.Append(Constants.GraphChars.Bar);
            strBuilder.Append(title);
            strBuilder.Append(Constants.GraphChars.Bar);
            return strBuilder.ToStringUnixNewline();
        }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal string DrawBar()
        {
            if (_width <= 0)
                return string.Empty;
            var strBuilder = new StringBuilder();
            if (_rulerWidth > 0)
                strBuilder.Append(new string((char)0x20, _rulerWidth));
            strBuilder.Append(Constants.GraphChars.BarRailIntersect);
            strBuilder.Append(new string(Constants.GraphChars.Rail, _width - 2));
            strBuilder.Append(Constants.GraphChars.BarRailIntersect);

            return strBuilder.ToStringUnixNewline();

        }
        #endregion
    }//end Plate

    /// <summary>
    /// This is for for creating fast sequence diagrams where you are not really 
    /// concerned with the values on the ruler - just the ordering of the entries.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// var myFPlate = new FastPlate("Band Practice", "Left side", "Middle side", "Right side");
    /// myFPlate.Blk("Left side").Txt("big meaty claws").Blk("Right side");
    /// myFPlate.Blk("Right side").Txt("d'er not just for attractin' mates").Blk("Left side");
    /// myFPlate.Blk("Left side").Txt("yeah - bring it on").Blk("Right side").Blk(null);
    /// myFPlate.Blk("Middle side").Txt("no people,").Blk("Left side").Blk(null);
    /// myFPlate.Blk("Middle side").Txt("lets bring it off.").Blk("Right side");
    /// System.Diagnostics.Debug.WriteLine(myFPlate.ToString());
    /// ]]>
    /// </example>
    [Serializable]
    public class FastPlate : Plate
    {
        #region fields
        private Block _liveBlock;
        private int _lineCounter = 2;
        private readonly Dictionary<int, string> _blockIdxName = new Dictionary<int, string>();
        #endregion

        #region ctor
        public FastPlate(string plateTitle, params string[] blockNames)
            : this(plateTitle, new Rule { StartValue = 1, EndValue = 16, RuleLineSpacing = 3 }, blockNames)
        {
        }

        public FastPlate(string plateTitle, Rule ruler, params string[] blockNames)
        {
            if (blockNames == null || blockNames.Length <= 0)
                throw new ArgumentNullException("blockNames");

            Name = string.IsNullOrWhiteSpace(plateTitle) ? "Sequence Diagram" : plateTitle;
            Ruler = ruler;
            var c = 0;
            foreach (var blkNm in blockNames)
            {
                var blk = new Block { Ruler = ruler, Title = blkNm };
                _blocks.Add(blk);
                _blockIdxName.Add(c, blkNm);
                c += 1;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// This identifies the current block. When the current block
        /// is being replaced an arrow will be written from the previous
        /// block to this block.
        /// </summary>
        /// <param name="blockName">Pass in null clear the current block.</param>
        /// <returns></returns>
        public virtual FastPlate Blk(string blockName)
        {
            if (string.IsNullOrWhiteSpace(blockName) || _blocks.All(x => x.Title != blockName))
            {
                _liveBlock = null;
                return this;
            }
            if (_liveBlock == null)
            {
                _liveBlock = _blocks.First(x => x.Title == blockName);
                return this;
            }

            var searchBlk = _blocks.First(x => x.Title == blockName);
            if (_liveBlock!= null && !searchBlk.Equals(_liveBlock))
            {
                AddArrow(new Arrow(_liveBlock, searchBlk){StartValue = _lineCounter});
                _lineCounter += 1;
            }

            _liveBlock = searchBlk;
            return this;
        }

        public virtual FastPlate Blk(int idx)
        {
            if (_blockIdxName.ContainsKey(idx)) return Blk(_blockIdxName[idx]);
            _liveBlock = null;
            return this;
        }

        /// <summary>
        /// Prints a single line on to current block.
        /// </summary>
        /// <param name="entryText">
        /// This is broken into many lines whenever the text is too large.</param>
        /// <param name="pl">Lets the caller determine the <see cref="PrintLocation"/> of this line</param>
        /// <returns></returns>
        public virtual FastPlate Txt(string entryText, PrintLocation pl = PrintLocation.Center)
        {
            if (_liveBlock == null)
                return this;
            entryText = entryText ?? string.Empty;

            var entryLine = new List<string>();

            foreach (var word in entryText.Split(' '))
            {
                entryLine.Add(word);
                if (string.Join(" ", entryLine).Length + 4 <= _liveBlock.Width)
                    continue;
                _liveBlock.AddEntry(_lineCounter, string.Join(" ", entryLine),pl);
                _lineCounter += 1;
                entryLine.Clear();
            }

            _liveBlock.AddEntry(_lineCounter, string.Join(" ", entryLine), pl);
            _lineCounter += 1;

            AdjustRuler();

            return this;
        }

        protected internal void AdjustRuler()
        {
            if (_lineCounter + 2 <= Ruler.EndValue) return;

            var nRule = new Rule
            {
                StartValue = Ruler.StartValue,
                EndValue = _lineCounter + 2,
                RuleLineSpacing = Ruler.RuleLineSpacing
            };
            Ruler = nRule;
            foreach (var blk in _blocks)
            {
                blk.Ruler = nRule;
            }            
        }
        #endregion

    }//end FastPlate

    public static class Extensions
    {
        public static string ToStringUnixNewline(this StringBuilder stringBuilder)
        {
            return stringBuilder.ToString()
                .Replace(new string(new[] {(char) 0x0D, (char) 0x0A}), new string(new[] {Constants.GraphChars.UNIX_NL_CHAR}));
        }//end ToStringUnixNewline
        public static TextCanvas ToTextCanvas(this IRuleEntry entry, Rule ruler)
        {
            if(ruler == null)
                throw new NoRuleSetException();

            var entryIndex = ruler.CalcEntryIndex(entry);
            var entryLines = entry.ToString().Split(Constants.GraphChars.UNIX_NL_CHAR);

            var ruleIndex = ruler.GetIndexRule();

            //iteration is on this difference so there needs to be at least that many lines in the string
            if (entryIndex.Item2 - entryIndex.Item1 > entryLines.Length)
            {
                throw new DrawingException(
                    string.Format(
                                  "Entry is indexed at '{0}' to '{1}', a difference of '{2}' while the entry as a string is composed of '{3}' lines",
                        entryIndex.Item1,
                        entryIndex.Item2,
                        (entryIndex.Item2 - entryIndex.Item1),
                        entryLines.Length));
            }

            var textCanvas = new TextCanvas
                             {
                                 Ruler = ruler,
                                 Items = new List<TextItem>(),
                                 MinIndex = entryIndex.Item1,
                                 MaxIndex = entryIndex.Item2,
                                 StartValue = entry.StartValue,
                                 EndValue = entry.EndValue,
                                 Width =  entry.Width,
                                 Id = entry.Id
                             };

            //single line entry
            if (entryIndex.Item2 == entryIndex.Item1)
            {
                if (entryIndex.Item1 < 0 || entryIndex.Item1 >= ruleIndex.Count)
                    throw new DrawingException(
                        string.Format(
                            "The entry by ID '{0}' having the text value of '{1}' " +
                            "is set to a position that is beyond the current ruler's max.",
                            entry.Id, entry));

                textCanvas.Items.Add(new TextItem
                                     {
                                         HashMarkValue = ruleIndex[entryIndex.Item1],
                                         Index = entryIndex.Item1,
                                         Text = entry.ToString().ToCharArray().ToList()
                                     });
                return textCanvas;
            }

            //multiple line entry
            for (var i = entryIndex.Item1; i <= entryIndex.Item2; i++)
            {
                var hashMark = ruleIndex[i];
                var text = entryLines[(i - entryIndex.Item1)];

                if (string.IsNullOrEmpty(text))
                    continue;
                var nti = new TextItem
                          {
                              HashMarkValue = hashMark,
                              Index = i,
                              Text = text.ToCharArray().ToList()
                          };
                nti.Ranges.Add(new TextRange(){Id = entry.Id,Length = nti.Text.Count, StartIndex = 0});
                textCanvas.Items.Add(nti);
            }

            return textCanvas;
        }//end ToTextCanvas
        public static TextCanvas Merge(this TextCanvas entry1Tc, TextCanvas entry2Tc, Rule ruler)
        {
            //check for null args
            if (ruler == null || entry2Tc == null)
                throw new DrawingException("A merge requires a source, a candidate and a ruler");

            //validate args have something to work with
            if(entry1Tc.IsEmptyItems() && entry2Tc.IsEmptyItems())
                return new TextCanvas();

            if (entry2Tc.IsEmptyItems() && !entry1Tc.IsEmptyItems())
                return entry1Tc;

            if (entry1Tc.IsEmptyItems() && !entry2Tc.IsEmptyItems())
                return entry2Tc;
            

            //determine total edges for the whole
            var entriesMin = entry1Tc.MinIndex < entry2Tc.MinIndex ? entry1Tc.MinIndex : entry2Tc.MinIndex;
            var entriesMax = entry1Tc.MaxIndex > entry2Tc.MaxIndex ? entry1Tc.MaxIndex : entry2Tc.MaxIndex;
            
            Func<TextItem, bool> tti = ti1 => ti1 != null && !ti1.IsEmptyText();

            var textCanvas = new TextCanvas
                             {
                                 Ruler = ruler,
                                 Items = new List<TextItem>(),
                                 MaxIndex = entriesMax,
                                 MinIndex = entriesMin,
                                 Width = entry1Tc.Width > entry2Tc.Width ? entry1Tc.Width : entry2Tc.Width
                             };
            for (var i = entriesMin; i <= entriesMax; i++)
            {
                var entry1i = entry1Tc.Items.FirstOrDefault(item => item.Index == i);
                var entry2i = entry2Tc.Items.FirstOrDefault(item => item.Index == i);

                if (entry1i == null && entry2i == null)
                    continue;

                //merge line
                if (tti(entry1i) && tti(entry2i))
                {
                    var newEntry = entry1i.Copy();
                    newEntry.Ranges.AddRange(entry2i.Ranges);
                    newEntry.Text =
                        Etc.MergeString(new string(entry1i.Text.ToArray()), new string(entry2i.Text.ToArray()))
                            .ToCharArray().Where(c => Convert.ToInt32(c) >= 0x20)
                            .ToList();
                    textCanvas.Items.Add(newEntry);

                    continue;
                }
                else if (tti(entry1i))
                {
                    textCanvas.Items.Add(entry1i.Copy());
                    continue;
                }
                textCanvas.Items.Add(entry2i.Copy());

            }
            return textCanvas;
        }//end Merge
        public static TextCanvas Concat(this TextCanvas block1Tc, TextCanvas block2Tc, Rule ruler)
        {
            if(ruler == null)
                throw new NoRuleSetException();
            
            if (block2Tc == null)
            {
                return block1Tc;
            }

            var minIndex = block1Tc.MinIndex < block2Tc.MinIndex ? block1Tc.MinIndex : block2Tc.MinIndex;
            var maxIndex = block1Tc.MaxIndex > block2Tc.MaxIndex ? block1Tc.MaxIndex : block2Tc.MaxIndex;
            var blockBarCount = 0;

            Func<char[], bool> printBar =
                c =>
                    new String(c).EndsWith(string.Format("{0}{1}",
                        Constants.GraphChars.Rail,
                        Constants.GraphChars.BarRailIntersect));

            var tc = new TextCanvas() { MaxIndex = maxIndex, Ruler = ruler, MinIndex = minIndex, Id = block1Tc.Id + Constants.GraphChars.IDSeparator + block2Tc.Id };
            for (var i = minIndex; i <= maxIndex; i++)
            {
                var ti1 = block1Tc.GetTextOrEmpty(i, block1Tc.Width);
                var ti2 = block2Tc.GetTextOrEmpty(i, block2Tc.Width);

                if (ti1.Index < 0 || ti2.Index < 0)
                    continue;

                var ntxt = new List<char>(ti1.Text);
                if (blockBarCount == 1 || block2Tc.MaxIndex >= i)
                    ntxt.Add(Constants.GraphChars.Bar);
                else
                    ntxt.Add((char)0x20);

                if (printBar(ti1.Text.ToArray()))
                    blockBarCount += 1;

                var txtRng = new TextRange() {Id = block2Tc.Id, Length = ti2.Text.Count, StartIndex = ntxt.Count};
                ntxt.AddRange(ti2.Text);

                var nti = new TextItem() {HashMarkValue = ti1.HashMarkValue, Index = i, Text = ntxt};
                nti.Ranges.AddRange(ti1.Ranges);
                nti.Ranges.Add(txtRng);
                tc.Items.Add(nti);

                if (ntxt.Count > tc.Width)
                    tc.Width = ntxt.Count;
            }
            return tc;
        }//end Concat
    }//end Extensions

    [Serializable]
    public class Rule : IRuleEntry
    {
        #region fields
        private double? _countIncrement;
        private int _width;
        private readonly string _id;
        #endregion

        #region ctors
        public Rule()
        {
            _width = Constants.Numerics.RuleWidth;
            _id = "Rule";
        }
        #endregion

        #region properties
        public virtual int RuleLineSpacing { get; set; }
        public virtual double? CountIncrement {
            get
            {
                _countIncrement = Math.Floor((double)(EndValue - StartValue) / 10);
                return _countIncrement;
            }
            set { _countIncrement = value; }
        }
        public virtual int Length
        {
            get { return GetIndexRule().Count; }
        }
        #endregion

        #region IRuleEntry methods
        public virtual int StartValue { get; set; }
        public virtual int EndValue { get; set; }
        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public virtual string Id { get { return _id; } }
        #endregion

        #region Object overrides
        public override string ToString()
        {
            //determine if required vals are assigned
            if (StartValue == EndValue)
                throw new ArgumentException("The start at value must be greater-than or less-than the end at value.");

            //default these if not assigned
            if (RuleLineSpacing == 0)
                RuleLineSpacing = Constants.Numerics.DefaultRuleLnSp;
            double increment = CountIncrement.Value;

            //construct a the ruler as a printable string 
            var strBuilder = new StringBuilder();
            double loopCounter = StartValue;

            Func<double, double, double, bool> loopOp = (a, z, i) => a > z ? i >= z : i <= z;

            while (loopOp(StartValue, EndValue, loopCounter))
            {
                strBuilder.AppendLine(string.Format("{0,-6}", loopCounter));
                for (var i = 0; i < RuleLineSpacing - 1; i++)
                {
                    strBuilder.AppendLine(string.Format("{0,-6}", " "));
                }
                loopCounter += increment;
            }

            return strBuilder.ToStringUnixNewline();
        }
        #endregion

        #region methods
        public virtual Tuple<int, int> CalcEntryIndex(IRuleEntry entry)
        {
            var ruleIndex = GetIndexRule();
            var item1 = -1;
            var item2 = -1;

            Func<double, int, double> rtOp = (d, i) => Math.Abs(d - i);
            var startDif = rtOp(ruleIndex[0], entry.StartValue);
            var endDif = rtOp(ruleIndex[0], entry.EndValue);

            for (var i = 1; i < ruleIndex.Count; i++)
            {
                if (item1 < 0)
                {
                    //there is a shorter distance between this rule index than the previous
                    if (startDif > rtOp(ruleIndex[i], entry.StartValue))
                        startDif = rtOp(ruleIndex[i], entry.StartValue);
                    else
                        item1 = i - 1;
                }

                if (item2 < 0)
                {
                    if (endDif > rtOp(ruleIndex[i], entry.EndValue))
                        endDif = rtOp(ruleIndex[i], entry.EndValue);
                    else
                        item2 = i - 1;
                }

                if (item1 > 0 && item2 > 0)
                    break;
            }
            return new Tuple<int, int>(item1, item2);
        }//end CalcEntryIndex
        public virtual List<double> GetIndexRule()
        {
            //determine edge
            var min = StartValue > EndValue ? EndValue : StartValue;

            var myRuleLines = ToString().Split(Constants.GraphChars.UNIX_NL_CHAR);
            var perLineIncrement = CountIncrement.Value / RuleLineSpacing;

            var indexRule = new List<double>();
            var iVal = (double)min;
            for (var i = 0; i < myRuleLines.Length; i++)
            {
                iVal = !string.IsNullOrWhiteSpace(myRuleLines[i])
                    ? Convert.ToDouble(myRuleLines[i].Trim())
                    : iVal + perLineIncrement;

                indexRule.Add(iVal);
            }

            return indexRule;
        }//end GetIndexRule
        public virtual TextCanvas ToTextCanvas()
        {
            var tc = this.ToTextCanvas(this);
            foreach (var ti in tc.Items)
            {
                ti.Text.Add(Constants.GraphChars.Bar);
            }
            return tc;
        }//end ToTextCanvas

        /// <summary>
        /// Examples 
        ///  ((789,797),(804,816)) as "789-97\804-16"
        ///  ((1797,1803)) as "1797-1803"
        ///  ((640, 609)) as "640-609" 
        ///  ((1588,null)) as "1588-"
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        /// <remarks>
        /// BCE values do NOT have the century removed
        /// </remarks>
        public virtual string PrintYearsRange(IEnumerable<Tuple<int?, int?>> years)
        {
            Func<Tuple<int?, int?>, string> yearFmt = tuple => string.Format("{0}-{1}", tuple.Item1, tuple.Item2);

            Func<int?, int, int?> remCentury;
            //CE
            if (StartValue < EndValue)
            {
                remCentury = (y, c) => y - c < 100 ? y - c : y;
            }
            else//BCE
            {
                remCentury = (y, c) => y;
            }

            years = years ?? new List<Tuple<int?, int?>>();

            //for tuple item 1 in the same century as item 2, we only want last two digits
            var yyyy = years.ToList();

            var pCentury = 0;
            var yy = new List<Tuple<int?, int?>>();
            for (var i = 0; i < yyyy.Count; i++)
            {
                var ccyy = yyyy[i].Item1;
                var century = ccyy.GetValueOrDefault(0) - ccyy.GetValueOrDefault(0) % 100;

                var yyyy1 = yyyy[i].Item1 == null ? null : remCentury(yyyy[i].Item1, century);
                var yyyy2 = yyyy[i].Item2 == null ? null : remCentury(yyyy[i].Item2, century);

                yy.Add(new Tuple<int?, int?>(i == 0 || pCentury != century ? yyyy[i].Item1 : yyyy1  , yyyy2));

                pCentury = century;
            }

            return string.Join("\\", yy.Select(x => yearFmt(x)));
        }//end PrintYearsRange

        /// <summary>
        /// The counterpart to <see cref="PrintYearsRange"/>
        /// </summary>
        /// <param name="printyears"></param>
        /// <returns></returns>
        public virtual IEnumerable<Tuple<int?, int?>> ParseYearsRange(string printyears)
        {
            if (string.IsNullOrWhiteSpace(printyears) ||
                !System.Text.RegularExpressions.Regex.IsMatch(printyears, "[0-9\x5C\x2d]+?"))
                return new List<Tuple<int?, int?>>();

            var century = StartValue - StartValue % 100;

            var lowerBound = StartValue < EndValue ? StartValue : EndValue;
            var upperBound = StartValue < EndValue ? EndValue : StartValue;

            //century is removed from most of the string values for CE years
            Func<int, bool> inBounds = v => (lowerBound <= v) && (v <= upperBound);

            var aYears = new List<Tuple<int?, int?>>();
            //now to transform back to tuples
            foreach (var t in printyears.Split('\\'))//slash separates a tuple, 
            {
                var u = new[] { string.Empty, string.Empty };
                var tt =
                    t.Split('-')//dash is each item therein
                        .Union(u.Select(y => y)).ToArray();
                int y1;
                int y2;
                int? yy1 = null;
                int? yy2 = null;
                if (int.TryParse(tt[0], out y1))
                {
                    if (!inBounds(y1))
                        y1 += century;
                    yy1 = y1;
                    century = y1 - y1%100;
                }
                if (int.TryParse(tt[1], out y2))
                {
                    if (!inBounds(y2))
                        y2 += century;
                    yy2 = y2;
                }

                aYears.Add(new Tuple<int?, int?>(yy1, yy2));
            }
            return aYears;
        }//end ParseYearsRange
        #endregion
    }//end Rule

    [Serializable]
    public class Block : IDrawing, IRuleEntry
    {
        #region fields
        protected internal List<Entry> _entries;
        protected internal List<Block> _innerBlocks;
        protected internal List<Arrow> _arrows;
        protected internal int _width;
        protected internal int _startValue;
        protected internal bool _startValueExplicitlySet;
        protected internal int _endValue;
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
            _width = Constants.Numerics.DefaultWidth;
            _arrows = new List<Arrow>();
        }
        #endregion

        #region IRuleEntry methods
        public virtual int StartValue {
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
        public virtual int EndValue {
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
        public virtual string Id { get { return Title; } }
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
        public virtual void AddEntry(int startValue, string text)
        {
            AddEntry(new Entry { StartValue = startValue, Text = text });
        }
        public virtual void AddEntry(int startValue, string text, PrintLocation location)
        {
            AddEntry(new Entry {StartValue = startValue,Text = text, Location = location});
        }
        public virtual void AddEntry(int startValue, int endValue, string text)
        {
            AddEntry(new Entry { StartValue = startValue, EndValue = endValue, Text = text });
        }
        public virtual void AddEntry(int startValue, int endValue, string text, PrintLocation location)
        {
            AddEntry(new Entry { StartValue = startValue, EndValue = endValue, Text = text, Location = location });
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
            strBuilder.Append(Constants.GraphChars.BarRailIntersect);
            for (var i = 1; i < Width - 1; i++)
            {
                strBuilder.Append(Constants.GraphChars.Rail);
            }
            strBuilder.Append(Constants.GraphChars.BarRailIntersect);
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
            var distinctStartVals = new List<int>();
            foreach (var entry in _entries.Where(entry => !distinctStartVals.Contains(entry.StartValue)))
                distinctStartVals.Add(entry.StartValue);

            //find contended start values
            foreach (var dsv in distinctStartVals.Where(dsv => _entries.Count(e => e.StartValue == dsv) > 1))
            {
                var reqW = 0;
                var contendedEntries = _entries.Where(e => e.StartValue == dsv).ToList();
                contendedEntries.ForEach(ce => reqW += ce.Text.Length);
                if (maxWidthReq < reqW + Constants.Numerics.MinPaddingBetweenContentions)
                    maxWidthReq = reqW + Constants.Numerics.MinPaddingBetweenContentions;
            }

            var maxInTextLen = _entries.Max(x => x.Text.Length);
            if (maxInTextLen > maxWidthReq)
                maxWidthReq = maxInTextLen;

            return maxWidthReq;
        }//end GetRequiredWidth
        #endregion
    }//end Block

    [Serializable]
    public class Entry : IDrawing, IRuleEntry 
    {
        #region fields
        protected internal int _height;
        protected internal int _width;
        protected internal int _endValue;
        protected internal bool _endValueSetExplicitly;
        private readonly string _id;
        #endregion

        #region properties
        public virtual PrintLocation Location { get; set; }
        public virtual Rule Ruler { get; set; }
        public virtual string Text { get; set; }
        #endregion

        #region ctors
        public Entry()
        {
            _width = Constants.Numerics.DefaultWidth;
            _id = Guid.NewGuid().ToString();
        }
        #endregion

        #region Object overrides
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Text))
                return string.Empty;
            if (Width == 0)
                _width = Constants.Numerics.DefaultWidth;
            if (_height == 0)
                _height = CalcHeight();

            Func<int, string> sfOpL = i => "{0,-" + i + "}";
            Func<int, string> sfOpR = i => "{0," + i + "}";

            var strBuilder = new StringBuilder();
            switch (Location)
            {
                case PrintLocation.Center:
                    strBuilder.AppendLine(Util.Etc.PrintInCenter(Width, Text));
                    break;
                case PrintLocation.Left:
                    strBuilder.AppendLine(string.Format(sfOpL(Width), Text));
                    break;
                case PrintLocation.Right:
                    strBuilder.AppendLine(string.Format(sfOpR(Width), Text.Trim()));
                    break;
            }

            if (_height <= 1)
                return strBuilder.ToStringUnixNewline();

            for (var i = 0; i < _height - 1; i++)
            {
                switch (Location)
                {
                    case PrintLocation.Center:
                        strBuilder.AppendLine(Util.Etc.PrintInCenter(Width, Constants.GraphChars.LifeLine.ToString()));
                        break;
                    case PrintLocation.Left:
                        strBuilder.AppendLine(string.Format(sfOpL(Width), Constants.GraphChars.LifeLine));
                        break;
                    case PrintLocation.Right:
                        strBuilder.AppendLine(string.Format(sfOpR(Width), Constants.GraphChars.LifeLine));
                        break;
                }
            }
            switch (Location)
            {
                case PrintLocation.Center:
                    strBuilder.AppendLine(Util.Etc.PrintInCenter(Width, new string(Constants.GraphChars.Rail,3)));
                    break;
                case PrintLocation.Left:
                    strBuilder.AppendLine(string.Format(sfOpL(Width), new string(Constants.GraphChars.Rail, 3)));
                    break;
                case PrintLocation.Right:
                    strBuilder.AppendLine(string.Format(sfOpR(Width), new string(Constants.GraphChars.Rail, 3)));
                    break;
            }
            return strBuilder.ToStringUnixNewline();
        }
        #endregion

        #region IRuleEntry methods
        public virtual int StartValue { get; set; }
        public virtual int EndValue 
        {
            get
            {
                if (!_endValueSetExplicitly)
                {
                    _endValue = StartValue;
                }
                return _endValue;
            }
            set
            {
                _endValueSetExplicitly = true;
                _endValue = value;
            }
        }
        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public virtual string Id { get { return _id; } }
        #endregion

        #region hidden
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal int CalcHeight()
        {
            if (StartValue == EndValue)
                return 1;
            if (Ruler == null)
                return 1;
            var idx = Ruler.CalcEntryIndex(this);

            var y = 0;
            var x = Ruler.Length;

            if (idx.Item1 >= 0)
                x = idx.Item1;
            if (idx.Item2 > 0)
                y = idx.Item2;

            return y - x;
        }//end CalcHeight
        #endregion
    }//end Entry

    [Serializable]
    public class TerritoryEntry : Entry
    {
        private const string REGEX_PATTERN = @"\+([^\(]*?)\(([0-9]*)\)";

        public string Name { get; set; }
        public int? Year { get; set; }

        public override string Text
        {
            get
            {
                return string.Format("+{0}({1})", Name, Year);
            }
            set
            {
                var tValue = value;
                string tName;
                string tYear;
                int iYear;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out tName, 1))
                {
                    Name = tName;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out tYear, 2) && int.TryParse(tYear, out iYear))
                {
                    Year = iYear;
                }
            }
        }
    }//end TerritoryEntry

    [Serializable]
    public class LeaderEntry : Entry
    {
        private const string REGEX_PATTERN = @"\[([a-zA-Z\.\x20]+?)\x20([0-9\x5C\x2d]+?)]";

        public string Name { get; set; }
        public IEnumerable<Tuple<int?, int?>> Years { get; set; }

        public override string Text
        {
            get
            {
                return string.Format("[{0} {1}]", Name, Ruler.PrintYearsRange(Years));
            }
            set
            {
                string s1;
                var tValue = value;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    Name = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    Years = Ruler.ParseYearsRange(s1);
                }
            }
        }
    }//end LeaderEntry

    [Serializable]
    public class ScienceAdvEntry : Entry
    {
        private const string REGEX_PATTERN = @"([a-zA-Z\.\x20]+?)\[([a-zA-Z\.\x20\x2C]+?)\]\(([0-9]+?)\)";

        public string DiscoveredBy { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }

        public override string Text
        {
            get
            {
                return string.Format("{0}[{1}]({2})", DiscoveredBy, Name, Year);
            }
            set
            {
                var tValue = value;
                string s1;
                int y1;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    DiscoveredBy = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    Name = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 3) && int.TryParse(s1, out y1))
                {
                    Year = y1;
                }
            }
        }
    }//end ScienceAdvEntry

    [Serializable]
    public class LiteraryWorkEntry : Entry
    {
        private const string REGEX_PATTERN = @"\'([a-zA-Z\.\x20]+?)\'([a-zA-Z\.\x20]+?)\(([0-9]+?)\)";

        public string Title { get; set; }
        public string Author { get; set; }
        public int? Year { get; set; }

        public override string Text
        {
            get
            {
                return string.Format("'{0}'{1}({2})", Title, Author, Year);
            }
            set
            {
                var tValue = value;
                string s1;
                int y1;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    Title = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    Author = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 3) && int.TryParse(s1, out y1))
                {
                    Year = y1;
                }
            }
        }
    }//end LiteraryWorkEntry

    [Serializable]
    public class Arrow : IRuleEntry 
    {
        #region fields
        protected Block _fromBlock;
        protected Block _toBlock;
        protected int _singleValue;
        private readonly string _id;
        private string _fromLeftToRightArrowHead = Constants.GraphStrings.ArrowHeadLeft;
        private string _arrowTail = Constants.GraphStrings.ArrowTailShaft;
        private string _fromRightToLeftArrowHead = Constants.GraphStrings.ArrowHeadRight;
        private char _arrowShaft = Constants.GraphChars.DirectShaft;

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
            _id = string.Format("From={0},To={1}", fromBlock.Id, toBlock.Id);
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
        public virtual int StartValue
        {
            get { return _singleValue; }
            set { _singleValue = value; }
        }
        public virtual int EndValue
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
    }//end Arrow

    [Serializable]
    public enum PrintLocation
    {
        Center,
        Left,
        Right

    }//end PrintLocation

    public class DrawingException : Exception
    {
        public DrawingException(string msg) : base(msg) { }
    }//end TimelineDrawingException

    public class NoRuleSetException : DrawingException
    {
        public NoRuleSetException() : base("The Rule object is required.") { }
    }//end TimelineNoRuleSetException

    public class InnerBlockRequiresStartValue : DrawingException
    {
        public InnerBlockRequiresStartValue(Block block) : base(string.Format("Block titled '{0}' must have a start value different from the start value of the Rule.", block.Title)) { }
    }//InnerBlockRequiresStartValue

    public class TooWideForPdf : DrawingException
    {
        public TooWideForPdf() : base("the content is too large for even the A0 page size") { }
    }

}//end NoFuture.Timeline
