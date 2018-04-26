using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Geo.CA;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// Base type representing the second half of a typical Postal Address
    /// </summary>
    [Serializable]
    public abstract class CityArea : GeoBase, ICited
    {
        #region constants

        public const string ZIP_CODE_SINGULAR = "zip-code";
        public const string ZIP_CODE_PLURAL = "zip-codes";
        public const string NAME = "name";
        public const string VALUE = "value";
        public const string PREFIX = "prefix";
        public const string MSA_CODE = "msa-code";
        public const string CBSA_CODE = "cbsa-code";
        public const string ZIP_STAT = "zip-stat";

        internal const string US_ZIP_CODE_DATA = "US_Zip_Data.xml";
        internal const string CA_POST_CODE_DATA = "CA_Postal_Data.xml";
        internal const string US_ZIP_PROB_TABLE = "US_Zip_ProbTable.xml";
        internal const string US_CITY_DATA = "US_City_Data.xml";

        #endregion

        #region fields
        internal static XmlDocument UsZipCodeXml;
        internal static XmlDocument CaPostCodeXml;
        internal static XmlDocument UsZipProbXml;
        internal static XmlDocument UsCityXml;
        #endregion

        #region ctor
        protected CityArea(AddressData d) :base(d)
        {
        }
        #endregion

        #region properties
        public virtual string Src { get; set; }
        public string City => GetData().City;

        #endregion

        #region methods

        public virtual string GetRegionAbbrev()
        {
            return GetData().RegionAbbrev ?? "";
        }

        public virtual string GetPostalCodePrefix()
        {
            if (String.IsNullOrWhiteSpace(GetData().PostalCode) || GetData().PostalCode.Length < 3)
                return null;
            return GetData().PostalCode.Substring(0, 3);

        }

        public override bool Equals(object obj)
        {
            var ca = obj as CityArea;
            return ca != null && GetData().Equals(ca.GetData());
        }

        public override int GetHashCode()
        {
            return GetData().GetHashCode();
        }

        /// <summary>
        /// Fetches, at random, a <see cref="UsCityStateZip"/> by zip code prefix
        /// </summary>
        /// <param name="zipCodePrefix">
        /// Optional, will be picked at random with respect to population if whitespace or null.
        /// </param>
        /// <param name="pickSuburbAtRandom">
        /// Optional, will pick the actual metro city itself or one if its surrounding suburbs at random.
        /// Set to false to force just the major metro city.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Ranking is by population totals sourced from
        /// https://www.census.gov/geo/maps-data/data/docs/rel/zcta_cbsa_rel_10.txt
        /// </remarks>
        [RandomFactory]
        public static UsCityStateZip RandomAmericanCity(string zipCodePrefix = null, bool pickSuburbAtRandom = true)
        {
            const string HAS_HIGH_SCHOOL = "has-high-school";
            const string VALUE = "value";
            //set defaults
            var ctz = new AddressData
            {
                PostalCode = $"{UsCityStateZip.DF_ZIPCODE_PREFIX}{Etx.RandomInteger(1, 99):00}",
                RegionAbbrev = UsCityStateZip.DF_STATE_ABBREV
            };

            //pick a zip code prefix at random
            if (String.IsNullOrWhiteSpace(zipCodePrefix))
                zipCodePrefix = UsCityStateZip.RandomAmericanPartialZipCode() ?? UsCityStateZip.DF_ZIPCODE_PREFIX;

            //x-ref it to the zip code data
            var xpathString = $"//{ZIP_CODE_PLURAL}//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zipCodePrefix}']";
            UsZipCodeXml = UsZipCodeXml ??
                            XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_ZIP_CODE_DATA, Assembly.GetExecutingAssembly());
            if (UsZipCodeXml == null)
                return null;
            var randZipCode =
                UsZipCodeXml.SelectSingleNode(xpathString);
            if (randZipCode?.ParentNode?.Attributes?[NAME] == null)
            {
                ctz.City = UsCityStateZip.DF_CITY_NAME;
                return new UsCityStateZip(ctz);
            }

            //get the containing us state
            ctz.RegionName =  randZipCode.ParentNode.Attributes[NAME].Value;
            var nfState = UsState.GetStateByName(ctz.RegionName) ??
                          UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV);

            ctz.RegionAbbrev = nfState.StateAbbrev ?? UsCityStateZip.DF_STATE_ABBREV;
            ctz.PostalCodeSuffix = $"{Etx.MyRand.Next(1, 9999):0000}";

            if (!randZipCode.HasChildNodes)
            {
                ctz.PostalCode = $"{zipCodePrefix}{Etx.RandomInteger(1, 99):00}";
            }
            else
            {
                //pick a particular zip-code (ZIP5)
                var zipCodes =
                    randZipCode.ChildNodes.Cast<XmlElement>()
                        .Where(
                            x =>
                                x.Attributes[HAS_HIGH_SCHOOL] != null &&
                                x.Attributes[HAS_HIGH_SCHOOL].Value == Boolean.TrueString)
                        .Select(x => x.Attributes[VALUE].Value).ToArray();
                if (zipCodes.Length <= 0)
                    return new UsCityStateZip(ctz);
                var pickNum = Etx.RandomInteger(0, zipCodes.Length - 1);
                ctz.PostalCode = zipCodes[pickNum];
            }
            var rr =  new UsCityStateZip(ctz);

            rr.SetAddrDataToXmlValues(pickSuburbAtRandom);
            return rr;
        }

        /// <summary>
        /// Fetches, at random, a <see cref="RandomCanadianCity"/> in which the values are only somewhat related.
        /// The Providence and City are related while the only first three characters of the Postal Code are actually realted.
        /// The last three are always '4Z4'.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CaCityProvidencePost RandomCanadianCity()
        {
            const string POSTAL_CODE = "postal-code";
            const string DF_FIRST_THREE_CHARS = "M5A";
            const string DF_CITY = "Toronto";
            const string DF_LAST_THREE_CHARS = "4Z4";
            const string MUNICIPALITY = "municipality";
            const string ABBREVIATION = "abbreviation";

            var ctz = new AddressData();
            CaPostCodeXml = CaPostCodeXml ??
                             XmlDocXrefIdentifier.GetEmbeddedXmlDoc(CA_POST_CODE_DATA, Assembly.GetExecutingAssembly());
            if (CaPostCodeXml == null)
                return null;
            var postalCodes = CaPostCodeXml.SelectNodes($"//{POSTAL_CODE}");
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

            ctz.RegionAbbrev = providenceElem.GetAttribute(ABBREVIATION);
            ctz.RegionName = providenceElem.GetAttribute(NAME);
            
            var postalPrefix = randPostalCode.GetAttribute(PREFIX);
            postalPrefix = String.IsNullOrWhiteSpace(postalPrefix) ? DF_FIRST_THREE_CHARS : postalPrefix;
            ctz.PostalCode = $"{postalPrefix} {DF_LAST_THREE_CHARS}";

            var municipalityNode = randPostalCode.ChildNodes.OfType<XmlElement>()
                .FirstOrDefault(x => x.LocalName == MUNICIPALITY && !String.IsNullOrWhiteSpace(x.InnerText));

            ctz.City = municipalityNode?.InnerText ?? DF_CITY;

            return new CaCityProvidencePost(ctz);
        }
        #endregion
    }
}
