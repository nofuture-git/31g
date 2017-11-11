using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public abstract class Firm : VocaBase, IFirm
    {
        #region constants
        protected const int ONE_THOUSAND = 1000;
        #endregion

        #region fields
        private NaicsPrimarySector _primarySector;
        private NaicsSector _sector;
        private NaicsMarket _market;
        private int _fiscalYearEndDay = 1;
        #endregion

        #region properties

        public string Name => GetName(KindsOfNames.Legal);
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
