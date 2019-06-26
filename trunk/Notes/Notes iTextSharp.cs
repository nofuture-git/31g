using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Notes
{
    public class MyITextSharpNotes
    {
        public static void CreatePdf()
        {
            var document = new Document(PageSize.LETTER.Rotate());
            PdfWriter.GetInstance(document, new FileStream(@"C:\Projects\31g\trunk\temp\pdf\myPdf.pdf", FileMode.OpenOrCreate));
            document.Open();
            document.Add(new Paragraph("Hello World"));
            document.Close();
        }
        
        public static void CreatePdfTable()
        {
            //first column is 3 times longer than the others
            var widths = new[] { 3f, 1f, 1f, 1f, 1f, 1f };
            var table = new PdfPTable(widths);
            table.DefaultCell.Border = PdfPCell.NO_BORDER;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;

            var hdr = new PdfPHeaderCell();
            hdr.Colspan = widths.Length;
            hdr.AddElement(new Paragraph("SUMMARY REPORT"));
            table.AddCell(hdr);
            
            table.AddCell("");
            
            var cell = new PdfPCell();
            cell.Border = PdfPCell.NO_BORDER;
            
            var p1 = new Paragraph("Mar");
            p1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(p1);

            var p2 = new Paragraph("2018");
            p1.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(p2);
            
            table.AddCell(cell);
            
            table.AddCell("Apr 2017");
            table.AddCell("May 2016");
            table.AddCell("Mar 2015");
            table.AddCell("Jul 2014");
            
            table.AddCell("Some row name");
            table.AddCell("1.1");
            table.AddCell("1.2");
            table.AddCell("1.3");
            table.AddCell("1.4");
            table.AddCell("1.5");
            
            var document = new Document(PageSize.LETTER);
            var pdfWriter = PdfWriter.GetInstance(document, new FileStream(@"C:\Projects\31g\trunk\temp\pdf\myPdfTable.pdf", FileMode.OpenOrCreate));
            document.Open();
            document.Add(table);
            document.Close();
            pdfWriter.Close();
        }
        
        public static void CropPdf()
        {
            var xll = 200;
            var yll = 170;
            var w = 800;
            var h = 800;
            var reader = new iTextSharp.text.pdf.PdfReader(@"C:\Projects\31g\trunk\temp\pdf\20140208110036_20.pdf");
            var n = reader.NumberOfPages;
            iTextSharp.text.pdf.PdfDictionary pageDict;
            
            var pfgRect = new iTextSharp.text.pdf.PdfRectangle(xll, yll, w, h);
            for (var i = 1; i <= n; i++)
            {
                pageDict = reader.GetPageN(i);
                pageDict.Put(iTextSharp.text.pdf.PdfName.CROPBOX, pfgRect);
            }

            var stamper = new iTextSharp.text.pdf.PdfStamper(reader,
                new System.IO.FileStream(string.Format(@"C:\Projects\31g\trunk\Notes\misc\Maps\Europe_565BCE.pdf", xll, yll, w, h), FileMode.Create));
            stamper.Close();
            reader.Close();


        }

        public static void BreakApart()
        {
            var reader = new iTextSharp.text.pdf.PdfReader(@"C:\Projects\31g\trunk\temp\20140208110036.pdf");

            for (var i = 1; i < reader.NumberOfPages; i++)
            {
                var document = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(i));
                
                var pdfCopyProvider = new iTextSharp.text.pdf.PdfCopy(document,
                    new System.IO.FileStream(string.Format(@"C:\Projects\31g\trunk\temp\pdf\20140208110036_{0}.pdf", i), System.IO.FileMode.Create));
                document.Open();
                var importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
                document.Close();
            }

            reader.Close();
        }

        public static void RotatePdf()
        {
            var reader = new iTextSharp.text.pdf.PdfReader(@"C:\Projects\31g\trunk\temp\pdf\Europe_565BCE.pdf");
            var pageDict = reader.GetPageN(1);
            pageDict.Put(iTextSharp.text.pdf.PdfName.ROTATE, new iTextSharp.text.pdf.PdfNumber(270));

            var stamper = new iTextSharp.text.pdf.PdfStamper(reader,
                new System.IO.FileStream(@"C:\Projects\31g\trunk\Notes\misc\Maps\Europe_565BCE.pdf", FileMode.Create));
            stamper.Close();
            reader.Close();

        }

        public static void MergePdf()
        {
            using (var fs = new FileStream(@"C:\Projects\31g\trunk\Notes\misc\OccidentalMaps.pdf", FileMode.Create))
            {
                var doc = new iTextSharp.text.Document();
                var pdf = new iTextSharp.text.pdf.PdfCopy(doc, fs);
                doc.Open();

                foreach (var file in Directory.GetFiles(@"C:\Projects\31g\trunk\Notes\misc\WestMaps"))
                {
                    pdf.AddDocument(new iTextSharp.text.pdf.PdfReader(file));
                }

                doc.Close();
            }
            
        }
    }
}