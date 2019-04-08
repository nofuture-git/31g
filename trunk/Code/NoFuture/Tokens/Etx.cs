using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Newtonsoft.Json;
using NoFuture.Antlr.HTML;
using Path = System.IO.Path;

namespace NoFuture.Tokens
{
    /// <summary>
    /// General utility class to concentrate various parsers into one place
    /// </summary>
    public static class Etx
    {
        /// <summary>
        /// Helper method to quick get the text out of a PDF file.
        /// </summary>
        /// <param name="pdfBuffer"></param>
        /// <returns></returns>
        public static string GetPdfText(byte[] pdfBuffer)
        {
            var pdfText = new StringBuilder();
            if (pdfBuffer == null || pdfBuffer.Length <= 0)
                return null;
            using (var reader = new PdfReader(pdfBuffer))
            {
                var numOfPages = reader.NumberOfPages;
                for (var i = 1; i <= numOfPages; i++)
                {
                    var pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                    if (String.IsNullOrWhiteSpace(pageText))
                        continue;
                    pdfText.Append(pageText);
                }
            }

            return pdfText.ToString();
        }

        /// <summary>
        /// Helper method to quick get the text out of a PDF file.
        /// </summary>
        public static void GetPdfText(string pdfFileFullName, int startPage = 1, int endPage = int.MaxValue, string outputDirectory = null)
        {
            var newFolderName = CheckPaths(pdfFileFullName, outputDirectory);
            if (string.IsNullOrWhiteSpace(newFolderName) || !Directory.Exists(newFolderName))
                return;
            using (var pdf = new PdfReader(pdfFileFullName))
            {
                for (var pageNumber = startPage; pageNumber <= pdf.NumberOfPages; pageNumber++)
                {
                    if (pageNumber >= endPage)
                        break;
                    var pageText = PdfTextExtractor.GetTextFromPage(pdf, pageNumber);
                    var textPage = Path.Combine(newFolderName, $"Page-{pageNumber:000}.txt");
                    File.WriteAllText(textPage, pageText);
                }
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/465172/merging-two-images-in-c-net
        /// </summary>
        public static Bitmap StackBitmaps(params string[] files)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    var bitmap = new Bitmap(image);

                    //update the size of the final bitmap
                    width = bitmap.Width > width ? bitmap.Width : width;
                    height += bitmap.Height+4;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width + 8, height + 8);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.White);

                    //go through each image and draw it on the final image
                    int offset = 4;
                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image,
                            new Rectangle(4, offset, image.Width, image.Height));
                        offset += image.Height+4;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        public static Bitmap PadBitmap(string imageFile, int padLen = 4)
        {
            //read all images into memory
            Bitmap finalImage = null;
            padLen = padLen <= 4 ? 4 : padLen;
            try
            {
                using (var bitmap = new Bitmap(imageFile))
                {
                    //create a bitmap to hold the combined image
                    finalImage = new Bitmap(bitmap.Width + 2 * padLen, bitmap.Height + 2 * padLen);

                    //get a graphics object from the image so we can draw on it
                    using (Graphics g = Graphics.FromImage(finalImage))
                    {
                        //set background color
                        g.Clear(Color.White);

                        g.DrawImage(bitmap,
                            new Rectangle(padLen, padLen, bitmap.Width, bitmap.Height));
                    }
                    bitmap.Dispose();
                    return finalImage;
                }

            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
        }

        /// <summary>
        /// Helper method to quickly extract all images out of a PDF file
        /// </summary>
        public static void GetPdfImages(string pdfFileFullName, string outputDirectory = null)
        {
            var newFolderName = CheckPaths(pdfFileFullName, outputDirectory);
            if (string.IsNullOrWhiteSpace(newFolderName) || !Directory.Exists(newFolderName))
                return;
            var images = new List<Image>();
            using (var pdf = new PdfReader(pdfFileFullName))
            {
                for (var pageNumber = 1; pageNumber <= pdf.NumberOfPages; pageNumber++)
                {
                    var pg = pdf.GetPageN(pageNumber);
                    images.AddRange(GetImagesFromPdfDict(pg, pdf));
                }
            }

            for (var i = 0; i < images.Count; i++)
            {
                var img = images[i];
                var imgPath = Path.Combine(newFolderName, $"Image-{i:0000}.bmp");
                img.Save(imgPath);
            }
        }

        internal static string CheckPaths(string pdfFileFullName, string outputDirectory = null)
        {
            if (String.IsNullOrEmpty(pdfFileFullName) || !File.Exists(pdfFileFullName))
                return null;
            outputDirectory =
                outputDirectory ?? Path.GetDirectoryName(pdfFileFullName);
            if (!Directory.Exists(outputDirectory))
                return null;

            var newFolderName = Path.GetFileNameWithoutExtension(pdfFileFullName);
            newFolderName = Path.Combine(outputDirectory, newFolderName);
            if (!Directory.Exists(newFolderName))
                Directory.CreateDirectory(newFolderName);

            return newFolderName;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/802269/extract-images-using-itextsharp
        /// </summary>
        internal static IList<Image> GetImagesFromPdfDict(PdfDictionary dict, PdfReader doc)
        {
            var images = new List<Image>();
            var res = (PdfDictionary)(PdfReader.GetPdfObject(dict.Get(PdfName.RESOURCES)));
            var xobj = (PdfDictionary)(PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT)));

            if (xobj == null)
                return images;
            foreach (var name in xobj.Keys)
            {
                var obj = xobj.Get(name);
                if (!obj.IsIndirect())
                    continue;
                var tg = (PdfDictionary)(PdfReader.GetPdfObject(obj));
                var subtype = (PdfName)(PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE)));
                if (PdfName.IMAGE.Equals(subtype))
                {
                    var xrefIdx = ((PRIndirectReference)obj).Number;
                    var pdfObj = doc.GetPdfObject(xrefIdx);
                    var str = (PdfStream)(pdfObj);

                    var pdfImage =
                        new PdfImageObject((PRStream)str);
                    var img = pdfImage.GetDrawingImage();

                    images.Add(img);
                }
                else if (PdfName.FORM.Equals(subtype) || PdfName.GROUP.Equals(subtype))
                {
                    images.AddRange(GetImagesFromPdfDict(tg, doc));
                }
            }

            return images;
        }

        /// <summary>
        /// Composition of the helper methods herein, extracts the text from <see cref="fi"/>
        /// and saves it to a file along side the original
        /// </summary>
        /// <param name="fi"></param>
        /// <returns>
        /// The path the the text file.
        /// </returns>
        public static string ToTextOnly(FileInfo fi)
        {
            var fullFileName = fi.FullName;
            if (String.IsNullOrWhiteSpace(fullFileName) || !File.Exists(fullFileName))
                return fullFileName;

            var fileExtentsion = fi.Extension;
            var dirOut = fi.DirectoryName ??
                         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fileOut = Path.GetFileNameWithoutExtension(fullFileName);
            var fullFileOut = Path.Combine(dirOut, $"{fileOut}.txt");

            switch (fileExtentsion)
            {
                case ".txt":
                    return fi.FullName;
                case ".pdf":
                    var bytes = File.ReadAllBytes(fullFileName);
                    var pdfTxt = GetPdfText(bytes);
                    File.WriteAllText(fullFileOut, pdfTxt);
                    return fullFileOut;
                case ".html":
                case ".htm":
                    var htmlDoc = GetHtmlDocument(File.ReadAllText(fullFileName));
                    var htmlTxt = htmlDoc?.DocumentNode.InnerText ?? "";
                    File.WriteAllText(fullFileOut, htmlTxt);
                    return fullFileOut;
                case ".xml":
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(fi.FullName);
                    var xmlTxt = xmlDoc.InnerText;
                    File.WriteAllText(fullFileOut, xmlTxt);
                    return fullFileOut;
            }

            return String.Empty;
        }

        /// <summary>
        /// Helper method to get <see cref="rawHtml"/> as a <see cref="HtmlDocument"/>
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static HtmlDocument GetHtmlDocument(string rawHtml)
        {
            if (String.IsNullOrWhiteSpace(rawHtml))
                return null;

            var html = new HtmlDocument
            {
                OptionFixNestedTags = true,
                OptionOutputAsXml = true
            };

            html.LoadHtml(rawHtml);
            return html;
        }

        /// <summary>
        /// Strips the content of <see cref="rawHtml"/> down to 
        /// just its html with no scrips, css styles, doc-types, etc.
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static string GetHtmlOnly(string rawHtml)
        {
            if (String.IsNullOrWhiteSpace(rawHtml))
                return null;

            var antlrRslts = AspNetParseTree.InvokeParse(new MemoryStream(Encoding.UTF8.GetBytes(rawHtml)));
            return antlrRslts?.HtmlOnly;
        }

        /// <summary>
        /// Helper method to take a raw html string, strip it 
        /// down and convert this striped html into xml.
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static XmlDocument GetHtmlAsXml(string rawHtml)
        {
            if (String.IsNullOrWhiteSpace(rawHtml))
                return null;

            var htmlOnly = GetHtmlOnly(rawHtml);
            if (String.IsNullOrWhiteSpace(htmlOnly))
                return null;
            var xml = new XmlDocument();
            xml.LoadXml(htmlOnly);
            return xml;
        }

        /// <summary>
        /// Helper method, chains together <see cref="GetHtmlAsXml"/>
        /// then converts the xml to json then the json to a dynamic
        /// </summary>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        public static dynamic GetHtmlAsJson(string rawHtml)
        {
            if (String.IsNullOrWhiteSpace(rawHtml))
                return null;
            var xml = GetHtmlAsXml(rawHtml);
            if (xml == null)
                return null;
            var json = JsonConvert.SerializeXmlNode(xml);

            return JsonConvert.DeserializeObject<dynamic>(json);
        }

        /// <summary>
        /// Gets only the body of all script tags from the web response content
        /// </summary>
        /// <param name="webResponseText"></param>
        /// <param name="filter"></param>
        /// <param name="scriptBodies"></param>
        /// <returns></returns>
        public static bool TryGetScriptBodies(string webResponseText, Func<string, bool> filter, out string[] scriptBodies)
        {
            scriptBodies = null;
            if (String.IsNullOrWhiteSpace(webResponseText))
                return false;

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(webResponseText));
            var antlrHtml = AspNetParseTree.InvokeParse(ms);
            if (antlrHtml == null)
                return false;

            var innerText = antlrHtml.CharData;

            if (innerText.Count <= 0)
                return false;

            scriptBodies = antlrHtml.ScriptBodies.ToArray();

            if (filter != null)
            {
                scriptBodies = scriptBodies.Where(filter).ToArray();
            }
            return scriptBodies.Length > 0;
        }
    }
}
