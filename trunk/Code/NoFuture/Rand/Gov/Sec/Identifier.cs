using System;
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
        public const string NotificationOfInabilityToTimelyFile = "NT";

        protected string secFormNumber;

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

        public string AccessionNumber { get; set; }

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

        protected SecForm(string secFormNumber)
        {
            this.secFormNumber = secFormNumber;
        }
    }

    [Serializable]
    public class Form10K : SecForm
    {
        public Form10K() : base("10-K") { }
        public override string Value { get; set; }
        public DateTime FilingDate { get; set; }
        public override FederalStatute Statute { get { return new SecuritiesAct(); } }
        public FinancialData FinancialData { get; set; }
        public SummaryOfBusiness Summary { get; set; }
    }
}
