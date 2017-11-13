using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// A type for any kind of business entity which
    /// sells goods or services to make a profit
    /// </summary>
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

        /// <summary>
        /// A method to merge data from various source into the given instance
        /// </summary>
        void LoadXrefXmlData();
    }
}