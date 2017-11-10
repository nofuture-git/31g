using System;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Gov.Sec
{
    [Serializable]
    public class CentralIndexKey : Identifier
    {
        public override string Abbrev => "CIK";
    }

    [Serializable]
    public abstract class SecForm : Identifier
    {
        #region constants
        public const string NotificationOfInabilityToTimelyFile = "NT";
        #endregion

        #region fields
        protected string secFormNumber;
        #endregion

        #region properties
        public CentralIndexKey CIK { get; set; }
        public override string Abbrev => secFormNumber;
        public override string Value { get; set; }
        public DateTime FilingDate { get; set; }

        /// <summary>
        /// This html will contain, when available from the SEC, the uri to the XBRL xml
        /// </summary>
        public Uri HtmlFormLink { get; set; }
        
        /// <summary>
        /// Reports prefixed with <see cref="NotificationOfInabilityToTimelyFile"/> will 
        /// have this set to true.
        /// </summary>
        public bool IsLate { get; set; }
        public string AccessionNumber { get => Value; set => Value = value; }
        public Uri XmlLink { get; set; }
        #endregion

        #region methods
        protected SecForm(string secFormNumber)
        {
            this.secFormNumber = secFormNumber;
        }

        /// <summary>
        /// Helper method to ctor an instance of <see cref="SecForm"/>
        /// using it common abbreviation
        /// </summary>
        /// <param name="reportAbbrev"></param>
        /// <returns></returns>
        public static SecForm SecFormFactory(string reportAbbrev)
        {
            if (string.IsNullOrWhiteSpace(reportAbbrev))
                return null;
            var secFormTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.BaseType == typeof(SecForm)).ToList();

            Type secFormType = null;
            foreach (var mt in secFormTypes)
            {
                var fis = mt.GetFields().Where(x => x.IsLiteral);
                foreach (var fi in fis)
                {
                    var fiVal = fi.GetValue(null) as string;
                    if (fiVal == null)
                        continue;
                    if (!fiVal.Equals(reportAbbrev, StringComparison.OrdinalIgnoreCase))
                        continue;
                    secFormType = mt;
                    break;
                }
            }

            if (secFormType == null)
                return null;
            return Activator.CreateInstance(secFormType) as SecForm;
        }
        #endregion
    }

    [Serializable]
    public class Form10K : SecForm
    {
        public const string ABBREV = "10-K";
        public Form10K() : base(ABBREV) { }
        public override string Value { get; set; }

        public int FiscalYear => FinancialData?.FiscalYear ?? 0;
        public ComFinancialData FinancialData { get; set; }
        public SummaryOfBusiness Summary { get; set; }

        /// <summary>
        /// Returns the link to the SEC's interative version of the report.
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

        internal static Uri CtorInteractiveLink(string cik, string accessionNum)
        {
            var qry = new StringBuilder();
            qry.Append("?action=view&");
            qry.AppendFormat("cik={0}&", cik);
            qry.AppendFormat("accession_number={0}&", accessionNum);
            qry.Append("xbrl_type=v");

            return new Uri("https://www.sec.gov/cgi-bin/viewer" + qry);
        }
    }

    [Serializable]
    public class Form13Fhr : SecForm
    {
        public const string ABBREV = "13F-HR";
        public Form13Fhr() : base(ABBREV) { }
    }
}
