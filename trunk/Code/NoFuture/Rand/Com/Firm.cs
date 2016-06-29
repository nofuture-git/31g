using System;
using System.Collections.Generic;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Com
{
    public interface IFirm
    {
        string Name { get; set; }
        List<Tuple<KindsOfNames, string>> OtherNames { get; }
        Tuple<UsStreetPo, UsCityStateZip> MailingAddress { get; set; }
        Tuple<UsStreetPo, UsCityStateZip> BusinessAddress { get; set; }
        NorthAmericanPhone[] Phone { get; set; }
        StandardIndustryClassification SIC { get; set; }
        NaicsPrimarySector PrimarySector { get; set; }
        NaicsSector Sector { get; set; }
        NaicsMarket Market { get; set; }
        int FiscalYearEndDay { get; set; }
    }

    [Serializable]
    public abstract class Firm : IFirm
    {
        #region constants
        public const int ONE_THOUSAND = 1000;
        #endregion

        #region fields
        private NaicsPrimarySector _primarySector;
        private NaicsSector _sector;
        private NaicsMarket _market;
        private int _fiscalYearEndDay = 1;
        protected readonly List<Tuple<KindsOfNames, string>> _otherNames =
            new List<Tuple<KindsOfNames, string>>();
        #endregion

        #region properties
        public string Name { get; set; }
        public List<Tuple<KindsOfNames, string>> OtherNames => _otherNames;
        public Tuple<UsStreetPo, UsCityStateZip> MailingAddress { get; set; }
        public Tuple<UsStreetPo, UsCityStateZip> BusinessAddress { get; set; }
        public NorthAmericanPhone[] Phone { get; set; }
        public StandardIndustryClassification SIC { get; set; }
        public NaicsPrimarySector PrimarySector
        {
            get
            {
                if (_primarySector == null && SIC != null) 
                    ResolveNaicsOnSic();
                return _primarySector;
            }
            set { _primarySector = value; }
        }

        public NaicsSector Sector
        {
            get
            {
                if(_sector == null && SIC != null)
                    ResolveNaicsOnSic();
                return _sector;
            }
            set { _sector = value; }
        }

        public NaicsMarket Market
        {
            get
            {
                if(_market == null && SIC != null)
                    ResolveNaicsOnSic();
                return _market;
            }
            set { _market = value; }
        }

        public int FiscalYearEndDay { get { return _fiscalYearEndDay; } set { _fiscalYearEndDay = value; } }
        #endregion

        #region methods
        public abstract void LoadXrefXmlData();

        protected internal void ResolveNaicsOnSic()
        {
            if (SIC == null)
                return;
            var naics = StandardIndustryClassification.LookupNaicsBySic(SIC);
            if (naics == null)
                return;
            _primarySector = naics.Item1;
            _sector = naics.Item2;
            _market = naics.Item3;
        }
        #endregion
    }
}
