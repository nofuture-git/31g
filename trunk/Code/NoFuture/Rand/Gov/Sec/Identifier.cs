using System;
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
        public abstract FederalStatute Statute { get; }
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

        #endregion

        #region methods
        protected SecForm(string secFormNumber)
        {
            this.secFormNumber = secFormNumber;
        }
        #endregion
    }

    [Serializable]
    public class Form10K : SecForm
    {
        public Form10K() : base("10-K") { }
        public override string Value { get; set; }
        public DateTime FilingDate { get; set; }
        public int FiscalYear => FinancialData?.FiscalYear ?? 0;
        public override FederalStatute Statute => new SecuritiesExchangeAct();
        public ComFinancialData FinancialData { get; set; }
        public SummaryOfBusiness Summary { get; set; }
        /// <summary>
        /// Returns the link to the SEC's interative version of the report.
        /// </summary>
        public Uri InteractiveFormLink
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CIK?.Value) && !string.IsNullOrWhiteSpace(AccessionNumber) && XbrlXmlLink != null)
                    return Edgar.CtorInteractiveLink(CIK.Value, AccessionNumber);

                return null;
            }
        }
        public Uri XbrlXmlLink { get; set; }
    }
}
