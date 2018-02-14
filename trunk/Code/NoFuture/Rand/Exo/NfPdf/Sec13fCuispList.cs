using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Exo.NfPdf
{
    /// <summary>
    /// Parses quarterly PDF reports from the SEC listing all 
    /// Commitee on Uniform Securities Identification Procedures currently defined.
    /// </summary>
    [Serializable]
    public class Sec13FCuispList : NfDynDataBase
    {
        #region contants
        public const string CUSIP_LN_MATCH = @"[A-Z0-9]{6}\x20[A-Z0-9]{2}\x20";
        #endregion

        public Sec13FCuispList(Uri src) : base(src) { }

        public static Uri GetUri(DateTime? dt)
        {
            var currentDate = dt ?? DateTime.Today;

            var yyyy = currentDate.Year;
            var ddyy = currentDate.DayOfYear;

            if(ddyy < 105)
                return new Uri($"https://www.sec.gov/divisions/investment/13f/13flist{yyyy-1}q4.pdf");
            if(ddyy < 195)
                return new Uri($"https://www.sec.gov/divisions/investment/13f/13flist{yyyy}q1.pdf");
            if(ddyy < 285)
                return new Uri($"https://www.sec.gov/divisions/investment/13f/13flist{yyyy}q2.pdf");

            return new Uri($"https://www.sec.gov/divisions/investment/13f/13flist{yyyy}q3.pdf");
        }

        #region methods
        /// <summary>
        /// Parse pdf content from SEC to get CUSIP's
        /// [https://www.sec.gov/divisions/investment/13flists.htm]
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <remarks>
        /// Code sample from [http://stackoverflow.com/questions/12953965/how-to-convert-pdf-to-text-file-in-itextsharp]
        /// dynamic type names taken from [https://www.cusip.com/cusip/about-cgs-identifiers.htm]
        /// </remarks>
        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var pdfBuffer = content as byte[];

            if (pdfBuffer == null || pdfBuffer.Length <= 0)
                return null;
            var dataBuffer = new List<dynamic>();
            try
            {
                using (var reader = new PdfReader(pdfBuffer))
                {
                    var numOfPages = reader.NumberOfPages;
                    for (var i = 1; i <= numOfPages; i++)
                    {
                        var pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                        if (string.IsNullOrWhiteSpace(pageText))
                            continue;
                        var pageLines = pageText.Split(Constants.LF);
                        foreach (var line in pageLines)
                        {
                            string[] lineData;
                            if (!TryParseCusipLine(line.Trim(), out lineData))
                                continue;

                            dataBuffer.Add(
                                new
                                {
                                    Issuer = lineData[0],
                                    Issue = lineData[1],
                                    ChkDigit = lineData[2],
                                    Name = lineData[3]
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
            return dataBuffer;
        }

        protected internal bool IsCusipIdLine(string line)
        {
            return !string.IsNullOrWhiteSpace(line) && Regex.IsMatch(line, CUSIP_LN_MATCH);
        }

        protected internal bool TryParseCusipLine(string line, out string[] vals)
        {
            vals = null;
            if (!IsCusipIdLine(line))
                return false;

            var parseOnSpace = line.Split(' ');
            if (parseOnSpace.Length < 5)
                return false;

            var listOut = new List<string>
            {
                parseOnSpace[0],
                parseOnSpace[1],
                parseOnSpace[2],
                string.Join(" ", parseOnSpace.Skip(3).Take(parseOnSpace.Length - 3))
            };

            vals = listOut.ToArray();
            return vals.Length == 4;
        }
        #endregion
    }
}
