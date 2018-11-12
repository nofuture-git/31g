﻿using System;
using System.Xml;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc cref="Identifier" />
    /// <summary>
    /// Base type to represent a telephone number
    /// </summary>
    [Serializable]
    public abstract class Phone : LabelledIdentifier
    {
        public const string UriSchemaTelephone = "tel";

        public abstract string Notes { get; set; }
        public abstract string Formatted { get; }
        public abstract string Unformatted { get; }

        /// <summary>
        /// Drafts the phone number as a URI according to RFC 3966
        /// https://tools.ietf.org/html/rfc3966#page-4
        /// </summary>
        /// <returns></returns>
        public abstract Uri ToUri();

        internal const string US_AREA_CODE_DATA = "US_AreaCode_Data.xml";
        internal const string CA_AREA_CODE_DATA = "CA_AreaCode_Data.xml";
        internal static XmlDocument UsAreaCodeXml;
        internal static XmlDocument CaAreaCodeXml;
    }
}
