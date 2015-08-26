using System.IO;

namespace Notes
{
    public class MyITextSharpNotes
    {
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