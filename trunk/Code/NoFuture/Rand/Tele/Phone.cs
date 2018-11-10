using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc cref="Identifier" />
    /// <inheritdoc cref="IObviate" />
    /// <summary>
    /// Base type to represent a telephone number
    /// </summary>
    [Serializable]
    public abstract class Phone : Identifier, IObviate
    {
        public const string UriSchemaTelephone = "tel";

        public abstract string Notes { get; set; }
        public abstract string Formatted { get; }
        public abstract string Unformatted { get; }

        public KindsOfLabels? Descriptor { get; set; }

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

        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            var unForm = Unformatted;
            if (string.IsNullOrWhiteSpace(unForm))
                return itemData;

            var label = Descriptor?.ToString();
            itemData.Add(textFormat(label + nameof(Phone)), unForm);
            return itemData;
        }
    }
}
