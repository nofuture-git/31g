using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US.Irs;
using NoFuture.Rand.Gov.US.Sec;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Com
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a publicly traded company 
    /// (a.k.a. publicly traded company, publicly held company, publicly listed company, public corporation)
    /// </summary>
    [Serializable]
    public class PublicCompany : Firm
    {
        private readonly List<SecForm> _secReports = new List<SecForm>();
        private List<TickerSymbol> _tickerSymbols = new List<TickerSymbol>();

        public EmployerIdentificationNumber EIN { get; set; }
        public CentralIndexKey CIK { get; set; }
        public IEnumerable<SecForm> SecReports => _secReports;
        public string UsStateOfIncorporation { get; set; }
        public List<TickerSymbol> TickerSymbols
        {
            get
            {
                _tickerSymbols.Sort(new TickerComparer(Name));
                return _tickerSymbols;
            }
            set => _tickerSymbols = value;
        }

        public string UrlEncodedName => Util.Core.Etc.EscapeString(GetSearchCompanyName(Name), EscapeStringType.URI);

        public override void LoadXrefXmlData()
        {
            var firmName = Name;
            if (GetType() == typeof(Bank))
            {
                firmName = GetName(KindsOfNames.Abbrev) ?? Name;
            }
            var myAssocNodes = XRefGroup.GetXrefAddNodes(GetType(), firmName);
            if (myAssocNodes == null || myAssocNodes.Count <= 0)
                return;

            foreach (var node in myAssocNodes)
            {
                var elem = node as XmlElement;
                if (elem == null)
                    continue;

                XRefGroup.SetTypeXrefValue(elem, this);
            }
        }

        public virtual void AddSecReport(SecForm secReport)
        {
            if (secReport == null)
                return;
            _secReports.Add(secReport);
        }

        public virtual void AddTickerSymbol(TickerSymbol ticker)
        {
            if (ticker == null)
                return;
            _tickerSymbols.Add(ticker);
        }

        /// <summary>
        /// Attempts to assign the <see cref="Firm.Description"/> to the 
        /// the &apos;DESCRIPTION OF BUSINESS&apos; found in the 10-K report
        /// </summary>
        /// <returns></returns>
        public virtual string GetForm10KDescriptionOfBiz()
        {
            const string UNKNOWN = "UNKNOWN";

            //see if there are any sec reports
            if (!_secReports.Any())
            {
                return UNKNOWN;
            }

            //get the latest one annual report
            var secRptDates = _secReports.Where(sec => sec is Form10K).Select(sec => sec.FilingDate);

            //sort them by most-recent filing first
            secRptDates = secRptDates.OrderByDescending(sec => sec);

            foreach (var rptDate in secRptDates)
            {
                var secRpt = _secReports.FirstOrDefault(sec => sec.FilingDate == rptDate);
                if (secRpt == null || !secRpt.GetTextBlocks().Any())
                    continue;
                if (!(secRpt is Form10K from10K))
                    continue;

                //search for a text block with this common text
                var descOfBiz = from10K.GetDescriptionOfBiz();
                if (string.IsNullOrWhiteSpace(descOfBiz))
                    continue;

                //finding it, assign it and return it
                return descOfBiz;
            }

            return UNKNOWN;
        }
    }
}