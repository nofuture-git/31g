using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
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
        string Description { get; set; }
        IEnumerable<NetUri> NetUri { get; }
        PostalAddress MailingAddress { get; set; }
        PostalAddress BusinessAddress { get; set; }
        IEnumerable<Phone> PhoneNumbers { get; }
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
        /// Adds the given <see cref="uri"/> to this Firm
        /// </summary>
        /// <param name="uri"></param>
        void AddUri(NetUri uri);

        /// <summary>
        /// Parses and, when valid, adds the <see cref="uri"/> to this company
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="descriptor"></param>
        void AddUri(string uri, KindsOfLabels? descriptor = KindsOfLabels.Business);

        /// <summary>
        /// Add the given phone number to this person
        /// </summary>
        /// <param name="phone"></param>
        void AddPhone(Phone phone);

        /// <summary>
        /// Parses and, when valid, adds the phone number to this person
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="descriptor"></param>
        void AddPhone(string phoneNumber, KindsOfLabels? descriptor = null);
    }
}