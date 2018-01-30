using System;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc />
    /// <summary>
    /// Base type to represent a telephone number
    /// </summary>
    [Serializable]
    public abstract class Phone : Identifier
    {
        public abstract string Notes { get; set; }
        public abstract string Formatted { get; }
        public abstract string Unformatted { get; }
        public abstract Uri ToUri();

        internal const string US_AREA_CODE_DATA = "US_AreaCode_Data.xml";
        internal const string CA_AREA_CODE_DATA = "CA_AreaCode_Data.xml";
        internal static XmlDocument UsAreaCodeXml;
        internal static XmlDocument CaAreaCodeXml;

    }
}
