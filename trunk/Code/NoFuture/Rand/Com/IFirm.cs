using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo;

namespace NoFuture.Rand.Com
{
    public interface IFirm : IVoca
    {
        string Name { get;}
        Tuple<UsStreetPo, UsCityStateZip> MailingAddress { get; set; }
        Tuple<UsStreetPo, UsCityStateZip> BusinessAddress { get; set; }
        NorthAmericanPhone[] Phone { get; set; }
        StandardIndustryClassification SIC { get; set; }
        NaicsPrimarySector PrimarySector { get; set; }
        NaicsSector Sector { get; set; }
        NaicsMarket Market { get; set; }
        int FiscalYearEndDay { get; set; }
        void LoadXrefXmlData();
    }
}