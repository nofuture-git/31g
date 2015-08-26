using System;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Com
{
    public interface IFirm
    {
        string Name { get; set; }
        Tuple<UsAddress, UsCityStateZip> MailingAddress { get; set; }
        Tuple<UsAddress, UsCityStateZip> BusinessAddress { get; set; }
        NorthAmericanPhone[] Phone { get; set; }
        StandardIndustryClassification SIC { get; set; }
        NaicsSuperSector SuperSector { get; set; }
        NaicsSector Sector { get; set; }
        NaicsMarket Market { get; set; }
    }

    [Serializable]
    public class Firm : IFirm
    {
        #region fields
        private NaicsSuperSector _superSector;
        private NaicsSector _sector;
        private NaicsMarket _market;
        #endregion

        public string Name { get; set; }
        public Tuple<UsAddress, UsCityStateZip> MailingAddress { get; set; }
        public Tuple<UsAddress, UsCityStateZip> BusinessAddress { get; set; }
        public NorthAmericanPhone[] Phone { get; set; }
        public StandardIndustryClassification SIC { get; set; }

        public NaicsSuperSector SuperSector
        {
            get
            {
                if (_superSector == null && SIC != null) 
                    ResolveNaicsOnSic();
                return _superSector;
            }
            set { _superSector = value; }
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

        protected internal void ResolveNaicsOnSic()
        {
            if (SIC == null)
                return;
            var naics = StandardIndustryClassification.LookupNaicsBySic(SIC);
            if (naics == null)
                return;
            _superSector = naics.Item1;
            _sector = naics.Item2;
            _market = naics.Item3;
        }
    }
    [Serializable]
    public class LimitedLiabilityCompany : Firm
    {
        
    }
 
    [Serializable]
    public class Partnership : Firm
    {
        
    }
}
