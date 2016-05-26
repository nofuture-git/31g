using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    public abstract class CityArea : ICited
    {
        protected readonly AddressData data;
        protected CityArea(AddressData d)
        {
            data = d;
        }
        public virtual string Src { get; set; }
        public virtual AddressData AddressData { get { return data; } }

        public virtual string GetPostalCodePrefix()
        {
            if (data == null || string.IsNullOrWhiteSpace(data.PostalCode) || data.PostalCode.Length < 3)
                return null;
            return data.PostalCode.Substring(0, 3);

        }

        public override bool Equals(object obj)
        {
            var ca = obj as CityArea;
            if (ca == null)
                return false;
            return AddressData.Equals(ca.AddressData);
        }

        public override int GetHashCode()
        {
            return AddressData.GetHashCode();
        }

        /// <summary>
        /// Fetches, at random, a <see cref="UsCityStateZip"/> by zip code prefix
        /// </summary>
        /// <param name="zipCodePrefix">Optional, will be picked at random with respect to population if whitespace or null.</param>
        /// <returns></returns>
        /// <remarks>
        /// Ranking is by population totals sourced from
        /// https://www.census.gov/geo/maps-data/data/docs/rel/zcta_cbsa_rel_10.txt
        /// </remarks>
        public static UsCityStateZip American(string zipCodePrefix)
        {
            //set defaults
            var ctz = new AddressData() {City = "New York", PostalCode = "10066", StateAbbrv = "NY"};

            //pick a zip code prefix at random
            if(string.IsNullOrWhiteSpace(zipCodePrefix))
                zipCodePrefix = Etx.RandomAmericanZipWithRespectToPop();

            //x-ref it to the zip code data
            var randZipCode =
                TreeData.AmericanZipCodeData.SelectSingleNode(string.Format(
                    "//zip-codes//zip-code[@prefix='{0}']", zipCodePrefix));
            if (randZipCode == null || randZipCode.ParentNode == null || randZipCode.ParentNode.Attributes == null ||
                randZipCode.ParentNode.Attributes["name"] == null)
            {
                return new UsCityStateZip(ctz);
            }

            //get the containing us state
            ctz.StateName =  randZipCode.ParentNode.Attributes["name"].Value;
            var nfState = Gov.UsState.GetStateByName(ctz.StateName) ??
                          Gov.UsState.GetStateByPostalCode("NY");

            ctz.StateAbbrv = nfState.StateAbbrv ?? "NY";
            ctz.PostalCodeSuffix = string.Format("{0:0000}", Etx.MyRand.Next(1, 9999));

            if (!randZipCode.HasChildNodes)
            {
                ctz.PostalCode = string.Format("{0}{1:00}", zipCodePrefix, Etx.IntNumber(1, 99));
            }
            else
            {
                //pick a particular zip-code (ZIP5)
                var zipCodes =
                    randZipCode.ChildNodes.Cast<XmlElement>()
                        .Where(
                            x =>
                                x.Attributes["has-high-school"] != null &&
                                x.Attributes["has-high-school"].Value == bool.TrueString)
                        .Select(x => x.Attributes["value"].Value).ToArray();
                if (zipCodes.Length > 0)
                {
                    var pickNum = Etx.IntNumber(0, zipCodes.Length - 1);
                    ctz.PostalCode = zipCodes[pickNum];
                }
            }

            //pick a city based on zip-code prefix
            var randCityData =
                TreeData.AmericanCityData.SelectSingleNode(string.Format("//zip-code[@prefix='{0}']/..", zipCodePrefix));
            if (randCityData == null || randCityData.Attributes == null || randCityData.Attributes["name"] == null)
                return new UsCityStateZip(ctz);

            ctz.City = randCityData.Attributes["name"].Value;

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
            var ctz = new AddressData();
            var zipCodes = Data.TreeData.CanadianPostalCodeData.SelectNodes("//zip-code");

            var pickOne = Etx.MyRand.Next(0, zipCodes.Count);

            var randZipCode = zipCodes[pickOne];
            ctz.PostalCode = string.Format("{0} 4Z4", randZipCode.SelectSingleNode("prefix").InnerText);
            ctz.StateAbbrv = randZipCode.SelectSingleNode("state-abbreviation").InnerText;
            ctz.StateName = randZipCode.SelectSingleNode("state-name").InnerText;
            ctz.City = randZipCode.SelectSingleNode("municipality").InnerText.Replace("\n", "").Trim();

            return new CaCityProvidencePost(ctz);
        }

    }
    [Serializable]
    public class UsCityStateZip : CityArea
    {
        #region Regex Patterns
        public const string STATE_CODE_REGEX = @"[\x20|\x2C](AK|AL|AR|AZ|CA|CO|CT|DC|DE|FL|GA|HI|" + 
                                               "IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MS|" +
                                               "MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|RI|" +
                                               @"SC|SD|TN|TX|UT|VA|VI|VT|WA|WI|WV|WY)\x20*";
        public const string ZIP_CODE_REGEX = @"\x20([0-9]{5})(\x2d[0-9]{4})?";
        private Gov.UsState _myState;
        #endregion
        public UsCityStateZip(AddressData d) : base(d)
        {
        }

        public string PostalState { get { return data.StateAbbrv; } }
        public Gov.UsState State { get {
            return _myState ??
                   (_myState = Gov.UsState.GetStateByPostalCode(string.IsNullOrWhiteSpace(data.StateAbbrv) ? "NY" : data.StateAbbrv));
        }
        }
        public string City { get { return data.City; } }
        public string ZipCode { get { return data.PostalCode; } }
        public string PostalCodeAddonFour { get { return data.PostalCodeSuffix; } }
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(data.PostalCodeSuffix)
                           ? string.Format("{0}, {1} {2}-{3}", data.City, data.StateAbbrv, data.PostalCode, data.PostalCodeSuffix)
                           : string.Format("{0}, {1} {2}", data.City, data.StateAbbrv, data.PostalCode);
        }

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
                               PostalCodeSuffix = string.Empty,
                               StateAbbrv = string.Empty,
                               City = string.Empty
                           };
            //zip code
            var regex = new Regex(ZIP_CODE_REGEX, RegexOptions.IgnoreCase);
            if (regex.IsMatch(lastLine))
            {
                var matches = regex.Match(lastLine);

                var fiveDigitZip = matches.Groups.Count >= 2 && matches.Groups[1].Success && matches.Groups[1].Captures.Count > 0
                    ? matches.Groups[1].Captures[0].Value
                    : string.Empty;
                var zipPlusFour = matches.Groups.Count >= 3 && matches.Groups[2].Success && matches.Groups[2].Captures.Count > 0
                    ? matches.Groups[2].Captures[0].Value
                    : string.Empty;

                addrData.PostalCode = fiveDigitZip.Trim();
                addrData.PostalCodeSuffix = zipPlusFour.Replace("-", string.Empty).Trim();
            }

            //state
            regex = new Regex(STATE_CODE_REGEX,RegexOptions.IgnoreCase);
            if (regex.IsMatch(lastLine))
            {
                var matches = regex.Match(lastLine);

                var state = matches.Groups.Count >= 1 && matches.Groups[0].Success &&
                           matches.Groups[0].Captures.Count > 0
                    ? matches.Groups[0].Captures[0].Value
                    : string.Empty;

                addrData.StateAbbrv = state.Trim();

            }

            //city
            var city = lastLine;
            if (addrData.PostalCode.Length > 0)
                city = city.Replace(addrData.PostalCode, string.Empty);
            if (addrData.PostalCodeSuffix.Length > 0)
                city = city.Replace(string.Format("-{0}", addrData.PostalCodeSuffix), string.Empty);
            if (addrData.StateAbbrv.Length > 0 && city.Contains(" " + addrData.StateAbbrv))
                city = city.Replace(" " + addrData.StateAbbrv, string.Empty);
            if (addrData.StateAbbrv.Length > 0 && city.Contains("," + addrData.StateAbbrv))
                city = city.Replace("," + addrData.StateAbbrv, string.Empty);

            addrData.City = city.Replace(",",string.Empty).Trim();

            cityStateZip = new UsCityStateZip(addrData);
            return true;

        }
    }
    [Serializable]
    public class CaCityProvidencePost : CityArea
    {
        public CaCityProvidencePost(AddressData d) : base(d) { }
        public string ProvidenceAbbrv { get { return data.StateAbbrv; } }
        public string Providence { get { return data.StateName; } }
        public string City { get { return data.City; } }
        public string PostalCode { get { return data.PostalCode; } }

        public override string ToString()
        {
            return string.Format("{0} {1}, {2}", City, ProvidenceAbbrv, PostalCode);
        }
    }
}
