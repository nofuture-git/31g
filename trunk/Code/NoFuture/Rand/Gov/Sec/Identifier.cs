using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NoFuture.Rand.Data.Sp;
using NoFuture.Util.Binary;

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
        public virtual FederalStatute Statute => new SecuritiesExchangeAct();
        /// <summary>
        /// This html will contain, when available from the SEC, the uri to the XBRL xml
        /// </summary>
        public Uri HtmlFormLink { get; set; }
        /// <summary>
        /// Reports prefixed with <see cref="NotificationOfInabilityToTimelyFile"/> will 
        /// have this set to true.
        /// </summary>
        public bool IsLate { get; set; }
        public string AccessionNumber { get { return Value; } set { Value = value; } }
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
                .NfGetTypes()
                .Where(x => x.NfBaseType() == typeof(SecForm)).ToList();

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
                    return Edgar.CtorInteractiveLink(CIK.Value, AccessionNumber);

                return null;
            }
        }
    }

    [Serializable]
    public class Form13Fhr : SecForm
    {
        public const string ABBREV = "13F-HR";
        public Form13Fhr() : base(ABBREV) { }
    }
}
