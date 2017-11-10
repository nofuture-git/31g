using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Gov.Irs;
using NoFuture.Rand.Gov.Sec;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Represents a publicly traded corporation
    /// </summary>
    [Serializable]
    public class PublicCorporation : Firm
    {
        private readonly List<SecForm> _secReports = new List<SecForm>();
        private List<TickerSymbol> _tickerSymbols = new List<TickerSymbol>();

        public EmployerIdentificationNumber EIN { get; set; }
        public CentralIndexKey CIK { get; set; }
        public List<SecForm> SecReports => _secReports;
        public UsState UsStateOfIncorporation { get; set; }
        public Uri[] WebDomains { get; set; }
        public List<TickerSymbol> TickerSymbols
        {
            get
            {
                _tickerSymbols.Sort(new TickerComparer(Name));
                return _tickerSymbols;
            }
            set { _tickerSymbols = value; } //XRef.cs needs this as RW
        }
        public string UrlEncodedName => Uri.EscapeUriString(Cusip.GetSearchCompanyName(Name));

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
    }
}