using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc cref="Identifier" />
    /// <summary>
    /// Base type to represent a telephone number
    /// </summary>
    [Serializable]
    public abstract class Phone : NetUri
    {
        internal const string PHONE_CONTEXT = "phone-context";
        public const string URI_SCHEMA_TELEPHONE = "tel";

        public abstract string Notes { get; set; }
        public abstract string Formatted { get; }
        public abstract string Unformatted { get; }

        /// <summary>
        /// Drafts the phone number as a URI according to RFC 3966
        /// https://tools.ietf.org/html/rfc3966#page-4
        /// </summary>
        /// <returns></returns>
        public override Uri ToUri()
        {
            if (Descriptor == null)
                return new Uri($"{URI_SCHEMA_TELEPHONE}:{Unformatted}");

            var domainname = Descriptor.Value.ToString();

            var d = typeof(KindsOfLabels).FullName?.Split('.').Reverse();
            if (d != null && d.Any())
            {
                domainname = $"{domainname}.{string.Join(".", d)}";
            }

            return new Uri($"{URI_SCHEMA_TELEPHONE}:{Unformatted};{PHONE_CONTEXT}={domainname}");
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            var value = Value;
            if (string.IsNullOrWhiteSpace(value))
                return itemData;

            var label = Descriptor?.ToString();
            itemData.Add(textFormat(label + "Phone"), value);
            return itemData;
        }

        internal const string US_AREA_CODE_DATA = "US_AreaCode_Data.xml";
        internal const string CA_AREA_CODE_DATA = "CA_AreaCode_Data.xml";
        internal static XmlDocument UsAreaCodeXml;
        internal static XmlDocument CaAreaCodeXml;
    }
}
