using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NoFuture.Rand.Gov.US.Sec
{
    /// <summary>
    /// The annual report public corporations file with the SEC - it is 
    /// equivalent, but the same as, an Annual Report
    /// </summary>
    [Serializable]
    public class Form10K : SecForm
    {
        public const string ABBREV = "10-K";
        public Form10K() : base(ABBREV) { }
        public override string Value { get; set; }

        public int FiscalYear { get; set; }
        public string Summary { get; set; }

        public decimal Revenue { get; set; }
        public decimal OperatingIncome { get; set; }
        public decimal NetIncome { get; set; }

        public decimal DomesticAssets { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal TotalLiabilities { get; set; }

        public int NumOfShares { get; set; }

        /// <summary>
        /// Returns the link to the SEC's interactive version of the report.
        /// </summary>
        public Uri InteractiveFormLink
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CIK?.Value) && !string.IsNullOrWhiteSpace(AccessionNumber) && XmlLink != null)
                    return CtorInteractiveLink(CIK.Value, AccessionNumber);

                return null;
            }
        }

        public static Uri CtorInteractiveLink(string cik, string accessionNum)
        {
            var qry = new StringBuilder();
            qry.Append("?action=view&");
            qry.AppendFormat("cik={0}&", cik);
            qry.AppendFormat("accession_number={0}&", accessionNum);
            qry.Append("xbrl_type=v");

            return new Uri("https://www.sec.gov/cgi-bin/viewer" + qry);
        }

        /// <summary>
        /// Attempts to find &apos;DESCRIPTION OF BUSINESS&apos; from the 
        /// text blocks.
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionOfBiz()
        {
            if (!GetTextBlocks().Any())
                return null;

            //search for a text block with this common text
            var descOfBiz = GetTextBlocks().FirstOrDefault(tb =>
                Regex.IsMatch(tb.Item2, @"DESCRIPTION\s+OF\s+BUSINESS", RegexOptions.IgnoreCase));
            return descOfBiz?.Item2;
        }
    }
}