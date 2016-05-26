using System;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Gov.Sec
{
    [Serializable]
    public class CentralIndexKey : Identifier
    {
        public override string Abbrev { get { return "CIK"; } }
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
        public override string Abbrev { get { return secFormNumber; }}
        public override string Value { get; set; }
        public abstract FederalStatute Statute { get; }
        public Uri HtmlFormLink { get; set; }
        public Uri InteractiveFormLink { get; set; }
        public Uri XbrlZipLink { get; set; }

        /// <summary>
        /// Reports prefixed with <see cref="NotificationOfInabilityToTimelyFile"/> will 
        /// have this set to true.
        /// </summary>
        public bool IsLate { get; set; }

        public string AccessionNumber { get { return Value; } set { Value = value; } }

        public string FormattedAccessionNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AccessionNumber))
                    return AccessionNumber;
                if (AccessionNumber.Length < 18)
                    return AccessionNumber;
                var p1 = AccessionNumber.Substring(0, 10);
                var p2 = AccessionNumber.Substring(10, 2);
                var p3 = AccessionNumber.Substring(12, 6);
                return string.Join("-", new[] {p1, p2, p3});
            }
        }
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
        public override FederalStatute Statute { get { return new SecuritiesExchangeAct(); } }
        public FinancialData FinancialData { get; set; }
        public SummaryOfBusiness Summary { get; set; }
    }
}
