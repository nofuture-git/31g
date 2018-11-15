using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Text.RegularExpressions;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo.US;

namespace NoFuture.Rand.Geo.CA
{
    /// <inheritdoc cref="CityArea"/>
    /// <summary>
    /// Postal address for Canada
    /// </summary>
    [Serializable]
    public class CaCityProvidencePost : CityArea
    {
        internal const string CA_POST_CODE_DATA = "CA_Postal_Data.xml";

        internal const string POSTAL_CODE_REGEX = @"([A-Z0-9]{3})\x20?([A-Z0-9]{3})";

        internal const string POSTAL_CODE = "postal-code";
        internal const string DF_FIRST_THREE_CHARS = "M5A";
        internal const string DF_CITY = "Toronto";
        internal const string DF_LAST_THREE_CHARS = "4Z4";
        internal const string MUNICIPALITY = "municipality";
        internal const string ABBREVIATION = "abbreviation";

        internal static XmlDocument CaPostCodeXml;
        private static readonly Dictionary<string, string> _caProvAbbrev2Name = new Dictionary<string, string>();

        public CaCityProvidencePost(AddressData d) : base(d)
        {
            GetData().NationState = "CA";
        }
        public string ProvidenceAbbrv => GetData().RegionAbbrev;
        public string Providence => GetData().RegionName;
        public string PostalCode => GetData().PostalCode;

        internal static string[] CaPostalProvidenceAbbrev
        {
            get
            {
                if (_caProvAbbrev2Name != null && _caProvAbbrev2Name.Any())
                    return _caProvAbbrev2Name.Keys.ToArray();
                CaPostCodeXml = CaPostCodeXml ??
                                XmlDocXrefIdentifier.GetEmbeddedXmlDoc(CA_POST_CODE_DATA, Assembly.GetExecutingAssembly());
                var provNodes = CaPostCodeXml.SelectNodes("//providence");
                if (provNodes == null)
                    return new string[] { };
                for (var i = 0; i < provNodes.Count; i++)
                {
                    var provNode = provNodes[i] as XmlElement;
                    if(provNode == null)
                        continue;
                    var provAbbrev = provNode.Attributes[ABBREVIATION]?.Value;
                    if(string.IsNullOrWhiteSpace(provAbbrev))
                        continue;
                    var provName = provNode.Attributes[NAME]?.Value;
                    if(_caProvAbbrev2Name.ContainsKey(provAbbrev))
                        continue;
                    _caProvAbbrev2Name.Add(provAbbrev, provName);
                }

                return _caProvAbbrev2Name.Keys.ToArray();
            }
        }

        public static string RegexCaPostalProvidenceAbbrev => @"[\x20|\x2C](" + string.Join("|", CaPostalProvidenceAbbrev) + @")([^A-Za-z]|$)";

        public override string ToString()
        {
            return $"{City} {ProvidenceAbbrv}, {PostalCode}";
        }

        public static bool TryParse(string lastLine, out CaCityProvidencePost cityProvPost)
        {
            if (string.IsNullOrWhiteSpace(lastLine))
            {
                cityProvPost = null;
                return false;
            }
            lastLine = lastLine.Trim();

            var addrData = new AddressData
            {
                PostalCode = string.Empty,
                SortingCode = string.Empty,
                RegionAbbrev = string.Empty,
                Locality = string.Empty
            };
            GetPostalCode(lastLine, addrData);

            GetProvidence(lastLine, addrData);

            UsCityStateZip.GetCity(lastLine, addrData);
            addrData.RegionName = _caProvAbbrev2Name.ContainsKey(addrData.RegionAbbrev)
                ? _caProvAbbrev2Name[addrData.RegionAbbrev]
                : "";
            cityProvPost = new CaCityProvidencePost(addrData) {Src = lastLine};
            return !string.IsNullOrWhiteSpace(addrData.PostalCode)
                   && !string.IsNullOrWhiteSpace(addrData.RegionAbbrev)
                   && !string.IsNullOrWhiteSpace(addrData.Locality);
        }

        internal static void GetProvidence(string lastLine, AddressData addrData)
        {
            UsCityStateZip.GetState(lastLine, addrData, RegexCaPostalProvidenceAbbrev);
        }

        internal static void GetPostalCode(string lastLine, AddressData addrData)
        {
            var regex = new Regex(POSTAL_CODE_REGEX, RegexOptions.IgnoreCase);
            if (string.IsNullOrWhiteSpace(lastLine))
                return;

            var reverseLastLine = new string(lastLine.ToCharArray().Reverse().ToArray());

            if (!regex.IsMatch(reverseLastLine))
                return;
            var matches = regex.Match(reverseLastLine);

            var firstThree = matches.Groups.Count >= 2 && matches.Groups[1].Success &&
                             matches.Groups[1].Captures.Count > 0
                ? matches.Groups[1].Captures[0].Value
                : string.Empty;
            var lastThree = matches.Groups.Count >= 3 && matches.Groups[2].Success &&
                            matches.Groups[2].Captures.Count > 0
                ? matches.Groups[2].Captures[0].Value
                : string.Empty;

            var t0 = new string(lastThree.ToCharArray().Reverse().ToArray());
            var t1 = new string(firstThree.ToCharArray().Reverse().ToArray());

            addrData.PostalCode = $"{t0} {t1}";
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(City))
                itemData.Add(textFormat(nameof(City)), City);
            if (!string.IsNullOrWhiteSpace(ProvidenceAbbrv))
                itemData.Add(textFormat(nameof(Providence)), ProvidenceAbbrv);
            if (!string.IsNullOrWhiteSpace(PostalCode))
                itemData.Add(textFormat(nameof(PostalCode)), PostalCode);
            return itemData;
        }
    }
}