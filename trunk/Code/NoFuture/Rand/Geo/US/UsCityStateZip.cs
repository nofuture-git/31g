﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Gov.US.Census;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Rand.Geo.US
{
    /// <summary>
    /// A type to represent the typical City, State and ZIP code of a US Postal Address
    /// (i.e. the second line of a US Postal Address)
    /// </summary>
    [Serializable]
    public class UsCityStateZip : CityArea
    {
        #region constants

        public const string ZIP_CODE_REGEX = @"\x20([0-9]{5})(\x2d[0-9]{4})?";

        public const string DF_ZIPCODE_PREFIX = "100";//new york, new york
        public const string DF_STATE_ABBREV = "NY";
        public const string DF_CITY_NAME = "New York";

        internal const string ZIP_CODE_SINGULAR = "zip-code";
        internal const string ZIP_CODE_PLURAL = "zip-codes";
        internal const string MSA_CODE = "msa-code";
        internal const string CBSA_CODE = "cbsa-code";
        internal const string ZIP_STAT = "zip-stat";

        internal const string US_ZIP_CODE_DATA = "US_Zip_Data.xml";
        internal const string US_ZIP_PROB_TABLE = "US_Zip_ProbTable.xml";
        internal const string US_CITY_DATA = "US_City_Data.xml";

        #endregion

        #region fields

        internal static XmlDocument UsZipCodeXml;
        internal static XmlDocument UsZipProbXml;
        internal static XmlDocument UsCityXml;

        private static Dictionary<string, double> _zipCodePrefix2Pop = new Dictionary<string, double>();
        private static string[] _usPostalStateAbbrev;
        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance by query of the AmericanCityData
        /// </summary>
        /// <param name="d">
        /// The criteria which on this this instance is situated.
        /// </param>
        public UsCityStateZip(AddressData d) : base(d)
        {
            GetData().NationState = "US";
        }

        #endregion

        #region properties

        internal static string[] UsPostalStateAbbrev
        {
            get
            {
                if (_usPostalStateAbbrev != null && _usPostalStateAbbrev.Any())
                    return _usPostalStateAbbrev;

                _usPostalStateAbbrev = ReadTextFileData("US_Postal_State_Abbrev.txt");
                return _usPostalStateAbbrev;
            }
        }

        public static string RegexUsPostalStateAbbrev => @"[\x20|\x2C](" + String.Join("|", UsPostalStateAbbrev) + @")([^A-Za-z]|$)";

        public string StateName => GetData().RegionName ?? UsState.GetState(GetData().RegionAbbrev)?.ToString();
        public string StateAbbrev => GetData().RegionAbbrev ?? UsState.GetState(GetData().RegionName)?.StateAbbrev;
        public string ZipCode => GetData().PostalCode;

        /// <summary>
        /// The additional 4 digits appearing after the 5 digit ZIP Code
        /// </summary>
        public string PostalCodeAddonFour => GetData().SortingCode;

        /// <summary>
        /// The Metropolitan Statistical Area of the given US City
        /// </summary>
        public MStatArea Msa { get; set; }

        /// <summary>
        /// The core base statistical area of the given US City
        /// </summary>
        public ComboMStatArea CbsaCode { get; set; }

        /// <summary>
        /// The average earning, over time, for the given US City
        /// </summary>
        public LinearEquation AverageEarnings { get; set; }

        /// <summary>
        /// Dictionary parsed from zip code probability xml data
        /// </summary>
        public static Dictionary<string, double> ZipCodePrefix2Population
        {
            get
            {
                if (_zipCodePrefix2Pop.Any())
                    return _zipCodePrefix2Pop;
                UsZipProbXml = UsZipProbXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_ZIP_PROB_TABLE,
                    Assembly.GetExecutingAssembly());
                if (UsZipProbXml == null)
                    return null;
                _zipCodePrefix2Pop = XmlDocXrefIdentifier.GetProbTable(UsZipProbXml, ZIP_CODE_PLURAL, PREFIX);

                return _zipCodePrefix2Pop;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Selects a US Zip Code prefix at random taking into respect the population pertinent to that zip code prefix.
        /// </summary>
        [RandomFactory]
        public static string RandomAmericanPartialZipCode()
        {
            var zip2Wt = ZipCodePrefix2Population;

            return !zip2Wt.Any() ? DF_ZIPCODE_PREFIX : Etx.RandomPickOne(zip2Wt);
        }

        public override string ToString()
        {
            var data = GetData();
            return !String.IsNullOrWhiteSpace(data.SortingCode)
                ? $"{data.Locality}, {data.RegionAbbrev} {data.PostalCode}-{data.SortingCode}"
                : $"{data.Locality}, {data.RegionAbbrev} {data.PostalCode}";
        }

        /// <summary>
        /// Fetches and assigns the <see cref="AverageEarnings"/>, <see cref="Msa"/> and the <see cref="CbsaCode"/>
        /// </summary>
        public virtual void GetXmlData()
        {
            var xmlNode = GetCityXmlNode();
            if (xmlNode == null)
                return;
            ParseCityXmlNode(xmlNode);
        }

        /// <summary>
        /// Attempts to turn <see cref="lastLine"/> into an instance of <see cref="UsCityStateZip"/>
        /// </summary>
        /// <param name="lastLine">
        /// This is assumed to be a classic USPS Address&apos;s last-line (e.g. New York City, NY 10001). 
        /// </param>
        /// <param name="cityStateZip">
        /// The resulting instance when true is returned
        /// </param>
        /// <returns></returns>
        public static bool TryParse(string lastLine, out UsCityStateZip cityStateZip)
        {
            if (string.IsNullOrWhiteSpace(lastLine))
            {
                cityStateZip = null;
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
            GetZipCode(lastLine, addrData);

            GetState(lastLine, addrData);

            GetCity(lastLine, addrData);

            cityStateZip = new UsCityStateZip(addrData) {Src = lastLine};
            cityStateZip.GetXmlData();
            return !string.IsNullOrWhiteSpace(addrData.PostalCode) 
                   && !string.IsNullOrWhiteSpace(addrData.RegionAbbrev) 
                   && !string.IsNullOrWhiteSpace(addrData.Locality);
        }

        internal static void GetCity(string lastLine, AddressData addrData)
        {
            //city
            var rem = new[]
            {
                addrData.PostalCode,
                addrData.PostalCode?.Replace(" ",""),
                $"-{addrData.SortingCode}",
                $" {addrData.RegionAbbrev}" ,
                $",{addrData.RegionAbbrev}"
            };

            var city = lastLine ?? "";

            foreach (var rm in rem)
            {
                
                if (!string.IsNullOrWhiteSpace(rm) && rm.Length > 1 && city.Contains(rm))
                    city = city.Replace(rm, string.Empty);
            }

            city = city.Replace(",", string.Empty).Trim();

            city = FinesseCityName(city);

            addrData.Locality = city;
        }

        internal static void GetState(string lastLine, AddressData addrData, string regexPattern = null)
        {
            if (string.IsNullOrWhiteSpace(lastLine))
                return;

            regexPattern = regexPattern ?? RegexUsPostalStateAbbrev;
            var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(lastLine))
                return;
            var matches = regex.Match(lastLine);

            var state = matches.Groups.Count >= 1 && matches.Groups[0].Success &&
                        matches.Groups[0].Captures.Count > 0
                ? matches.Groups[0].Captures[0].Value
                : String.Empty;

            addrData.RegionAbbrev = state.Trim();
        }

        internal static void GetZipCode(string lastLine, AddressData addrData)
        {
            //zip code
            if (string.IsNullOrWhiteSpace(lastLine))
                return;

            var regex = new Regex(ZIP_CODE_REGEX, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(lastLine))
                return;
            var matches = regex.Match(lastLine);

            var fiveDigitZip = matches.Groups.Count >= 2 && matches.Groups[1].Success &&
                               matches.Groups[1].Captures.Count > 0
                ? matches.Groups[1].Captures[0].Value
                : String.Empty;
            var zipPlusFour = matches.Groups.Count >= 3 && matches.Groups[2].Success &&
                              matches.Groups[2].Captures.Count > 0
                ? matches.Groups[2].Captures[0].Value
                : String.Empty;

            addrData.PostalCode = fiveDigitZip.Trim();
            addrData.SortingCode = zipPlusFour.Replace("-", String.Empty).Trim();
        }

        /// <summary>
        /// Deals with various city name oddities 
        /// (e.g. New York -> New York City; Winston Salem -> Winston-Salem)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FinesseCityName(string name)
        {
            name = name ?? "";
            var workingName = name;
            var isHyphen = name.Contains("-");

            //take hyphens out
            if (isHyphen)
                workingName = workingName.Replace("-", " ");

            workingName = NfString.CapWords(workingName.ToLower(), ' ');

            if (workingName == "New York")
                return "New York City";

            if (workingName == "Winston Salem")
                return "Winston-Salem";

            var isMcSomething = workingName.Split(' ').Any(p => p.ToLower().StartsWith("mc")) ||
                                name.ToLower().StartsWith("mc");

            if (isMcSomething)
            {
                var bldr = new StringBuilder();
                var flags = new[] { false, false };
                foreach (var t in workingName.ToCharArray())
                {
                    if (t == 'M')
                    {
                        flags[0] = true;
                        flags[1] = false;
                    }
                    if (flags[0] && t == 'c')
                    {
                        flags[1] = true;
                        bldr.Append(t);
                        continue;
                    }

                    if (flags.All(x => x))
                    {
                        if (t == ' ')
                            continue;

                        bldr.Append(Char.ToUpper(t));
                        flags[0] = false;
                        flags[1] = false;
                        continue;
                    }
                    bldr.Append(t);
                }
                workingName = bldr.ToString();
            }

            //put hyphens back
            if (isHyphen)
                workingName = workingName.Replace(" ", "-");

            return workingName;
        }

        /// <summary>
        /// Based on the <see cref="ZipCode"/> and <see cref="StateName"/> that are assigned.
        /// </summary>
        /// <returns></returns>
        protected internal XmlNode GetCityXmlNode()
        {
            UsCityXml = UsCityXml ??
                        XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_CITY_DATA, Assembly.GetExecutingAssembly());
            if (UsCityXml == null)
                return null;
            string searchCrit;
            XmlNodeList matchedNodes = null;
            var data = GetData();
            //search by city-state
            if (!String.IsNullOrWhiteSpace(data.RegionAbbrev) && !String.IsNullOrWhiteSpace(data.Locality) &&
                UsState.GetStateByPostalCode(data.RegionAbbrev) != null)
            {
                var cityName = FinesseCityName(data.Locality);
                searchCrit =
                    $"//state[@abbreviation='{data.RegionAbbrev}']/city[@name='{cityName.EscapeString(EscapeStringType.XML)}']";
                matchedNodes = UsCityXml.SelectNodes(searchCrit);

                //try again on place names
                if (matchedNodes == null || matchedNodes.Count <= 0)
                {
                    searchCrit =
                        $"//state[@abbreviation='{data.RegionAbbrev}']//place[@name='{cityName.EscapeString(EscapeStringType.XML)}']/..";
                    matchedNodes = UsCityXml.SelectNodes(searchCrit);
                }
            }
            //search by first 3 of Zip Code
            else if (!String.IsNullOrWhiteSpace(data.PostalCode) && data.PostalCode.Length >= 3)
            {
                var zipCodePrefix = data.PostalCode.Substring(0, 3);
                searchCrit = $"//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zipCodePrefix}']/..";
                matchedNodes = UsCityXml.SelectNodes(searchCrit);
            }

            if (matchedNodes == null || matchedNodes.Count <= 0)
                return null;
            if (matchedNodes.Count == 1)
                return matchedNodes[0];

            //choose one with a msa-code value if possible
            foreach (var matchedNode in matchedNodes)
            {
                var matchedElem = matchedNode as XmlElement;
                if (matchedElem == null)
                    continue;
                if (!matchedElem.HasAttributes)
                    continue;
                var hasMsaCode = !String.IsNullOrWhiteSpace(matchedElem.GetAttribute(MSA_CODE));
                var hasCbsaCode = !String.IsNullOrWhiteSpace(matchedElem.GetAttribute(CBSA_CODE));
                if (hasCbsaCode || hasMsaCode)
                    return matchedElem;
            }

            var pick = Etx.RandomInteger(0, matchedNodes.Count - 1);
            return matchedNodes[pick];
        }

        /// <summary>
        /// Returns a hashtable whose keys as American's call Race based on the given <see cref="zipCode"/>
        /// </summary>
        /// <param name="zipCode"></param>
        public static AmericanRacePercents RandomAmericanRaceWithRespectToZip(string zipCode)
        {
            var pick = 0;
            //if calling assembly passed in no-args then return all zeros
            if (String.IsNullOrWhiteSpace(zipCode))
                return AmericanRacePercents.GetNatlAvg();
            UsZipCodeXml = UsZipCodeXml ??
                           XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_ZIP_CODE_DATA, Assembly.GetExecutingAssembly());
            if(UsZipCodeXml == null)
                return AmericanRacePercents.GetNatlAvg();
            //get the data for the given zip code
            var zipStatElem = UsZipCodeXml.SelectSingleNode(
                    $"//{ZIP_STAT}[@{VALUE}='{zipCode}']");

            if (zipStatElem == null || !zipStatElem.HasChildNodes)
            {
                //try to find on the zip code prefix 
                var zip3 = zipCode.Substring(0, 3);
                var zipCodeElem = UsZipCodeXml.SelectSingleNode(
                        $"//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zip3}']");

                if (zipCodeElem == null || !zipCodeElem.HasChildNodes)
                    return AmericanRacePercents.GetNatlAvg();

                pick = Etx.MyRand.Next(0, zipCodeElem.ChildNodes.Count - 1);

                zipStatElem = zipCodeElem.ChildNodes[pick];
                if (zipStatElem == null || zipStatElem.ChildNodes.Count == 0)
                    return AmericanRacePercents.GetNatlAvg();
            }

            pick = Etx.MyRand.Next(0, zipStatElem.ChildNodes.Count - 1);
            var arpNode = zipStatElem.ChildNodes[pick];
            return !TryParseRacePercentsXmlNode(arpNode, out var arp) 
                    ? AmericanRacePercents.GetNatlAvg() 
                    : arp;
        }


        /// <summary>
        /// Return a <see cref="NorthAmericanRace"/> randomly with weight based on <see cref="zipCode"/>.
        /// </summary>
        /// <param name="zipCode">Null to get natl averages.</param>
        /// <returns>
        /// Defaults to randomly to national averages
        /// [http://kff.org/other/state-indicator/distribution-by-raceethnicity/]
        /// </returns>
        public static NorthAmericanRace GetAmericanRace(string zipCode)
        {
            var amRace = RandomAmericanRaceWithRespectToZip(zipCode);

            var raceHashByZip = amRace != null
                ? new Dictionary<string, double>
                {
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.AmericanIndian), amRace.AmericanIndian},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Asian), amRace.Asian},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Black), amRace.Black},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Hispanic), amRace.Hispanic},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Mixed), amRace.Mixed},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Pacific), amRace.Pacific},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.White), amRace.White}
                }
                : AmericanRacePercents.GetNatlAvgAsDict();

            var randPick = Etx.RandomPickOne(raceHashByZip);

            Enum.TryParse(randPick, out NorthAmericanRace pickOut);

            return pickOut;

        }

        /// <summary>
        /// Attempt to parse the race-percent-by-hs child node of a zip-stat.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="arp"></param>
        /// <returns></returns>
        internal static bool TryParseRacePercentsXmlNode(XmlNode node, out AmericanRacePercents arp)
        {
            arp = new AmericanRacePercents();
            if (node?.Attributes == null)
                return false;

            var attr = node.Attributes["american-indian"];
            if (attr != null)
            {
                if (Double.TryParse(attr.Value, out var dblOut))
                    arp.AmericanIndian = dblOut;
            }
            attr = node.Attributes["asian"];
            if (attr != null)
            {
                if (Double.TryParse(attr.Value, out var dblOut))
                    arp.Asian = dblOut;
            }
            attr = node.Attributes["hispanic"];
            if (attr != null)
            {
                if (Double.TryParse(attr.Value, out var dblOut))
                    arp.Hispanic = dblOut;
            }
            attr = node.Attributes["black"];
            if (attr != null)
            {
                if (Double.TryParse(attr.Value, out var dblOut))
                    arp.Black = dblOut;
            }
            attr = node.Attributes["white"];
            if (attr != null)
            {
                if (Double.TryParse(attr.Value, out var dblOut))
                    arp.White = dblOut;
            }
            attr = node.Attributes["pacific"];
            if (attr != null)
            {
                if (Double.TryParse(attr.Value, out var dblOut))
                    arp.Pacific = dblOut;
            }
            attr = node.Attributes["mixed-race"];
            if (attr != null)
            {
                if (Double.TryParse(attr.Value, out var dblOut))
                    arp.Mixed = dblOut;
            }

            return !arp.IsEmpty();
        }

        /// <summary>
        /// Assigns the Msa and AverageEarnings properties based on a match to the underlying data.
        /// </summary>
        /// <param name="node"></param>
        protected internal void ParseCityXmlNode(XmlNode node)
        {
            const string METRO = "Metro";
            const string MSA_TYPE = "msa-type";
            const string CBSA_TYPE = "cbsa-type";

            var cityNode = node as XmlElement;

            if (!String.IsNullOrWhiteSpace(cityNode?.Attributes[MSA_CODE]?.Value))
            {
                Msa = new MStatArea { Value = cityNode.Attributes[MSA_CODE].Value };
                if (!String.IsNullOrWhiteSpace(cityNode.Attributes[MSA_TYPE]?.Value))
                {
                    Msa.MsaType = cityNode.Attributes[MSA_TYPE].Value == METRO
                        ? UrbanCentric.City | UrbanCentric.Large
                        : UrbanCentric.City | UrbanCentric.Small;
                }
            }
            if (!String.IsNullOrWhiteSpace(cityNode?.Attributes[CBSA_CODE]?.Value))
            {
                Msa = new MStatArea { Value = cityNode.Attributes[CBSA_CODE].Value };
                if (!String.IsNullOrWhiteSpace(cityNode.Attributes[CBSA_TYPE]?.Value))
                {
                    Msa.MsaType = cityNode.Attributes[CBSA_TYPE].Value == METRO
                        ? UrbanCentric.City | UrbanCentric.Large
                        : UrbanCentric.City | UrbanCentric.Small;
                }
            }
            AverageEarnings = GetAvgEarningsPerYear(cityNode) ??
                              UsStateData.GetStateData(StateName)?.AverageEarnings;
        }

        /// <summary>
        /// Overwrites the data of this instance with the values present in the underlying xml data.
        /// </summary>
        /// <param name="pickSuburbAtRandom"></param>
        protected internal void SetAddrDataToXmlValues(bool pickSuburbAtRandom = true)
        {
            var cityNode = GetCityXmlNode() as XmlElement;
            if (cityNode?.Attributes[NAME] == null)
                return;
            GetData().Locality = cityNode.Attributes[NAME].Value ?? GetData().Locality;
            if (pickSuburbAtRandom)
                GetSuburbCityName(cityNode);
            ParseCityXmlNode(cityNode);
        }

        /// <summary>
        /// Converts the attributes data of the &quot;avg-earning-per-year&quot; into 
        /// a <see cref="LinearEquation"/>
        /// </summary>
        /// <param name="someNode"></param>
        /// <returns></returns>
        protected internal static LinearEquation GetAvgEarningsPerYear(XmlNode someNode)
        {
            const string AVG_EARNINGS_PER_YEAR = "avg-earning-per-year";
            var cityNode = someNode as XmlElement;
            if (String.IsNullOrWhiteSpace(cityNode?.Attributes[AVG_EARNINGS_PER_YEAR]?.Value))
                return null;
            var attrVal = cityNode.Attributes[AVG_EARNINGS_PER_YEAR].Value;
            return !LinearEquation.TryParse(attrVal, out var lq) ? null : lq;
        }

        /// <summary>
        /// Picks a 'place' name from the given city node.
        /// </summary>
        /// <param name="cityNode"></param>
        /// <remarks>
        /// The census.gov data&apos;s grouping of MSA\CBSA&apos;s are a higher level 
        /// aggregate of cities.  The census.gov also has another type named
        /// &quot;Places&quot; which are the more common city names citizens are used to.
        /// </remarks>
        protected internal void GetSuburbCityName(XmlNode cityNode)
        {
            const string PLACE = "place";
            if (cityNode == null)
                return;
            if (!cityNode.HasChildNodes)
                return;
            var places =
                cityNode.ChildNodes.OfType<XmlElement>().Where(x => x.LocalName == PLACE && x.HasAttributes).ToList();
            if (!places.Any())
                return;
            var pick = Etx.RandomInteger(0, places.Count - 1);
            var pickedPlace = places[pick];
            var suburbName = pickedPlace.GetAttribute(NAME);
            if (String.IsNullOrWhiteSpace(suburbName))
                return;
            GetData().Locality = suburbName;
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            if(!String.IsNullOrWhiteSpace(City))
                itemData.Add(textFormat(nameof(City)), City);
            if(!String.IsNullOrWhiteSpace(StateAbbrev))
                itemData.Add(textFormat("State"), StateAbbrev);
            if(!String.IsNullOrWhiteSpace(ZipCode))
                itemData.Add(textFormat(nameof(ZipCode)), ZipCode);
            return itemData;
        }

        #endregion
    }
}