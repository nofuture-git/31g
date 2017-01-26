using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Util;

namespace NoFuture.Timeline
{
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
        public List<Block> Blocks => _blocks;
        public string Name { get; set; }
        public List<string> Notes { get; set; }
        public string FileName { get; set; }

        public List<Arrow> Arrows => _arrows;

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
            tc = _blocks.Aggregate(tc, (current, block) => TimelineExtensions.Concat(current, block.PrintEntriesOnCanvas(), Ruler));

            foreach (var ti in tc.Items)
            {
                ti.Text.Add(Config.GraphChars.Bar);
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
            var printSv = Ruler?.StartValue ?? 0;
            var printEv = Ruler?.EndValue ?? 0;
            var printEpc = Ruler?.GetEpochName() ?? string.Empty;
            var printPlateName = Name ?? string.Empty;
            var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
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
                doc.AddAuthor(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                doc.AddSubject($"{nameof(Occidental)} timeline plate for '{Name}' from {printSv} to {printEv} {printEpc}");
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
            strBuilder.Append(Config.GraphChars.Bar);
            strBuilder.Append(title);
            strBuilder.Append(Config.GraphChars.Bar);
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
            strBuilder.Append(Config.GraphChars.BarRailIntersect);
            strBuilder.Append(new string(Config.GraphChars.Rail, _width - 2));
            strBuilder.Append(Config.GraphChars.BarRailIntersect);

            return strBuilder.ToStringUnixNewline();

        }
        #endregion
    }
}