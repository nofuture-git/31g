using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.Census;
using NoFuture.Util.Math;
using NoFuture.Rand.Edu;
using NoFuture.Util;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    public abstract class CityArea : ICited
    {
        #region constants

        protected const string ZIP_CODE_SINGULAR = "zip-code";
        protected const string ZIP_CODE_PLURAL = "zip-codes";
        protected const string NAME = "name";
        protected const string VALUE = "value";
        protected const string PREFIX = "prefix";
        protected const string STATE = "state";
        protected const string CITY = "city";
        protected const string MSA_CODE = "msa-code";
        protected const string CBSA_CODE = "cbsa-code";
        protected const string ZIP_STAT = "zip-stat";

        #endregion

        #region fields
        protected readonly AddressData data;
        #endregion

        #region ctor
        protected CityArea(AddressData d)
        {
            data = d;
        }
        #endregion

        #region properties
        public virtual string Src { get; set; }
        public virtual AddressData AddressData => data;
        #endregion

        #region methods
        public virtual string GetPostalCodePrefix()
        {
            if (String.IsNullOrWhiteSpace(data?.PostalCode) || data.PostalCode.Length < 3)
                return null;
            return data.PostalCode.Substring(0, 3);

        }

        public override bool Equals(object obj)
        {
            var ca = obj as CityArea;
            return ca != null && AddressData.Equals(ca.AddressData);
        }

        public override int GetHashCode()
        {
            return AddressData.GetHashCode();
        }

        /// <summary>
        /// Fetches, at random, a <see cref="UsCityStateZip"/> by zip code prefix
        /// </summary>
        /// <param name="zipCodePrefix">
        /// Optional, will be picked at random with respect to population if whitespace or null.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Ranking is by population totals sourced from
        /// https://www.census.gov/geo/maps-data/data/docs/rel/zcta_cbsa_rel_10.txt
        /// </remarks>
        public static UsCityStateZip American(string zipCodePrefix = null)
        {
            const string HAS_HIGH_SCHOOL = "has-high-school";
            const string VALUE = "value";
            //set defaults
            var ctz = new AddressData
            {
                PostalCode = $"{UsCityStateZip.DF_ZIPCODE_PREFIX}{Etx.IntNumber(1, 99):00}",
                StateAbbrv = UsCityStateZip.DF_STATE_ABBREV
            };

            //pick a zip code prefix at random
            if (String.IsNullOrWhiteSpace(zipCodePrefix))
                zipCodePrefix = NAmerUtil.RandomAmericanZipWithRespectToPop() ?? UsCityStateZip.DF_ZIPCODE_PREFIX;

            //x-ref it to the zip code data
            var randZipCode =
                TreeData.AmericanZipCodeData.SelectSingleNode($"//{ZIP_CODE_PLURAL}//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zipCodePrefix}']");
            if (randZipCode?.ParentNode?.Attributes?[NAME] == null)
            {
                ctz.City = UsCityStateZip.DF_CITY_NAME;
                return new UsCityStateZip(ctz);
            }

            //get the containing us state
            ctz.StateName =  randZipCode.ParentNode.Attributes[NAME].Value;
            var nfState = UsState.GetStateByName(ctz.StateName) ??
                          UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV);

            ctz.StateAbbrv = nfState.StateAbbrv ?? UsCityStateZip.DF_STATE_ABBREV;
            ctz.PostalCodeSuffix = $"{Etx.MyRand.Next(1, 9999):0000}";

            if (!randZipCode.HasChildNodes)
            {
                ctz.PostalCode = $"{zipCodePrefix}{Etx.IntNumber(1, 99):00}";
            }
            else
            {
                //pick a particular zip-code (ZIP5)
                var zipCodes =
                    randZipCode.ChildNodes.Cast<XmlElement>()
                        .Where(
                            x =>
                                x.Attributes[HAS_HIGH_SCHOOL] != null &&
                                x.Attributes[HAS_HIGH_SCHOOL].Value == bool.TrueString)
                        .Select(x => x.Attributes[VALUE].Value).ToArray();
                if (zipCodes.Length <= 0)
                    return new UsCityStateZip(ctz);
                var pickNum = Etx.IntNumber(0, zipCodes.Length - 1);
                ctz.PostalCode = zipCodes[pickNum];
            }
            return new UsCityStateZip(ctz);
        }

        /// <summary>
        /// Fetches, at random, a <see cref="Canadian"/> in which the values are only somewhat related.
        /// The Providence and City are related while the only first three characters of the Postal Code are actually realted.
        /// The last three are always '4Z4'.
        /// </summary>
        /// <returns></returns>
        public static CaCityProvidencePost Canadian()
        {
            const string POSTAL_CODE = "postal-code";
            const string DF_FIRST_THREE_CHARS = "M5A";
            const string DF_CITY = "Toronto";
            const string DF_LAST_THREE_CHARS = "4Z4";
            const string MUNICIPALITY = "municipality";
            const string ABBREVIATION = "abbreviation";

            var ctz = new AddressData();
            var postalCodes = TreeData.CanadianPostalCodeData.SelectNodes($"//{POSTAL_CODE}");
            var dfReturn = new CaCityProvidencePost(ctz);
            if (postalCodes == null)
                return dfReturn;

            var pickOne = Etx.MyRand.Next(0, postalCodes.Count);

            var randPostalCode = postalCodes[pickOne] as XmlElement;
            if(randPostalCode == null)
                return dfReturn;

            var providenceElem = randPostalCode.ParentNode as XmlElement;
            if (providenceElem == null)
                return dfReturn;

            ctz.StateAbbrv = providenceElem.GetAttribute(ABBREVIATION);
            ctz.StateName = providenceElem.GetAttribute(NAME);
            
            var postalPrefix = randPostalCode.GetAttribute(PREFIX);
            postalPrefix = string.IsNullOrWhiteSpace(postalPrefix) ? DF_FIRST_THREE_CHARS : postalPrefix;
            ctz.PostalCode = $"{postalPrefix} {DF_LAST_THREE_CHARS}";

            var municipalityNode = randPostalCode.ChildNodes.OfType<XmlElement>()
                .FirstOrDefault(x => x.LocalName == MUNICIPALITY && !string.IsNullOrWhiteSpace(x.InnerText));

            ctz.City = municipalityNode?.InnerText ?? DF_CITY;

            return new CaCityProvidencePost(ctz);
        }
        #endregion
    }

    [Serializable]
    public class UsCityStateZip : CityArea
    {
        #region constants

        public const string STATE_CODE_REGEX = @"[\x20|\x2C](AK|AL|AR|AZ|CA|CO|CT|DC|DE|FL|GA|HI|" +
                                               "IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MS|" +
                                               "MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|RI|" +
                                               @"SC|SD|TN|TX|UT|VA|VI|VT|WA|WI|WV|WY)\x20*";

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

        public UsCityStateZip(AddressData d) : base(d)
        {
            var xmlNode = GetCityXmlNode();
            if (xmlNode == null)
                return;
            ParseCityXmlNode(xmlNode);
        }

        #endregion

        #region properties

        public string PostalState => data.StateAbbrv;

        public UsState State => _myState ??
                                    (_myState =
                                        UsState.GetStateByPostalCode(data.StateAbbrv));

        public string City => data.City;
        public string ZipCode => data.PostalCode;
        public string PostalCodeAddonFour => data.PostalCodeSuffix;
        public MStatArea Msa { get; set; }
        public ComboMStatArea CbsaCode { get; set; }
        public LinearEquation AverageEarnings { get; set; }

        /// <summary>
        /// Dictionary parsed from 'US_Zip_ProbTable.xml'
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

                    if (String.IsNullOrWhiteSpace(key) || String.IsNullOrWhiteSpace(strVal))
                        continue;

                    double val;
                    if (!Double.TryParse(strVal, out val))
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

            AmericanHighSchool hsOut;
            pick = Etx.MyRand.Next(0, zipStatElem.ChildNodes.Count - 1);
            var hsNode = zipStatElem.ChildNodes[pick];
            if (!AmericanHighSchool.TryParseXml(hsNode as XmlElement, out hsOut))
                return AmericanRacePercents.GetNatlAvg();
            return hsOut.RacePercents;
        }

        public override string ToString()
        {
            return !String.IsNullOrWhiteSpace(data.PostalCodeSuffix)
                ? $"{data.City}, {data.StateAbbrv} {data.PostalCode}-{data.PostalCodeSuffix}"
                : $"{data.City}, {data.StateAbbrv} {data.PostalCode}";
        }

        public static bool TryParse(string lastLine, out UsCityStateZip cityStateZip)
        {
            if (String.IsNullOrWhiteSpace(lastLine))
            {
                cityStateZip = null;
                return false;
            }
            lastLine = lastLine.Trim();

            var addrData = new AddressData
                           {
                               PostalCode = String.Empty,
                               PostalCodeSuffix = String.Empty,
                               StateAbbrv = String.Empty,
                               City = String.Empty
                           };
            GetZipCode(lastLine, addrData);

            GetState(lastLine, addrData);

            GetCity(lastLine, addrData);

            cityStateZip = new UsCityStateZip(addrData);
            return true;
        }

        internal static void GetCity(string lastLine, AddressData addrData)
        {
            //city
            var city = lastLine;
            if (addrData.PostalCode.Length > 0)
                city = city.Replace(addrData.PostalCode, String.Empty);
            if (addrData.PostalCodeSuffix.Length > 0)
                city = city.Replace($"-{addrData.PostalCodeSuffix}", String.Empty);
            if (addrData.StateAbbrv.Length > 0 && city.Contains(" " + addrData.StateAbbrv))
                city = city.Replace(" " + addrData.StateAbbrv, String.Empty);
            if (addrData.StateAbbrv.Length > 0 && city.Contains("," + addrData.StateAbbrv))
                city = city.Replace("," + addrData.StateAbbrv, String.Empty);

            addrData.City = city.Replace(",", String.Empty).Trim();
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
                : String.Empty;

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
                : String.Empty;
            var zipPlusFour = matches.Groups.Count >= 3 && matches.Groups[2].Success &&
                              matches.Groups[2].Captures.Count > 0
                ? matches.Groups[2].Captures[0].Value
                : String.Empty;

            addrData.PostalCode = fiveDigitZip.Trim();
            addrData.PostalCodeSuffix = zipPlusFour.Replace("-", String.Empty).Trim();
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
            var searchCrit = String.Empty;
            if (!String.IsNullOrWhiteSpace(data.StateAbbrv) && !String.IsNullOrWhiteSpace(data.City) &&
                UsState.GetStateByPostalCode(data.StateAbbrv) != null)
            {
                var usState = UsState.GetStateByPostalCode(data.StateAbbrv);
                var cityName = Etc.CapWords(data.City.ToLower(), ' ');
                searchCrit = $"//{STATE}[@{NAME}='{usState}']/{CITY}[@{NAME}='{cityName}']";
            }
            else if (!String.IsNullOrWhiteSpace(data.PostalCode) && data.PostalCode.Length >= 3)
            {
                var zipCodePrefix= data.PostalCode.Substring(0, 3);
                searchCrit = $"//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zipCodePrefix}']/..";
            }
            if (String.IsNullOrWhiteSpace(searchCrit))
                return null;
            var matchedNodes = TreeData.AmericanCityData.SelectNodes(searchCrit);
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
                var hasMsaCode = !String.IsNullOrWhiteSpace(matchedElem.GetAttribute(MSA_CODE));
                var hasCbsaCode = !String.IsNullOrWhiteSpace(matchedElem.GetAttribute(CBSA_CODE));
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
        protected internal void ParseCityXmlNode(XmlNode node)
        {
            const string METRO = "Metro";
            const string MSA_TYPE = "msa-type";
            const string CBSA_TYPE = "cbsa-type";

            var cityNode = node as XmlElement;
            if (cityNode?.Attributes[NAME] == null)
                return;
            data.City = cityNode.Attributes[NAME].Value ?? data.City;

            if (!String.IsNullOrWhiteSpace(cityNode.Attributes[MSA_CODE]?.Value))
            {
                Msa = new MStatArea { Value = cityNode.Attributes[MSA_CODE].Value };
                if (!String.IsNullOrWhiteSpace(cityNode.Attributes[MSA_TYPE]?.Value))
                {
                    Msa.MsaType = cityNode.Attributes[MSA_TYPE].Value == METRO
                        ? UrbanCentric.City | UrbanCentric.Large
                        : UrbanCentric.City | UrbanCentric.Small;
                }
            }
            if (!String.IsNullOrWhiteSpace(cityNode.Attributes[CBSA_CODE]?.Value))
            {
                Msa = new MStatArea { Value = cityNode.Attributes[CBSA_CODE].Value };
                if (!String.IsNullOrWhiteSpace(cityNode.Attributes[CBSA_TYPE]?.Value))
                {
                    Msa.MsaType = cityNode.Attributes[CBSA_TYPE].Value == METRO
                        ? UrbanCentric.City | UrbanCentric.Large
                        : UrbanCentric.City | UrbanCentric.Small;
                }
            }
            AverageEarnings = GetAvgEarningsPerYear(cityNode) ?? _myState?.GetStateData()?.AverageEarnings;
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
            if (String.IsNullOrWhiteSpace(cityNode?.Attributes[AVG_EARNINGS_PER_YEAR]?.Value))
                return null;
            var attrVal = cityNode.Attributes[AVG_EARNINGS_PER_YEAR].Value;
            LinearEquation lq;
            return !LinearEquation.TryParse(attrVal, out lq) ? null : lq;
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
            if (String.IsNullOrWhiteSpace(suburbName))
                return;
            data.City = suburbName;
        }
        #endregion
    }
    [Serializable]
    public class CaCityProvidencePost : CityArea
    {
        public CaCityProvidencePost(AddressData d) : base(d) { }
        public string ProvidenceAbbrv => data.StateAbbrv;
        public string Providence => data.StateName;
        public string City => data.City;
        public string PostalCode => data.PostalCode;

        public override string ToString()
        {
            return $"{City} {ProvidenceAbbrv}, {PostalCode}";
        }
    }
}
