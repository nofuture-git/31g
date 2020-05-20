using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Geo.US
{
    /// <summary>
    /// The typical Line 1 of a US Postal Address
    /// </summary>
    [Serializable]
    public class UsStreetPo : StreetPo
    {
        #region fields
        public const string STD_ADDR_LINE_REGEX = @"^([0-9\x5c\x2f\x2d]*)\x20*([\x21-\x7e]*)(\x20*([\x21-\x7e][\x20-\x7e]*))?";

        public const string POUND_UNIT_ID = @"\x20\x23\x20?([0-9]+)";

        internal static XmlDocument UsPostalStreetKindProbXml;
        private static Dictionary<string, double> _streetKind2Prob = new Dictionary<string, double>();

        private static string[] _usPostalStreetKindFullName;
        private static string[] _usPostalStreetKindAbbrev;
        private static string[] _usPostalSecondaryUnits;
        private static string[] _usPostalStreetNames;

        #endregion

        public UsStreetPo(AddressData d) : base(d)
        {
        }

        #region properties
        public string CountyTownship { get; set; }
        public string PostBox => GetData().ThoroughfareNumber;
        public string StreetName => GetData().ThoroughfareName;
        public string StreetKind => GetData().ThoroughfareType;
        public string Line1 => ToString();
        public string Line2 => $"{GetData().SecondaryUnitDesignator} {GetData().SecondaryUnitId}".Trim();

        /// <summary>
        /// Dictionary parsed from US Postal street kinds probability xml data
        /// </summary>
        public static Dictionary<string, double> UsPostalStreetKind2Prob
        {
            get
            {
                if (_streetKind2Prob.Any())
                    return _streetKind2Prob;
                UsPostalStreetKindProbXml = UsPostalStreetKindProbXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc("US_PostalStreet_Kind_ProbTable.xml",
                                                Assembly.GetExecutingAssembly());
                if (UsPostalStreetKindProbXml == null)
                    return null;
                _streetKind2Prob = XmlDocXrefIdentifier.GetProbTable(UsPostalStreetKindProbXml, "us-postal-street-kind", "id");

                return _streetKind2Prob;
            }
        }

        internal static string[] UsPostalStreetKindFullNames
        {
            get
            {
                if (_usPostalStreetKindFullName != null && _usPostalStreetKindFullName.Any())
                    return _usPostalStreetKindFullName;

                _usPostalStreetKindFullName = ReadTextFileData("US_PostalStreet_Kind_Fullname.txt");
                return _usPostalStreetKindFullName;
            }
        }

        internal static string[] UsPostalStreetKindAbbrev
        {
            get
            {
                if (_usPostalStreetKindAbbrev != null && _usPostalStreetKindAbbrev.Any())
                    return _usPostalStreetKindAbbrev;
                _usPostalStreetKindAbbrev = ReadTextFileData("US_PostalStreet_Kind_Abbrev.txt");
                return _usPostalStreetKindAbbrev;
            }
        }

        internal static string[] UsPostalSecondaryUnits
        {
            get
            {
                if (_usPostalSecondaryUnits != null && _usPostalSecondaryUnits.Any())
                    return _usPostalSecondaryUnits;
                _usPostalSecondaryUnits = ReadTextFileData("US_Postal_Secondary_Units.txt");
                return _usPostalSecondaryUnits;
            }
        }

        internal static string[] UsPostalStreetNames
        {
            get
            {
                if (_usPostalStreetNames != null && _usPostalStreetNames.Any())
                    return _usPostalStreetNames;
                _usPostalStreetNames = ReadTextFileData("US_PostalStreet_Names.txt");
                return _usPostalStreetNames;
            }
        }

        internal static string[] UsPostalDirectionalAbbrev
        {
            get { return new[] {"N", "S", "E", "W", "NE", "NW", "SE", "SW"}; }
        }

        public static string RegexUsPostalStreetKindFullName =>
            @"\x20(" + string.Join("|", UsPostalStreetKindFullNames) + @")(\x2e|\x20|$)";

        /// <summary>
        /// These are limited to the abbreviations used for street types (e.g. STREET -> ST) only.
        /// </summary>
        /// <remarks>
        /// see [http://pe.usps.gov/text/pub28/28apc_002.htm]
        /// </remarks>
        public static string RegexUsPostalStreeKindAbbrev =>
            @"\x20(" + string.Join("|", UsPostalStreetKindAbbrev) + @")(\x2e|\x20|$)";

        public static string RegexUsPostalSecondaryUnits =>
            @"\x20(" + string.Join(@"\x2e?|", UsPostalSecondaryUnits) + @")\x20([\x21-\x7a]*)?";

        public static string RegexUsPostalDirectionalAbbrev =>
            @"\x20(" + string.Join(@"|", UsPostalDirectionalAbbrev) + @")(\x2e|\x20|$)";

        #endregion

        /// <summary>
        /// Selects, at random, an American street kind (e.g. 'St', 'Road', 'Cir', etc.).
        /// </summary>
        [RandomFactory]
        public static string RandomAmericanStreetKind()
        {
            var stKind2Prob = UsPostalStreetKind2Prob;

            var val = !stKind2Prob.Any() ? "st" : Etx.RandomPickOne(stKind2Prob);
            return val.CapWords(' ');
        }

        [RandomFactory]
        public static string RandomAmericanStreetName()
        {
            var usStNames = UsPostalStreetNames;
            return usStNames.Any() ? Etx.RandomPickOne(usStNames) : "Elm";
        }

        [RandomFactory]
        public static string RandomAmericanAddressLine2()
        {
            var pickOne = Etx.MyRand.Next(0, 10);
            switch (pickOne)
            {
                case 0:
                case 1:
                case 2:
                    return $"Apt {Etx.MyRand.Next(0, 999)}";
                case 3:
                    return $"Suite {Etx.MyRand.Next(0, 999)}";
                case 4:
                    return $"Apt. {Etx.MyRand.Next(0, 99)}";
                case 5:
                    return $"Ste. {Etx.MyRand.Next(0, 99)}";
                case 6:
                    return $"Unit {Etx.MyRand.Next(0, 999)}";
                case 8:
                    return $"P.O. Box {Etx.MyRand.Next(0, 999)}";
                case 9:
                    return $"PO Box {Etx.MyRand.Next(0, 999)}";
                default:
                    return $"Apt {Etx.MyRand.Next(0, 999)}";
            }
        }

        /// <summary>
        /// Based on the USPS Pub. 28 [http://pe.usps.gov/cpim/ftp/pubs/Pub28/pub28.pdf]
        /// </summary>
        /// <param name="addressLine"></param>
        /// <param name="streetPo"></param>
        /// <returns></returns>
        public static bool TryParse(string addressLine, out UsStreetPo streetPo)
        {
            if (string.IsNullOrWhiteSpace(addressLine))
            {
                streetPo = null;
                return false;
            }

            addressLine = addressLine.DistillSpaces().Trim();

            if (addressLine.ToCharArray().All(char.IsDigit))
            {
                streetPo = null;
                return false;
            }

            var regex = new Regex(STD_ADDR_LINE_REGEX,RegexOptions.IgnoreCase);
            if (!regex.IsMatch(addressLine))
            {
                streetPo = null;
                return false;
            }

            var matches = regex.Match(addressLine);

            if (matches.Groups.Count < 2)
            {
                streetPo = null;
                return false;
            }
            
            //get the address box number off the front of the string
            var addrData = new AddressData
            {
                ThoroughfareNumber = string.Empty,
                ThoroughfareName = string.Empty,
                ThoroughfareType = string.Empty,
                SecondaryUnitDesignator = string.Empty,
                SecondaryUnitId = string.Empty,
                ThoroughfareDirectional = string.Empty
            };

            var addrNumber = matches.Groups[1].Success && matches.Groups[1].Captures.Count > 0
                ? matches.Groups[1].Captures[0].Value
                : string.Empty;
            addrData.ThoroughfareNumber = addrNumber.Trim();

            Func<Regex, string, string> getStreetKind = (regex1, s) =>
            {
                var m = regex1.Match(s);
                var stKind = m.Groups[0].Success && m.Groups[0].Captures.Count > 0
                    ? m.Groups[0].Captures[0].Value
                    : string.Empty;
                return stKind.Trim();
            };

            //find a match to the street kind
            regex = new Regex(RegexUsPostalStreeKindAbbrev, RegexOptions.IgnoreCase);
            if (regex.IsMatch(addressLine))
            {
                addrData.ThoroughfareType = getStreetKind(regex, addressLine);
            }
            else
            {
                regex = new Regex(RegexUsPostalStreetKindFullName, RegexOptions.IgnoreCase);
                if (regex.IsMatch(addressLine))
                {
                    addrData.ThoroughfareType = getStreetKind(regex, addressLine);
                }
            }

            //look for a secondary unit designator
            regex = new Regex(RegexUsPostalSecondaryUnits, RegexOptions.IgnoreCase);//postal standard
            if (regex.IsMatch(addressLine))
            {
                matches = regex.Match(addressLine);

                var secUnit = matches.Groups.Count >= 2 && 
                              matches.Groups[1].Success && 
                              matches.Groups[1].Captures.Count > 0
                    ? matches.Groups[1].Captures[0].Value
                    : string.Empty;

                var secUnitId = matches.Groups.Count >= 3 && 
                                matches.Groups[2].Success &&
                                matches.Groups[2].Captures.Count > 0
                    ? matches.Groups[2].Captures[0].Value
                    : string.Empty;

                addrData.SecondaryUnitDesignator = secUnit.Trim();
                addrData.SecondaryUnitId = secUnitId.Trim();
            }

            regex = new Regex(POUND_UNIT_ID, RegexOptions.IgnoreCase);//typical non-standard
            if (regex.IsMatch(addressLine))
            {
                matches = regex.Match(addressLine);

                var secUnitId = matches.Groups.Count > 1 && matches.Groups[1].Success &&
                                matches.Groups[1].Captures.Count > 0
                    ? matches.Groups[1].Captures[0].Value
                    : string.Empty;
                addrData.SecondaryUnitDesignator = secUnitId.Trim();
            }

            regex = new Regex(RegexUsPostalDirectionalAbbrev, RegexOptions.IgnoreCase);
            if (regex.IsMatch(addressLine))
            {
                matches = regex.Match(addressLine);
                var directional = matches.Groups.Count > 0 && matches.Groups[0].Success &&
                                  matches.Groups[0].Captures.Count > 0
                    ? matches.Groups[0].Captures[0].Value
                    : string.Empty;

                addrData.ThoroughfareDirectional = directional;
            }

            //consider whatever remains after rem of other parts as 'StreetName'
            var streetName = addressLine;

            if(addrData.ThoroughfareNumber.Length > 0)
                streetName = streetName.Replace(addrData.ThoroughfareNumber, string.Empty);
            if(addrData.ThoroughfareType.Length > 0)
                streetName = streetName.Replace(addrData.ThoroughfareType, string.Empty);
            if(addrData.SecondaryUnitDesignator.Length > 0)
                streetName = streetName.Replace(addrData.SecondaryUnitDesignator, string.Empty);
            if(addrData.SecondaryUnitId.Length > 0)
                streetName = streetName.Replace(addrData.SecondaryUnitId, string.Empty);
            if (addrData.ThoroughfareDirectional.Length > 0)
            {
                streetName = streetName.Replace(addrData.ThoroughfareDirectional, string.Empty);
                //only now do we want to drop spaces and periods from the directional
                addrData.ThoroughfareDirectional = addrData.ThoroughfareDirectional.Replace(".", "").Trim();
            }

            addrData.ThoroughfareName = streetName.Replace("#", "").DistillSpaces().Trim();//per the standard, these should be removed

            if (string.IsNullOrWhiteSpace(addrData.ThoroughfareName))
            {
                streetPo = null;
                return false;
            }

            //consider whatever remains as the street's name
            streetPo = new UsStreetPo(addrData) {Src = addressLine};
            return true;
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();
            var line1 = ToString();
            if(!string.IsNullOrWhiteSpace(line1))
                itemData.Add(textFormat("AddressLine1"), ToString());

            return itemData;
        }

        /// <summary>
        /// Prints the address-line-1 as it would appear as post marked.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var data = GetData();
            return NfString.DistillSpaces(string.Join(" ", data.ThoroughfareNumber, data.ThoroughfareDirectional, data.ThoroughfareName,
                data.ThoroughfareType).Trim());
        }
    }
}