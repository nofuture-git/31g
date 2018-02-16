using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Org;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// A type for any kind of business entity which
    /// sells goods or services to make a profit
    /// </summary>
    public interface IFirm : IVoca
    {
        string Name { get; set; }
        string Description { get; set; }
        IEnumerable<Uri> NetUri { get; }
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

        /// <summary>
        /// Adds the given <see cref="uri"/> to this company
        /// </summary>
        /// <param name="uri"></param>
        void AddUri(Uri uri);

        /// <summary>
        /// Parses and, when valid, adds the <see cref="uri"/> to this company
        /// </summary>
        /// <param name="uri"></param>
        void AddUri(string uri);
    }
}