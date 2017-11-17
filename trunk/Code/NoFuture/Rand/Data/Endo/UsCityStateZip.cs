using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.Census;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data.Endo
{
    /// <summary>
    /// A type to represent the typical City, State and ZIP code of a US Postal Address
    /// </summary>
    [Serializable]
    public class UsCityStateZip : CityArea
    {
        #region constants

        public const string STATE_CODE_REGEX = @"[\x20|\x2C](AK|AL|AR|AZ|CA|CO|CT|DC|DE|FL|GA|HI|" +
                                               "IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MS|" +
                                               "MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|RI|" +
                                               @"SC|SD|TN|TX|UT|VA|VI|VT|WA|WI|WV|WY)([^A-Za-z]|$)";

        public const string ZIP_CODE_REGEX = @"\x20([0-9]{5})(\x2d[0-9]{4})?";

        public const string DF_ZIPCODE_PREFIX = "100";//new york, new york
        public const string DF_STATE_ABBREV = "NY";
        public const string DF_CITY_NAME = "New York";

        #endregion

        #region fields

        private UsState _myState;
        private static readonly Dictionary<string, double> _zipCodePrefix2Pop = new Dictionary<string, double>();
        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance by query of the <see cref="TreeData.AmericanCityData"/>
        /// </summary>
        /// <param name="d">
        /// The criteria which on this this instance is situated.
        /// </param>
        /// <param name="pickSuburbAtRandom">
        /// Optional, will pick the actual metro city itself or one if its surrounding suburbs at random.
        /// Set to false to force just the major metro city.
        /// </param>
        public UsCityStateZip(AddressData d, bool pickSuburbAtRandom = true) : base(d)
        {
            var xmlNode = GetCityXmlNode();
            if (xmlNode == null)
                return;
            ParseCityXmlNode(xmlNode, pickSuburbAtRandom);
        }

        #endregion

        #region properties

        /// <summary>
        /// The two letter code used by the USPS (e.g. California is CA)
        /// </summary>
        public string PostalState => data.StateAbbrv;

        public UsState State => _myState ??
                                (_myState =
                                    UsState.GetStateByPostalCode(data.StateAbbrv));

        public string City => data.City;
        public string ZipCode => data.PostalCode;

        /// <summary>
        /// The additional 4 digits appearing after the 5 digit ZZIP Code
        /// </summary>
        public string PostalCodeAddonFour => data.PostalCodeSuffix;
        public MStatArea Msa { get; set; }
        public ComboMStatArea CbsaCode { get; set; }
        public LinearEquation AverageEarnings { get; set; }

        /// <summary>
        /// Dictionary parsed from <see cref="TreeData.UsZipProbabilityTable"/>
        /// </summary>
        public static Dictionary<string, double> ZipCodePrefix2Population
        {
            get
            {
                const string WEIGHT = "weight";
                if (_zipCodePrefix2Pop.Any()) return _zipCodePrefix2Pop;
                var usZipsXmlDocument = TreeData.UsZipProbabilityTable;

                var zipCodesElem = usZipsXmlDocument?.SelectSingleNode($"//{ZIP_CODE_PLURAL}");

                if (zipCodesElem == null || !zipCodesElem.HasChildNodes)
                    return _zipCodePrefix2Pop;

                foreach (var zipNode in zipCodesElem.ChildNodes)
                {
                    var zipElem = zipNode as XmlElement;
                    if (zipElem == null)
                        continue;
                    var key = zipElem.Attributes[PREFIX]?.Value;
                    var strVal = zipElem.Attributes[WEIGHT]?.Value;

                    if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(strVal))
                        continue;

                    if (!double.TryParse(strVal, out var val))
                        continue;

                    if (_zipCodePrefix2Pop.ContainsKey(key))
                        _zipCodePrefix2Pop[key] = val;
                    else
                        _zipCodePrefix2Pop.Add(key, val);
                }
                return _zipCodePrefix2Pop;
            }
        }

        #endregion

        #region methods

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

            //get the data for the given zip code
            var zipStatElem = TreeData.AmericanHighSchoolData.SelectSingleNode($"//{ZIP_STAT}[@{VALUE}='{zipCode}']");

            if (zipStatElem == null || !zipStatElem.HasChildNodes)
            {
                //try to find on the zip code prefix 
                var zip3 = zipCode.Substring(0, 3);
                var zipCodeElem =
                    TreeData.AmericanHighSchoolData.SelectSingleNode($"//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zip3}']");

                if (zipCodeElem == null || !zipCodeElem.HasChildNodes)
                    return AmericanRacePercents.GetNatlAvg();

                pick = Etx.MyRand.Next(0, zipCodeElem.ChildNodes.Count - 1);

                zipStatElem = zipCodeElem.ChildNodes[pick];
                if (zipStatElem == null)
                    return AmericanRacePercents.GetNatlAvg();
            }

            pick = Etx.MyRand.Next(0, zipStatElem.ChildNodes.Count - 1);
            var hsNode = zipStatElem.ChildNodes[pick];
            if (!AmericanHighSchool.TryParseXml(hsNode as XmlElement, out var hsOut))
                return AmericanRacePercents.GetNatlAvg();
            return hsOut.RacePercents;
        }

        public override string ToString()
        {
            return !String.IsNullOrWhiteSpace(data.PostalCodeSuffix)
                ? $"{data.City}, {data.StateAbbrv} {data.PostalCode}-{data.PostalCodeSuffix}"
                : $"{data.City}, {data.StateAbbrv} {data.PostalCode}";
        }

        /// <summary>
        /// Attempts to turn <see cref="lastLine"/> into an instance of <see cref="UsCityStateZip"/>
        /// </summary>
        /// <param name="lastLine">
        /// This is assumed to be a classic USPS Address&apos;s last-line (e.g. New York City, NW 10001). 
        /// </param>
        /// <param name="cityStateZip">
        /// The resulting instance when true is returned
        /// </param>
        /// <param name="pickSuburbAtRandom">
        /// Optional, will choose one of the surrounding suburbs at random, set to false to force to the 
        /// city proper.
        /// </param>
        /// <returns></returns>
        public static bool TryParse(string lastLine, out UsCityStateZip cityStateZip, bool pickSuburbAtRandom = false)
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
                PostalCodeSuffix = string.Empty,
                StateAbbrv = string.Empty,
                City = string.Empty
            };
            GetZipCode(lastLine, addrData);

            GetState(lastLine, addrData);

            GetCity(lastLine, addrData);

            cityStateZip = new UsCityStateZip(addrData, pickSuburbAtRandom);
            return true;
        }

        internal static void GetCity(string lastLine, AddressData addrData)
        {
            //city
            var city = lastLine ?? "";
            if (!string.IsNullOrEmpty(addrData.PostalCode))
                city = city.Replace(addrData.PostalCode, string.Empty);
            if (!string.IsNullOrEmpty(addrData.PostalCodeSuffix))
                city = city.Replace($"-{addrData.PostalCodeSuffix}", string.Empty);
            if (!string.IsNullOrEmpty(addrData.StateAbbrv) && city.Contains(" " + addrData.StateAbbrv))
                city = city.Replace(" " + addrData.StateAbbrv, string.Empty);
            if (!string.IsNullOrEmpty(addrData.StateAbbrv) && city.Contains("," + addrData.StateAbbrv))
                city = city.Replace("," + addrData.StateAbbrv, string.Empty);

            city = city.Replace(",", string.Empty).Trim();

            //need to deal with oddities 
            if (string.Equals("new york", city, StringComparison.OrdinalIgnoreCase))
                city = "New York City";

            addrData.City = city;
        }

        internal static void GetState(string lastLine, AddressData addrData)
        {
            var regex = new Regex(STATE_CODE_REGEX, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(lastLine))
                return;
            var matches = regex.Match(lastLine);

            var state = matches.Groups.Count >= 1 && matches.Groups[0].Success &&
                        matches.Groups[0].Captures.Count > 0
                ? matches.Groups[0].Captures[0].Value
                : string.Empty;

            addrData.StateAbbrv = state.Trim();
        }

        internal static void GetZipCode(string lastLine, AddressData addrData)
        {
            //zip code
            var regex = new Regex(ZIP_CODE_REGEX, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(lastLine))
                return;
            var matches = regex.Match(lastLine);

            var fiveDigitZip = matches.Groups.Count >= 2 && matches.Groups[1].Success &&
                               matches.Groups[1].Captures.Count > 0
                ? matches.Groups[1].Captures[0].Value
                : string.Empty;
            var zipPlusFour = matches.Groups.Count >= 3 && matches.Groups[2].Success &&
                              matches.Groups[2].Captures.Count > 0
                ? matches.Groups[2].Captures[0].Value
                : string.Empty;

            addrData.PostalCode = fiveDigitZip.Trim();
            addrData.PostalCodeSuffix = zipPlusFour.Replace("-", string.Empty).Trim();
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

            workingName = Etc.CapWords(workingName.ToLower(), ' ');

            if (workingName == "New York")
                return "New York City";

            if (workingName == "Winston Salem")
                return "Winston-Salem";

            var isMcSomething = workingName.Split(' ').Any(p => p.ToLower().StartsWith("mc")) || name.ToLower().StartsWith("mc");

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

                        bldr.Append(char.ToUpper(t));
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
        /// Based on the <see cref="ZipCode"/> and <see cref="State"/> are 
        /// assigned, picks a node, at random, from <see cref="TreeData.AmericanCityData"/>
        /// </summary>
        /// <returns></returns>
        protected internal XmlNode GetCityXmlNode()
        {
            if (TreeData.AmericanCityData == null)
                return null;
            string searchCrit;
            XmlNodeList matchedNodes = null;

            //search by city-state
            if (!string.IsNullOrWhiteSpace(data.StateAbbrv) && !string.IsNullOrWhiteSpace(data.City) &&
                UsState.GetStateByPostalCode(data.StateAbbrv) != null)
            {
                var usState = UsState.GetStateByPostalCode(data.StateAbbrv);
                var cityName = FinesseCityName(data.City);
                searchCrit = $"//state[@name='{usState}']/city[@name='{cityName}']";
                matchedNodes = TreeData.AmericanCityData.SelectNodes(searchCrit);

                //try again on place names
                if (matchedNodes == null || matchedNodes.Count <= 0)
                {
                    searchCrit = $"//state[@name='{usState}']//place[@name='{cityName}']/..";
                    matchedNodes = TreeData.AmericanCityData.SelectNodes(searchCrit);
                }
            }
            //search by first 3 of Zip Code
            else if (!string.IsNullOrWhiteSpace(data.PostalCode) && data.PostalCode.Length >= 3)
            {
                var zipCodePrefix= data.PostalCode.Substring(0, 3);
                searchCrit = $"//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zipCodePrefix}']/..";
                matchedNodes = TreeData.AmericanCityData.SelectNodes(searchCrit);
            }
            
            if (matchedNodes == null || matchedNodes.Count <= 0)
                return null;
            if (matchedNodes.Count == 1)
                return matchedNodes[0];

            //choose one with a msa-code value if possiable
            foreach (var matchedNode in matchedNodes)
            {
                var matchedElem = matchedNode as XmlElement;
                if (matchedElem == null)
                    continue;
                if(!matchedElem.HasAttributes)
                    continue;
                var hasMsaCode = !string.IsNullOrWhiteSpace(matchedElem.GetAttribute(MSA_CODE));
                var hasCbsaCode = !string.IsNullOrWhiteSpace(matchedElem.GetAttribute(CBSA_CODE));
                if (hasCbsaCode || hasMsaCode)
                    return matchedElem;
            }

            var pick = Etx.IntNumber(0, matchedNodes.Count - 1);
            return matchedNodes[pick];
        }

        /// <summary>
        /// Parses into this instance the data defined in <see cref="node"/>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pickSuburbAtRandom">Optional, will choose a suburb of the given metro area.</param>
        protected internal void ParseCityXmlNode(XmlNode node, bool pickSuburbAtRandom = true)
        {
            const string METRO = "Metro";
            const string MSA_TYPE = "msa-type";
            const string CBSA_TYPE = "cbsa-type";

            var cityNode = node as XmlElement;
            if (cityNode?.Attributes[NAME] == null)
                return;
            data.City = cityNode.Attributes[NAME].Value ?? data.City;

            if (!string.IsNullOrWhiteSpace(cityNode.Attributes[MSA_CODE]?.Value))
            {
                Msa = new MStatArea { Value = cityNode.Attributes[MSA_CODE].Value };
                if (!string.IsNullOrWhiteSpace(cityNode.Attributes[MSA_TYPE]?.Value))
                {
                    Msa.MsaType = cityNode.Attributes[MSA_TYPE].Value == METRO
                        ? UrbanCentric.City | UrbanCentric.Large
                        : UrbanCentric.City | UrbanCentric.Small;
                }
            }
            if (!string.IsNullOrWhiteSpace(cityNode.Attributes[CBSA_CODE]?.Value))
            {
                Msa = new MStatArea { Value = cityNode.Attributes[CBSA_CODE].Value };
                if (!string.IsNullOrWhiteSpace(cityNode.Attributes[CBSA_TYPE]?.Value))
                {
                    Msa.MsaType = cityNode.Attributes[CBSA_TYPE].Value == METRO
                        ? UrbanCentric.City | UrbanCentric.Large
                        : UrbanCentric.City | UrbanCentric.Small;
                }
            }
            AverageEarnings = GetAvgEarningsPerYear(cityNode) ?? _myState?.GetStateData()?.AverageEarnings;
            if(pickSuburbAtRandom)
                GetSuburbCityName(cityNode);
        }

        /// <summary>
        /// Converts the attributes data of the 'avg-earning-per-year' into 
        /// a <see cref="LinearEquation"/>
        /// </summary>
        /// <param name="someNode"></param>
        /// <returns></returns>
        protected internal static LinearEquation GetAvgEarningsPerYear(XmlNode someNode)
        {
            const string AVG_EARNINGS_PER_YEAR = "avg-earning-per-year";
            var cityNode = someNode as XmlElement;
            if (string.IsNullOrWhiteSpace(cityNode?.Attributes[AVG_EARNINGS_PER_YEAR]?.Value))
                return null;
            var attrVal = cityNode.Attributes[AVG_EARNINGS_PER_YEAR].Value;
            return !LinearEquation.TryParse(attrVal, out var lq) ? null : lq;
        }

        /// <summary>
        /// Picks a 'place' name from the given city node.
        /// </summary>
        /// <param name="cityNode"></param>
        /// <remarks>
        /// The census.gov data's grouping of MSA\CBSA's are a higher level 
        /// aggregate of cities.  The census.gov also has another type named
        /// 'Places' which are the more common city names citizens are used to.
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
            var pick = Etx.IntNumber(0, places.Count - 1);
            var pickedPlace = places[pick];
            var suburbName = pickedPlace.GetAttribute(NAME);
            if (string.IsNullOrWhiteSpace(suburbName))
                return;
            data.City = suburbName;
        }
        #endregion
    }
}