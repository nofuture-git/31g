using System;
using System.Linq;
using iTextSharp.text;

namespace NoFuture.Timeline
{
    public abstract class DrawingDimensions
    {
        public static iTextSharp.text.Document ToPdfDoc(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException("content");

            var lines = content.Split(Constants.GraphChars.UNIX_NL_CHAR);
            var line = lines.FirstOrDefault(x => x.Length > 0);
            if (line == null)
                throw new DrawingException("a page size cannot be determined from the given content.");
            var charWidth = line.Length;
            var charHeight = lines.Length;

            var inchesWidth = ((float) charWidth/CHARS_PER_INCH_WIDTH_MONOSPACE) + 5;
            var inchesHeight = ((float)charHeight / LINES_PER_INCH_HEIGHT_MONOSPACE) + 5;

            var doc = new Document(new Rectangle((inchesWidth*72), inchesHeight*72), PRINT_MARGIN, PRINT_MARGIN,
                PRINT_MARGIN, PRINT_MARGIN);

            return doc;
        }

        public const float PRINT_MARGIN = 16.0F;
        public const int CHARS_PER_INCH_WIDTH_MONOSPACE = 11;
        public const int LINES_PER_INCH_HEIGHT_MONOSPACE = 5;
    }
}
